namespace Hive.Common
{
    public class Bug
    {
        public Bug(PlayerColor color, BugType type)
        {
            Color = color;
            Type = type;
        }

        public PlayerColor Color { get; set; }
        public BugType Type { get; set; }

        public override string ToString()
        {
            return $"{Color} {Type}";
        }
    }
}