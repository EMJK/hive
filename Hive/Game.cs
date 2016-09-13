using System;
using System.Collections.Generic;
using System.Linq;
using Hive.Common;
using Hive.Engine;
using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Core;

namespace Hive.EngineWrapper
{
    public class Game : IGameActions
    {
        private Types.GameState _state;

        public string StringRepresentation => GetStringRepresentation();

        public GameStateData GameStateData { get; private set; }

        public Game() : this(Engine.Engine.newGame())
        {
        }

        public Game(Types.GameState state)
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
            ReadState();
        }

        private string GetStringRepresentation()
        {
            if (!GameStateData.Bugs.Any()) return "Board is empty";
            var ret = GameStateData.Bugs
                .OrderBy(x => x.Item1.X)
                .ThenBy(x => x.Item1.Y)
                .ThenBy(x => x.Item1.Z)
                .Select(x => new { Coords = x.Item1, Stack = x.Item2 })
                .Select(x => $"[{x.Coords.ToString()}] {String.Join(">", x.Stack.Select(b => b.ToString()))}");
            return String.Join(Environment.NewLine, ret);
        }

        private void ReadState()
        {
            GameStateData = new GameStateData();
            GameStateData.PreviousPlayer = MapFsharpColor(_state.Moves.LastOrDefault()?.Player);           
            ReadBugs();
            ReadWinner();
            ReadMoves();
            ReadNewBugPlacement();
        }

        private void ReadNewBugPlacement()
        {
            var newBlack = Engine.Engine.getNewBugPlacementOptions(_state.Board, Types.PlayerColor.Black);
            var newWhite = Engine.Engine.getNewBugPlacementOptions(_state.Board, Types.PlayerColor.White);

            GameStateData.BlackNewBugPlacementOptions = newBlack.Select(MapFsharpCoords).ToList();
            GameStateData.WhiteNewBugPlacementOptions = newWhite.Select(MapFsharpCoords).ToList();
        }

        private void ReadMoves()
        {
            var whiteMoves = Engine.Engine.getPossibleMovesForPlayer(_state.Board, Types.PlayerColor.White);
            GameStateData.WhitePlayerMoves = whiteMoves
                .Select(x => x.Item2.Select(t => t.Select(MapFsharpCoords).ToList()))
                .SelectMany(x => x)
                .ToList();
                
            var blackMoves = Engine.Engine.getPossibleMovesForPlayer(_state.Board, Types.PlayerColor.Black);
            GameStateData.BlackPlayerMoves = blackMoves
                .Select(x => x.Item2.Select(t => t.Select(MapFsharpCoords).ToList()))
                .SelectMany(x => x)
                .ToList();
        }

        private void ReadWinner()
        {
            var winner = Engine.Engine.findWinner(_state);
            if (FSharpOption<Types.Winner>.get_IsNone(winner))
            {
                GameStateData.Winner = Winner.None;
            }
            else if (winner.Value.IsDraw)
            {
                GameStateData.Winner = Winner.Draw;
            }
            else
            {
                var color = ((Types.Winner.Player)winner.Value).Item;
                GameStateData.Winner = color.IsBlack ? Winner.Black : Winner.White;
            }
        }

        private void ReadBugs()
        {
            var board = _state.Board.Map;
            GameStateData.Bugs = board
                .Select(kvp => Pair.Create(
                    MapFsharpCoords(kvp.Key),
                    kvp.Value.Select(MapFsharpBug).ToList()))
                .ToList();
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
            return new GridCoords(coords.X, coords.Y, coords.Z);
        }

        private BugType MapFsharpBugType(Types.BugType type)
        {
            if (Equals(type, Types.BugType.Beetle)) return BugType.Beetle;
            if (Equals(type, Types.BugType.Grasshopper)) return BugType.Grasshopper;
            if (Equals(type, Types.BugType.Ladybug)) return BugType.Ladybug;
            if (Equals(type, Types.BugType.Mosquito)) return BugType.Mosquito;
            if (Equals(type, Types.BugType.PillBug)) return BugType.PillBug;
            if (Equals(type, Types.BugType.QueenBee)) return BugType.QueenBee;
            if (Equals(type, Types.BugType.SoldierAnt)) return BugType.SoldierAnt;
            if (Equals(type, Types.BugType.Spider)) return BugType.Spider;
            throw new ArgumentException();
        }

        private Bug MapFsharpBug(Types.Bug bug)
        {
            return new Bug(
                MapFsharpColor(bug.Color),
                MapFsharpBugType(bug.BugType)
            );
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
            var moves = color == PlayerColor.Black
                ? GameStateData.BlackPlayerMoves
                : color == PlayerColor.White ? GameStateData.WhitePlayerMoves : null;
            var move = moves.FirstOrDefault(x => x.First().Equals(from) && x.Last().Equals(to));
            if (move == null) return null;
            return ListModule.OfSeq(move.Select(MapCsharpCoords));
        }
    }
}
