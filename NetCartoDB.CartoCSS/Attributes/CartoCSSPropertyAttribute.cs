using System;

namespace NetCarto.CartoCSS.Attributes
{
    public class CartoCSSValueAttribute : Attribute
    {
        public Symbolyzer Symbolyzer { get; } = Symbolyzer.All;
        public string Name { get; }

        public CartoCSSValueAttribute(string name, Symbolyzer symbolizer) : this(name)
        {
            this.Symbolyzer = symbolizer;
        }

        public CartoCSSValueAttribute(string name) : this()
        {
            this.Name = name;
        }

        public CartoCSSValueAttribute()
        {

        }
    }
}
