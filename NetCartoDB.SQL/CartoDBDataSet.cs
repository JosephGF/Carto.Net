using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCarto.Core.Utils;
using NetCarto.Core.ComponentModel;

namespace NetCarto.SQL
{
    public class CartoDataSet<T> : ICartoDataset<T> where T : ICartoEntity
    {
        public string Table { get; }
        public ICartoContext Context { get; private set; }
        public ICartoQueryBuilder Builder { get; private set; }
        
        public CartoDataSet(ICartoContext context) {
            this.Context = context;
            this.Table = GetTableName();
            this.Builder = new CartoQueryBuilder(this.Table);
        }

        private string GetTableName()
        {
            return Reflection.GetAttribute<SQLTableAttribute>(typeof(T))?.Name ?? typeof(T).Name;
        }
    }
}
