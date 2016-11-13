using System.Collections.Generic;
using System.Linq;

namespace NetCarto.SQL
{
    public interface ICartoQueryable<T> where T : ICartoEntity
    {
        ICartoQueryBuilder Builder { get; }
        ICartoContext Context { get; }
    }
}
