using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCarto.Map.Common
{
    public interface IMapHelper
    {
        object Execute(string function, params object[] args);
        object Execute(string function);
        object ExecuteAsync(string function, params object[] args);
        object ExecuteAsync(string function);

        void Create(MapOptions options);
    }
}
