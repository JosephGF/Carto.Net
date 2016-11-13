using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCarto.Core;

namespace NetCarto.SQL
{
    public class CartoContext : ICartoContext
    {
        public Autentication Autentication { get; }
        private ICartoQueryBuilder Builder { get; set; }

        public CartoContext(string userName)
        {
            this.Autentication = new Autentication(userName);
            Builder = new CartoQueryBuilder();
        }

        public CartoContext(string userName, string apiKey)
        {
            this.Autentication = new Autentication(userName, apiKey);
            Builder = new CartoQueryBuilder();
        }

        public CartoContext(Autentication Autentication, ICartoQueryBuilder builder)
        {
            this.Builder = builder;
        }

        public ICartoDataset<TEntity> Set<TEntity>() where TEntity : ICartoEntity
        {
            return new CartoDataSet<TEntity>(this);
        }
    }
}
