using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCartoDB.SQL.Linq.CartoDBConfigurations
{
    internal class CartoDBProvider : IDbProvider
    {
        private string _connectionStr = "https://{username}.cartodb.com/api/v2/sql?q={sql}";
        private string _sqlClient = "NetCartoDB";

        public CartoDBProvider() { }
        public CartoDBProvider(string connectonString)
        {
            this._connectionStr = connectonString;
        }
        public CartoDBProvider(string connectonString, string sqlClient)
        {
            this._connectionStr = connectonString;
            this._sqlClient = sqlClient;
        }

        public IEnumerable<TEntity> Query<TEntity>(string sql, object[] args)
        {
            return this.ExecuteQuery<TEntity>(sql, args);
        }
        public int NonQuery(string sql, object[] args)
        {
            return this.ExecuteNonQuery(sql, args);
        }

        private int ExecuteNonQuery(string sql, object[] args)
        {
            int result = -1;
            return result;
        }
        private IEnumerable<TEntity> ExecuteQuery<TEntity>(string sql, object[] args)
        {
            List<TEntity> rows = new List<TEntity>();
            
            try
            {
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }

            return rows;
        }

    }
}
