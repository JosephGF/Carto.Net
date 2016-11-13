using NetCarto.Core.ComponentModel;
using NetCarto.Map.Common.Layers;
using System.ComponentModel;
using System.Reflection;
using System.Security.Permissions;

namespace NetCarto.Map.WinForms.Designer.Layers.Objects
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class CartoLayerDesigner : TileLayer
    {
        CartoLayers _kind = CartoLayers.Carto_DarkMatter;

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
        
        public new CartoLayers Type
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

        private string GetDescription(CartoLayers layerType)
        {
            FieldInfo fi = layerType.GetType().GetRuntimeField(layerType.ToString());
            return fi.GetCustomAttribute<DataAttribute>(true).Data;
        }

        public override string ToString()
        {
            return "Carto Layers";
        }
    }
}
