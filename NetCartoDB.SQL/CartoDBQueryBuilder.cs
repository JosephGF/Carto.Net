using NetCarto.Core;
using NetCarto.Core.ComponentModel;
using NetCarto.Core.Utils;
using NetCarto.SQL.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetCarto.SQL
{
    internal class CartoQueryBuilder : ICartoQueryBuilder
    {
        public static int SQL_API_TIMEOUT = 1000 * 15;

        private enum DataType
        {
            Column, Where, Order, Group, Table, Value
        }

        private enum QueryType { Select, Insert, Update, Delete }

        private QueryType TypeOfQuery { get; set; }
        private int?  Limit { get; set; } = null;
        private int? Offset { get; set; } = null;

        private Dictionary<DataType, List<string>> Data = new Dictionary<DataType, List<string>>()
        {
            { DataType.Column, new List<string>() },
            { DataType.Table, new List<string>() },
            { DataType.Where, new List<string>() },
            { DataType.Order, new List<string>() },
            { DataType.Group, new List<string>() },
            { DataType.Value, new List<string>() },
        };

        public CartoQueryBuilder() { }

        public CartoQueryBuilder(String table)
        {
            Data[DataType.Table] = new List<string>(1) { table };
        }

        public ICartoQueryBuilder Avg(string data)
        {
            Data[DataType.Column].Add(data);
            return this;
        }

        public ICartoQueryBuilder Max(string data)
        {
            Data[DataType.Column].Add(data);
            return this;
        }

        public ICartoQueryBuilder Min(string data)
        {
            Data[DataType.Column].Add(data);
            return this;
        }

        public ICartoQueryBuilder Count(string data)
        {
            Data[DataType.Column].Add(data);
            return this;
        }

        public ICartoQueryBuilder Sum(string data)
        {
            Data[DataType.Column].Add(data);
            return this;
        }

        public ICartoQueryBuilder From(string data)
        {
            Data[DataType.Table] = new List<string>(1) { data };
            return this;
        }

        public ICartoQueryBuilder GroupBy(params string[] data)
        {
            Data[DataType.Group] = new List<string>(data);
            return this;
        }
        
        public ICartoQueryBuilder OrderBy(string data)
        {
            Data[DataType.Order] = new List<string>() { data };
            return this;
        }

        public ICartoQueryBuilder ThenBy(string data)
        {
            Data[DataType.Order].Add(data);
            return this;
        }

        public ICartoQueryBuilder OrderByDescending(string data)
        {
            Data[DataType.Order] = new List<string>() { data + " desc" };
            return this;
        }

        public ICartoQueryBuilder ThenByDescending(string data)
        {
            Data[DataType.Order].Add(data + " desc");
            return this;
        }

        public ICartoQueryBuilder Select(params string[] data)
        {
            this.TypeOfQuery = QueryType.Select;
            if (data != null && data.Length > 0)
                Data[DataType.Column] = new List<string>(data);
            else
                Data[DataType.Column] = new List<string>() { "*" };

            return this;
        }

        public ICartoQueryBuilder Take(int number)
        {
            this.Limit = number;
            return this;
        }

        public ICartoQueryBuilder Skip(int number)
        {
            this.Offset = number;
            return this;
        }

        public ICartoQueryBuilder Update(object data)
        {
            this.TypeOfQuery = QueryType.Update;
            throw new NotImplementedException();
        }

        public ICartoQueryBuilder Insert(object data)
        {
            this.TypeOfQuery = QueryType.Insert;
            throw new NotImplementedException();
        }

        public ICartoQueryBuilder Delete(int id)
        {
            this.TypeOfQuery = QueryType.Delete;
            return this;
        }

        public ICartoQueryBuilder Where(params string[] data)
        {
            Data[DataType.Where] = new List<string>(data);
            return this;
        }
        
        private CartoSQLResponseDto<CartoGenericEntity> Get(Autentication auth)
        {
            return CartoWebAPI.SQLQuery<CartoGenericEntity>(auth, this.ToSqlString());
        }

        private Task<CartoSQLResponseDto<CartoGenericEntity>> GetAsync(Autentication auth)
        {
            return CartoWebAPI.SQLQueryAsync<CartoGenericEntity>(auth, this.ToSqlString());
        }

        public CartoGenericEntity Find(Autentication auth, int cartoDBId)
        {
            this.Where("cartodb_id=" + cartoDBId);
            return Get(auth).Rows.FirstOrDefault();
        }

        public CartoGenericEntity First(Autentication auth)
        {
            this.Take(1);
            return Get(auth).Rows.First();
        }

        public CartoGenericEntity FirstOrDefault(Autentication auth)
        {
            this.Take(1);
            return Get(auth).Rows.FirstOrDefault();
        }

        public CartoGenericEntity[] ToArray(Autentication auth)
        {
            return Get(auth).Rows.ToArray();
        }

        public List<CartoGenericEntity> ToList(Autentication auth)
        {
            return Get(auth).Rows;
        }

        public Task<CartoSQLResponseDto<CartoGenericEntity>> ToListAsync(Autentication auth)
        {
            return CartoWebAPI.SQLQueryAsync<CartoGenericEntity>(auth, this.ToSqlString());
        }

        public string Raw(Autentication auth)
        {
            var task =  CartoWebAPI.SQLQuery(auth, this.ToSqlString());
            task.Wait(SQL_API_TIMEOUT);
            return task.Result;
        }

        public async Task<string> RawAsync(Autentication auth)
        {
            return await CartoWebAPI.SQLQuery(auth, this.ToSqlString());
        }

        public object Execute(string sql)
        {
            return null;
        }

        public string ToSqlString()
        {
            StringBuilder sb = new StringBuilder();
            switch (this.TypeOfQuery)
            {
                case QueryType.Select:
                    sb.AppendFormat("Select {0} From {1}", String.Join(",", Data[DataType.Column]), Data[DataType.Table].FirstOrDefault());
                    break;
                case QueryType.Insert:
                    sb.AppendFormat("Insert Into {1} Values {0}", String.Join(",", Data[DataType.Column]), Data[DataType.Table].FirstOrDefault());
                    break;
                case QueryType.Update:
                    sb.AppendFormat("Update {1} Set {0}", String.Join(",", Data[DataType.Column]), Data[DataType.Table].FirstOrDefault());
                    break;
                case QueryType.Delete:
                    sb.AppendFormat("Delete From {0}", Data[DataType.Table].FirstOrDefault());
                    break;
            }

            if (!IsNullOrEmpty(Data[DataType.Where]))
                sb.AppendFormat(" Where 1=1 AND {0}", String.Join(" AND ", Data[DataType.Where]));

            if (this.TypeOfQuery == QueryType.Select && !IsNullOrEmpty(Data[DataType.Order]))
                sb.AppendFormat(" Order By {0}", String.Join(", ", Data[DataType.Order]));

            if (this.TypeOfQuery == QueryType.Select && !IsNullOrEmpty(Data[DataType.Group]))
                sb.AppendFormat(" Group By {0}", String.Join(", ", Data[DataType.Group]));
            
            if (this.Limit != null || this.Offset != null)
                sb.AppendFormat(" LIMIT {0} OFFSET {1}", this.Limit == null ? "ALL" : this.Limit.ToString(), this.Offset == null ? "0" : this.Offset.ToString());
            
            return sb.ToString();
        }
        
        private bool IsNullOrEmpty(IList list)
        {
            return (list == null || list.Count == 0);
        }

        private Dictionary<string, string> ObjectToSQL(object obj)
        {
            var properties = Reflection.GetProperties(obj.GetType());
            Dictionary<string, string>  result = new Dictionary<string, string>();
            for (int i = 0; i < properties.Count(); i++)
            {
                string name = Reflection.GetAttribute<SQLColumnAttribute>(properties.ElementAt(i))?.Name ?? properties.ElementAt(i).Name;
                string value = Reflection.GetValue(properties.ElementAt(i), obj)?.ToString() ?? "null";
                result.Add(name, value);
            }
            
            return result;
        }
    }
}
