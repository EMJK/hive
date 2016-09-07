namespace Hive.Common
{
    public class Bug
    {
        public PlayerColor Color { get; set; }
        public BugType Type { get; set; }

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
