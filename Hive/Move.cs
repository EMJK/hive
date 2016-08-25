using Hive;

public class Move
{
    public GridCoords[] Sequence { get; }
    public PlayerColor Color { get; }

    public Move(GridCoords[] sequence, PlayerColor color)
    {
        Sequence = sequence;
        Color = color;
    }
}