using System.Collections.Generic;

namespace NetCarto.CartoCSS
{
    interface ICssSelector
    {
        Dictionary<string, string> Properties { get; }

        string Selector { get; set; }

        void Add(string property, string value);

        void Add(Dictionary<string, string> properties);

        string ToCssString();
    }
}
