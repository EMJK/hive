using System;
using System.Collections.Generic;
using System.Linq;
using Hive.Engine;
using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Core;

namespace Hive
{
    class GameState
    {
        private Types.GameState _state;    

        public Dictionary<GridCoords, List<BugType>> Bugs { get; private set; }
        public Dictionary<GridCoords, List<List<GridCoords>>> WhitePlayerMoves { get; private set; }
        public Dictionary<GridCoords, List<List<GridCoords>>> BlackPlayerMoves { get; private set; }
        public PlayerColor PreviousPlayer { get; private set; }
        public Winner Winner { get; private set; }

        public GameState() : this(Engine.Engine.newGame())
        { }

        public GameState(Types.GameState state)
        {
            _state = state;
            ReadState();
        }
        
        public void PlaceNewBug(PlayerColor color, BugType bug, GridCoords coords)
        {
            _state = Engine.Engine.applyAction(
                _state, 
                Types.Action.NewNew(
                    new Types.NewBug(
                        CreateBug(color, bug), 
                        MapCsharpCoords(coords))), 
                MapCsharpColor(color));
            ReadState();
        }

        public void MoveBug(PlayerColor color, GridCoords from, GridCoords to)
        {
            _state = Engine.Engine.applyAction(
                _state,
                Types.Action.NewMove(FindMove(color, from, to)),
                MapCsharpColor(color));
        }

        public void MoveBug(PlayerColor color, List<GridCoords> sequence) => MoveBug(color, sequence.First(), sequence.Last());

        private void ReadState()
        {
            var whiteMoves = Engine.Engine.getPossibleMovesForPlayer(_state.Board, Types.PlayerColor.White);
            WhitePlayerMoves = whiteMoves.ToDictionary(
                x => new GridCoords(x.Item1),
                x => x.Item2.Select(t => t.Select(c => new GridCoords(c)).ToList()).ToList());
            var blackMoves = Engine.Engine.getPossibleMovesForPlayer(_state.Board, Types.PlayerColor.Black);
            BlackPlayerMoves = blackMoves.ToDictionary(
                x => new GridCoords(x.Item1),
                x => x.Item2.Select(t => t.Select(c => new GridCoords(c)).ToList()).ToList());

            PreviousPlayer = MapFsharpColor(_state.Moves.LastOrDefault()?.Player);

            var winner = Engine.Engine.findWinner(_state);
            if (FSharpOption<Types.Winner>.get_IsNone(winner))
            {
                Winner = Winner.None;
            }
            else if (winner.Value.IsDraw)
            {
                Winner = Winner.Draw;
            }
            else
            {
                var color = ((Types.Winner.Player)winner.Value).Item;
                Winner = color.IsBlack ? Winner.Black : Winner.White;
            }
        }

        private PlayerColor MapFsharpColor(Types.PlayerColor color)
        {
            if (color == null) return PlayerColor.Empty;
            if (color.IsBlack) return PlayerColor.Black;
            return PlayerColor.White;
        }

        private Types.PlayerColor MapCsharpColor(PlayerColor color)
        {
            if (color == PlayerColor.Black) return Types.PlayerColor.Black;
            if (color == PlayerColor.White) return Types.PlayerColor.White;
            return null;
        }

        private Types.FieldCoords MapCsharpCoords(GridCoords coords)
        {
            return new Types.FieldCoords(coords.X, coords.Y, coords.Z);
        }

        private GridCoords MapFsharpCoords(Types.FieldCoords coords)
        {
            return new GridCoords(coords);
        }

        private Types.Bug CreateBug(PlayerColor color, BugType bug)
        {
            switch (bug)
            {
                case BugType.Beetle: return new Types.Bug(Types.BugType.Beetle, MapCsharpColor(color));
                case BugType.Grasshopper: return new Types.Bug(Types.BugType.Grasshopper, MapCsharpColor(color));
                case BugType.Ladybug: return new Types.Bug(Types.BugType.Ladybug, MapCsharpColor(color));
                case BugType.Mosquito: return new Types.Bug(Types.BugType.Mosquito, MapCsharpColor(color));
                case BugType.PillBug: return new Types.Bug(Types.BugType.PillBug, MapCsharpColor(color));
                case BugType.QueenBee: return new Types.Bug(Types.BugType.QueenBee, MapCsharpColor(color));
                case BugType.SoldierAnt: return new Types.Bug(Types.BugType.SoldierAnt, MapCsharpColor(color));
                case BugType.Spider: return new Types.Bug(Types.BugType.Spider, MapCsharpColor(color));
                default: return null;
            }
        }

        private FSharpList<Types.FieldCoords> FindMove(PlayerColor color, GridCoords from, GridCoords to)
        {
            var moves = (color == PlayerColor.Black ? BlackPlayerMoves : color == PlayerColor.White ? WhitePlayerMoves : null)?.Select(x => x.Value)?.SelectMany(x => x);
            var move = moves.FirstOrDefault(x => x.First().Equals(from) && x.Last().Equals(to));
            if (move == null) return null;
            return ListModule.OfSeq(move.Select(x => MapCsharpCoords(x)));
        }
    }
}
