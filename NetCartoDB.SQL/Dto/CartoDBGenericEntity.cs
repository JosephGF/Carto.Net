using NetCarto.Core.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCarto.SQL
{
    public class CartoGenericEntity : Dictionary<string, string>, ICartoEntity
    {
        [JsonProperty("cartodb_id")]
        public int CartoId { get; set; }
        [JsonProperty("the_geom")]
        public string Geometry { get; set; }
    }
}
