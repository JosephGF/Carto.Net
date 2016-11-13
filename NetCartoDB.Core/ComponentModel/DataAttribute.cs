using System;

namespace NetCarto.Core.ComponentModel
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class DataAttribute : Attribute
    {
        public string Data { get; set; }
        public DataAttribute(string data)
        {
            this.Data = data;
        }
    }
}
