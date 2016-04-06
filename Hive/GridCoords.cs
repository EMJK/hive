using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hive
{
    struct GridCoords
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public GridCoords(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override bool Equals(object obj)
        {
            if (obj is GridCoords)
            {
                var coords = (GridCoords) obj;
                return coords.X == X && coords.Y == Y && coords.Z == Z;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return X ^ Y ^ Z;
        }

        public override string ToString()
        {
            return $"{{{X};{Y};{Z}}}";
        }

        public static GridCoords Zero => new GridCoords(0, 0, 0);
    }
}
