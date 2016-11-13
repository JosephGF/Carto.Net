using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCarto.Map.Common.Layers
{
    public interface ILayer
    {
        string Name { get; set; }
        string Type { get; }
        ILayerOptions Options { get; }

        string Create();
        //string Remove();
    }
}
