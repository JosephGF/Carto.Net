using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetCarto.Core.Spatial.Geometry
{
    public abstract class Geometry
    {
        public abstract bool IsEmpty { get; protected set; }

        protected static List<double> WKTToValues(string pattern, string wkt)
        {
            List<double> coordinates = new List<double>(4);
            Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = reg.Match(wkt);
            while (match.Success)
            {
                coordinates.Add(Convert.ToDouble(match.Value));
                match = match.NextMatch();
            }

            return coordinates;
        }
    }
}
