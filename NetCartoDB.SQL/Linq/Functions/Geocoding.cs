using System;
using System.Linq.Expressions;
using NetCarto.Core.ComponentModel;

namespace NetCarto.SQL.Linq.Functions
{
    public static class Constants
    {
        public static Func<ICartoEntity, object> CARTODB_SQL_FUNCTION = (e) => { return true; };
    }

    public static class Geocoding
    {
        /// <summary>
        /// Geocodes the text name of a country, province or state into a Level-1 administrative region into a name geometry, displayed as polygon data
        /// </summary>
        /// <param name="country">Name of the country, province or state</param>
        /// <returns>Geometry (polygon, EPSG 4326) or null</returns>
        [SQLFunctionExtensios("cdb_geocode_admin0_polygon('{0}')")]
        public static Func<T, object> CountryPolygon<T>(this T entity, string name) where T : ICartoEntity
        {
            return Constants.CARTODB_SQL_FUNCTION as Func<T, object>;
        }

        /// <summary>
        /// Geocodes the text name of a province or state into a Level-1 administrative region into a name geometry, displayed as polygon data
        /// </summary>
        /// <param name="name">Name of the province or state</param>
        /// <param name="country">Name of the country</param>
        /// <returns>Geometry (polygon, EPSG 4326) or null</returns>
        [SQLFunctionExtensios("cdb_geocode_admin0_polygon('{0}', '{1}')")]
        public static Func<T, object> CountryPolygon<T>(this T entity, string name, string country) where T : ICartoEntity
        {
            return Constants.CARTODB_SQL_FUNCTION as Func<T, object>;
        }

        /// <summary>
        /// Geocodes the text name of a city into a named place geometry, displayed as point data
        /// </summary>
        /// <param name="city">Name of the city</param>
        /// <param name="country">Name of the country</param>
        /// <returns>Geometry (point, EPSG 4326) or null</returns>
        [SQLFunctionExtensios("cdb_geocode_namedplace_point('{0}', '{1}')")]
        public static Func<T, object> NamePlacePoint<T>(this T entity, string name, string country) where T : ICartoEntity
        {
            return Constants.CARTODB_SQL_FUNCTION as Func<T, object>;
        }

        /// <summary>
        /// Geocodes the text name of a city into a named place geometry, displayed as point data
        /// </summary>
        /// <param name="city">Name of the city</param>
        /// <param name="province">Name of the province/state in which the city is locate</param>
        /// <param name="country">Name of the country in which the city is locate</param>
        /// <returns>Geometry (point, EPSG 4326) or null</returns>
        [SQLFunctionExtensios("cdb_geocode_namedplace_point('{0}', '{1}', '{2}')")]
        public static Func<T, object> NamePlacePoint<T>(this T entity, string name, string province, string country) where T : ICartoEntity
        {
            return Constants.CARTODB_SQL_FUNCTION as Func<T, object>;
        }

        /// <summary>
        /// Geocodes the text name of a city into a named place geometry, displayed as point data
        /// </summary>
        /// <param name="code">Postal code</param>
        /// <param name="country">Name of the country in which the postal code is located</param>
        /// <returns>Geometry (point, EPSG 4326) or null</returns>
        [SQLFunctionExtensios("cdb_geocode_postalcode_point('{0}', '{1}')")]
        public static Func<T, object> PostalCodePolygon<T>(this T entity, string code, string country) where T : ICartoEntity
        {
            return Constants.CARTODB_SQL_FUNCTION as Func<T, object>;
        }

        /// <summary>
        /// Geocodes the text name of a city into a named place geometry, displayed as point data
        /// </summary>
        /// <param name="code">Postal code</param>
        /// <param name="country">Name of the country in which the postal code is located</param>
        /// <returns>Geometry (point, EPSG 4326) or null</returns>
        [SQLFunctionExtensios("cdb_geocode_ipaddress_point('{0}')")]
        public static Func<T, object> IPAdressPoint<T>(this T entity, string ipAdress) where T : ICartoEntity
        {
            return Constants.CARTODB_SQL_FUNCTION as Func<T, object>;
        }
    }
}
