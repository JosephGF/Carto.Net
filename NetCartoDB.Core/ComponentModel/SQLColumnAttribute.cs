using System;

namespace NetCarto.Core.ComponentModel
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class SQLColumnAttribute : Attribute
    {
        public string Name { get; set; }
        public bool Requided { get; set; }
    }
}
