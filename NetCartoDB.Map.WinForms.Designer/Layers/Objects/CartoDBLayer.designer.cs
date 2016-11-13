using NetCartoDB.Core.ComponentModel;
using NetCartoDB.Map.Common.Layers;
using System.ComponentModel;
using System.Reflection;
using System.Security.Permissions;

namespace NetCartoDB.Map.WinForms.Designer.Layers.Objects
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class CartoDBLayerDesigner : TileLayer
    {
        CartoDBLayers _kind = CartoDBLayers.CartoDB_DarkMatter;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new string Url
        {
            get {
                return base.Url;
            }
            private set {
                base.Url = value;
            }
        }
        
        public new CartoDBLayers Type
        {
            get
            {
                return _kind;
            }
            set
            {
                _kind = value;
                this.Url = GetDescription(value);
            }
        }

        private string GetDescription(CartoDBLayers layerType)
        {
            FieldInfo fi = layerType.GetType().GetRuntimeField(layerType.ToString());
            return fi.GetCustomAttribute<DataAttribute>(true).Data;
        }

        public override string ToString()
        {
            return "CartoDB Layers";
        }
    }
}
