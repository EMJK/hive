using System;

namespace Hive.Common
{
    public class GridCoords : IEquatable<GridCoords>
    {
        public GridCoords(int x, int y, int z)
        {
            if (x + y + z != 0) throw new ArgumentException("The sum of x, y and z must be equal to 0");
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public bool Equals(GridCoords other)
        {
            return (X == other.X) && (Y == other.Y) && (Z == other.Z);
        }

        public override bool Equals(object obj)
        {
            if (obj is GridCoords)
            {
                var other = (GridCoords) obj;
                return Equals(other);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return X ^ Y ^ Z;
        }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}, Z: {Z}";
        }
    }
}