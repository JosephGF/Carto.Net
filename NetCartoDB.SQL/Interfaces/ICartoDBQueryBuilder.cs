using NetCarto.Core;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCarto.SQL
{
    public interface ICartoQueryBuilder
    {
        //IDictionary<string, string> QueryData { get; }

        ICartoQueryBuilder Where(params string[] data);
        ICartoQueryBuilder From(string data);

        ICartoQueryBuilder OrderBy(string data);
        ICartoQueryBuilder OrderByDescending(string data);
        ICartoQueryBuilder ThenBy(string data);
        ICartoQueryBuilder ThenByDescending(string data);
        ICartoQueryBuilder GroupBy(params string[] data);

        ICartoQueryBuilder Select(params string[] data);
        ICartoQueryBuilder Count(string data);
        ICartoQueryBuilder Sum(string data);
        ICartoQueryBuilder Avg(string data);
        ICartoQueryBuilder Max(string data);
        ICartoQueryBuilder Min(string data);
        ICartoQueryBuilder Take(int number);
        ICartoQueryBuilder Skip(int number);

        ICartoQueryBuilder Update(object data);
        ICartoQueryBuilder Insert(object data);
        ICartoQueryBuilder Delete(int id);

        object Execute(string sql);
        
        CartoGenericEntity[] ToArray(Autentication auth);
        List<CartoGenericEntity> ToList(Autentication auth);
        CartoGenericEntity First(Autentication auth);
        CartoGenericEntity Find(Autentication auth, int id);
        CartoGenericEntity FirstOrDefault(Autentication auth);
        string Raw(Autentication auth);
        Task<string> RawAsync(Autentication auth);

        string ToSqlString();
    }
}