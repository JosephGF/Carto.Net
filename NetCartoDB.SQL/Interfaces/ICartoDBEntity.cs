using NetCarto.Core.ComponentModel;

namespace NetCarto.SQL
{
    public interface ICartoEntity
    {
        [SQLColumn(Name = "cartodb_id")]
        int CartoId { get; set; }
        [SQLColumn(Name = "the_geom")]
        string Geometry { get; set; }
    }
}
