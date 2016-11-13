using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCartoDB.Core.ComponentModel;

namespace NetCartoDB.Map.Common.Layers
{
    public class MapnikLayerOptions : LayerOptions
    {
        [Required]
        public string SQL { get; set; }
    }
}
