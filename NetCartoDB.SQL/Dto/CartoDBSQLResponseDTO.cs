using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NetCarto.SQL.Dto
{
    public class CartoSQLResponseDto<T> where T : ICartoEntity
    {
        [JsonProperty("rows")]
        public List<T> Rows { get; set; }
        [JsonProperty("time")]
        public double Time { get; set; }
        [JsonProperty("total_rows")]
        public int TotalRows { get; set; }
    }
}
