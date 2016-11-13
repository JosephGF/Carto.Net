using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public static IEnumerable<TEntity> SelectData()
        {
            return new SqlBuilderLambda<TEntity>().Get<TEntity>();
        }

        public static IEnumerable<T> SelectData<T>() where T : class
        {
            return new SqlBuilderLambda<TEntity>().Get<T>();
        }

        public static IEnumerable<TEntity> SelectData(Expression<Func<TEntity, bool>> where)
        {
            return new SqlBuilderLambda<TEntity>(where).Get<TEntity>();
        }

        public static IEnumerable<T> SelectData<T>(Expression<Func<TEntity, bool>> where) where T : class
        {
            return new SqlBuilderLambda<TEntity>(where).Get<T>();
        }

        public static TEntity FirstRow(Expression<Func<TEntity, bool>> where)
        {
            return new SqlBuilderLambda<TEntity>().First(where);
        }

        public static TEntity FirstOrDefaultRow(Expression<Func<TEntity, bool>> where)
        {
            return new SqlBuilderLambda<TEntity>().FirstOrDefault(where);
        }

        public static bool ExistsRow(Expression<Func<TEntity, bool>> where)
        {
            return new SqlBuilderLambda<TEntity>().Exists(where);
        }

        public static int InsertData(TEntity data)
        {
            return new SqlBuilderLambda<TEntity>().Insert(data).Run();
        }

        public static int InsertData(Object data)
        {
            return new SqlBuilderLambda<TEntity>().Insert(data).Run();
        }

        public static int UpdateData(Expression<Func<TEntity, bool>> where, Object data)
        {
            return new SqlBuilderLambda<TEntity>(where).Update(data).Run();
        }

        public static int UpdateData(Expression<Func<TEntity, bool>> where, TEntity data)
        {
            return new SqlBuilderLambda<TEntity>(where).Update(data).Run();
        }

        public static int DeleteData(Expression<Func<TEntity, bool>> where)
        {
            return new SqlBuilderLambda<TEntity>(where).Run();
        }

        public static IEnumerable<TEntity> Execute(string sql)
        {
            return new SqlBuilder().Query<TEntity>(sql);
        }
    }
}