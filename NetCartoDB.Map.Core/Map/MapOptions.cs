using NetCarto.Map.Common.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NetCarto.Map.Common
{
    public class MapOptions : IDisposable
    {
        [JsonProperty("zoom")]
        public int Zoom { get; set; } = 6;
        [JsonProperty("maxZoom")]
        public int MaxZoom { get; set; } = 18;
        [JsonProperty("minZoom")]
        public int MinZoom { get; set; } = 0;
        [JsonProperty("zoomControl")]
        public bool ZoomControl { get; set; } = false;
        [JsonProperty("scrollWheelZoom")]
        public bool ScrollWheelZoom { get; set; } = true;
        [JsonProperty("doubleClickZoom")]
        public bool DoubleClickZoom { get; set; } = true;
        [JsonProperty("touchZoom")]
        public bool TouchZoom { get; set; } = true;
        //public bool DoubleClickZoom { get; set; } = true;

        [JsonProperty("version")]
        public string Version { get; set; } = "1.0.0";
        [JsonProperty("center")]
        public double[] Center { get; } = new double[2] { 0, 0 };
        [JsonProperty("maxBounds")]
        public List<double[]> MaxBounds { get; } = new List<double[]>(2) { new double[2] { -85, -180 }, new double[2] { 85, 180 } };

        //[Serialize("srid")]
        [JsonIgnore]
        public string SRID { get; set; } = "3857";

        [JsonIgnore]
        public LayersCollection Layers { get; set; } = new LayersCollection();
        
        public void Dispose() {  }
    }
}
