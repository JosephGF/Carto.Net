using Newtonsoft.Json;
using System;

namespace NetCarto.Map.Common.Layers
{
    public class WmsLayerOptions : ILayerOptions
    {
        /// <summary>
        /// (required) Comma-separated list of WMS layers to show.
        /// </summary>
        [JsonProperty("layers")]
        public string Layers { get; set; } = String.Empty;
        /// <summary>
        /// Comma-separated list of WMS styles
        /// </summary>
        [JsonProperty("styles")]
        public string Styles { get; set; } = String.Empty;
        /// <summary>
        /// WMS image format (use 'image/png' for layers with transparency)
        /// </summary>
        [JsonProperty("format")]
        public string Format { get; set; } = "image/png'";
        /// <summary>
        /// Version of the WMS service to use
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; } = "1.1.1";
        /// <summary>
        /// If true, the WMS service will return images with transparency
        /// </summary>
        [JsonProperty("transparent")]
        public bool Transparent { get; set; } = false;
        //public CRS CRS;
    }
}
