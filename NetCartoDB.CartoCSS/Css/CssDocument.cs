using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCarto.CartoCSS
{
    public class CssDocument
    {
        protected Dictionary<string, CssSelectorBase> Class { get; set; } = new Dictionary<string, CssSelectorBase>();

        public CssSelectorBase this[string className]
        {
            get { return (Class.ContainsKey(className) ? Class[className] : null); }
            set { AddClass(value); }
        }

        public CssDocument() { }

        public CssDocument AddClass(string name, Dictionary<string, string> properties)
        {
            CssSelectorBase cls = new CssSelectorBase(name);

            foreach (var p in properties)
            {
                cls.Add(p.Key, p.Value);
            }

            return this;
        }

        public CssDocument AddClass(CssSelectorBase @class)
        {
            CartoCssSelector aux = this[@class.Selector] as CartoCssSelector;

            aux.Add(@class.Properties);

            return this;
        }
    }
}
