using System;
using Newtonsoft.Json;


namespace NetCarto.Map.Common.Layers
{
    public class TileLayerOptions : ILayerOptions
    {
        /// <summary>
        /// Maximum zoom number
        /// </summary>
        /// 
        [JsonProperty("maxZoom")]
        public int MaxZoom { get; set; } = 18;
        /// <summary>
        /// Minimum zoom number
        /// </summary>
        [JsonProperty("minZoom")]
        public int MinZoom { get; set; } = 0;
        /// <summary>
        /// Maximum zoom number the tiles source has available. If it is specified, the tiles on all zoom levels higher than maxNativeZoom will be loaded from maxNativeZoom level and auto-scaled
        /// </summary>
        [JsonProperty("maxNativeZoom")]
        public int? MaxNativeZoom { get; set; } = null;
        /// <summary>
        /// Tile size (width and height in pixels, assuming tiles are square)
        /// </summary>
        [JsonProperty("tileSize")]
        public int TileSize { get; set; } = 256;
        /// <summary>
        /// URL to the tile image to show in place of the tile that failed to load
        /// </summary>
        [JsonProperty("errorTileUrl")]
        public string ErrorTileUrl { get; set; } = String.Empty;
        /// <summary>
        /// he string used by the attribution control, describes the layer data
        /// </summary>
        [JsonProperty("attribution")]
        public string Attribution { get; set; } = String.Empty;
        /// <summary>
        /// If true, inverses Y axis numbering for tiles (turn this on for TMS services).
        /// </summary>
        [JsonProperty("tms")]
        public bool Tms { get; set; } = false;
        /// <summary>
        /// f set to true, the tile coordinates won't be wrapped by world width (-180 to 180 longitude) or clamped to lie within world height (-90 to 90). Use this if you use Leaflet for maps that don't reflect the real world (e.g. game, indoor or photo maps).
        /// </summary>
        [JsonProperty("continuousWorld")]
        public bool ContinuousWorld { get; set; } = false;
        /// <summary>
        /// If set to true, the tiles just won't load outside the world width (-180 to 180 longitude) instead of repeating
        /// </summary>
        [JsonProperty("noWrap")]
        public bool NoWrap { get; set; } = false;
        /// <summary>
        /// The zoom number used in tile URLs will be offset with this value
        /// </summary>
        [JsonProperty("zoomOffset")]
        public int ZoomOffset { get; set; } = 0;
        /// <summary>
        /// If set to true, the zoom number used in tile URLs will be reversed (maxZoom - zoom instead of zoom).
        /// </summary>
        [JsonProperty("zoomReverse")]
        public bool ZoomReverse { get; set; } = false;
        /// <summary>
        /// The opacity of the tile layer
        /// </summary>
        [JsonProperty("opacity")]
        public double Opacity { get; set; } = 1.0;
        /// <summary>
        /// The explicit zIndex of the tile layer. Not set by default
        /// </summary>
        [JsonProperty("zIndex")]
        public int? ZIndex { get; set; } = null;
    }
}
