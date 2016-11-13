using NetCarto.CartoCSS.Attributes;
using NetCarto.Core.Utils;
using NetCarto.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCarto.CartoCSS
{
    public class CartoCss
    {
        public dynamic Polygon { get; set; }
        public dynamic Line { get; set; }
        public dynamic Markers { get; set; }
        public dynamic Shield { get; set; }
        public dynamic Line_Pattern { get; set; }
        public dynamic Polygon_Pattern { get; set; }
        public dynamic Raster { get; set; }
        public dynamic Point { get; set; }
        public dynamic Text { get; set; }
        public dynamic Building { get; set; }

        public string ToCssString()
        {
            string css = String.Empty;
            var properties = Reflection.GetProperties(this.GetType());

            foreach(var p in properties)
            {

            }

            return css;
        }
    }
}