using System.Collections.Generic;

namespace Hive.Common
{
    public interface IGameData
    {
        Dictionary<GridCoords, List<List<GridCoords>>> BlackPlayerMoves { get; }
        Dictionary<GridCoords, List<Bug>> Bugs { get; }
        PlayerColor PreviousPlayer { get; }
        string StringRepresentation { get; }
        Dictionary<GridCoords, List<List<GridCoords>>> WhitePlayerMoves { get; }
        Winner Winner { get; }
    }
}