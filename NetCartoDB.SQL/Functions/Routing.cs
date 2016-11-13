using System;
using NetCarto.Core.Spatial.Geometry;

namespace NetCarto.SQL.Functions
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

        public string PointToPoint(Point origin, Point destination, Transport transport)
        {
            return String.Format("cdb_route_point_to_point('{0}'::geometry, '{1}'::geometry, {2})", origin, destination, transport);
        }

        public string PointToPoint(Point origin, Point destination, Transport transport, Options options)
        {
            return String.Format("cdb_route_point_to_point('{0}'::geometry, '{1}'::geometry, {2}, {3})", origin, destination, transport, options);
        }

        public string PointToPoint(Point origin, Point destination, Transport transport, Options options, Units units)
        {
            return String.Format("cdb_route_point_to_point('{0}'::geometry, '{1}'::geometry, {2}, {3}::text[], '{4}')", origin, destination, transport, options, units);
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
