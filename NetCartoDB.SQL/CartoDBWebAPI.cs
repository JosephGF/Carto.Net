using NetCarto.Core;
using NetCarto.Core.WebApi;
using NetCarto.SQL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCarto.SQL
{
    public class CartoWebAPI : WebAPIClient
    {
        public const string URL_SQL_API = "https://{0}.cartodb.com/api/v2/sql?q={1}&api_key={2}";

        public async static Task<CartoSQLResponseDto<T>> SQLQueryAsync<T>(Autentication auth, string query) where T : ICartoEntity, new()
        {
            string url = String.Format(URL_SQL_API, auth.UserName, query,  auth.ApiKey);
            CartoSQLResponseDto<T> result = await WebAPIClient.GetJsonAsync<CartoSQLResponseDto<T>>(url);
            return result;
        }

        public static CartoSQLResponseDto<T> SQLQuery<T> (Autentication auth, string query) where T : ICartoEntity, new()
        {
            string url = String.Format(URL_SQL_API, auth.UserName, query,  auth.ApiKey);
            CartoSQLResponseDto<T> result = WebAPIClient.GetJson<CartoSQLResponseDto<T>>(url);
            return result;
        }

        public async static Task<string> SQLQuery(Autentication auth, string query)
        {
            string url = String.Format(URL_SQL_API, query, auth.UserName, query, auth.ApiKey);
            return await WebAPIClient.GetAsync(url);
        }
    }
}
