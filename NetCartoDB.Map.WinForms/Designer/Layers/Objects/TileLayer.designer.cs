using NetCarto.Map.Common.Layers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace NetCarto.Map.WinForms.Design.Layers.Objects
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class TileLayer_Designer : TileLayer
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new TileLayerOptions_Designer Options { get; protected set; } = new TileLayerOptions_Designer();

        new virtual public string ToString()
        {
            return this.Name;
        }
    }
}
