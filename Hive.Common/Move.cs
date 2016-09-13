namespace Hive.Common
{
    public class Move
    {
        public Move(GridCoords[] sequence, PlayerColor color)
        {
            Sequence = sequence;
            Color = color;
        }

        public GridCoords[] Sequence { get; set; }
        public PlayerColor Color { get; set; }
    }
}