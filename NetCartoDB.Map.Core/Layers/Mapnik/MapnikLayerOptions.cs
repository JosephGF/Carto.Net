using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NetCarto.Map.Common.Layers
{
    public class MapnikLayerOptions : ILayerOptions
    {
        [JsonProperty("sql", Required=Required.Always)]
        public string SQL { get; set; }
    }
}
