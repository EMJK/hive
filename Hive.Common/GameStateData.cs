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
        public int MoveNumber { get; set; }
        public GridCoords PreviouslyMovedBug { get; set; }

        public List<GridCoords> GetPossibleTargetsForBug(PlayerColor currentPlayer, GridCoords coords)
        {
            if ((coords == null) || (currentPlayer == PlayerColor.Empty))
                return new List<GridCoords>();
            return WhitePlayerMoves
                .Where(x => x.FirstOrDefault()?.Equals(coords) == true)
                .Where(IsMoveInsideBoard)
                .Select(x => x.LastOrDefault())
                .Where(x => x != null)
                .Distinct()
                .ToList();
        }

        public Bug GetTopBugAtCoords(GridCoords coords)
        {
            var stack = Bugs.FirstOrDefault(x => x.Item1 == coords);
            return stack?.Item2?.FirstOrDefault();
        }

        public bool CheckNewBugPlacement(PlayerColor bugColor, BugType bugType, GridCoords coords)
        {
            if (MoveNumber >= 5 && !PlayerHasPlacedQueen(bugColor))
            {
                return bugType == BugType.QueenBee && GetPlacementOptionsForPlayer(bugColor).Contains(coords);
            }
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
            if (!PlayerHasPlacedQueen(color)) return new List<List<GridCoords>>();
            List<List<GridCoords>> pool = null;
            if (color == PlayerColor.Black) pool = BlackPlayerMoves; 
            if (color == PlayerColor.White) pool = WhitePlayerMoves;
            if (pool == null) return null;

            return pool
                .Where(IsMoveInsideBoard)
                .Distinct(GetPathComparer())
                .Where(x => PreviouslyMovedBug == null || x.FirstOrDefault() != PreviouslyMovedBug)
                .ToList();
        }

        private List<GridCoords> GetPlacementOptionsForPlayer(PlayerColor color)
        {
            if (color == PlayerColor.White) return WhiteNewBugPlacementOptions.Where(x => x.IsInsideBoard()).Distinct().ToList();
            if (color == PlayerColor.Black) return BlackNewBugPlacementOptions.Where(x => x.IsInsideBoard()).Distinct().ToList();
            return null;
        }

        private bool PlayerHasPlacedQueen(PlayerColor color)
        {
            var bugs = Bugs.Select(x => x.Item2).SelectMany(x => x);
            return bugs.Any(x => x.Type == BugType.QueenBee && x.Color == color);
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

        private IEqualityComparer<List<GridCoords>> GetPathComparer()
        {
            return new FuncEqualityComparer<List<GridCoords>>((x, y) =>
                x.FirstOrDefault() == y.FirstOrDefault() &&
                x.LastOrDefault() == y.LastOrDefault());
        }
    }
}