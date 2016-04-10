using Hive.Bugs;

namespace Hive
{
    class Move
    {
        public Bug Bug { get; set; }
        public GridCoords[] Sequence { get; set; }
        public PlayerColor Player { get; set; }
    }
}