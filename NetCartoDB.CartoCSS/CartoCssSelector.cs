using NetCarto.CartoCSS.Attributes;
using NetCarto.Core.Utils;
using NetCarto.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCarto.CartoCSS
{
    public class CartoCssSelector : CssSelectorBase
    {
        protected Dictionary<string, bool> IsSymbolize { get; private set; } = new Dictionary<string, bool>();

        protected Symbolyzer Symbolyzer { get; set; } = Symbolyzer.All;

        public CartoCssSelector(string className) : base(className) { }

        public Composite Composite { set { SetProperty(value, true); } }

        public Composite BackgroundComposite { set { SetProperty("background-image-comp-op", value, false); } }

        public double Opacity { set { SetProperty("opacity", value.ToString(), true); } }

        public double BackgroundOpacity { set { SetProperty("background-image-opacity", value.ToString(), false); } }

        public float BufferSize { set { SetProperty("buffer-size", value.ToString(), false); } }

        public void SetImageFilter(ImageFilter filter, double alpha)
        {
            string property = Reflection.GetAttribute<CartoCssPropertyAttribute>(filter.GetType()).Name;
            string function = Reflection.GetAttribute<CartoCssFunctionAttribute>(filter.GetType(), filter.ToString()).Format;
            
            SetPropertyFunction(property, function, false, new string[] { alpha.ToString()});
        }

        public void SetImageFilter(double alpha)
        {
            SetImageFilter(ImageFilter.ColorizeAlpha, alpha);
        }

        public void SetBackgroundImage(Background bg, string url)
        {
            string property = Reflection.GetAttribute<CartoCssPropertyAttribute>(bg.GetType()).Name;
            string function = Reflection.GetAttribute<CartoCssFunctionAttribute>(bg.GetType(), bg.ToString()).Format;

            SetPropertyFunction(property, function, false, new string[] { url });
        }

        public void SetBackgroundImage(string url)
        {
            SetBackgroundImage(Background.Uri, url);
        }

        protected void SetPropertyFunction(string property, string pattern, bool isSymbolizer, params string[] args)
        {
            string value = String.Format(pattern, args);
            this.SetProperty(property, value, isSymbolizer);
        }

        protected void SetProperty(object data, bool isSymbolizer)
        {
            string property = Reflection.GetAttribute<CartoCssPropertyAttribute>(data.GetType())?.Name;

            if (property == null)
                throw new ArgumentException("The Type of 'data' hasn't CartoCSSPropertyAttribute attribute");

            string value = Reflection.GetAttribute<CartoCssPropertyAttribute>(data.GetType(), data.ToString())?.Name;

            if (value == null)
                throw new ArgumentException("The property of 'data' hasn't CartoCSSPropertyAttribute attribute");

            this.SetProperty(property, value, isSymbolizer);
        }

        protected void SetProperty(string property, HtmlColor value, bool isSymbolizer)
        {
            this.SetProperty(property, value.ToHtmlColor(), isSymbolizer);
        }

        protected void SetProperty(string property, object data, bool isSymbolizer)
        {
            string value = Reflection.GetAttribute<CartoCssPropertyAttribute>(data.GetType(), data.ToString())?.Name;

            if (value == null)
                throw new ArgumentException("The property of 'data' hasn't CartoCSSPropertyAttribute attribute");

            this.SetProperty(property, value, isSymbolizer);
        }

        protected void SetProperty(string property, string value, bool isSymbolizer)
        {
            this.IsSymbolize.AppendOrReplace(property, isSymbolizer);
            this.Properties.AppendOrReplace(property, value);
        }

        protected object GetProperty(Type type)
        {
            string property = Reflection.GetAttribute<CartoCssPropertyAttribute>(type).Name;

            if (!this.Properties.ContainsKey(property))
                return null;

            return this.Properties[property];
        }

        public override string ToCssString()
        {
            StringBuilder sb = new StringBuilder();

            if (!String.IsNullOrEmpty(this.Selector)) 
                sb.AppendLine(this.Selector + " {");

            foreach (var item in Properties)
            {
                bool isSymbolize = this.IsSymbolize.ContainsKey(item.Key) ? this.IsSymbolize[item.Key] : false;

                if (isSymbolize)
                {
                    foreach (var s in this.Symbolyzer.GetIndividualFlags())
                    {
                        string str = Reflection.GetAttribute<CartoCssSymbolAttribute>(s.GetType(), s.ToString()).Name;
                        sb.AppendFormat("\t{1}-{2}:{3};", str, item.Key, item.Value).AppendLine();
                    }
                }
                else
                {
                    sb.AppendFormat("\t{1}:{2};", item.Key, item.Value).AppendLine();
                }
            }

            if (!String.IsNullOrEmpty(this.Selector))
                sb.AppendLine("}");

            return sb.ToString();
        }
    }
}