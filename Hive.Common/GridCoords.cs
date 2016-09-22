using System;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

namespace Hive.Common
{
    public class GridCoords : IEquatable<GridCoords>
    {
        public const int BoardWidth = 21;
        public const int BoardHeight = 21;
        public int CX { get; set; }
        public int CY { get; set; }
        public int CZ { get; set; }
        public int OX { get; set; }
        public int OY { get; set; }
        
        [JsonConstructor]
        private GridCoords() : this(0,0,0) { }

        public GridCoords(int cx, int cy, int cz)
        {
            if (cx + cy + cz != 0) throw new ArgumentException("The sum of cx, cy and cz must be equal to 0");
            CX = cx;
            CY = cy;
            CZ = cz;

            var axial = ConvertCubeToOffset(cx, cy, cz);
            OX = axial[0];
            OY = axial[1];
        }

        public GridCoords(int cx, int cy)
        {
            OX = cx;
            OY = cy;

            var cube = ConvertOffsetToCube(cx, cy);
            CX = cube[0];
            CY = cube[1];
            CZ = cube[2];
        }

        public bool IsInsideBoard()
        {
            return 
                OX >=  0 && 
                OX <= 20 && 
                OY >=  0 && 
                OY <= 20;
        }

        public bool Equals(GridCoords other)
        {
            return (CX == other.CX) && (CY == other.CY) && (CZ == other.CZ);
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
            return CX ^ CY ^ CZ;
        }

        public override string ToString()
        {
            return $"{{{CX},{CY},{CZ}}}-{{{OX},{OY}}}";
        }

        private int[] ConvertCubeToOffset(int cx, int cy, int cz)
        {
            int ox = cx + (cz - (cz & 1))/2;
            int oy = -cz;
            ox += BoardWidth/2;
            oy += BoardHeight/2;
            return new[] {ox, oy};
        }

        private int[] ConvertOffsetToCube(int ox, int oy)
        {
            ox -= BoardWidth/2;
            oy -= BoardHeight/2;
            oy = -oy;
            var ax = ox - (oy - (oy & 1))/2;
            var az = oy;
            var ay = -ax - az;
            return new[] {ax, ay, az};
        }
    }
}