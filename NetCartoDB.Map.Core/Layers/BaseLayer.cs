using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCarto.Map.Common.Layers
{
    public class BaseLayer : ILayer
    {
        private static int LayersCreated = 0;

        public BaseLayer()
        {
            this.Name = "Layer_" + (++LayersCreated).ToString("00");
        }

        public string Name { get; set; }

        public virtual ILayerOptions Options { get; protected set; }

        public virtual string Type { get { return "none"; } }

        public virtual string Create()
        {
            return String.Empty;
        }
    }
}
