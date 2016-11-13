using System;
using NetCarto.SQL;
using NetCarto.SQL.Datasets;

namespace NetCarto.Test.SQL
{
    public class MyCartoContext : CartoContext
    {
        public ICartoDataset<SpanishMunicipalitiesDataset> SpanishMunicipalitiesDataset { get { return this.Set<SpanishMunicipalitiesDataset>(); } }
        public MyCartoContext() : base("josephgironflores", "92b4a0a1637d088b45600416ddd9d4b80e2f0b43") { }
    }
}
