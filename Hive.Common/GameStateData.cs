using System;
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
        public Winner Winner { get; set; }

        public List<GridCoords> GetPossibleTargetsForBug(PlayerColor currentPlayer, GridCoords coords)
        {
            if (coords == null || currentPlayer == PlayerColor.Empty)
            {
                return new List<GridCoords>();
            }
            return WhitePlayerMoves
                .Where(x => x.FirstOrDefault()?.Equals(coords) == true)
                .Select(x => x.LastOrDefault())
                .Where(x => x != null)
                .ToList();
        }

        public List<GridCoords> GetPossibleNewBugPlacements(PlayerColor color)
        {
            if (color == PlayerColor.Empty) return new List<GridCoords>();
            return GetPossibleNewBugPlacements(color);
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
            if (currentPlayer == PlayerColor.Empty || from == null || to == null)
            {
                return null;
            }
            return GetMovesForPlayer(currentPlayer)
                .FirstOrDefault(x => 
                    x.FirstOrDefault()?.Equals(from) == true && 
                    x.LastOrDefault()?.Equals(to) == true);
        }

        private List<List<GridCoords>> GetMovesForPlayer(PlayerColor color)
        {
            if (color == PlayerColor.Black) return BlackPlayerMoves;
            if (color == PlayerColor.White) return WhitePlayerMoves;
            return null;
        }

        private List<GridCoords> GetPlacementOptionsForPlayer(PlayerColor color)
        {
            if (color == PlayerColor.White) return WhiteNewBugPlacementOptions;
            if (color == PlayerColor.Black) return BlackNewBugPlacementOptions;
            return null;
        }
    }
}