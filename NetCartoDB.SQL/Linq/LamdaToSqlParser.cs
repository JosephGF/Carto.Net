using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetCarto.Core.ComponentModel;
using NetCarto.Core.Utils;
using System.Collections;

namespace NetCarto.SQL.Linq
{
    internal class LamdaToSqlParser
    {
        internal const string COLUMN_VALUE = "({0}{1}{2})";
        internal const string COLUMN_VALUE_NULL = "({0} is NULL)";
        internal const string COLUMN_KEY_DELIMITER = "\"{0}\"";

        internal static string LambdaExpresionTranslate(Expression<Func<object, bool>> predicate)
        {
            return ExpressionValue(predicate, COLUMN_VALUE) as string;
        }
        internal static string LambdaExpresionTranslate<TEntityAux>(Expression<Func<TEntityAux, object>> predicate) where TEntityAux : ICartoEntity
        {
            return ExpressionValue(predicate, COLUMN_VALUE) as string;
        }
        internal static string LambdaExpresionTranslate<TEntityAux>(Expression<Func<TEntityAux, bool>> predicate) where TEntityAux : ICartoEntity
        {
            return ExpressionValue(predicate, COLUMN_VALUE) as string;
        }
        private static string ToSQL(MethodInfo method, Type typeOfValue)
        {
            switch (method.Name)
            {
                case "Equals":
                    return "{0}={1}";
                case "StartsWith":
                    return "{0} LIKE '{1}%'";
                case "EndsWith":
                    return "{0} LIKE '%{1}'";
                case "Contains":
                    return (typeOfValue.IsArray == true) ? "{0} IN ({1})" : "{0} LIKE '{1}'";
                case "Count":
                    return "Count({0})";
                case "Sum":
                    return "Sum({0})";
                case "Max":
                    return "Max({0})";
                case "Min":
                    return "Min({0})";
                case "Average":
                    return "Avg({0})";
                default:
                    return Reflection.GetAttribute<SQLFunctionExtensiosAttribute>(method)?.Pattern;
            }
        }
        private static string ToSQL(ExpressionType expression)
        {
            switch (expression)
            {
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.And:
                    return " AND ";
                case ExpressionType.AndAlso:
                    return " AND ";
                case ExpressionType.Or:
                    return " OR ";
                case ExpressionType.OrElse:
                    return " OR ";
                case ExpressionType.Add:
                    return " + ";
                case ExpressionType.Subtract:
                    return " - ";
                case ExpressionType.Multiply:
                    return " * ";
                case ExpressionType.Divide:
                    return " / ";
                case ExpressionType.Power:
                    return " ^ ";
                default:
                    throw new NotSupportedException("Carto.SQL.Linq no support " + expression.ToString() + " lamda expression");
            }

            return string.Empty;
        }
        private static object ExpressionValue(Expression expression)
        {
            return ExpressionValue(expression, null);
        }
        private static object ExpressionValue(Expression expression, string formatText)
        {
            object value = null;
            if (expression is LambdaExpression)
            {
                var expr = expression as LambdaExpression;
                value = ExpressionValue(expr.Body, formatText);
            }
            else if (expression is BinaryExpression)
            {
                BinaryExpression operation = expression as BinaryExpression;
                Expression left = operation.Left;
                Expression right = operation.Right;

                object rightVal = ExpressionValue(right, formatText);
                object leftVal = ExpressionValue(left, formatText);

                if (rightVal == null && operation.NodeType == ExpressionType.Equal)
                    formatText = COLUMN_VALUE_NULL;

                if (leftVal == null)
                    throw new ArgumentException("Left lambda binary expression can't be null");

                value = String.Format(formatText, leftVal, ToSQL(operation.NodeType), rightVal);
            }
            else if (expression is MemberExpression)
            {
                var expr = expression as MemberExpression;
                try
                {
                    value = GetExpressionValue(expr);
                }
                catch (InvalidOperationException ex)
                {
                    string tblName = TableName(expr.Member.DeclaringType);
                    string colName = ColumnName(expr.Member.DeclaringType, expr.Member.Name);
                    value = ColumnName(colName, tblName);
                }
            }
            else if (expression is UnaryExpression)
            {
                var expr = expression as UnaryExpression;
                value = ExpressionValue(expr.Operand, formatText);
            }
            else if (expression is ConstantExpression)
            {
                var expr = expression as ConstantExpression;
                value = expr.Value;

                if (!new string[] { "int32", "int64", "double", "decimal", "boolean" }.Contains(expr.Type.Name.ToLowerInvariant()))
                    value = String.Format("'{0}'", value);
            }
            else if (expression is MethodCallExpression)
            {
                var expr = expression as MethodCallExpression;
                object col = null;
                List<object> arguments = new List<object>();

                string fSql = ToSQL(expr.Method, expr.Arguments[0].Type);
                if (expr.Object != null)
                    arguments.Add(ExpressionValue(expr.Object).ToString());
                else if (expr.Arguments.Count > 1)
                    for(var i = 1; i < expr.Arguments.Count; i++)
                        arguments.Add(ExpressionValue(expr.Arguments[i], formatText));

                object val = ExpressionValue(expr.Arguments[0], formatText);
                arguments.Add(val);
                value = String.Format(fSql, arguments.ToArray());
            }
            else if (expression is NewArrayExpression)
            {
                List<object> arguments = new List<object>();
                var expr = expression as NewArrayExpression;
                    for (var i = 0; i < expr.Expressions.Count; i++)
                        arguments.Add(ExpressionValue(expr.Expressions[i], formatText));

                value = String.Join(",", arguments);
            }
            //else if(expression is TypedParameterExpression)
            else
            {
            }

            return value;
        }

        internal static object FormatValue(object value)
        {
            if (!new string[] { "int32", "int64", "double", "decimal", "boolean" }.Contains(value.GetType().Name.ToLowerInvariant()))
                value = String.Format("'{0}'", value);

            return value;
        }

        internal static object GetExpressionValue(Expression expression)
        {
            object value = null;
            if (expression is MemberExpression)
            {
                MemberExpression expr = expression as MemberExpression;

                var objectMember = Expression.Convert(expr, typeof(object));
                var getterLambda = Expression.Lambda<Func<object>>(objectMember);
                var getter = getterLambda.Compile();
                value = getter();
                if (expr.Type.IsArray)
                {
                    Array arrValue = (Array)value;
                    for (int i = 0; i < arrValue.Length; i++)
                        arrValue.SetValue(FormatValue(arrValue), i);
                    value = String.Join(",", arrValue);

                }
                else if (!new string[] { "int32", "int64", "double", "decimal", "boolean" }.Contains(expr.Type.Name.ToLowerInvariant()))
                    value = FormatValue(value);
            }

            return value;
        }

        internal static string ColumnName(string column, string table)
        {
            string tbl = String.Format(COLUMN_KEY_DELIMITER, table);
            string col = column.Equals("*") ? column : String.Format(COLUMN_KEY_DELIMITER, column);
            return tbl + "." + col;
        }

        internal static string ColumnName(Type entityType, string property)
        {
            return Reflection.GetAttribute<SQLColumnAttribute>(entityType, property)?.Name ?? property;
        }

        internal static string TableName(Type entityType)
        {
            return Reflection.GetAttribute<SQLTableAttribute>(entityType)?.Name ?? entityType.Name;
        }
    }
}
