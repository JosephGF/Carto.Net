using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetCarto.Core.ComponentModel;

namespace NetCarto.Map.Common.Layers
{
    public enum LayerType
    {
        /// <summary>
        /// rasterized tiles
        /// </summary>
        [Title("Mapnik Layer")]
        mapnik,
        /// <summary>
        /// an alias for mapnik 
        /// </summary>
        [Title("Carto Layer")]
        cartodb,
        /// <summary>
        /// render vector tiles in torque format
        /// </summary>
        [Title("Vector Torque Layer")]
        torque,
        /// <summary>
        /// load tiles over HTTP
        /// </summary>
        [Title("Http Layer")]
        http,
        /// <summary>
        /// color or background image url
        /// </summary>
        [Title("Plain Layer")]
        plain,
        /// <summary>
        /// use a Named Map as a layer
        /// </summary>
        [Title("Named Map Layer")]
        named
    }
}
