using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCarto.Map.Common
{
    public interface IMapPlugin
    {
        string[] Scripts { get; }
        string[] Styles { get; }
    }
}
