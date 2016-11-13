using NetCartoDB.Map.Common.Layers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace NetCartoDB.Map.WinForms.Design.Layers.Objects
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class WinFormsTileLayer : TileLayer
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public new WinFormsTileLayerOptions Options { get; protected set; } = new WinFormsTileLayerOptions();

        new virtual public string ToString()
        {
            return this.Name;
        }
    }
}
