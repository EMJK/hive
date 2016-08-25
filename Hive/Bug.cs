namespace Hive
{
    public class Bug
    {
        public PlayerColor Color { get; }
        public BugType Type { get; }
        public GridCoords Coords { get; }

        public Bug(PlayerColor color, BugType type, GridCoords coords)
        {
            Color = color;
            Type = type;
            Coords = coords;
        }
    }
}
