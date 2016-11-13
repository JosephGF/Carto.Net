using System.Linq;

namespace NetCarto.SQL
{
    public interface ICartoDataset<T> :  ICartoQueryable<T> where T : ICartoEntity
    {
        ICartoContext Context { get; }
        string Table { get; }
    }
}
