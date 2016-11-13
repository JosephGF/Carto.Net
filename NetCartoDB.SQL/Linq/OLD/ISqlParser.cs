using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NetCartoDB.SQL.Linq
{
    public interface ISqlParser
    {
        #region SQL SINTAX
        string StrcolumnAlias { get; }
        string StrNULL { get; }
        string StrARGS { get; }
        string StrWHERE { get; }
        string StrWHERENull { get; }
        string StrVALUE { get; }
        string StrWHERELamda { get; }
        string StrJOIN { get; }
        string StrBETWEEN { get; }
        string StrIN { get; }
        string StrORDERBYSeparator { get; }
        string StrcolumnSeparator { get; }
        string StrGROPUPBYSeparator { get; }
        string StrINSeparator { get; }
        string StrFROMSeparator { get; }
        string StrDBEncapsule { get; }
        #endregion

        #region SQL Functions
        string StrCOUNT { get; }
        string StrSUM { get; }
        string StrMAX { get; }
        string StrMIN { get; }
        string StrAVG { get; }
        #endregion

        string MethodToSql(System.Reflection.MethodInfo method, Type valueType);
        string GenerateSQL(SqlBuilder sqlBuilder);
        string GetOperator(SqlConditions enumerator);
        string GetOperator(SqlJoin enumerator);
        string GetOperator(SqlOrder enumerator);
        string GetOperator(ExpressionType enumerator);
        bool IsIgnore(PropertyInfo property);
    }
}
