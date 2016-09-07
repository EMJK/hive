namespace Hive.Common
{
    public class Move
    {
        public GridCoords[] Sequence { get; set; }
        public PlayerColor Color { get; set; }

        public Move(GridCoords[] sequence, PlayerColor color)
        {
            Sequence = sequence;
            Color = color;
        }
    }
}