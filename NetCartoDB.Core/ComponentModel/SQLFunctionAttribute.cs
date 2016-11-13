using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCarto.Core.ComponentModel
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SQLFunctionExtensiosAttribute : Attribute
    {
        public SQLFunctionExtensiosAttribute(string pattern) { this.Pattern = pattern; }

        public string Pattern { get; }
    }
}
