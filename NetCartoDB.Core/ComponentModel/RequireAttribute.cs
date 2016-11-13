using System;

namespace NetCarto.Core.ComponentModel
{
    [Obsolete("Use Newtownsoft instead of this")]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class RequiredAttribute : Attribute
    {
        public bool IsRequired { get; protected set; } = true;
        public RequiredAttribute() { }
        public RequiredAttribute(bool requided) { this.IsRequired = requided; }
    }
}
