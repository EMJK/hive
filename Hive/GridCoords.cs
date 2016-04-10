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

        public GridCoords[] GetSurroundingCoords()
        {
            return new[]
            {
                Add(1, 0, -1),
                Add(1, -1, 0),
                Add(0, -1, 1),
                Add(-1, 0, 1),
                Add(-1, 1, 0),
                Add(0, 1, -1)
            };
        }

        public GridCoords Add(GridCoords coords)
        {
            return new GridCoords(X + coords.X, Y + coords.Y, Z + coords.Z);
        }

        public GridCoords Add(int x, int y, int z)
        {
            return new GridCoords(X + x, Y + y, Z + z);
        }

        public GridCoords Substract(GridCoords coords)
        {
            return new GridCoords(X - coords.X, Y - coords.Y, Z - coords.Z);
        }

        public GridCoords Substract(int x, int y, int z)
        {
            return new GridCoords(X - x, Y - y, Z - z);
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
