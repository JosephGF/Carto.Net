using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCartoDB.SQL.Linq
{
    public interface IDbProvider
    {
        IEnumerable<TEntity> Query<TEntity>(string sql, object[] args);
        int NonQuery(string sql, object[] args);
    }
}
