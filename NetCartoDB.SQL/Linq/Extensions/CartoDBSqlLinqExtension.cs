using NetCarto.Core;
using NetCarto.SQL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetCarto.SQL.Linq.Extensions
{
    public static class CartoSqlLinqExtension
    {
        public static ICartoQueryable<T> Select<T>(this ICartoQueryable<T> iCartoQueryable, params Expression<Func<T, Object>>[] columns) where T : ICartoEntity
        {
            string[] str = null;
            if (columns != null || columns.Any())
            {
                str = new string[columns.Length];
                for (int i = 0; i < columns.Length; i++)
                    str[i] = LamdaToSqlParser.LambdaExpresionTranslate<T>(columns[i]); //TODO: get columns from expressions
            }

            iCartoQueryable.Builder.Select(str);
            return iCartoQueryable;
        }

        public static ICartoQueryable<T> Select<T>(this ICartoQueryable<T> iCartoQueryable, string[] columns) where T : ICartoEntity
        {
            iCartoQueryable.Builder.Select(columns);
            return iCartoQueryable;
        }

        public static ICartoQueryable<T> Where<T>(this ICartoQueryable<T> iCartoQueryable, params Expression<Func<T, bool>>[] clausule) where T : ICartoEntity
        {
            string[] str = null;

            if (clausule != null || clausule.Any())
            {
                str = new string[clausule.Length];
                for (int i = 0; i < clausule.Length; i++)
                    str[i] = LamdaToSqlParser.LambdaExpresionTranslate<T>(clausule[i]); //TODO: get columns from expressions
            }

            iCartoQueryable.Builder.Where(str);

            return iCartoQueryable;
        }

        public static ICartoQueryable<T> Update<T>(this ICartoQueryable<T> iCartoQueryable, T entity) where T : ICartoEntity
        {
            string[] str = null;

            if (entity != null)
                str = null; //TODO: get columns from expressions

            iCartoQueryable.Builder.Update(str);


            return iCartoQueryable;
        }

        public static ICartoQueryable<T> Delete<T>(this ICartoQueryable<T> iCartoQueryable, T entity) where T : ICartoEntity
        {
            string[] str = null;

            if (entity != null)
                str = null; //TODO: get columns from expressions

            iCartoQueryable.Builder.Delete(entity.CartoId);
            
            return iCartoQueryable;
        }

        public static ICartoQueryable<T> Sum<T>(this ICartoQueryable<T> iCartoQueryable, Expression<Func<T, Object>> column) where T : ICartoEntity
        {
            string str = null;

            if (column != null)
                str = LamdaToSqlParser.LambdaExpresionTranslate<T>(column);

            iCartoQueryable.Builder.Sum(str);

            return iCartoQueryable;
        }

        public static ICartoQueryable<T> Avg<T>(this ICartoQueryable<T> iCartoQueryable, Expression<Func<T, Object>> column) where T : ICartoEntity
        {
            string str = null;

            if (column != null)
                str = LamdaToSqlParser.LambdaExpresionTranslate<T>(column);

            iCartoQueryable.Builder.Avg(str);

            return iCartoQueryable;
        }

        public static ICartoQueryable<T> Count<T>(this ICartoQueryable<T> iCartoQueryable, Expression<Func<T, Object>> column) where T : ICartoEntity
        {
            string str = null;

            if (column != null)
                str = LamdaToSqlParser.LambdaExpresionTranslate<T>(column);

            iCartoQueryable.Builder.Count(str);

            return iCartoQueryable;
        }

        public static ICartoQueryable<T> Max<T>(this ICartoQueryable<T> iCartoQueryable, Expression<Func<T, Object>> column) where T : ICartoEntity
        {
            string str = null;

            if (column != null)
                str = LamdaToSqlParser.LambdaExpresionTranslate<T>(column);

            iCartoQueryable.Builder.Max(str);

            return iCartoQueryable;
        }

        public static ICartoQueryable<T> Min<T>(this ICartoQueryable<T> iCartoQueryable, Expression<Func<T, Object>> column) where T : ICartoEntity
        {
            string str = null;

            if (column != null)
                str = LamdaToSqlParser.LambdaExpresionTranslate<T>(column);

            iCartoQueryable.Builder.Min(str);

            return iCartoQueryable;
        }

        public static ICartoQueryable<T> OrderBy<T>(this ICartoQueryable<T> iCartoQueryable, Expression<Func<T, Object>> column) where T : ICartoEntity
        {
            string str = null;
            if (column != null)
                str = LamdaToSqlParser.LambdaExpresionTranslate<T>(column);

            iCartoQueryable.Builder.OrderBy(str);

            return iCartoQueryable;
        }

        public static ICartoQueryable<T> ThenBy<T>(this ICartoQueryable<T> iCartoQueryable, Expression<Func<T, Object>> column) where T : ICartoEntity
        {
            string str = null;
            if (column != null)
                str = LamdaToSqlParser.LambdaExpresionTranslate<T>(column);

            iCartoQueryable.Builder.ThenBy(str);

            return iCartoQueryable;
        }

        public static ICartoQueryable<T> OrderByDescending<T>(this ICartoQueryable<T> iCartoQueryable, Expression<Func<T, Object>> column) where T : ICartoEntity
        {
            string str = null;
            if (column != null)
                str = LamdaToSqlParser.LambdaExpresionTranslate<T>(column); 
            
            iCartoQueryable.Builder.OrderByDescending(str);

            return iCartoQueryable;
        }

        public static ICartoQueryable<T> ThenByDescending<T>(this ICartoQueryable<T> iCartoQueryable, Expression<Func<T, Object>> column) where T : ICartoEntity
        {
            string str = null;
            if (column != null)
                str = LamdaToSqlParser.LambdaExpresionTranslate<T>(column);

            iCartoQueryable.Builder.ThenByDescending(str);

            return iCartoQueryable;
        }

        public static ICartoQueryable<T> GroupBy<T>(this ICartoQueryable<T> iCartoQueryable, params Expression<Func<T, Object>>[] columns) where T : ICartoEntity
        {
            string[] str = null;
            if (columns != null || columns.Any())
            {
                str = new string[columns.Length];
                for (int i = 0; i < columns.Length; i++)
                    str[i] = LamdaToSqlParser.LambdaExpresionTranslate<T>(columns[i]); //TODO: get columns from expressions
            }

            iCartoQueryable.Builder.GroupBy(str);

            return iCartoQueryable;
        }

        public static ICartoQueryable<T> Take<T>(this ICartoQueryable<T> iCartoQueryable, int number) where T : ICartoEntity
        {
            iCartoQueryable.Builder.Take(number);

            return iCartoQueryable;
        }

        public static ICartoQueryable<T> Skip<T>(this ICartoQueryable<T> iCartoQueryable, int number) where T : ICartoEntity
        {
            iCartoQueryable.Builder.Skip(number);

            return iCartoQueryable;
        }

        public static string ToSqlString<T>(this ICartoQueryable<T> iCartoQueryable) where T : ICartoEntity
        {
            return iCartoQueryable.Builder.ToSqlString();
        }

        public static List<T> ToList<T>(this ICartoQueryable<T> iCartoQueryable) where T : ICartoEntity, new()
        {
            return Get(iCartoQueryable, iCartoQueryable.Context.Autentication).Rows;
        }

        public static T[] ToArray<T>(this ICartoQueryable<T> iCartoQueryable) where T : ICartoEntity, new()
        {
            return Get(iCartoQueryable, iCartoQueryable.Context.Autentication).Rows.ToArray();
        }

        public static T First<T>(this ICartoQueryable<T> iCartoQueryable) where T : ICartoEntity, new()
        {
            return Get(iCartoQueryable, iCartoQueryable.Context.Autentication).Rows.First();
        }

        public static T FirstOrDefault<T>(this ICartoQueryable<T> iCartoQueryable) where T : ICartoEntity, new()
        {
            return Get(iCartoQueryable, iCartoQueryable.Context.Autentication).Rows.FirstOrDefault();
        }

        private static CartoSQLResponseDto<T> Get<T>(ICartoQueryable<T> iCartoQueryable, Autentication auth) where T : ICartoEntity, new()
        {
            return CartoWebAPI.SQLQuery<T>(auth, iCartoQueryable.ToSqlString());
        }
    }
}
