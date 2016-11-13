using NetCarto.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCarto.SQL
{
    public interface ICartoContext
    {
        NetCarto.Core.Autentication Autentication { get; }
        //ICartoSet[] DataSet { get; }
    }
}
