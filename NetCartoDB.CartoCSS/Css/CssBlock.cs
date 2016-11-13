using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCarto.CartoCSS
{
    public class CssBlock
    {
        public string Selector { get; protected set; }
        internal Dictionary<string,string> Properties { get; set; } = new Dictionary<string, string>();
        
        public string this[string key] {
            get { return (Properties.ContainsKey(key) ? this.Properties[key] : string.Empty); }
            set { this.Add(key, value); }
        }

        public CssBlock(string selector)
        {
            this.Selector = selector;
        }

        public void Add(string property, string value)
        {
            if (Properties.ContainsKey(property))
                this.Properties[property] = value;
            else
                this.Properties.Add(property, value);
        }

        public void AddRange(Dictionary<string, string> properties)
        {
            foreach (var prop in properties)
            {
                this.Add(prop.Key, prop.Value);
            }
        }

        public string ToCssString()
        {
            var sb = new StringBuilder();
            sb.Append(this.Selector).AppendLine(" {");

            foreach (var prop in this.Properties)
                sb.AppendLine("\t").AppendFormat("{0}:{1};", prop.Key, prop.Value);

            sb.Append("}");

            return sb.ToString();
        }
    }
}
