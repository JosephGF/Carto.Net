using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NetCartoDB.SQL.Linq.CartoDBConfigurations
{
    internal class CartoDbSQLParser : ISqlParser
    {
        public CartoDbSQLParser()
        {
            this.StrARGS = "@{0}";
            this.StrNULL = "NULL";
            this.StrcolumnAlias = "({0}) as {1}";

            this.StrWHERE = "({0}={1})";
            this.StrWHERENull = "({0} is NULL)";
            this.StrVALUE = "{0}={1}";
            this.StrWHERELamda = "({0}{1}{2})";
            this.StrJOIN = " {0} {1} ON ({2}) ";
            this.StrBETWEEN = "{0} BETWEEEN {1} AND {2}";
            this.StrIN = "{0} IN ({1})";

            this.StrcolumnSeparator = ", ";
            this.StrGROPUPBYSeparator = ", ";
            this.StrORDERBYSeparator = ", ";
            this.StrINSeparator = ", ";
            this.StrFROMSeparator = ", ";

            this.StrCOUNT = "COUNT({0})";
            this.StrSUM = "SUM({0})";
            this.StrMAX = "MAX({0})";
            this.StrMIN = "MIN({0})";
            this.StrAVG = "AVG({0})";
            this.StrDBEncapsule = "[{0}]";
        }

        #region SQL SINTAX
        public string StrcolumnAlias { get; private set; }
        public string StrNULL { get; private set; }
        public string StrARGS { get; private set; }
        public string StrWHERE { get; private set; }
        public string StrWHERENull { get; private set; }
        public string StrVALUE { get; private set; }
        public string StrWHERELamda { get; private set; }
        public string StrJOIN { get; private set; }
        public string StrBETWEEN { get; private set; }
        public string StrIN { get; private set; }
        public string StrcolumnSeparator { get; private set; }
        public string StrGROPUPBYSeparator { get; private set; }
        public string StrORDERBYSeparator { get; private set; }
        public string StrINSeparator { get; private set; }
        public string StrFROMSeparator { get; private set; }
        public string StrDBEncapsule { get; private set; }
        #endregion

        #region SQL Functions
        public string StrCOUNT { get; private set; }
        public string StrSUM { get; private set; }
        public string StrMAX { get; private set; }
        public string StrMIN { get; private set; }
        public string StrAVG { get; private set; }
        #endregion

        public string MethodToSql(System.Reflection.MethodInfo method, Type valueType)
        {
            string data = "";
            switch (method.Name)
            {
                case "StartsWith":
                    data = "{0} LIKE '{1}%'";
                    break;
                case "EndsWith":
                    data = "{0} LIKE '%{1}'";
                    break;
                case "Equals":
                    data = "{0}={1}";
                    break;
                case "Contains":
                    data = (valueType.IsArray == true) ? "{0} IN ({1})" : "{0} LIKE '{1}'";
                    break;
                case "Count":
                    data = this.StrCOUNT;
                    break;
                case "Sum":
                    data = this.StrSUM;
                    break;
                case "Max":
                    data = this.StrMAX;
                    break;
                case "Min":
                    data = this.StrMIN;
                    break;
                case "Average":
                    data = this.StrAVG;
                    break;
                default:
                    throw new Exception("This method can't be process");
            }
            return data;
        }
        public string GenerateSQL(SqlBuilder sqlBuilder)
        {
            StringBuilder sb = new StringBuilder();
            switch (sqlBuilder.TypeQuery)
            {
                case SqlSentence.SELECT:
                    sb.Append("SELECT ");

                    if (sqlBuilder.distinct)
                        sb.Append("DISTINCT ");

                    if (sqlBuilder.rowsLimit > 0)
                        sb.Append("TOP " + sqlBuilder.rowsLimit.ToString() + " ");

                    sb.AppendLine(sqlBuilder.sqlcolumns)
                      .Append(" FROM ").AppendLine(sqlBuilder.sqlTables);

                    if (!String.IsNullOrEmpty(sqlBuilder.sqlJoins))
                        sb.AppendLine(sqlBuilder.sqlJoins);

                    if (!String.IsNullOrEmpty(sqlBuilder.sqlWhere))
                        sb.Append(" WHERE ").AppendLine(sqlBuilder.sqlWhere);

                    if (!String.IsNullOrEmpty(sqlBuilder.sqlGroupBy))
                        sb.Append(" GROUP BY ").AppendLine(sqlBuilder.sqlGroupBy);

                    if (!String.IsNullOrEmpty(sqlBuilder.sqlHaving))
                        sb.Append(" HAVING ").AppendLine(sqlBuilder.sqlHaving);

                    if (!String.IsNullOrEmpty(sqlBuilder.sqlOrderBy))
                        sb.Append(" ORDER BY ").AppendLine(sqlBuilder.sqlOrderBy);

                    break;
                case SqlSentence.INSERT:
                    sb.Append("INSERT INTO ").AppendLine(sqlBuilder.sqlTables);

                    if (!String.IsNullOrEmpty(sqlBuilder.sqlcolumns))
                        sb.Append(" (").Append(sqlBuilder.sqlcolumns).AppendLine(")");

                    sb.Append(" VALUES (").Append(sqlBuilder.sqlValues).Append(")");
                    break;
                case SqlSentence.UPDATE:
                    sb.Append("UPDATE ").AppendLine(sqlBuilder.sqlTables)
                      .Append(" SET ").AppendLine(sqlBuilder.sqlValues);

                    if (!String.IsNullOrEmpty(sqlBuilder.sqlWhere))
                        sb.Append(" WHERE ").AppendLine(sqlBuilder.sqlWhere);

                    if (!String.IsNullOrEmpty(sqlBuilder.sqlJoins))
                        sb.AppendLine(sqlBuilder.sqlJoins);
                    break;
                case SqlSentence.DELETE:
                    sb.Append("DELETE FROM ").AppendLine(sqlBuilder.sqlTables);

                    if (!String.IsNullOrEmpty(sqlBuilder.sqlWhere))
                        sb.Append(" WHERE ").Append(sqlBuilder.sqlWhere);

                    if (!String.IsNullOrEmpty(sqlBuilder.sqlJoins))
                        sb.AppendLine(sqlBuilder.sqlJoins);
                    break;

            }
            return sb.ToString().Trim() + ";";
        }
        public string GetOperator(SqlConditions enumerator)
        {
            switch (enumerator)
            {
                case SqlConditions.SEPARATOR:
                    return this.StrcolumnSeparator;
                case SqlConditions.NONE:
                    return String.Empty;
                default:
                    return " " + enumerator.ToString().Replace('_', ' ') + " ";
            }
        }
        public string GetOperator(SqlJoin enumerator)
        {
            return " " + enumerator.ToString().Replace('_', ' ') + " ";
        }
        public string GetOperator(SqlOrder enumerator)
        {
            return " " + enumerator.ToString().Replace('_', ' ') + " ";
        }
        public string GetOperator(ExpressionType enumerator)
        {
            var ops = new Dictionary<ExpressionType, String>();
            ops.Add(ExpressionType.Equal, "=");
            ops.Add(ExpressionType.NotEqual, "<>");
            ops.Add(ExpressionType.GreaterThan, ">");
            ops.Add(ExpressionType.GreaterThanOrEqual, ">=");
            ops.Add(ExpressionType.LessThan, "<");
            ops.Add(ExpressionType.LessThanOrEqual, "<=");
            ops.Add(ExpressionType.And, " AND ");
            ops.Add(ExpressionType.AndAlso, " AND ");
            ops.Add(ExpressionType.Or, " OR ");
            ops.Add(ExpressionType.OrElse, " OR ");

            return ops[enumerator];
        }
        public bool IsIgnore(PropertyInfo property)
        {
            //Type valType = property.PropertyType.IsGenericType ? property.PropertyType.GetGenericTypeDefinition() : property.PropertyType;
            //if (valType == typeof(object)) valType = property.PropertyType;

            //bool i = (valType.IsSubclassOf(typeof(EntityReference)) || valType == typeof(EntityCollection<>) || valType.IsSubclassOf(typeof(EntityObject)));
            //return i;
            return false;
        }
    }
}