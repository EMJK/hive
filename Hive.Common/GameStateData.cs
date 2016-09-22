using System.Collections.Generic;
using System.Linq;

namespace Hive.Common
{
    public class GameStateData
    {
        public List<Pair<GridCoords, List<Bug>>> Bugs { get; set; }
        public List<List<GridCoords>> WhitePlayerMoves { get; set; }
        public List<List<GridCoords>> BlackPlayerMoves { get; set; }
        public List<GridCoords> BlackNewBugPlacementOptions { get; set; }
        public List<GridCoords> WhiteNewBugPlacementOptions { get; set; }
        public PlayerColor PreviousPlayer { get; set; }
        public PlayerColor CurrentPlayer => GetCurrentPlayer();
        public Winner Winner { get; set; }

        public List<GridCoords> GetPossibleTargetsForBug(PlayerColor currentPlayer, GridCoords coords)
        {
            if ((coords == null) || (currentPlayer == PlayerColor.Empty))
                return new List<GridCoords>();
            return WhitePlayerMoves
                .Where(x => x.FirstOrDefault()?.Equals(coords) == true)
                .Where(IsMoveInsideBoard)
                .Select(x => x.LastOrDefault())
                .Where(x => x != null)
                .ToList();
        }

        public bool CheckNewBugPlacement(PlayerColor bugColor, GridCoords coords)
        {
            return GetPlacementOptionsForPlayer(bugColor).Contains(coords);
        }

        public bool CheckIfBugCanMove(PlayerColor currentPlayer, GridCoords from, GridCoords to)
        {
            return GetMoveSequenceForBug(currentPlayer, from, to) != null;
        }

        public List<GridCoords> GetMoveSequenceForBug(PlayerColor currentPlayer, GridCoords from, GridCoords to)
        {
            if ((currentPlayer == PlayerColor.Empty) || (from == null) || (to == null))
                return null;
            return GetMovesForPlayer(currentPlayer)
                .FirstOrDefault(x =>
                    (x.FirstOrDefault()?.Equals(from) == true) &&
                    (x.LastOrDefault()?.Equals(to) == true));
        }

        private List<List<GridCoords>> GetMovesForPlayer(PlayerColor color)
        {
            if (color == PlayerColor.Black) return BlackPlayerMoves.Where(IsMoveInsideBoard).ToList();
            if (color == PlayerColor.White) return WhitePlayerMoves.Where(IsMoveInsideBoard).ToList();
            return null;
        }

        private List<GridCoords> GetPlacementOptionsForPlayer(PlayerColor color)
        {
            if (color == PlayerColor.White) return WhiteNewBugPlacementOptions.Where(x => x.IsInsideBoard()).ToList();
            if (color == PlayerColor.Black) return BlackNewBugPlacementOptions.Where(x => x.IsInsideBoard()).ToList();
            return null;
        }

        private bool IsMoveInsideBoard(IEnumerable<GridCoords> move)
        {
            return move.All(x => x.IsInsideBoard());
        }

        private PlayerColor GetCurrentPlayer()
        {
            if (PreviousPlayer == PlayerColor.Empty) return PlayerColor.White;
            var possibleNextPlayer = OppositeColor(PreviousPlayer);
            if (GetMovesForPlayer(possibleNextPlayer).Any() || GetPlacementOptionsForPlayer(possibleNextPlayer).Any())
            {
                return possibleNextPlayer;
            }
            return PreviousPlayer;
        }

        private PlayerColor OppositeColor(PlayerColor color)
        {
            if (color == PlayerColor.White) return PlayerColor.Black;
            if (color == PlayerColor.Black) return PlayerColor.White;
            return PlayerColor.Empty;
        }
    }
}