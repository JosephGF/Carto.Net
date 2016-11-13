using System;

namespace NetCarto.SQL.Functions
{
    public class Geocoding
    {
        /// <summary>
        /// Geocodes the text name of a country, province or state into a Level-1 administrative region into a name geometry, displayed as polygon data
        /// </summary>
        /// <param name="country">Name of the country, province or state</param>
        /// <returns>Geometry (polygon, EPSG 4326) or null</returns>
        public static string CountryPolygon(string name)
        {
            return String.Format("cdb_geocode_admin0_polygon('{0}')", name);
        }

        /// <summary>
        /// Geocodes the text name of a province or state into a Level-1 administrative region into a name geometry, displayed as polygon data
        /// </summary>
        /// <param name="name">Name of the province or state</param>
        /// <param name="country">Name of the country</param>
        /// <returns>Geometry (polygon, EPSG 4326) or null</returns>
        public static string CountryPolygon(string name, string country)
        {
            return String.Format("cdb_geocode_admin0_polygon('{0}', '{1}')", name, country);
        }

        /// <summary>
        /// Geocodes the text name of a city into a named place geometry, displayed as point data
        /// </summary>
        /// <param name="city">Name of the city</param>
        /// <param name="country">Name of the country</param>
        /// <returns>Geometry (point, EPSG 4326) or null</returns>
        public static string NamePlacePoint(string name, string country)
        {
            return String.Format("cdb_geocode_namedplace_point('{0}', '{1}')", name, country);
        }

        /// <summary>
        /// Geocodes the text name of a city into a named place geometry, displayed as point data
        /// </summary>
        /// <param name="city">Name of the city</param>
        /// <param name="province">Name of the province/state in which the city is locate</param>
        /// <param name="country">Name of the country in which the city is locate</param>
        /// <returns>Geometry (point, EPSG 4326) or null</returns>
        public static string NamePlacePoint(string name, string province, string country)
        {
            return String.Format("cdb_geocode_namedplace_point('{0}', '{1}', '{2}')", name, province, country);
        }

        /// <summary>
        /// Geocodes the text name of a city into a named place geometry, displayed as point data
        /// </summary>
        /// <param name="code">Postal code</param>
        /// <param name="country">Name of the country in which the postal code is located</param>
        /// <returns>Geometry (point, EPSG 4326) or null</returns>
        public static string PostalCodePolygon(string code, string country)
        {
            return String.Format("cdb_geocode_postalcode_point('{0}', '{1}')", code, country);
        }

        /// <summary>
        /// Geocodes the text name of a city into a named place geometry, displayed as point data
        /// </summary>
        /// <param name="code">Postal code</param>
        /// <param name="country">Name of the country in which the postal code is located</param>
        /// <returns>Geometry (point, EPSG 4326) or null</returns>
        public static string IPAdressPoint(string ipAdress)
        {
            return String.Format("cdb_geocode_ipaddress_point('{0}')", ipAdress);
        }
    }
}
