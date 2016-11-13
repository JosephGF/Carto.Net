using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetCartoDB.SQL.Linq
{
    /// <summary>
    /// Permite crear querys basadas en lambda
    /// </summary>
    /// <typeparam name="TEntity">Entidad sobre la que se realizarán consultas</typeparam>
    public partial class SqlBuilderLambda<TEntity> : SqlBuilder where TEntity : class, new()
    {
        private string TableName { get { return this.GetTableName(); } }

        /// <summary>
        /// Crea una instancia de QueryBuilderLambda
        /// </summary>
        public SqlBuilderLambda()
        {
            this.initialize();
        }

        /// <summary>
        /// Crea una instancia de QueryBuilderLambda aplicandole una condición
        /// </summary>
        /// <param name="where">Condición que se aplicará a la consulta</param>
        public SqlBuilderLambda(Expression<Func<TEntity, bool>> where)
        {
            this.initialize();
            this.Where(where);
        }

        protected override void initialize()
        {
            base.initialize();
            this.sqlTables = String.Format(this.SQLParser.StrDBEncapsule, this.TableName);
            this.sqlcolumns = sqlTables + ".*";
        }

        #region LAMBDA PARSER

        private string LambdaExpresionTranslate(Expression<Func<TEntity, bool>> predicate)
        {
            return ExpressionValue(predicate, this.SQLParser.StrWHERELamda) as string;
        }

        private string LambdaExpresionTranslate<TEntityAux>(Expression<Func<TEntityAux, object>> predicate) where TEntityAux : class
        {
            return ExpressionValue(predicate, this.SQLParser.StrWHERELamda) as string;
        }

        private string LambdaExpresionTranslate<TEntityAux>(Expression<Func<TEntityAux, bool>> predicate) where TEntityAux : class
        {
            return ExpressionValue(predicate, this.SQLParser.StrWHERELamda) as string;
        }

        private string LambdaExpresionTranslate<TEntityRelationship>(Expression<Func<TEntity, TEntityRelationship, bool>> predicate) where TEntityRelationship : class
        {
            return ExpressionValue(predicate, this.SQLParser.StrWHERELamda) as string;
        }

        private string LambdaExpresionTranslate<TEntityRelationship1, TEntityRelationship2>(Expression<Func<TEntity, TEntityRelationship1, TEntityRelationship2, bool>> predicate)
            where TEntityRelationship1 : class
            where TEntityRelationship2 : class
        {
            return ExpressionValue(predicate, this.SQLParser.StrWHERELamda) as string;
        }

        private string LambdaExpresionTranslate<TEntityRelationship1, TEntityRelationship2, TEntityRelationship3>(Expression<Func<TEntity, TEntityRelationship1, TEntityRelationship2, TEntityRelationship3, bool>> predicate)
            where TEntityRelationship1 : class
            where TEntityRelationship2 : class
            where TEntityRelationship3 : class
        {
            return ExpressionValue(predicate, this.SQLParser.StrWHERELamda) as string;
        }

        private object ExpressionValue(Expression expression)
        {
            return ExpressionValue(expression, null);
        }

        private object ExpressionValue(Expression expression, string formatText)
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

                object rightVal = this.ExpressionValue(right, formatText);
                object leftVal = this.ExpressionValue(left, formatText);

                if (rightVal == null && operation.NodeType == ExpressionType.Equal)
                    formatText = this.SQLParser.StrWHERENull;

                if (leftVal == null)
                    throw new SqlArgsErrorException("Left lambda binary expression can't be null", this);

                value = String.Format(formatText, leftVal, this.SQLParser.GetOperator(operation.NodeType), rightVal);
            }
            else if (expression is MemberExpression)
            {
                var expr = expression as MemberExpression;
                try
                {
                    value = this.GetExpressionValue(expr);
                    value = this.AddSqlArgs(value);
                }
                catch (InvalidOperationException ex)
                {
                    string tblName = SqlAttributes.Manager.GetTableName(expr.Member.DeclaringType);
                    string colName = SqlAttributes.Manager.GetColumnName(expr.Member.DeclaringType, expr.Member.Name);
                    value = base.GetColumnNameSQL(colName, tblName);
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
                value = this.AddSqlArgs(expr.Value);
            }
            else if (expression is MethodCallExpression)
            {
                object col = null;
                var expr = expression as MethodCallExpression;
                string fSql = this.SQLParser.MethodToSql(expr.Method, expr.Arguments[0].Type);
                if (expr.Object != null)
                    col = this.ExpressionValue(expr.Object).ToString();
                else if (expr.Arguments.Count > 1)
                    col = this.ExpressionValue(expr.Arguments[1], formatText);
                else
                    col = 0;

                object val = this.ExpressionValue(expr.Arguments[0], formatText);
                value = String.Format(fSql, col, val);
            }
            //else if(expression is ?)

            return value;
        }

        private object GetExpressionValue(Expression expression)
        {
            object value = null;
            if (expression is MemberExpression)
            {
                MemberExpression expr = expression as MemberExpression;

                var objectMember = Expression.Convert(expr, typeof(object));
                var getterLambda = Expression.Lambda<Func<object>>(objectMember);
                var getter = getterLambda.Compile();
                value = getter();
            }

            return value;
        }

        #endregion
        /// <summary>
        /// Realiza un "SELECT * "
        /// </summary>
        /// <remarks>
        /// NOTE: Sobreescribe las columnas anteriores
        ///       Es el valor por defecto si no se especifica ninguno
        /// </remarks>
        /// <returns>Instancia actual de la clase SQLBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> Select()
        {
            string columns = GetAllColumnsString<TEntity>();
            base.Select(columns);
            return this;
        }

        /// <summary>
        /// Realiza un "SELECT [TABLE].* "
        /// </summary>
        /// <typeparam name="TEntityRelationship">Tabla de relación</typeparam>
        /// <returns>Instancia actual de la clase SQLBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> Select<TEntityRelationship>() where TEntityRelationship : class
        {
            string columns = GetAllColumnsString<TEntityRelationship>();

            base.Select(columns);
            return this;
        }

        /// <summary>
        /// Especifica las columnas de la clausula select
        /// 
        /// NOTE: Sobreescribe las columnas anteriores
        /// </summary>
        /// <param name="columns">Cadena que representa la/s columna/s seleccionadas</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        new public SqlBuilderLambda<TEntity> Select(string columns)
        {
            base.Select(columns);
            return this;
        }

        /// <summary>
        /// Especifica la columna de la clausula select
        /// 
        /// NOTE: Sobreescribe las columnas anteriores
        /// </summary>
        /// <param name="column">Representa la columna a seleccionar</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> Select(Expression<Func<TEntity, Object>> column)
        {
            string col = this.ExpressionValue(column) as string;
            return this.Select(col);
        }

        /// <summary>
        /// Especifica la columna de la clausula select
        /// 
        /// NOTE: Sobreescribe las columnas anteriores
        /// </summary>
        /// <param name="column">Representa la columna a seleccionar</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> SelectThem(Expression<Func<TEntity, Object>> column)
        {
            string col = this.ExpressionValue(column) as string;
            base.SelectThem(col);
            return this;
        }

        /// <summary>
        /// Especifica la columna de la clausula select
        /// 
        /// NOTE: Sobreescribe las columnas anteriores
        /// </summary>
        /// <param name="column">Representa la columna a seleccionar</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> SelectThem(Expression<Func<TEntity, Object>> column1, Expression<Func<TEntity, Object>> column2)
        {
            return this.SelectThem(column1)
                       .SelectThem(column2);
        }

        /// <summary>
        /// Especifica la columna de la clausula select
        /// 
        /// NOTE: Sobreescribe las columnas anteriores
        /// </summary>
        /// <param name="column">Representa la columna a seleccionar</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> SelectThem(Expression<Func<TEntity, Object>> column1, Expression<Func<TEntity, Object>> column2, Expression<Func<TEntity, Object>> column3)
        {
            return this.SelectThem(column1)
                       .SelectThem(column2)
                       .SelectThem(column3);
        }

        /// <summary>
        /// Especifica la columna de la clausula select
        /// 
        /// NOTE: Sobreescribe las columnas anteriores
        /// </summary>
        /// <param name="column">Representa la columna a seleccionar</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> SelectThem(Expression<Func<TEntity, Object>> column1, Expression<Func<TEntity, Object>> column2, Expression<Func<TEntity, Object>> column3, Expression<Func<TEntity, Object>> column4)
        {
            return this.SelectThem(column1)
                       .SelectThem(column2)
                       .SelectThem(column3)
                       .SelectThem(column4);
        }

        /// <summary>
        /// Especifica la columna de la clausula select
        /// 
        /// NOTE: Sobreescribe las columnas anteriores
        /// </summary>
        /// <param name="column">Representa la columna a seleccionar</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> SelectThem(Expression<Func<TEntity, Object>> column1, Expression<Func<TEntity, Object>> column2, Expression<Func<TEntity, Object>> column3, Expression<Func<TEntity, Object>> column4, Expression<Func<TEntity, Object>> column5)
        {
            return this.SelectThem(column1)
                       .SelectThem(column2)
                       .SelectThem(column3)
                       .SelectThem(column4)
                       .SelectThem(column5);
        }


        /// <summary>
        /// Especifica la columna de la clausula select
        /// 
        /// NOTE: Sobreescribe las columnas anteriores
        /// </summary>
        /// <param name="column">Representa la columna a seleccionar</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> SelectThem(Expression<Func<IEnumerable<TEntity>, Object>> column, string alias)
        {
            string col = this.ExpressionValue(column) as string;
            base.SelectThem(col, alias);
            return this;
        }


        /// <summary>
        /// Especifica la columna de la clausula select
        /// 
        /// NOTE: Sobreescribe las columnas anteriores
        /// </summary>
        /// <param name="column">Representa la columna a seleccionar</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> SelectThem<TEntityRelationship>(Expression<Func<TEntityRelationship, Object>> column) where TEntityRelationship : class
        {
            string col = this.ExpressionValue(column) as string;
            base.SelectThem(col);
            return this;
        }

        /// <summary>
        /// Especifica la columna de la clausula select
        /// 
        /// NOTE: Sobreescribe las columnas anteriores
        /// </summary>
        /// <param name="column">Representa la columna a seleccionar</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> SelectThem<TEntityRelationship>(Expression<Func<TEntityRelationship, Object>> column1, Expression<Func<TEntityRelationship, Object>> column2)
            where TEntityRelationship : class
        {
            return this.SelectThem(column1)
                       .SelectThem(column2);
        }

        /// <summary>
        /// Especifica la columna de la clausula select
        /// 
        /// NOTE: Sobreescribe las columnas anteriores
        /// </summary>
        /// <param name="column">Representa la columna a seleccionar</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> SelectThem<TEntityRelationship>(Expression<Func<TEntityRelationship, Object>> column1, Expression<Func<TEntityRelationship, Object>> column2, Expression<Func<TEntityRelationship, Object>> column3)
            where TEntityRelationship : class
        {
            return this.SelectThem(column1)
                       .SelectThem(column2)
                       .SelectThem(column3);
        }

        /// <summary>
        /// Especifica la columna de la clausula select
        /// 
        /// NOTE: Sobreescribe las columnas anteriores
        /// </summary>
        /// <param name="column">Representa la columna a seleccionar</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> SelectThem<TEntityRelationship>(Expression<Func<TEntityRelationship, Object>> column1, Expression<Func<TEntityRelationship, Object>> column2, Expression<Func<TEntityRelationship, Object>> column3, Expression<Func<TEntityRelationship, Object>> column4)
            where TEntityRelationship : class
        {
            return this.SelectThem(column1)
                       .SelectThem(column2)
                       .SelectThem(column3)
                       .SelectThem(column4);
        }

        /// <summary>
        /// Especifica la columna de la clausula select
        /// 
        /// NOTE: Sobreescribe las columnas anteriores
        /// </summary>
        /// <param name="column">Representa la columna a seleccionar</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> SelectThem<TEntityRelationship>(Expression<Func<TEntityRelationship, Object>> column1, Expression<Func<TEntityRelationship, Object>> column2, Expression<Func<TEntityRelationship, Object>> column3, Expression<Func<TEntityRelationship, Object>> column4, Expression<Func<TEntityRelationship, Object>> column5)
            where TEntityRelationship : class
        {
            return this.SelectThem(column1)
                       .SelectThem(column2)
                       .SelectThem(column3)
                       .SelectThem(column4)
                       .SelectThem(column5);
        }

        /// <summary>
        /// Especifica si se descartarán las filas repetidas
        /// </summary>
        /// <param name="distinct">establecer clausula distinct</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        new public SqlBuilderLambda<TEntity> Distinct(bool distinct = true)
        {
            base.Distinct(distinct);
            return this;
        }

        /// <summary>
        /// Especifica el nombre de la tabla
        /// 
        /// NOTE: Sobreescribe la tabla anterior
        /// </summary>
        /// <param name="table">Nombre de la tabla</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        new public SqlBuilderLambda<TEntity> From(string table)
        {
            base.From(table);
            return this;
        }

        /// <summary>
        /// Especifica el nombre de la tabla
        /// 
        /// NOTE: Sobreescribe las tablas anteriores
        /// </summary>
        /// <param name="tables">Nombre de las tablas</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        new public SqlBuilderLambda<TEntity> From(IEnumerable<string> tables)
        {
            base.From(tables);
            return this;
        }

        #region SQL CONDITIONS
        /// <summary>
        /// Crea la condición a la consulta
        /// 
        /// NOTE: Sobreescribe las condiciones anteriores
        /// </summary>
        /// <param name="where">Condición para filtrar los registros</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> Where(Expression<Func<TEntity, bool>> where)
        {
            this.Where(LambdaExpresionTranslate(where));
            return this;
        }

        /// <summary>
        /// Crea la condición a la consulta
        /// 
        /// NOTE: Sobreescribe las condiciones anteriores
        /// </summary>
        /// <typeparam name="TEntityRelationship">Entidad relacionada</typeparam>
        /// <param name="where">Condición para filtrar los registros</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> Where<TEntityRelationship>(Expression<Func<TEntity, TEntityRelationship, bool>> where) where TEntityRelationship : class
        {
            this.Where(LambdaExpresionTranslate(where));
            return this.FromThen<TEntityRelationship>();
        }

        /// <summary>
        /// Crea la condición a la consulta
        /// 
        /// NOTE: Sobreescribe las condiciones anteriores
        /// </summary>
        /// <typeparam name="TEntityRelationship1">Entidaad que representa a la tabla 1</typeparam>
        /// <typeparam name="TEntityRelationship2">Entidaad que representa a la tabla 2</typeparam>
        /// <param name="where">Condición para filtrar los registros</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> Where<TEntityRelationship1, TEntityRelationship2>(Expression<Func<TEntity, TEntityRelationship1, TEntityRelationship2, bool>> where)
            where TEntityRelationship1 : class
            where TEntityRelationship2 : class
        {
            this.Where(LambdaExpresionTranslate(where));
            return this.FromThen<TEntityRelationship1>()
                       .FromThen<TEntityRelationship2>();
        }

        /// <summary>
        /// Crea la condición a la consulta
        /// 
        /// NOTE: Sobreescribe las condiciones anteriores
        /// </summary>
        /// <typeparam name="TEntityRelationship1">Entidaad que representa a la tabla 1</typeparam>
        /// <typeparam name="TEntityRelationship2">Entidaad que representa a la tabla 2</typeparam>
        /// <typeparam name="TEntityRelationship3">Entidaad que representa a la tabla 3</typeparam>
        /// <param name="where">Condición para filtrar los registros</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> Where<TEntityRelationship1, TEntityRelationship2, TEntityRelationship3>(Expression<Func<TEntity, TEntityRelationship1, TEntityRelationship2, TEntityRelationship3, bool>> where)
            where TEntityRelationship1 : class
            where TEntityRelationship2 : class
            where TEntityRelationship3 : class
        {
            this.Where(LambdaExpresionTranslate(where));
            return this.FromThen<TEntityRelationship1>()
                       .FromThen<TEntityRelationship2>()
                       .FromThen<TEntityRelationship3>();
        }

        /// <summary>
        /// Añade una tabla a la consulta
        /// </summary>
        /// <typeparam name="TEntityFrom">Nueva Entidad</typeparam>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> FromThen<TEntityFrom>() where TEntityFrom : class
        {
            string table = String.Format(this.SQLParser.StrDBEncapsule, SqlAttributes.Manager.GetTableName<TEntityFrom>());
            base.FromThen(table);
            return this;
        }

        /// <summary>
        /// Añade una tabla a la consulta
        /// </summary>
        /// <param name="tables">Nombre de las tablas</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        new public SqlBuilderLambda<TEntity> FromThen(string table)
        {
            base.FromThen(table);
            return this;
        }

        /// <summary>
        /// Crea la condición having al groupby de la consulta
        /// </summary>
        /// <param name="having">Condición para filtrar el having</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> Having(Expression<Func<IEnumerable<TEntity>, Object>> having)
        {
            //string strcolumn = this.ExpressionValue(column) as string;
            this.Having(this.LambdaExpresionTranslate(having));
            return this;
        }

        /// <summary>
        /// Agrega una condicion AND al los filtros
        /// </summary>
        /// <param name="where">Condición que se añadirá</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> And(Expression<Func<TEntity, bool>> where)
        {
            string condition = LambdaExpresionTranslate(where);
            this.AppendCondition(condition, SqlConditions.AND);
            return this;
        }

        /// <summary>
        /// Agrega una condicion AND al los filtros
        /// </summary>
        /// <param name="where">Condición que se añadirá</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        //public SqlBuilderLambda<TEntity> And<TEntityRelationship>(Expression<Func<TEntity, TEntityRelationship, bool>> where) where TEntityRelationship : class
        //{
        //    string condition = LambdaExpresionTranslate<TEntityRelationship>(where);
        //    this.AppendCondition(condition, SqlConditions.AND);
        //    return this;
        //}

        /// <summary>
        /// Agrega una condicion AND al los filtros
        /// </summary>
        /// <typeparam name="TEntityRelationship1">Entidaad que representa a la tabla 1</typeparam>
        /// <typeparam name="TEntityRelationship2">Entidaad que representa a la tabla 2</typeparam>
        /// <param name="where">Condición que se añadirá</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> And<TEntityRelationship1, TEntityRelationship2>(Expression<Func<TEntity, TEntityRelationship1, TEntityRelationship2, bool>> where)
            where TEntityRelationship1 : class
            where TEntityRelationship2 : class
        {
            string condition = LambdaExpresionTranslate(where);
            this.AppendCondition(condition, SqlConditions.AND);
            return this;
        }

        /// <summary>
        /// Agrega una condicion AND al los filtros
        /// </summary>
        /// <typeparam name="TEntityRelationship1">Entidaad que representa a la tabla 1</typeparam>
        /// <typeparam name="TEntityRelationship2">Entidaad que representa a la tabla 2</typeparam>
        /// <typeparam name="TEntityRelationship3">Entidaad que representa a la tabla 3</typeparam>
        /// <param name="where">Condición que se añadirá</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> And<TEntityRelationship1, TEntityRelationship2, TEntityRelationship3>(Expression<Func<TEntity, TEntityRelationship1, TEntityRelationship2, TEntityRelationship3, bool>> where)
            where TEntityRelationship1 : class
            where TEntityRelationship2 : class
            where TEntityRelationship3 : class
        {
            string condition = LambdaExpresionTranslate(where);
            this.AppendCondition(condition, SqlConditions.AND);
            return this;
        }

        /// <summary>
        /// Agrega una condicion AND al los filtros
        /// </summary>
        /// <param name="where">Condición que se añadirá</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> And<TEntityRelationship>(Expression<Func<TEntity, TEntityRelationship, bool>> where) where TEntityRelationship : class
        {
            string condition = LambdaExpresionTranslate(where);
            this.AppendCondition(condition, SqlConditions.AND);
            return this;
        }

        /// <summary>
        /// Agrega una condicion AND NOT al los filtros
        /// </summary>
        /// <param name="where">Condición que se añadirá</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> AndNot(Expression<Func<TEntity, bool>> where)
        {
            string condition = LambdaExpresionTranslate(where);
            this.AppendCondition(condition, SqlConditions.AND_NOT);
            return this;
        }

        /// <summary>
        /// Agrega una condicion AND NOT al los filtros
        /// </summary>
        /// <typeparam name="TEntityRelationship">Entidaad que representa a la tabla 1</typeparam>
        /// <param name="where">Condición que se añadirá</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> AndNot<TEntityRelationship>(Expression<Func<TEntity, TEntityRelationship, bool>> where)
            where TEntityRelationship : class
        {
            string condition = LambdaExpresionTranslate(where);
            this.AppendCondition(condition, SqlConditions.AND_NOT);
            return this;
        }

        /// <summary>
        /// Agrega una condicion AND NOT al los filtros
        /// </summary>
        /// <typeparam name="TEntityRelationship1">Entidaad que representa a la tabla 1</typeparam>
        /// <typeparam name="TEntityRelationship2">Entidaad que representa a la tabla 2</typeparam>
        /// <param name="where">Condición que se añadirá</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> AndNot<TEntityRelationship1, TEntityRelationship2>(Expression<Func<TEntity, TEntityRelationship1, TEntityRelationship2, bool>> where)
            where TEntityRelationship1 : class
            where TEntityRelationship2 : class
        {
            string condition = LambdaExpresionTranslate(where);
            this.AppendCondition(condition, SqlConditions.AND_NOT);
            return this;
        }

        /// <summary>
        /// Agrega una condicion AND NOT al los filtros
        /// </summary>
        /// <typeparam name="TEntityRelationship1">Entidaad que representa a la tabla 1</typeparam>
        /// <typeparam name="TEntityRelationship2">Entidaad que representa a la tabla 2</typeparam>
        /// <typeparam name="TEntityRelationship3">Entidaad que representa a la tabla 3</typeparam>
        /// <param name="where">Condición que se añadirá</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> AndNot<TEntityRelationship1, TEntityRelationship2, TEntityRelationship3>(Expression<Func<TEntity, TEntityRelationship1, TEntityRelationship2, TEntityRelationship3, bool>> where)
            where TEntityRelationship1 : class
            where TEntityRelationship2 : class
            where TEntityRelationship3 : class
        {
            string condition = LambdaExpresionTranslate(where);
            this.AppendCondition(condition, SqlConditions.AND_NOT);
            return this;
        }

        /// <summary>
        /// Agrega una condicion OR al los filtros
        /// </summary>
        /// <param name="where">Condición que se añadirá</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> Or(Expression<Func<TEntity, bool>> where)
        {
            string condition = LambdaExpresionTranslate(where);
            this.AppendCondition(condition, SqlConditions.OR);
            return this;
        }

        /// <summary>
        /// Agrega una condicion OR al los filtros
        /// </summary>
        /// <param name="where">Condición que se añadirá</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> Or<TEntityRelationship>(Expression<Func<TEntity, TEntityRelationship, bool>> where) where TEntityRelationship : class
        {
            string orStr = LambdaExpresionTranslate(where);
            base.Or(orStr);
            return this;
        }

        /// <summary>
        /// Agrega una condicion OR al los filtros
        /// </summary>
        /// <typeparam name="TEntityRelationship1">Entidaad que representa a la tabla 1</typeparam>
        /// <typeparam name="TEntityRelationship2">Entidaad que representa a la tabla 2</typeparam>
        /// <param name="where">Condición que se añadirá</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> Or<TEntityRelationship1, TEntityRelationship2>(Expression<Func<TEntity, TEntityRelationship1, TEntityRelationship2, bool>> where)
            where TEntityRelationship1 : class
            where TEntityRelationship2 : class
        {
            string orStr = LambdaExpresionTranslate(where);
            base.Or(orStr);
            return this;
        }

        /// <summary>
        /// Agrega una condicion OR al los filtros
        /// </summary>
        /// <typeparam name="TEntityRelationship1">Entidaad que representa a la tabla 1</typeparam>
        /// <typeparam name="TEntityRelationship2">Entidaad que representa a la tabla 2</typeparam>
        /// <typeparam name="TEntityRelationship3">Entidaad que representa a la tabla 3</typeparam>
        /// <param name="where">Condición que se añadirá</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> Or<TEntityRelationship1, TEntityRelationship2, TEntityRelationship3>(Expression<Func<TEntity, TEntityRelationship1, TEntityRelationship2, TEntityRelationship3, bool>> where)
            where TEntityRelationship1 : class
            where TEntityRelationship2 : class
            where TEntityRelationship3 : class
        {
            string orStr = LambdaExpresionTranslate(where);
            base.Or(orStr);
            return this;
        }

        /// <summary>
        /// Agrega una condicion OR NOT al los filtros
        /// </summary>
        /// <param name="where">Condición que se añadirá</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> OrNot(Expression<Func<TEntity, bool>> where)
        {
            string condition = LambdaExpresionTranslate(where);
            this.AppendCondition(condition, SqlConditions.OR_NOT);
            return this;
        }


        /// <summary>
        /// Agrega una condicion OR NOT al los filtros
        /// </summary>
        /// <typeparam name="TEntityRelationship">Entidaad que representa a la tabla 1</typeparam>
        /// <param name="where">Condición que se añadirá</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> OrNot<TEntityRelationship>(Expression<Func<TEntity, TEntityRelationship, bool>> where) where TEntityRelationship : class
        {
            string condition = LambdaExpresionTranslate(where);
            this.AppendCondition(condition, SqlConditions.OR_NOT);
            return this;
        }


        /// <summary>
        /// Agrega una condicion OR NOT al los filtros
        /// </summary>
        /// <typeparam name="TEntityRelationship1">Entidaad que representa a la tabla 1</typeparam>
        /// <typeparam name="TEntityRelationship2">Entidaad que representa a la tabla 2</typeparam>
        /// <param name="where">Condición que se añadirá</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> OrNot<TEntityRelationship1, TEntityRelationship2>(Expression<Func<TEntity, TEntityRelationship1, TEntityRelationship2, bool>> where)
            where TEntityRelationship1 : class
            where TEntityRelationship2 : class
        {
            string condition = LambdaExpresionTranslate(where);
            this.AppendCondition(condition, SqlConditions.OR_NOT);
            return this;
        }


        /// <summary>
        /// Agrega una condicion OR NOT al los filtros
        /// </summary>
        /// <typeparam name="TEntityRelationship1">Entidaad que representa a la tabla 1</typeparam>
        /// <typeparam name="TEntityRelationship2">Entidaad que representa a la tabla 2</typeparam>
        /// <typeparam name="TEntityRelationship3">Entidaad que representa a la tabla 3</typeparam>
        /// <param name="where">Condición que se añadirá</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> OrNot<TEntityRelationship1, TEntityRelationship2, TEntityRelationship3>(Expression<Func<TEntity, TEntityRelationship1, TEntityRelationship2, TEntityRelationship3, bool>> where)
            where TEntityRelationship1 : class
            where TEntityRelationship2 : class
            where TEntityRelationship3 : class
        {
            string condition = LambdaExpresionTranslate(where);
            this.AppendCondition(condition, SqlConditions.OR_NOT);
            return this;
        }


        /// <summary>
        /// Añade una condición BETWEEN a las condiciones de la consulta
        /// </summary>
        /// <param name="column">columna a comparar </param>
        /// <param name="first">Condición desde</param>
        /// <param name="second">Condición hasta</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> Between(Expression<Func<TEntity, Object>> column, object first, object second)
        {
            return this.Between(column, first, second, SqlConditions.AND);
        }

        /// <summary>
        /// Añade una condición BETWEEN a las condiciones de la consulta
        /// </summary>
        /// <param name="column">columna a comparar </param>
        /// <param name="first">Condición desde</param>
        /// <param name="second">Condición hasta</param>
        /// <param name="condition">Tipo de condición aplicada</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> Between(Expression<Func<TEntity, Object>> column, object first, object second, SqlConditions condition)
        {
            string strcolumn = this.ExpressionValue(column) as string;
            this.Between(strcolumn, first, second, condition);
            return this;
        }

        /// <summary>
        /// Añade una condición IN a las condiciones de la consulta
        /// </summary>
        /// <param name="column">columna a comparar</param>
        /// <param name="values">Valores validos</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> In(Expression<Func<TEntity, Object>> column, IEnumerable<object> values)
        {
            return In(column, values, SqlConditions.AND);
        }

        /// <summary>
        /// Añade una condición IN a las condiciones de la consulta
        /// </summary>
        /// <param name="column">columna a comparar</param>
        /// <param name="values">Valores validos</param>
        /// <param name="condition">Tipo de condición aplicada</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> In(Expression<Func<TEntity, Object>> column, IEnumerable<object> values, SqlConditions condition)
        {
            string strcolumn = this.ExpressionValue(column) as string;
            this.In(strcolumn, values, condition);
            return this;
        }

        /// <summary>
        /// Añade una condición IN a las condiciones de la consulta
        /// </summary>
        /// <typeparam name="TEntityRelation">Tabla relación sob</typeparam>
        /// <param name="column"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public SqlBuilderLambda<TEntity> In<TEntityRelation>(Expression<Func<TEntityRelation, Object>> column, Expression<Func<TEntity, TEntityRelation, bool>> where) where TEntityRelation : class
        {
            throw new NotImplementedException("Sin hacer");
            string strcolumn = this.ExpressionValue(column) as string;
            string condition = this.LambdaExpresionTranslate(where);
            return this;
        }
        #endregion

        /// <summary>
        /// Añade un join en la consulta
        /// </summary>
        /// <typeparam name="TEntityJoin">Entidad del join</typeparam>
        /// <param name="on">Condiciones del join</param>
        /// <param name="join">Tipo de join que se realiza</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> Join<TEntityJoin>(Expression<Func<TEntity, TEntityJoin, bool>> on, SqlJoin join) where TEntityJoin : class
        {
            string table = String.Format(this.SQLParser.StrDBEncapsule, typeof(TEntityJoin).Name);
            string condition = this.LambdaExpresionTranslate(on);
            this.Join(table, condition, join);
            return this;
        }

        /// <summary>
        /// Agrega un LEFT JOIN / LEFT OUTER JOIN en la consulta
        /// </summary>
        /// <typeparam name="TEntityJoin">Entidad del join</typeparam>
        /// <param name="on">Condiciones del JOIN</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> LeftJoin<TEntityJoin>(Expression<Func<TEntity, TEntityJoin, bool>> on) where TEntityJoin : class
        {
            return this.Join<TEntityJoin>(on, SqlJoin.LEFT_JOIN);
        }

        /// <summary>
        /// Agrega un RIGHT JOIN / RIGHT OUTER JOIN en la consulta
        /// </summary>
        /// <typeparam name="TEntityJoin">Entidad del join</typeparam>
        /// <param name="on">Condiciones del JOIN</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> RightJoin<TEntityJoin>(Expression<Func<TEntity, TEntityJoin, bool>> on) where TEntityJoin : class
        {
            return this.Join<TEntityJoin>(on, SqlJoin.RIGHT_JOIN);
        }

        /// <summary>
        /// Agrega un NATURAL JOIN en la consulta
        /// </summary>
        /// <typeparam name="TEntityJoin">Entidad del join</typeparam>
        /// <param name="on">Condiciones del join</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> NaturalJoin<TEntityJoin>(Expression<Func<TEntity, TEntityJoin, bool>> on) where TEntityJoin : class
        {
            return this.Join<TEntityJoin>(on, SqlJoin.NATURAL_JOIN);
        }

        /// <summary>
        /// Agrega un INNER JOIN en la consulta
        /// </summary>
        /// <typeparam name="TEntityJoin">Entidad del join</typeparam>
        /// <param name="on">Condiciones del join</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> InnerJoin<TEntityJoin>(Expression<Func<TEntity, TEntityJoin, bool>> on) where TEntityJoin : class
        {
            return this.Join<TEntityJoin>(on, SqlJoin.INNER_JOIN);
        }

        /// <summary>
        /// Añade un orden ascendente a la consulta
        /// 
        /// NOTE: Sobreescribe los anteriores
        /// </summary>
        /// <param name="orderBy">Propiedad sobre la que se ordenará</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> OrderBy(Expression<Func<TEntity, Object>> orderBy)
        {
            this.sqlOrderBy = this.ExpressionValue(orderBy) as string + " " + this.SQLParser.GetOperator(SqlOrder.ASC);
            return this;
        }

        /// <summary>
        /// Añade un orden descendente a la consulta 
        /// 
        /// NOTE: Sobreescribe los anteriores
        /// </summary>
        /// <param name="orderBy">Propiedad sobre la que se ordenará</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> OrderByDescending(Expression<Func<TEntity, Object>> orderBy)
        {
            this.sqlOrderBy = this.ExpressionValue(orderBy) as string + " " + this.SQLParser.GetOperator(SqlOrder.DESC);
            return this;
        }

        /// <summary>
        /// Añade nuevas propiedades al orden actual de la consulta de forma ascendente 
        /// 
        /// NOTE: Se debe llamar primero a la función "OrderBy"
        /// </summary>
        /// <param name="orderBy">Propiedad sobre la que se ordenará</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> OrderThenBy(Expression<Func<TEntity, Object>> orderBy)
        {
            if (string.IsNullOrEmpty(this.sqlOrderBy))
                throw new SqlNoPermittedException("Only can be called after that OrderBy function.", this);

            this.sqlOrderBy += this.SQLParser.StrORDERBYSeparator + this.ExpressionValue(orderBy) as string + " " + this.SQLParser.GetOperator(SqlOrder.ASC);
            return this;
        }

        /// <summary>
        /// Añade nuevas propiedades al orden actual de la consulta de forma descendente 
        /// 
        /// NOTE: Se debe llamar primero a la función "OrderBy"
        /// </summary>
        /// <param name="orderBy">Propiedad sobre la que se ordenará</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> OrderThenByDescending(Expression<Func<TEntity, Object>> orderBy)
        {
            if (string.IsNullOrEmpty(this.sqlOrderBy))
                throw new SqlNoPermittedException("Only can be called after that OrderBy function.", this);

            this.sqlOrderBy += this.SQLParser.StrORDERBYSeparator + this.ExpressionValue(orderBy) as string + " " + this.SQLParser.GetOperator(SqlOrder.DESC);
            return this;
        }

        /// <summary>
        /// Añade una agrupación a la consuta
        /// </summary>
        /// <param name="groupBy">Propiedad sobre la que se agrupará</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> GroupBy(Expression<Func<TEntity, Object>> groupBy)
        {
            this.sqlGroupBy = this.ExpressionValue(groupBy) as string;
            return this;
        }

        /// <summary>
        /// Añade nuevas propiedades al orden actual de la consulta de forma ascendente 
        /// 
        /// NOTE: Se debe llamar primero a la función "OrderBy"
        /// </summary>
        /// <param name="groupBy">Propiedad sobre la que se ordenará</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> GroupThenBy(Expression<Func<TEntity, Object>> groupBy)
        {
            if (string.IsNullOrEmpty(this.sqlGroupBy))
                throw new SqlNoPermittedException("Only can be called after that GroupBy function", this);

            this.sqlGroupBy += this.SQLParser.StrGROPUPBYSeparator + this.ExpressionValue(groupBy) as string;
            return this;
        }

        /// <summary>
        /// Genera el estamento SQL para un insert
        /// </summary>
        /// <param name="data">Datos a insertar</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> Insert(TEntity data)
        {
            this.Insert(data as Object);
            return this;
        }

        /// <summary>
        /// Genera el estamento SQL para un insert
        /// </summary>
        /// <param name="data">Datos a insertar</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> Insert(TEntity data, bool ignoreNull)
        {
            this.Insert(data as Object, ignoreNull);
            return this;
        }

        /// <summary>
        /// Genera el estamento SQL para un insert
        /// </summary>
        /// <param name="data">Datos a insertar</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> Insert(Object data, bool ignoreNull)
        {
            this.Insert<TEntity>(data, ignoreNull);
            return this;
        }

        /// <summary>
        /// Genera el estamento SQL para un insert
        /// </summary>
        /// <param name="data">Datos a insertar</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> Insert(Object data)
        {
            this.Insert<TEntity>(data);
            return this;
        }

        /// <summary>
        /// Genera el estamento SQL para un update
        /// </summary>
        /// <param name="data">Datos a actualizar</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> Update(TEntity data)
        {
            this.Update(data as Object);
            return this;
        }

        /// <summary>
        /// Genera el estamento SQL para un UPDATE
        /// </summary>
        /// <param name="data">Datos a actualizar</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> Update(Object data)
        {
            base.Update(this.TableName, data);
            return this;
        }

        /// <summary>
        /// Genera el estamento SET para un UPDATE
        /// </summary>
        /// <param name="data">Datos a actualizar</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> Set(Expression<Func<TEntity, bool>> set)
        {
            this.Set(LambdaExpresionTranslate(set));
            return this;
        }

        /// <summary>
        /// Genera el estamento SQL para un UPDATE
        /// </summary>
        /// <param name="data">Datos a actualizar</param>
        /// <param name="where">Condición para filtrar los registros</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> Update(TEntity data, Expression<Func<TEntity, bool>> where)
        {
            this.Update(data);
            return this.Where(where);
        }

        /// <summary>
        /// Genera el estamento SQL para un UPDATE
        /// </summary>
        /// <param name="data">Datos a actualizar</param>
        /// <param name="where">Condición para filtrar los registros</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> Update(Object data, Expression<Func<TEntity, bool>> where)
        {
            this.Update(data);
            return this.Where(where);
        }

        /// <summary>
        /// Genera el estamento SQL para elimminar registros
        /// </summary>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> Delete()
        {
            this.Delete(this.TableName);
            return this;
        }

        /// <summary>
        /// Genera el estamento SQL para elimminar registros
        /// </summary>
        /// <param name="where">Condición para filtrar los registros</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public SqlBuilderLambda<TEntity> Delete(Expression<Func<TEntity, bool>> where)
        {
            this.Delete(this.TableName);
            return this.Where(where);
        }

        /// <summary>
        /// Realiza una union entre dos querys
        /// </summary>
        /// <param name="sql">Nueva select que se unirá a la actual</param>
        /// <returns></returns>
        new public SqlBuilderLambda<TEntity> Union(SqlBuilder sqlBuilder)
        {
            return this.Union(sqlBuilder);
        }

        /// <summary>
        /// Comprueba si existe el elemento en la base de datos
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns>Booleano indicando si existe el elemento</returns>
        new public bool Exists()
        {
            this.Select(String.Format(this.SQLParser.StrCOUNT, 1));
            return this.First<int>() > 0;
        }

        /// <summary>
        /// Comprueba si existe el elemento en la base de datos
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="where">Condición aplicada</param>
        /// <returns>Booleano indicando si existe el elemento</returns>
        public bool Exists(Expression<Func<TEntity, bool>> where)
        {
            this.Where(where);
            return this.Exists<TEntity>();
        }

        /// <summary>
        /// Obtiene el valor máximo de la columna especificada
        /// </summary>
        /// <typeparam name="T">Tipo devuelto</typeparam>
        /// <param name="column">Columna de la bbdd</param>
        /// <param name="where">Condición aplilcada</param>
        /// <returns>Valor máximo encontrado</returns>
        public T Max<T>(Expression<Func<TEntity, Object>> column, Expression<Func<TEntity, bool>> where)
        {
            this.Where(where);
            return this.Max<T>(column);
        }

        /// <summary>
        /// Obtiene el valor máximo de la columna especificada
        /// </summary>
        /// <typeparam name="T">Tipo devuelto</typeparam>
        /// <param name="column">Columna de la bbdd</param>
        /// <returns>Valor máximo encontrado</returns>
        public T Max<T>(Expression<Func<TEntity, Object>> column)
        {
            string col = this.ExpressionValue(column) as string;
            this.Select(String.Format(this.SQLParser.StrMAX, col));
            return this.FirstOrDefault<T>();
        }

        /// <summary>
        /// Obtiene el valor mínimo de la columna especificada
        /// </summary>
        /// <typeparam name="T">Tipo devuelto</typeparam>
        /// <param name="column">Columna de la bbdd</param>
        /// <param name="where">Condición aplilcada</param>
        /// <returns>Valor mínimo encontrado</returns>
        public T Min<T>(Expression<Func<TEntity, Object>> column, Expression<Func<TEntity, bool>> where)
        {
            this.Where(where);
            return this.Min<T>(column);
        }
        /// <summary>
        /// Obtiene el valor mínimo de la columna especificada
        /// </summary>
        /// <typeparam name="T">Tipo devuelto</typeparam>
        /// <param name="column">Columna de la bbdd</param>
        /// <returns>Valor mínimo encontrado</returns>
        public T Min<T>(Expression<Func<TEntity, Object>> column)
        {
            string col = this.ExpressionValue(column) as string;
            this.Select(String.Format(this.SQLParser.StrMIN, col));
            return this.FirstOrDefault<T>();
        }
        /// <summary>
        /// Obtiene el valor medio de la columna especificada
        /// </summary>
        /// <typeparam name="T">Tipo devuelto</typeparam>
        /// <param name="column">Columna de la bbdd</param>
        /// <param name="where">Condición aplilcada</param>
        /// <returns>Valor medio encontrado</returns>
        public T Avg<T>(Expression<Func<TEntity, Object>> column, Expression<Func<TEntity, bool>> where)
        {
            this.Where(where);
            return this.Avg<T>(column);
        }
        /// <summary>
        /// Obtiene el valor medio de la columna especificada
        /// </summary>
        /// <typeparam name="T">Tipo devuelto</typeparam>
        /// <param name="column">Columna de la bbdd</param>
        /// <returns>Valor medio encontrado</returns>
        public T Avg<T>(Expression<Func<TEntity, Object>> column)
        {
            string col = this.ExpressionValue(column) as string;
            this.Select(String.Format(this.SQLParser.StrAVG, col));
            return this.FirstOrDefault<T>();
        }
        /// <summary>
        /// Obtiene el número de filas encontradas
        /// </summary>
        /// <param name="where">Condición aplilcada</param>
        /// <returns>Filas encontradas</returns>
        public Int64 Count(Expression<Func<TEntity, bool>> where)
        {
            this.Where(where);
            return this.Count();
        }

        /// <summary>
        /// Obtiene el número de filas encontradas
        /// </summary>
        /// <returns>Filas encontradas</returns>
        public Int64 Count()
        {
            this.Select(String.Format(this.SQLParser.StrCOUNT, 1));
            return this.FirstOrDefault<Int64>();
        }

        /// <summary>
        /// Obtiene el valor medio de la suma de todos los valores de la columna especificada
        /// </summary>
        /// <typeparam name="T">Tipo devuelto</typeparam>
        /// <param name="column">Columna de la bbdd</param>
        /// <param name="where">Condición aplilcada</param>
        /// <returns>Valor de la suma</returns>
        public T Sum<T>(Expression<Func<TEntity, Object>> column, Expression<Func<TEntity, bool>> where)
        {
            this.Where(where);
            return this.Sum<T>(column);
        }

        /// <summary>
        /// Obtiene el valor medio de la suma de todos los valores de la columna especificada
        /// </summary>
        /// <typeparam name="T">Tipo devuelto</typeparam>
        /// <param name="column">Columna de la bbdd</param>
        /// <returns>Valor de la suma</returns>
        public T Sum<T>(Expression<Func<TEntity, Object>> column)
        {
            string col = this.ExpressionValue(column) as string;
            this.Select(String.Format(this.SQLParser.StrSUM, col));
            return this.FirstOrDefault<T>();
        }

        /// <summary>
        /// Obtiene el primer elemento de la consulta actual
        /// </summary>
        /// <param name="where">Condición de busqueda</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public TEntity First(Expression<Func<TEntity, bool>> where)
        {
            return this.Where(where).First();
        }

        /// <summary>
        /// Obtiene el primer elemento de la consulta actual
        /// </summary>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public TEntity First()
        {
            return this.First<TEntity>();
        }

        /// <summary>
        /// Obtiene el primer elemento de la consulta actual
        /// </summary>
        /// <param name="where">Condición de busqueda</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public T First<T>(Expression<Func<TEntity, bool>> where)
        {
            return this.Where(where).First<T>();
        }

        /// <summary>
        /// Obtiene el primer elemento de la consulta actual
        /// </summary>
        /// <param name="where">Condición de busqueda</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public T FirstOrDefault<T>(Expression<Func<TEntity, bool>> where)
        {
            return this.Where(where).FirstOrDefault<T>();
        }

        /// <summary>
        /// Obtiene el primer elemento de la consulta actual
        /// </summary>
        /// <param name="where">Condición de busqueda</param>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> where)
        {
            return this.Where(where).FirstOrDefault();
        }

        /// <summary>
        /// Obtiene el primer elemento de la consulta actual
        /// </summary>
        /// <returns>Instancia actual de la clase QueryBuilderLambda</returns>
        public TEntity FirstOrDefault()
        {
            return base.FirstOrDefault<TEntity>();
        }

        /// <summary>
        /// Ejecuta la consulta actual y obtiene los elementos encontrados
        /// </summary>
        /// <param name="where">Condición de busqueda</param>
        /// <returns>Enumerable con los resultados</returns>
        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> where)
        {
            return this.Where(where).Get();
        }

        /// <summary>
        /// Ejecuta la consulta actual y obtiene los elementos encontrados
        /// </summary>
        /// <returns>Enumerable con los resultados</returns>
        public IEnumerable<TEntity> Get()
        {
            return this.Get<TEntity>();
        }

        /// <summary>
        /// Ejecuta la consulta actual y obtiene los elementos encontrados
        /// </summary>
        /// <returns>Enumerable con los resultados</returns>
        public IEnumerable<TEntity> Get(int limit)
        {
            return this.Get<TEntity>(limit);
        }

        #region REFLECTION METHODS
        protected bool IsIgnore(string name)
        {
            return base.IsIgnore(typeof(TEntity).GetRuntimeProperty(name));
        }

        protected string GetTableName()
        {
            return base.GetTableName<TEntity>();
        }

        protected string GetcolumnName(string pName)
        {
            return base.GetcolumnName<TEntity>(pName);
        }

        protected IEnumerable<string> GetAllcolumns()
        {
            return this.GetAllcolumns(typeof(TEntity));
        }
        #endregion
    }
}