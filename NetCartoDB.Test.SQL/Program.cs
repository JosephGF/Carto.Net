using System;
using System.Linq;

using NetCarto.SQL.Linq.Extensions;
using NetCarto.SQL.Linq.Functions;

namespace NetCarto.Test.SQL
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new MyCartoContext();
            var muni = new string[] { "Oria", "Pedro Muñoz" };
            var sql = db.SpanishMunicipalitiesDataset.Select()
                                                     //.Where(c => c.CartoId < 5)
                                                     //.Where(c => c.Name == "Oria" || c.Name == "Pedro Muñoz") // , c => c.Geometry.Equals(c.CountryPolygon("Espana"))
                                                     .Where(c => (muni.Contains(c.Name)))//, c => c.Geometry.Equals(c.CountryPolygon("Espana")))
                                                     //.OrderBy(c => c.Name)
                                                     //.ThenByDescending(c => c.Country)
                                                     //.GroupBy(c => c.Geometry)
                                                     .ToList();
                                                     //.ToSqlString();
            Console.WriteLine(sql);
            Console.ReadLine();
        }
    }
}
