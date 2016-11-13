using NetCarto.CartoCSS.Attributes;
using System;

namespace NetCarto.CartoCSS
{
    public class CartoCssPolygon : CartoCssSelector
    {
        public HtmlColor Fill { set { this.SetProperty("fill", value, true); } }

        public int Gamma {
            set
            {
                if (value > 5 || value < 0)
                    throw new ArgumentException("Gamma can be between 0 and 5");
                this.SetProperty("gamma", value, true);
            }
        }

        public GammaType GammaMethod { set { this.SetProperty("gamma-method", value.ToString(), true); } }

        public bool Clip { set { this.SetProperty("clip", value.ToString().ToLower(), true); } }

        public float Simplify { set { this.SetProperty("simplify", value, true); } }

        public SimplifyAlgorithm SimplifyAgorithm { set { this.SetProperty("simplify-algorithm", value, true); } }

        public float Smooth { set { this.SetProperty("smooth", value, true); } }

        public string GeometryTransform { set { this.SetProperty("geometry-transform", value, true); } }

        public enum GammaType
        {
            power, lineal, none, threshold, multiply
        }

        public enum SimplifyAlgorithm {
            [CartoCSSValue("radial-distance")]
            radial,
            [CartoCSSValue("zhao-saalfeld")]
            zhao,
            [CartoCSSValue("saalfeld visvalingam-whyatt")]
            visvaligram
        }

        public CartoCssPolygon(string name) : base(name)
        {
            Symbolyzer = Symbolyzer.Polygon;
        }
    }
}
