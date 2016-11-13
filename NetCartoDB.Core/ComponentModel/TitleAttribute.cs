using System;

namespace NetCarto.Core.ComponentModel
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class TitleAttribute : Attribute
    {
        public string Name { get; set; }
        public TitleAttribute(string name)
        {
            this.Name = name;
        }
    }
}
