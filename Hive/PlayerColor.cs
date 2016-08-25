namespace Hive
{
    public enum PlayerColor
    {
        Empty,
        Black,
        White
    }

    public enum Winner
    {
        None,
        Black,
        White,
        Draw
    }

    public enum BugType
    {
        Beetle,
        Grasshopper,
        Ladybug,
        Mosquito,
        PillBug,
        QueenBee,
        SoldierAnt,
        Spider
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