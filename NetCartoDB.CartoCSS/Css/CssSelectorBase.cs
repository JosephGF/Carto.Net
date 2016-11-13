using NetCarto.Core.Extensions;
using System.Collections.Generic;
using System.Text;

namespace NetCarto.CartoCSS
{
    public class CssSelectorBase : ICssSelector
    {
        public CssSelectorBase(string selector) { this.Selector = selector; }

        public string Selector { get; set; }
         
        public Dictionary<string, string> Properties { get; protected set; } = new Dictionary<string, string>();

        public void Add(string property, string value)
        {
            if (!this.Properties.ContainsKey(property))
                this.Properties.Add(property, value);
            else
                this.Properties[property] = value;
        }

        public void Add(Dictionary<string, string> properties)
        {
            foreach (var p in properties)
                this.Add(p.Key, p.Value);
        }
        
        public virtual string ToCssString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(this.Selector + " {");
            foreach (var item in Properties)
            {
                sb.AppendFormat("\t{1}:{2};", item.Key, item.Value).AppendLine();
            }

            return sb.AppendLine("}").ToString();
        }
    }
}
