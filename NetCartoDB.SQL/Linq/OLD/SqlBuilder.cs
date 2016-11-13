using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NetCartoDB.SQL.Linq
{
    public class SqlBuilder
    {
        //private const BindingFlags VALID_FLAGS = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty;
        #region Valores por defecto
        protected static ISqlParser SQLDefaultParser { get; set; } = new CartoDBConfigurations.CartoDbSQLParser();
        protected static IDbProvider DbHelperDefault { get; set; } = new CartoDBConfigurations.CartoDBProvider();

        static SqlBuilder()
        {
            //QueryBuilder.SQLDefaultParser = new QueryBuilder();
            //QueryBuilder.DbHelperDefault = new QueryBuilder();
        }
        #endregion

        protected ISqlParser SQLParser { get; set; }
        protected IDbProvider DbHelper { get; set; }
        /// <summary>
        /// Indica si se permite realiza UPDATES o DELETEs sin condiciones
        /// </summary>
        public bool SafeMode { get; set; }

        public string sqlWhere { get; protected set; }
        public string sqlTables { get; protected set; }

        public string sqlUnions { get; protected set; }
        public string sqlcolumns { get; protected set; }
        public string sqlValues { get; protected set; }
        public string sqlOrderBy { get; protected set; }
        public string sqlGroupBy { get; protected set; }
        public string sqlHaving { get; protected set; }
        public int rowsLimit { get; protected set; }
        public string sqlJoins { get; protected set; }
        public bool distinct { get; protected set; }
        public SqlSentence TypeQuery { get; protected set; }

        /// <summary>
        /// Genera la representacion en string de la query actual
        /// </summary>
        public string SQL
        {
            get
            {
                return this.SQLParser.GenerateSQL(this);
            }
        }

        /// <summary>
        /// Argumentos que se enviaran a la query, se añaden desde AddSqlArgs.
        /// </summary>
        public IDictionary<string, object> Args { get; private set; }

        /// <summary>
        /// Crea una instancia de SQLBuilder
        /// </summary>
        public SqlBuilder()
        {
            initialize();
        }
        /// <summary>
        /// Crea una instancia de SQLBuilder
        /// </summary>
        /// <param name="parser">Parseador a sql</param>
        public SqlBuilder(ISqlParser parser)
        {
            initialize();
            this.SQLParser = parser;
        }
        /// <summary>
        /// Crea una instancia de SQLBuilder
        /// </summary>
        /// <param name="dbHelper">Gestor de base de datos</param>
        public SqlBuilder(IDbProvider dbHelper)
        {
            initialize();
            this.DbHelper = dbHelper;
        }
        /// <summary>
        /// Crea una instancia de SQLBuilder
        /// </summary>
        /// <param name="parser">Parseador a sql</param>
        /// <param name="dbHelper">Gestor de base de datos</param>
        public SqlBuilder(ISqlParser parser, IDbProvider dbHelper)
        {
            initialize();
            this.SQLParser = parser;
            this.DbHelper = dbHelper;
        }

        protected virtual void initialize()
        {
            this.Args = new Dictionary<string, object>();
            this.SQLParser = SqlBuilder.SQLDefaultParser;
            this.DbHelper = SqlBuilder.DbHelperDefault;
            this.TypeQuery = SqlSentence.SELECT;
            this.SafeMode = true;
            this.sqlcolumns = "*";
        }
        protected virtual string Condition(IEnumerable<string> where, SqlConditions comparer)
        {
            return Condition(where, this.SQLParser.GetOperator(comparer));
        }
        protected virtual string Condition(IEnumerable<string> where, string comparer)
        {
            return String.Join(" " + comparer + " ", where);
        }
        protected virtual string Condition(IEnumerable<string> where)
        {
            return Condition(where, SqlConditions.AND);
        }
        protected virtual string Condition(string where, IEnumerable<object> data)
        {
            string[] values = new string[data.Count()];
            int idx = 0;
            foreach (object item in data)
            {
                //values[idx] = this.ToSqlString(item, typeof(Object));
                values[idx] = this.AddSqlArgs(item);
                idx++;
            }

            return String.Format(where, values);
        }
        protected virtual string Condition(string where, object data)
        {
            return Condition(where, data, "");
        }
        protected virtual string Condition(string where, object data, string sqlCondition)
        {
            //string value = this.ToSqlString(data, typeof(Object));
            string value = this.AddSqlArgs(data);
            return sqlCondition + String.Format(where, value);
        }
        protected virtual string Condition(string where, object data, SqlConditions comparer)
        {
            return Condition(where, data, this.SQLParser.GetOperator(comparer));
        }
        protected virtual string Condition(object where)
        {
            return Condition(where, SqlConditions.AND);
        }
        protected virtual string Condition(object where, SqlConditions comparer)
        {
            return this.Condition(where, comparer, this.SQLParser.StrWHERE);
        }
        protected virtual string Condition(object where, SqlConditions comparer, string format)
        {
            IEnumerable<string> w = this.ObjectToString(where, format);
            return Condition(w, comparer);
        }
        protected virtual string Condition(string where, SqlConditions condition)
        {
            string strCondition = condition.ToString().Replace('_', ' ');
            if (String.IsNullOrEmpty(this.sqlWhere))
            {
                switch (condition)
                {
                    case SqlConditions.AND:
                        strCondition = "";
                        break;
                    case SqlConditions.AND_NOT:
                        strCondition = this.SQLParser.GetOperator(condition);
                        break;
                    default:
                        throw new Exception("Can't be used " + strCondition + " condition in first condition.");
                }
            }
            return " " + strCondition + " " + where;
        }
        protected SqlBuilder AppendCondition(string where, SqlConditions condition)
        {
            this.sqlWhere += Condition(where, condition);
            return this;
        }
        protected SqlBuilder AppendCondition(string where, object data, SqlConditions condition)
        {
            this.AppendCondition(this.Condition(where, data), condition);
            return this;
        }

        #region SQL CONDITIONS
        /// <summary>
        /// Añade el filtro que se aplicará a la consulta
        /// </summary>
        /// <example>
        /// <code>
        /// where -> string[] { "ID=100", "NAME='JULIA'"}
        /// </code>
        /// </example>
        /// <param name="where">
        /// Filtros de la consuta
        /// Cada elemento representa un filtro completo sin las palabras reservadas AND, OR, ...
        /// </param>
        /// <remarks>
        /// Sobreescribe los filtros anteriores
        /// </remarks>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Where(IEnumerable<string> where)
        {
            return Where(Condition(where));
        }

        /// <summary>
        /// Añade el filtro que se aplicará a la consulta
        /// 
        /// NOTE: Sobreescribe los filtros anteriores
        /// EXAMPLE: where -> "ID={0} OR NAME={1} AND (...)"
        ///          data  -> Object[] { 100, "JULIA", (...) }
        /// </summary>
        /// <param name="having">
        /// Cadena de condicíon entera
        /// NOTE: Sobreescribe los filtros anteriores
        /// NOTE: No necesita la palabra reservada "WHERE"
        /// </param>
        /// <param name="data">Datos de las condiciones</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Where(string where, IEnumerable<object> data)
        {
            return Where(Condition(where, data));
        }

        /// <summary>
        /// Añade el filtro que se aplicará a la consulta
        /// 
        /// Cadena de condicíon entera
        /// NOTE: Sobreescribe los filtros anteriores
        /// NOTE: No necesita la palabra reservada "WHERE"
        /// 
        /// EXAMPLE: where -> "ID={0}"
        ///          data  -> 100
        /// 
        /// EXAMPLE: where -> "NAME={0}"
        ///          data  -> "Julia"
        /// 
        /// EXAMPLE: where -> "ID={0} and NAME='Julia'"
        ///          data  -> 100
        /// </summary>
        /// <param name="where">Condición de la consulta</param>
        /// <param name="data">Dato de la condicion</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Where(string where, object data)
        {
            return Where(Condition(where, data));
        }

        /// <summary>
        /// Añade el filtro que se aplicará a la consulta
        /// 
        /// NOTE: Sobreescribe los filtros anteriores
        /// EXAMPLE: where -> new { ID = 100, NAME="JULIA" }
        /// </summary>
        /// <param name="where"></param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Where(object where)
        {
            return Where(Condition(where));
        }

        /// <summary>
        /// Añade el filtro que se aplicará a la consulta
        /// 
        /// NOTE: Sobreescribe los filtros anteriores
        /// NOTE: No necesita la palabra reservada "WHERE"
        /// 
        /// EXAMPLE: where -> "ID=100 AND NAME='JULIA'"
        /// </summary>
        /// <param name="where">
        /// Cadena con el/los filtros que se aplicarán
        /// NOTE: No se modificará la sintaxis (No añade comillas simples a los valores que lo requieran)
        /// </param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Where(string where)
        {
            this.sqlWhere = where;
            return this;
        }

        /// <summary>
        /// Agrega una condicion AND al los filtros
        /// 
        /// EXAMPLE: condition -> "NAME = {0}"
        ///               data -> "JULIA"
        ///               
        /// EXAMPLE: condition -> "ID > {0}"
        ///               data -> 100
        /// </summary>
        /// <param name="condition">Condición que se añadirá</param>
        /// <param name="data">Valor</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder And(string condition, object data)
        {
            return this.AppendCondition(condition, data, SqlConditions.AND);
        }

        /// <summary>
        /// Agrega una condicion AND NOT al los filtros
        /// EXAMPLE: condition -> "NAME = {0}"
        ///               data -> "JULIA"
        /// EXAMPLE: condition -> "ID > {0}"
        ///               data -> 100
        /// </summary>
        /// <param name="condition">Condición que se añadirá</param>
        /// <param name="data">Valor</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder AndNot(string condition, object data)
        {
            return this.AppendCondition(condition, data, SqlConditions.AND_NOT);
        }

        /// <summary>
        /// Agrega una condicion OR al los filtros
        /// EXAMPLE: condition -> "NAME = {0}"
        ///               data -> "JULIA"
        /// EXAMPLE: condition -> "ID > {0}"
        ///               data -> 100
        /// </summary>
        /// <param name="condition">Condición que se añadirá</param>
        /// <param name="data">Valor</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Or(string condition, object data)
        {
            return this.AppendCondition(condition, data, SqlConditions.OR);
        }

        /// <summary>
        /// Agrega una condicion OR al los filtros
        /// EXAMPLE: condition -> "NAME = {0}"
        ///               data -> "JULIA"
        /// EXAMPLE: condition -> "ID > {0}"
        ///               data -> 100
        /// </summary>
        /// <param name="condition">Condición que se añadirá</param>
        /// <param name="data">Valor</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Or(string condition)
        {
            return this.AppendCondition(condition, SqlConditions.OR);
        }

        /// <summary>
        /// Agrega una condicion OR al los filtros
        /// </summary>
        /// <example>
        /// EXAMPLE: 
        /// <code>
        ///          condition -> "NAME = {0}"
        ///               data -> "JULIA"
        /// </code>              
        /// EXAMPLE:
        /// <code>
        ///          condition -> "ID > {0}"
        ///               data -> 100
        /// </code>
        /// </example>
        /// <param name="condition">Condición que se añadirá</param>
        /// <param name="data">Valor</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder OrNot(string condition, object data)
        {
            return this.AppendCondition(condition, data, SqlConditions.OR_NOT);
        }

        /// <summary>
        /// Añade una condición BETWEEN a las condiciones de la consulta
        /// </summary>
        /// <example>
        /// EXAMPLE: 
        /// <code>
        ///          column -> "AGE"
        ///          first  -> 18
        ///          second -> 65
        /// </code>
        /// </example>
        /// <param name="column">Nombre de la columna a comparar</param>
        /// <param name="first">Condición desde</param>
        /// <param name="second">Condición hasta</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Between(string column, object first, object second)
        {
            return Between(column, first, second, SqlConditions.AND);
        }

        /// <summary>
        /// Añade una condición BETWEEN a las condiciones de la consulta
        /// </summary>
        /// <example>
        /// EXAMPLE: 
        /// <code>
        ///          column -> "AGE"
        ///          first  -> 18
        ///          second -> 65
        ///          condition -> SQLConditions.AND_NOT
        /// </code>
        /// </example>
        /// <param name="column">Nombre de la columna a comparar</param>
        /// <param name="first">Condición desde</param>
        /// <param name="second">Condición hasta</param>
        /// <param name="condition">Tipo de condición aplicada</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Between(string column, object first, object second, SqlConditions condition)
        {
            //string strFirst = this.ToSqlString(first, typeof(Object));
            string strFirst = this.AddSqlArgs(first);
            //string strSecond = this.ToSqlString(second, typeof(Object));
            string strSecond = this.AddSqlArgs(second);
            this.sqlWhere += AppendCondition(String.Format(this.SQLParser.StrBETWEEN, column, first, second), condition);
            return this;
        }

        /// <summary>
        /// Añade una condición IN a las condiciones de la consulta
        /// </summary>
        /// <example>
        /// EXAMPLE: 
        /// <code>
        ///           column -> "AGE"
        ///           values -> new object[] {18, 19, 20, 21, 22, 23, 24, 25}
        /// </code>
        /// </example>
        /// <param name="column">Nombre de la columna </param>
        /// <param name="values">Valores de la condición</param>
        /// <returns></returns>
        public SqlBuilder In(string column, IEnumerable<object> values)
        {
            return this.In(column, values, SqlConditions.AND);
        }

        /// <summary>
        /// Añade una condición IN a las condiciones de la consulta
        /// </summary>
        /// <example>
        /// EXAMPLE: 
        /// <code>
        ///           column -> "AGE"
        ///           values -> new object[] {18, 19, 20, 21, 22, 23, 24, 25}
        ///        condition -> SQLConditions.AND
        /// </code>
        /// </example>
        /// <param name="column">Nombre de la columna </param>
        /// <param name="values">Valores de la condición</param>
        /// <param name="condition">Tipo de condición aplicada</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder In(string column, IEnumerable<object> values, SqlConditions condition)
        {
            string[] strValues = new string[values.Count()];
            int idx = 0;
            foreach (object item in values)
            {
                //strValues[idx] = this.ToSqlString(item, typeof(Object));
                strValues[idx] = this.AddSqlArgs(item);
                idx++;
            }

            string sql = String.Format(this.SQLParser.StrIN, column, String.Join(this.SQLParser.StrINSeparator, strValues));
            return this.AppendCondition(sql, condition);
        }

        /// <summary>
        /// Añade una condición IN a las condiciones de la consulta
        /// </summary>
        /// <example>
        /// EXAMPLE: 
        /// <code>
        ///            column -> "ID_RELATION"
        ///          inSelect -> new SqlBuilder().Select("ID").From("RELATION").Where("ID={0}", 5);
        /// </code>
        /// </example>
        /// <param name="column">Nombre de la columna </param>
        /// <param name="inSelect">SQLBuilder que representa la select anidada</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder In(string column, SqlBuilder inSelect)
        {
            return this.In(column, inSelect, SqlConditions.AND);
        }

        /// <summary>
        /// Añade una condición IN a las condiciones de la consulta
        /// </summary>
        /// <example>
        /// EXAMPLE: 
        /// <code>
        ///            column -> "ID_RELATION"
        ///          inSelect -> new SqlBuilder().Select("ID").From("RELATION").Where("ID={0}", 5);
        /// </code>
        /// </example>
        /// <param name="column">Nombre de la columna </param>
        /// <param name="inSelect">SQLBuilder que representa la select anidada</param>
        /// <param name="condition">Tipo de condición aplicada</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder In(string column, SqlBuilder inSelect, SqlConditions condition)
        {
            string sql = String.Format(this.SQLParser.StrIN, column, String.Join(this.SQLParser.StrINSeparator, inSelect.SQL));
            return this.AppendCondition(sql, condition);
        }
        #endregion

        /// <summary>
        /// Crea una instancia de SQLBuilder indicandole el nombre de la tabla
        /// </summary>
        /// <param name="table">Nombre de la tabla</param>
        public SqlBuilder(string table)
        {
            initialize();
            this.sqlTables = table;
        }

        /// <summary>
        /// Crea una instancia de SQLBuilder indicandole la tabla y las columnas
        /// </summary>
        /// <param name="table">Nombre de la tabla</param>
        /// <param name="columns">Cadena que representa la/s columna/s</param>
        public SqlBuilder(string table, string columns)
        {
            initialize();
            this.sqlTables = table;
            this.sqlcolumns = columns;
        }

        /// <summary>
        /// Crea una instancia de SQLBuilder indicandole la tabla, las columnas y la condición
        /// </summary>
        /// <param name="table">Nombre de la tabla</param>
        /// <param name="columns">Cadena que representa la/s columna/s seleccionadas</param>
        /// <param name="where">Condición de la consulta</param>
        public SqlBuilder(string table, string columns, string where)
        {
            initialize();
            this.sqlTables = table;
            this.sqlcolumns = columns;
            this.sqlWhere = where;
        }

        /// <summary>
        /// Define el número máximo de rows que se devolverán
        /// </summary>
        /// <param name="limit">numero de rows</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Limit(int limit)
        {
            this.rowsLimit = limit;
            return this;
        }

        /// <summary>
        /// Realiza un "SELECT * "
        /// </sumary>
        /// <remarks>
        /// Sobreescribe las columnas anteriores.
        /// Es el valor por defecto si no se especifica ninguno.
        /// </remarks>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Select()
        {
            this.sqlcolumns = "*";
            return this;
        }

        /// <summary>
        /// Especifica las columnas de la clausula select
        /// </sumary>
        /// <remarks>
        /// Sobreescribe las columnas anteriores
        /// </remarks>
        /// <param name="columns">Cadena que representa la/s columna/s seleccionadas</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Select(string columns)
        {
            this.sqlcolumns = columns;
            return this;
        }

        /// <summary>
        /// Especifica las columnas de la clausula select
        /// </sumary>
        /// <remarks>
        /// Sobreescribe las columnas anteriores
        /// </remarks>
        /// <param name="columns">Array con las columnas seleccionadas</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Select(IEnumerable<string> columns)
        {
            this.sqlcolumns = String.Join(this.SQLParser.StrcolumnSeparator, columns);
            return this;
        }

        /// <summary>
        /// Agrega una columna a las columnas ya agregadas
        /// </summary>
        /// <param name="column">Nombre de la columna</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder SelectThem(string column)
        {
            this.sqlcolumns += this.SQLParser.StrcolumnSeparator + column;
            return this;
        }

        /// <summary>
        /// Agrega las columnas especificadas a las columnas ya agregadas
        /// </summary>
        /// <param name="column">Nombre de la columna</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder SelectThem(IEnumerable<string> columns)
        {
            foreach (string column in columns)
                this.sqlcolumns += this.SQLParser.StrcolumnSeparator + column;
            return this;
        }

        /// <summary>
        /// Agrega una columna a las columnas ya agregadas
        /// </summary>
        /// <param name="column">Nombre de la columna</param>
        /// <param name="alias">Alias que recibirá la columna</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder SelectThem(string column, string alias)
        {
            this.sqlcolumns += this.SQLParser.StrcolumnSeparator + String.Format(this.SQLParser.StrcolumnAlias, column, alias);
            return this;
        }

        /// <summary>
        /// Agrega una consulta como columna de la consulta actual
        /// </summary>
        /// <param name="sqlcolumn">Query que se agregará como columna</param>
        /// <param name="alias">Alias que se le dará al resultado</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder SelectThem(SqlBuilder sqlcolumn, string alias)
        {
            sqlcolumn.Limit(1);
            return this.SelectThem(sqlcolumn.SQL, alias);
        }

        /// <summary>
        /// Especifica si se descartarán las filas repetidas
        /// </summary>
        /// <param name="distinct">establecer clausula distinct</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Distinct(bool distinct = true)
        {
            this.distinct = distinct;
            return this;
        }

        /// <summary>
        /// Especifica el nombre de la tabla
        /// </sumary>
        /// <remarks>
        /// Sobreescribe la tabla anterior
        /// </remarks>
        /// <param name="table">Nombre de la tabla</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder From(string table)
        {
            this.sqlTables = table;
            return this;
        }

        /// <summary>
        /// Especifica el nombre de la tabla
        /// </sumary>
        /// <remarks>
        /// Sobreescribe las tablas anteriores
        /// </remarks>
        /// <param name="tables">Nombre de las tablas</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder From(IEnumerable<string> tables)
        {
            return this.From(String.Join(this.SQLParser.StrFROMSeparator, tables));
        }

        /// <summary>
        /// Añade una tabla a la consulta
        /// </summary>
        /// <param name="tables">Nombre de las tablas</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder FromThen(string table)
        {
            if (!Regex.IsMatch(this.sqlTables, "\\b" + table + "\\b", RegexOptions.IgnoreCase))
                this.sqlTables += this.SQLParser.StrFROMSeparator + table;

            return this;
        }

        /// <summary>
        /// Añade las tablas especificadas a la consulta
        /// </summary>
        /// <param name="tables">Nombre de las tablas</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder FromThen(IEnumerable<string> tables)
        {
            foreach (string table in tables)
                this.FromThen(table);

            return this;
        }

        /// <summary>
        /// Añade un join en la consulta
        /// </summary>
        /// <example>
        /// EXAMPLE:
        /// <code>
        ///          table -> "DIRECTION"
        ///          on    -> "ID_DIRECTION = ID"
        ///          join  -> SQLJoin.LEFT_JOIN
        /// </code>
        /// </example>
        /// <param name="table">Nombre de la tabla del Join</param>
        /// <param name="on">Condiciones del join</param>
        /// <param name="join">Tipo de join que se realiza</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Join(string table, string on, SqlJoin join)
        {
            string sql = String.Format(this.SQLParser.StrJOIN, this.SQLParser.GetOperator(join), table, on);
            this.sqlJoins += sql;
            return this;
        }

        /// <summary>
        /// Añade un join en la consulta
        /// </summary>
        /// <example>
        /// EXAMPLE:
        /// <code>
        ///          table -> "DIRECTION"
        ///          on    -> new string[] { "ID = ID_DIRECTION", "STREET = 'SUN'", (...) }
        ///          join  -> SQLJoin.LEFT_JOIN
        /// </code>
        /// </example>
        /// <param name="table">Nombre de la tabla del Join</param>
        /// <param name="on">Condiciones del join</param>
        /// <param name="join">Tipo de join que se realiza</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Join(string table, IEnumerable<string> on, SqlJoin join)
        {
            string condition = this.Condition(on);
            return Join(table, condition, join);
        }

        /// <summary>
        /// Añade un join en la consulta
        /// </summary>
        /// <example>
        /// EXAMPLE:
        /// <code>
        ///          table -> "DIRECTION"
        ///          on    -> "ID = ID_DIRECTION AND STREET = {1}, (...)
        ///          data  -> new object[] { "SUN" }
        ///          join  -> SQLJoin.LEFT_JOIN
        /// </code>
        /// </example>
        /// <param name="table">Nombre de la tabla del Join</param>
        /// <param name="on">Condiciones del join</param>
        /// <param name="data">Datos de las condiciones</param>
        /// <param name="join">Tipo de join que se realiza</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Join(string table, string on, IEnumerable<object> data, SqlJoin join)
        {
            string condition = this.Condition(on, data);
            return Join(table, condition, join);
        }

        /// <summary>
        /// Añade un join en la consulta
        /// </summary>
        /// <example>
        /// EXAMPLE:
        /// <code>
        ///          table -> "DIRECTION"
        ///          on    -> "ID = ID_DIRECTION AND STREET = {1}
        ///          data  -> "SUN"
        ///          join  -> SQLJoin.LEFT_JOIN
        /// </code>
        /// </example>
        /// <param name="table">Nombre de la tabla del Join</param>
        /// <param name="on">Condiciones del join</param>
        /// <param name="data">Dato de las condiciones</param>
        /// <param name="join">Tipo de join que se realiza</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Join(string table, string on, object data, SqlJoin join)
        {
            string condition = this.Condition(on, data);
            return Join(table, condition, join);
        }

        /// <summary>
        /// Añade un join en la consulta
        /// </summary>
        /// <example>
        /// EXAMPLE:
        /// <code>
        ///          table -> "DIRECTION"
        ///          on    -> new { ID = 5, STREET = "SUN" }
        ///          join  -> SQLJoin.LEFT_JOIN
        /// </code>
        /// </example>
        /// <param name="table">Nombre de la tabla del Join</param>
        /// <param name="on">Condiciones del join</param>
        /// <param name="join">Tipo de join que se realiza</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Join(string table, object on, SqlJoin join)
        {
            string condition = this.Condition(on);
            return Join(table, condition, join);
        }

        /// <summary>
        /// columnas sobre las que se ordenara la query
        /// </summary>
        /// <remarks>
        /// NOTE: Sobreescribe el orden anterior
        /// </remarks>
        /// <example>
        /// EXAMPLE:
        /// <code>
        ///      orderBy  -> string[] { "ID", "NAME DESC" }
        /// </code>
        /// </example>
        /// <param name="orderBy">columnas sobre las que se ordenará</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder OrderBy(IEnumerable<string> orderBy)
        {
            return OrderBy(String.Join(this.SQLParser.StrORDERBYSeparator, orderBy));
        }

        /// <summary>
        /// columnas sobre las que se ordenara la query
        /// </summary>
        /// <remarks>
        /// Sobreescribe el orden anterior
        /// </remarks>
        /// <example>
        /// EXAMPLE:
        /// <code>
        ///     orderBy  -> "ID, NAME DESC"
        /// </code>
        /// </example>
        /// <param name="orderBy">columnas sobre las que se ordenará</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder OrderBy(string orderBy)
        {
            this.sqlOrderBy = orderBy;
            return this;
        }

        /// <summary>
        /// columnas sobre las que se agrupará la query
        /// </summary>
        /// <remarks>
        ///  Sobreescribe el orden anterior
        /// </remarks>
        /// <example>
        /// EXAMPLE:
        /// <code>
        ///  groupBy  -> string[] { "ID", "NAME" }
        /// </code>
        /// </example>
        /// <param name="orderBy">columnas sobre las que se ordenará</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder GroupBy(IEnumerable<string> groupBy)
        {
            return GroupBy(String.Join(this.SQLParser.StrGROPUPBYSeparator, groupBy));
        }

        /// <summary>
        /// columnas sobre las que se ordenara la query
        /// </summary>
        /// <remarks>
        ///  Sobreescribe el orden anterior
        /// </remarks>
        /// <example>
        /// EXAMPLE:
        /// <code>
        /// groupBy  -> "ID, NAME"
        /// </code>
        /// </example>
        /// <param name="orderBy">columnas sobre las que se ordenará</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder GroupBy(string groupBy)
        {
            this.sqlGroupBy = groupBy;
            return this;
        }

        /// <summary>
        /// Añade el filtro que se aplicará a la consulta
        /// </summary>
        /// <remarks>
        /// NOTE: Sobreescribe los filtros anteriores
        /// NOTE: No necesita la palabra reservada "HAVING"
        /// </remarks>
        /// <example>
        /// EXAMPLE:
        /// <code>
        /// having -> string[] { "Max(AGE) <= 65", "Min(AGE) >= 18"}
        /// </code>
        /// </example>
        /// <param name="having">
        /// Filtros de la consuta
        /// Cada elemento representa un filtro completo sin la palabra reservada AND
        /// </param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Having(IEnumerable<string> having)
        {
            return Having(Condition(having));
        }

        /// <summary>
        /// Añade el filtro que se aplicará a la consulta
        /// </summary>
        /// <remarks>
        /// Sobreescribe los filtros anteriores.
        /// No necesita la palabra reservada "HAVING".
        /// </remarks>
        /// <example>
        /// EXAMPLE:
        /// <code>
        ///         having -> "Max(AGE) <= {0} AND Min(AGE) >= {1} AND (...)"
        ///          data  -> Object[] { 65, 18, (...) }
        /// </code>
        /// </example>
        /// <param name="having">
        /// Cadena de condicíon entera
        /// </param>
        /// <param name="data">Datos de las condiciones</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Having(string having, IEnumerable<object> data)
        {
            return Having(Condition(having, data));
        }

        /// <summary>
        /// Añade el filtro having que se aplicará a la consulta
        /// </summary>
        /// <remarks>
        /// Sobreescribe los filtros anteriores.
        /// No necesita la palabra reservada "HAVING".
        /// </remarks>
        /// <example>
        /// EXAMPLE:
        /// <code>
        ///          where -> "Max(AGE) < {0}"
        ///          data  -> 100
        /// </code>
        /// </example>
        /// <param name="having">Condición de la consulta, Cadena de condicíon entera</param>
        /// <param name="data">Dato de la condicion</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Having(string having, object data)
        {
            return Having(Condition(having, data));
        }

        /// <summary>
        /// Añade el filtro having que se aplicará a la consulta
        /// </summary>
        /// <remarks>
        /// Sobreescribe los filtros anteriores
        /// No necesita la palabra reservada "HAVING"
        /// </remarks>
        /// <example>
        /// EXAMPLE:
        /// <code>
        /// "Max(AGE) <=65 AND Min(AGE) >= 18 AND (...)"
        /// </code>
        /// </example>
        /// <param name="having"></param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Having(string having)
        {
            this.sqlHaving = having;
            return this;
        }

        /// <summary>
        /// Genera un estamento de base de datos para insertar registros
        /// </summary>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Insert()
        {
            this.TypeQuery = SqlSentence.INSERT;
            return this;
        }

        /// <summary>
        /// Genera un estamento de base de datos para insertar registros
        /// </summary>
        /// <param name="table">Tabla donde se insertará</param>
        /// <param name="data">Datos que se insertarán</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Insert(string table, object data)
        {
            return this.Insert(table, this.ObjectToDictionary(data));
        }

        /// <summary>
        /// Genera un estamento de base de datos para insertar registros
        /// </summary>
        /// <param name="table">Tabla donde se insertará</param>
        /// <param name="data">Datos que se insertarán</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Insert(string table, IDictionary<string, object> data)
        {
            this.TypeQuery = SqlSentence.INSERT;
            this.Into(table);
            string strcolumns = "", strValues = "";

            foreach (KeyValuePair<string, object> par in data)
            {
                if (!String.IsNullOrEmpty(strcolumns))
                    strcolumns += this.SQLParser.StrcolumnSeparator;
                strcolumns += par.Key;

                if (!String.IsNullOrEmpty(strValues))
                    strValues += this.SQLParser.StrcolumnSeparator;
                strValues += this.AddSqlArgs(par.Value);
            }

            this.columns(strcolumns);
            this.Values(strValues);
            return this;
        }

        /// <summary>
        /// Genera un estamento de base de datos para insertar registros
        /// </summary>
        /// <typeparam name="TEntity">Tipo de la entidad</typeparam>
        /// <param name="data">Datos a insertar</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Insert<TEntity>(object data) where TEntity : class
        {
            return this.Insert<TEntity>(data, false);
        }

        /// <summary>
        /// Genera un estamento de base de datos para insertar registros
        /// </summary>
        /// <typeparam name="TEntity">Tipo de la entidad</typeparam>
        /// <param name="data">Datos a insertar</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Insert<TEntity>(object data, bool ignoreNulls) where TEntity : class
        {
            string table = this.GetTableName<TEntity>();
            return this.Insert(table, this.ObjectToDictionary<TEntity>(data, ignoreNulls));
        }

        /// <summary>
        /// Genera un estamento de base de datos para insertar registros
        /// </summary>
        /// <param name="table">Tabla donde se insertará</param>
        /// <param name="columns">columnas que se insertarán</param>
        /// <param name="data">Datos a insertar</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Insert(string table, string columns, string data)
        {
            this.TypeQuery = SqlSentence.INSERT;
            return this.Into(table).columns(columns).Values(data);
        }

        /// <summary>
        /// Genera un estamento de base de datos para insertar registros
        /// </summary>
        /// <param name="table">Tabla donde se insertará</param>
        /// <param name="columns">columnas que se insertarán</param>
        /// <param name="data">Datos a insertar</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Insert(string table, string columns, IEnumerable<object> data)
        {
            this.TypeQuery = SqlSentence.INSERT;
            return this.Into(table).columns(columns).Values(data);
        }

        /// <summary>
        /// Genera un estamento de base de datos para insertar registros
        /// </summary>
        /// <param name="table">Tabla donde se insertará</param>
        /// <param name="columns">columnas que se insertarán</param>
        /// <param name="data">Datos a insertar</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Insert(string table, IEnumerable<string> columns, IEnumerable<object> data)
        {
            this.TypeQuery = SqlSentence.INSERT;
            return this.Into(table).columns(columns).Values(data);
        }

        /// <summary>
        /// Define la tabla donde se insertará
        /// </summary>
        /// <param name="table">Nombre de la tabla</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Into(string table)
        {
            this.sqlTables = table;
            return this;
        }

        /// <summary>
        /// columnas que se insertarán
        /// </summary>
        /// <param name="columns">columna/s a insertar</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder columns(string columns)
        {
            this.sqlcolumns = columns;
            return this;
        }

        /// <summary>
        /// columnas que se insertarán
        /// </summary>
        /// <param name="columns">columna/s a insertar</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder columns(IEnumerable<string> columns)
        {
            return this.columns(String.Join(this.SQLParser.StrcolumnSeparator, columns));
        }

        /// <summary>
        /// Valores que se insertarán
        /// </summary>
        /// <param name="values">Valor/es a insertar</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Values(string values)
        {
            this.sqlValues = values;
            return this;
        }

        /// <summary>
        /// Valores que se insertarán
        /// </summary>
        /// <param name="values">Valor/es a insertar</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Values(IEnumerable<object> data)
        {
            string[] values = new string[data.Count()];
            int idx = 0;
            foreach (object item in data)
            {
                //values[idx] = this.ToSqlString(item, typeof(Object));
                values[idx] = this.AddSqlArgs(item);
                idx++;
            }
            string strValues = String.Join(this.SQLParser.StrcolumnSeparator, values);
            return this.Values(strValues);
        }

        /// <summary>
        /// Genera un estamento de base de datos para insertar actualizar registros
        /// </summary>
        /// <param name="table">Nombre de la tabla a actualizar</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Update(string table)
        {
            this.TypeQuery = SqlSentence.UPDATE;
            return this.From(table);
        }

        /// <summary>
        /// Genera un estamento de base de datos para insertar actualizar registros
        /// </summary>
        /// <param name="table">Nombre de la tabla a actualizar</param>
        /// <param name="data">Datos que se insertarán</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Update(string table, object data)
        {
            this.TypeQuery = SqlSentence.UPDATE;
            return this.Update(table).Set(data);
        }

        /// <summary>
        /// Genera un estamento de base de datos para insertar actualizar registros
        /// </summary>
        /// <param name="table">Nombre de la tabla a actualizar</param>
        /// <param name="data">Datos que se insertarán</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Update(string table, IEnumerable<string> data)
        {
            return this.Update(table).Set(data);
        }

        /// <summary>
        /// Define la columna/valor que se actualizarán
        /// </summary>
        /// <param name="values">Valores a actualizar</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Set(string values)
        {
            if (!String.IsNullOrEmpty(this.sqlValues))
                this.sqlValues += this.SQLParser.StrcolumnSeparator;

            this.sqlValues += values;
            return this;
        }

        /// <summary>
        /// Define la columna/valor que se actualizarán
        /// </summary>
        /// <param name="data">Datos que se insertarán</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Set(IEnumerable<string> data)
        {
            return Set(Condition(data, SqlConditions.SEPARATOR));
        }

        /// <summary>
        /// Define la columna/valor que se actualizarán
        /// </summary>
        /// <param name="setStr">Cadena del 'set' actual</param>
        /// <param name="data">Valor a actualizar</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Set(string setStr, IEnumerable<object> data)
        {
            return Set(Condition(setStr, data, SqlConditions.SEPARATOR));
        }

        /// <summary>
        /// Define la columna/valor que se actualizarán
        /// </summary>
        /// <param name="setStr">Cadena del 'set' actual</param>
        /// <param name="data">Valor a actualizar</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Set(string setStr, object data)
        {
            return Set(Condition(setStr, data, SqlConditions.SEPARATOR));
        }

        /// <summary>
        /// Define la columna/valor que se actualizarán
        /// </summary>
        /// <param name="data">Valor/es a actualizar</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Set(object data)
        {
            return Set(Condition(data, SqlConditions.SEPARATOR, this.SQLParser.StrVALUE));
        }

        /// <summary>
        /// Genera un estamento de base de datos para eliminar
        /// </summary>
        /// <param name="table">Nombre de la tabla</param>
        /// <returns>Instancia actual de la clase SQLBuilder</returns>
        public SqlBuilder Delete(string table)
        {
            this.TypeQuery = SqlSentence.DELETE;
            this.sqlTables = table;
            return this;
        }

        /// <summary>
        /// Realiza una union entre dos querys
        /// </summary>
        /// <param name="sql">Nueva select que se unirá a la actual</param>
        /// <returns></returns>
        public SqlBuilder Union(string sql)
        {
            this.sqlUnions += sql;
            return this;
        }

        /// <summary>
        /// Realiza una union entre dos querys
        /// </summary>
        /// <param name="sql">Nueva select que se unirá a la actual</param>
        /// <returns></returns>
        public SqlBuilder Union(SqlBuilder sqlBuilder)
        {
            this.sqlUnions += sqlBuilder.SQL;
            return this;
        }

        #region DATABASE METHODS
        /// <summary>
        /// Comprueba si el elemento existe
        /// </summary>
        /// <typeparam name="T">Tipo de entidad</typeparam>
        /// <returns></returns>
        public bool Exists<T>()
        {
            return this.Exists();
        }

        /// <summary>
        /// Comprueba si el elemento existe
        /// </summary>
        /// <returns></returns>
        public bool Exists()
        {
            return this.First<int>() > 0;
        }

        /// <summary>
        /// Ejecuta la consulta actual y obtiene los elementos encontrados
        /// </summary>
        /// <typeparam name="T">Tipo del los valores espereados</typeparam>
        /// <returns>Enumerable con los resultados</returns>
        public IEnumerable<T> Get<T>()
        {
            //throw new NotImplementedException();
            return this.Query<T>();
        }

        /// <summary>
        /// Ejecuta la consulta especificada y obtiene los elementos encontrados
        /// </summary>
        /// <typeparam name="T">Tipo del los valores espereados</typeparam>
        /// <param name="limit">Número máximo de registros</param>
        /// <returns>Enumerable con los resultados</returns>
        public IEnumerable<T> Get<T>(int limit)
        {
            this.rowsLimit = limit;
            return this.Get<T>();
        }

        /// <summary>
        /// Obtiene el primer elemento de la consulta actual
        /// </summary>
        /// <returns>Instancia actual de la clase SQLBuilderLambda</returns>
        public T First<T>()
        {
            this.rowsLimit = 1;
            return this.Query<T>().First();
        }

        /// <summary>
        /// Obtiene el primer elemento de la consulta actual
        /// </summary>
        /// <returns>Instancia actual de la clase SQLBuilderLambda</returns>
        public T FirstOrDefault<T>()
        {
            this.rowsLimit = 1;
            return this.Query<T>().FirstOrDefault();
        }

        /// <summary>
        /// Obtiene el primer elemento de la consulta actual
        /// </summary>
        /// <returns>Instancia actual de la clase SQLBuilderLambda</returns>
        public T FirstOrDefault<T>(string sql, object[] args)
        {
            return this.Query<T>(sql, args).FirstOrDefault();
        }

        /// <summary>
        /// Ejecuta la consulta especificada y obtiene los elementos encontrados
        /// </summary>
        /// <typeparam name="T">Tipo del los valores espereados</typeparam>
        /// <param name="sql">Consulta sql que se ejecutará</param>
        /// <returns>Enumerable con los resultados</returns>
        public IEnumerable<T> Query<T>()
        {
            if (!this.TypeQuery.Equals(SqlSentence.SELECT))
                throw new SqlNoPermittedException("Only can be executed SELECT sentences.", this);

            return this.Query<T>(this.SQL, this.Args.Values.ToArray());
        }

        /// <summary>
        /// Ejecuta la consulta especificada y obtiene los elementos encontrados
        /// </summary>
        /// <typeparam name="T">Tipo del los valores espereados</typeparam>
        /// <param name="sql">Consulta sql que se ejecutará</param>
        /// <returns>Enumerable con los resultados</returns>
        public IEnumerable<T> Query<T>(string sql)
        {
            //throw new NotImplementedException();
            return this.DbHelper.Query<T>(sql, new object[] { });
        }

        /// <summary>
        /// Ejecuta la consulta especificada y obtiene los elementos encontrados
        /// </summary>
        /// <typeparam name="T">Tipo del los valores espereados</typeparam>
        /// <param name="sql">Consulta sql que se ejecutará</param>
        /// <returns>Enumerable con los resultados</returns>
        public IEnumerable<T> Query<T>(string sql, object[] args)
        {
            //throw new NotImplementedException();
            return this.DbHelper.Query<T>(sql, args);
        }

        /// <summary>
        /// Ejecuta la consulta especificada y obtiene los elementos encontrados
        /// </summary>
        /// <param name="sql">Consulta sql que se ejecutará</param>
        /// <returns>Objeto con los resultados</returns>
        public object Query(string sql)
        {
            //throw new NotImplementedException();
            return this.DbHelper.Query<dynamic>(sql, new object[] { });
        }

        /// <summary>
        /// Ejecuta la consulta especificada y obtiene los elementos encontrados
        /// </summary>
        /// <param name="sql">Consulta sql que se ejecutará</param>
        /// <returns>Objeto con los resultados</returns>
        public object Query(string sql, object[] args)
        {
            //throw new NotImplementedException();
            return this.DbHelper.Query<dynamic>(sql, args);
        }

        /// <summary>
        /// Ejecuta la sentencia actual (INSERT; DELETE; UPDATE)
        /// </summary>
        /// <returns></returns>
        public int Run()
        {
            if (this.TypeQuery == SqlSentence.SELECT)
                throw new SqlNoPermittedException("Can't be executed SELECT statements, call Query function", this);

            if (this.SafeMode && String.IsNullOrEmpty(this.sqlWhere) && (this.TypeQuery == SqlSentence.UPDATE || this.TypeQuery == SqlSentence.DELETE) && String.IsNullOrEmpty(this.sqlWhere) && String.IsNullOrEmpty(this.sqlJoins))
                throw new SqlNoPermittedException("Can't be executed UPDATE or DELETE statement without WHERE clause if 'Safe Mode' is enabled.", this);

            return this.Run(this.SQL, this.Args.Values.ToArray());
        }

        /// <summary>
        /// Ejecuta la sentencia actual (INSERT; DELETE; UPDATE), es sinónimo de Run
        /// </summary>
        /// <returns></returns>
        public int Save()
        {
            return this.Run();
        }

        /// <summary>
        /// Ejecuta la sentencia especificada (INSERT; DELETE; UPDATE)
        /// </summary>
        /// <param name="sql">Sentencia</param>
        /// <returns></returns>
        public int Run(string sql, object[] args)
        {
            return this.DbHelper.NonQuery(sql, args);
        }

        #endregion

        #region REFLECTION METHODS
        protected bool HasProperty(object obj, string name)
        {
            return (obj.GetType().GetRuntimeProperty(name) != null);
        }

        protected IEnumerable<string> ObjectToString(object obj, string format)
        {
            List<string> results = new List<string>();
            foreach (KeyValuePair<string, object> Par in this.ObjectToDictionary(obj))
            {
                results.Add(string.Format(format, Par.Key, this.AddSqlArgs(Par.Value)));
            }

            return results;
        }
        protected IDictionary<string, object> ObjectToDictionary<TEntity>(object obj, bool ignoreNull = false)
        {
            Type entityType = typeof(TEntity);
            Dictionary<string, object> results = new Dictionary<string, object>();

            PropertyInfo[] properties = obj.GetType().GetRuntimeProperties().ToArray();
            PropertyInfo[] propertiesEntity = entityType.GetRuntimeProperties().ToArray();
            foreach (PropertyInfo pInfo in properties)
            {
                if (this.SafeMode && propertiesEntity.Count(p => p.Name.Equals(pInfo.Name)) == 0)
                    throw new SqlArgsErrorException("The property " + pInfo.Name + " not exists in TEntity type", this);

                KeyValuePair<string, object> item = ObjectToDictionary(obj, pInfo, ignoreNull);
                if (!String.IsNullOrEmpty(item.Key))
                    results.Add(item.Key, item.Value);
            }

            return results;
        }
        protected IDictionary<string, object> ObjectToDictionary(object obj, bool ignoreNull = false)
        {
            Dictionary<string, object> results = new Dictionary<string, object>();

            PropertyInfo[] properties = obj.GetType().GetRuntimeProperties().ToArray();

            foreach (PropertyInfo pInfo in properties)
            {
                KeyValuePair<string, object> item = ObjectToDictionary(obj, pInfo, ignoreNull);
                if (!String.IsNullOrEmpty(item.Key))
                    results.Add(item.Key, item.Value);
            }

            return results;
        }
        protected KeyValuePair<string, object> ObjectToDictionary(object obj, PropertyInfo pInfo, bool ignoreNull = false)
        {
            if (this.IsIgnore(pInfo)) return new KeyValuePair<string, object>();
            object value = pInfo.GetValue(obj, null);
            if (ignoreNull && value == null) return new KeyValuePair<string, object>();
            //string strValue = this.ToSqlString(value, pInfo.PropertyType);
            return new KeyValuePair<string, object>(this.GetcolumnName(pInfo), value);
        }

        protected string GetTableName<TEntity>() where TEntity : class
        {
            return SqlAttributes.Manager.GetTableName<TEntity>();
        }
        protected bool IsIgnore(PropertyInfo property)
        {
            return SqlAttributes.Manager.IsIgnored(property) || this.SQLParser.IsIgnore(property);
        }
        protected string GetcolumnName<TEntity>(string pName) where TEntity : class
        {
            return SqlAttributes.Manager.GetColumnName<TEntity>(pName);
        }
        protected string GetcolumnName(PropertyInfo pInfo)
        {
            return SqlAttributes.Manager.GetColumnName(pInfo);
        }
        protected IEnumerable<string> GetAllcolumns<TEntity>() where TEntity : class
        {
            return this.GetAllcolumns(typeof(TEntity));
        }
        protected IEnumerable<string> GetAllcolumns(Type type)
        {
            List<string> columns = new List<string>();
            foreach (PropertyInfo p in type.GetRuntimeProperties())
            {
                if (!this.IsIgnore(p))
                    columns.Add(this.GetcolumnName(p));
            }

            return columns;
        }
        protected string GetAllColumnsString<T>() where T : class
        {
            string columns = "";
            string tableName = this.GetTableName<T>();
            foreach (string col in this.GetAllcolumns<T>())
            {
                if (!String.IsNullOrEmpty(columns)) columns += this.SQLParser.StrcolumnSeparator;
                columns += GetColumnNameSQL(tableName, col);
            }

            return columns;
        }
        protected string GetColumnNameSQL(string column, string table)
        {
            string tbl = String.Format(this.SQLParser.StrDBEncapsule, table);
            string col = column.Equals("*") ? column : String.Format(this.SQLParser.StrDBEncapsule, column);
            return tbl + "." + col;
        }
        protected string GetColumnNameSQL<T>(string column) where T : class
        {
            string table = this.GetTableName<T>();
            return this.GetColumnNameSQL(table, column);
        }
        protected string AddSqlArgs(object value)
        {
            string keyName = String.Format(this.SQLParser.StrARGS, this.Args.Count);
            this.Args.Add(keyName, value);
            return keyName;
        }
        #endregion

        /// <summary>
        /// Devuelve una cadena sql con los valores actuales del buelder
        /// </summary>
        /// <returns>Cadena sql</returns>
        public override string ToString()
        {
            return this.SQL;
        }
    }
}