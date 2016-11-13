using NetCarto.Core.ComponentModel;
using Newtonsoft.Json;

namespace NetCarto.SQL.Datasets
{
    [SQLTable(Name = "ign_spanish_adm3_municipalities")]
    public class SpanishMunicipalitiesDataset : ICartoEntity
    {
        [SQLColumn(Name = "cartodb_id")]
        [JsonProperty("cartodb_id")]
        public int CartoId { get; set; }
        [SQLColumn(Name = "the_geom")]
        [JsonProperty("the_geom")]
        public string Geometry { get; set; }
        [SQLColumn(Name = "codnut1")]
        [JsonProperty("codnut1")]
        public string Codnut1 { get; set; }
        [SQLColumn(Name = "codnut2")]
        [JsonProperty("codnut2")]
        public string Codnut2 { get; set; }
        [SQLColumn(Name = "codnut3")]
        [JsonProperty("codnut3")]
        public string Codnut3 { get; set; }
        [SQLColumn(Name = "country")]
        [JsonProperty("country")]
        public string Country { get; set; }
        [SQLColumn(Name = "inspireid")]
        [JsonProperty("inspireid")]
        public string InspireId { get; set; }
        [SQLColumn(Name = "nameunit")]
        [JsonProperty("nameunit")]
        public string Name { get; set; }
        [SQLColumn(Name = "natcode")]
        [JsonProperty("natcode")]
        public string NatCode { get; set; }
    }
}
