using System;

namespace NetCarto.CartoCSS.Attributes
{
    public class CartoCssPropertyAttribute : Attribute
    {
        public string Name { get; set; }

        public CartoCssPropertyAttribute(string name)
        {
            Name = name;
        }
    }

    public class CartoCssFunctionAttribute : Attribute
    {
        public string Format { get; set; }

        public CartoCssFunctionAttribute(string format)
        {
            Format = format;
        }
    }

    public class CartoCssSymbolAttribute : Attribute
    {
        public string Name { get; set; }

        public CartoCssSymbolAttribute(string name)
        {
            Name = name;
        }
    }
}