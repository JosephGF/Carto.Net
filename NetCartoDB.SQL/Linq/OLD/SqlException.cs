using System;

namespace NetCartoDB.SQL.Linq
{
    public class SqlBaseException : Exception
    {
        public string Sql { get; internal set; }
        public SqlSentence TypeQuery { get; internal set; }

        protected SqlBaseException(string message) : base(message) { }
        protected SqlBaseException(string message, SqlBuilder builder)
            : base(message)
        {
            this.Sql = builder.SQL;
            this.TypeQuery = builder.TypeQuery;
        }
    }

    public class SqlNoPermittedException : SqlBaseException
    {
        public SqlNoPermittedException(string message) : base(message) { }
        public SqlNoPermittedException(string message, SqlBuilder builder)
            : base(message)
        {
            this.Sql = builder.SQL;
            this.TypeQuery = builder.TypeQuery;
        }
    }

    public class SqlArgsErrorException : SqlBaseException
    {
        public SqlArgsErrorException(string message) : base(message) { }
        public SqlArgsErrorException(string message, SqlBuilder builder)
            : base(message)
        {
            this.Sql = builder.SQL;
            this.TypeQuery = builder.TypeQuery;
        }
    }
}
