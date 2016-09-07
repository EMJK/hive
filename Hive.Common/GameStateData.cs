using System.Collections.Generic;

namespace Hive.Common
{
    public class GameStateData
    {
        public Dictionary<GridCoords, List<Bug>> Bugs { get; set; }
        public Dictionary<GridCoords, List<List<GridCoords>>> WhitePlayerMoves { get; set; }
        public Dictionary<GridCoords, List<List<GridCoords>>> BlackPlayerMoves { get; set; }
        public PlayerColor PreviousPlayer { get; set; }
        public Winner Winner { get; set; }
    }
}