using System;

namespace Hive.Common
{
    [Serializable]
    public enum PlayerColor
    {
        Empty,
        Black,
        White
    }

    [Serializable]
    public enum Winner
    {
        None,
        Black,
        White,
        Draw
    }

    [Serializable]
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
}