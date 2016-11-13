using System;
using NetCarto.Core.Spatial.Geometry;
using NetCarto.Core.ComponentModel;

namespace NetCarto.SQL.Linq.Functions
{
    public class Routing
    {
        public enum Transport
        {
            car, walk, bicycle, public_transport
        }

        public enum Units
        {
            kilometers, miles
        }

        [SQLFunctionExtensios("cdb_route_point_to_point('{0}'::geometry, '{1}'::geometry, {2})")]
        public Func<T, object> PointToPoint<T>(Point origin, Point destination, Transport transport) where T : ICartoEntity
        {
            return Constants.CARTODB_SQL_FUNCTION as Func<T, object>;
        }

        [SQLFunctionExtensios("cdb_route_point_to_point('{0}'::geometry, '{1}'::geometry, {2}, {3})")]
        public Func<T, object> PointToPoint<T>(Point origin, Point destination, Transport transport, Options options) where T : ICartoEntity
        {
            return Constants.CARTODB_SQL_FUNCTION as Func<T, object>;
        }

        [SQLFunctionExtensios("cdb_route_point_to_point('{0}'::geometry, '{1}'::geometry, {2}, {3}::text[], '{4}')")]
        public Func<T, object> PointToPoint<T>(Point origin, Point destination, Transport transport, Options options, Units units) where T : ICartoEntity
        {
            return Constants.CARTODB_SQL_FUNCTION as Func<T, object>;
        }

        public class Options
        {
            public enum RouteType
            {
                shortest
            }

            public RouteType ModeType { get; set; } = RouteType.shortest;

            public override string ToString()
            {
                return String.Format("mode_type={0}", this.ModeType);
            }
        }
    }
}
