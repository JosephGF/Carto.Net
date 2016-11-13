using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetCarto.Core.Spatial.Geometry
{
    public class Point : Geometry
    {
        public static Point Empty = new Point();

        public static implicit operator Point(string wkt)
        {
            List<double> coordinates = WKTToValues(@"(-?(?:\d+\.\d*|(?:\d+)))", wkt);

            if (coordinates.Count < 2)
            {
                return new Point();
            }
            if (coordinates.Count > 4)
                throw new InvalidCastException("Wkt Point geometry is invalid ("+wkt+")");

            var point = new Point(coordinates[0], coordinates[1]);

            if (coordinates.Count > 2)
                point.Z = coordinates[2];

            if (coordinates.Count > 3)
                point.M = coordinates[3];

            return point;
        }

        public Point()
        {
            IsEmpty = true;
        }

        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public override bool IsEmpty
        {
            get; protected set;
        }
        
        public double X { get; private set; }

        public double Y { get; private set; }

        public double? Z { get; private set; }

        public double? M { get; private set; }

        public override string ToString()
        {
            if (this.IsEmpty)
                return "POINT EMPTY";

            if (Z == null)
                return String.Format("POINT({0} {1})", this.X, this.Y);

            if (M == null)
                return String.Format("POINT({0} {1} {2})", this.X, this.Y, this.Z);
            
            return String.Format("POINT({0} {1} {2} {3})", this.X, this.Y, this.Z, this.M);
        }
    }
}
