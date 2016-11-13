using NetCartoDB.Map.Common.Layers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCartoDB.Map.WinForms.Design.Layers.Objects
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class WinFormsTileLayerOptions : TileLayerOptions
    {
    }
}
