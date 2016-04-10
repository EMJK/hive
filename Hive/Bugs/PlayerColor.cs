namespace Hive.Bugs
{
    enum PlayerColor
    {
        Empty,
        Black,
        White
    }

    static class PlayerColorExtensions
    {
        public static PlayerColor Opposite(this PlayerColor color)
        {
            if(color == PlayerColor.Black) return PlayerColor.White;
            if(color == PlayerColor.White) return PlayerColor.Black;
            return PlayerColor.Empty;
        }
    }
}