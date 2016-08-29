namespace Hive
{
    public class Bug
    {
        public PlayerColor Color { get; }
        public BugType Type { get; }

        public Bug(PlayerColor color, BugType type)
        {
            Color = color;
            Type = type;
        }

        public override string ToString()
        {
            return $"{Color} {Type}";
        }
    }
}
