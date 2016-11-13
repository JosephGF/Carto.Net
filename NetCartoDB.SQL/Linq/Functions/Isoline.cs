using System;
using NetCarto.Core.Spatial.Geometry;
using NetCarto.Core.ComponentModel;

namespace NetCarto.SQL.Linq.Functions
{
    public class Isoline
    {
        public enum Mode {
            Car,
            Walk
        }

        /// <summary>
        /// Displays a contoured line on a map, connecting geometries to a defined area, measured by an equal range of distance (in meters).
        /// </summary>
        /// <param name="point">Source point, in 4326 projection, which defines the start location</param>
        /// <param name="mode">Type of transport used to calculate the isolines</param>
        /// <param name="range">Range of the isoline, in meters</param>
        /// <returns></returns>
        [SQLFunctionExtensios("cdb_isodistance('{0}'::geometry, '{1}', ARRAY[{2}]::integer[])")]
        public static Func<T, object> IsoDistance<T>(Point point, Mode mode, int[] range) where T : ICartoEntity
        {
            return Constants.CARTODB_SQL_FUNCTION as Func<T, object>;
        }

        /// <summary>
        /// Displays a contoured line on a map, connecting geometries to a defined area, measured by an equal range of distance (in meters).
        /// </summary>
        /// <param name="point">Source point, in 4326 projection, which defines the start location</param>
        /// <param name="mode">Type of transport used to calculate the isolines</param>
        /// <param name="range">Range of the isoline, in meters</param>
        /// <param name="options">Multiple options to add more capabilities to the analysis.</param>
        /// <returns></returns>
        [SQLFunctionExtensios("cdb_isodistance('{0}'::geometry, '{1}', ARRAY[{2}]::integer[], ARRAY[{3}]::text[])")]
        public static Func<T, object> IsoDistance<T>(Point point, Mode mode, int[] range, Options options) where T : ICartoEntity
        {
            return Constants.CARTODB_SQL_FUNCTION as Func<T, object>;
        }

        /// <summary>
        /// Displays a contoured line on a map, connecting geometries to a defined area, measured by an equal range of distance (in meters).
        /// </summary>
        /// <param name="point">Source point, in 4326 projection, which defines the start location</param>
        /// <param name="mode">Type of transport used to calculate the isolines</param>
        /// <param name="range">Range of the isoline, in meters</param>
        /// <returns></returns>
        [SQLFunctionExtensios("cdb_isochrone('{0}'::geometry, '{1}', ARRAY[{2}]::integer[])")]
        public static Func<T, object> IsoChrone<T>(Point point, Mode mode, int[] range) where T : ICartoEntity
        {
            return Constants.CARTODB_SQL_FUNCTION as Func<T, object>;
        }

        /// <summary>
        /// Displays a contoured line on a map, connecting geometries to a defined area, measured by an equal range of distance (in meters).
        /// </summary>
        /// <param name="point">Source point, in 4326 projection, which defines the start location</param>
        /// <param name="mode">Type of transport used to calculate the isolines</param>
        /// <param name="range">Range of the isoline, in meters</param>
        /// <param name="options">Multiple options to add more capabilities to the analysis.</param>
        /// <returns></returns>
        [SQLFunctionExtensios("cdb_isochrone('{0}'::geometry, '{1}', ARRAY[{2}]::integer[], ARRAY[{3}]::text[])")]
        public static Func<T, object> IsoChrone<T>(Point point, Mode mode, int[] range, Options options) where T : ICartoEntity
        {
            return Constants.CARTODB_SQL_FUNCTION as Func<T, object>;
        }

        public class Options
        {
            public enum RouteType
            {
                shortest,
                faster
            }

            public enum State
            {
                enabled,
                disabled
            }

            public enum IsolineQuality
            {
                Low = 0,
                Medium = 1,
                Hight = 2,
            }

            /// <summary>
            /// If true, the source point is the destination instead of the starting location
            /// </summary>
            public bool IsDestination { get; set; } = false;
            /// <summary>
            /// Type of route calculation
            /// </summary>
            public RouteType ModeType { get; set; } = RouteType.shortest;
            /// <summary>
            /// Use traffic data to calculate the route
            /// </summary>
            public State ModeTraffic { get; set; } = State.disabled;
            /// <summary>
            /// Allows you to specify the level of detail needed for the isoline polygon. Unit is meters per pixel. Higher resolution may increase the response time of the service
            /// </summary>
            public string Resolution { get; set; } = String.Empty;
            /// <summary>
            /// Allows you to limit the amount of points in the returned isoline. If the isoline consists of multiple components, the sum of points from all components is considered. Each component will have at least two points. It is possible that more points than specified could be returned, in case when 2 * number of components is higher than the maxpoints value itself. Increasing the number of maxpoints may increase the response time of the service.	
            /// </summary>
            public int MaxPoints { get; set; } = 999999;
            /// <summary>
            /// Allows you to reduce the quality of the isoline in favor of the response time.
            /// </summary>
            public IsolineQuality Quality { get; set; } = IsolineQuality.Medium;

            public override string ToString()
            {
                return String.Format("is_destination={0},mode_type={1},mode_traffic={2},resolution={3},maxpoints={4},quality={5}", this.IsDestination, this.ModeType, this.ModeTraffic, this.Resolution, this.MaxPoints, (int)this.Quality);
            }
        }
    }
}
