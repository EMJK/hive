using Hive.Bugs;

namespace Hive
{
    class Move
    {
        public Bug Bug { get; set; }
        public GridCoords From { get; set; }
        public GridCoords To { get; set; }
    }
}