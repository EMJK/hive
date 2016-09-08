﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Hive;
using Hive.Common;
using Hive.EngineWrapper;

namespace Hive.Tests
{
    public class EngineTests
    {
        [Fact]
        public void BeetleMovementTest()
        {
            var game = new Game();
            game.PlaceNewBug(PlayerColor.White, BugType.Beetle, new GridCoords(0, 0, 0));
            game.PlaceNewBug(PlayerColor.Black, BugType.Beetle, new GridCoords(1, 0, -1));
            var beetleMoves = game.GameStateData.WhitePlayerMoves;
            Assert.Equal(1, beetleMoves.Count);
            var moves = beetleMoves[new GridCoords(0, 0, 0)];
            Assert.Equal(3, moves.Count);
            Assert.True(moves.All(move => move.Count == 2));
        }

        [Fact]
        public void QueenBeeMovementTest()
        {
            var game = new Game();
            game.PlaceNewBug(PlayerColor.White, BugType.QueenBee, new GridCoords(0, 0, 0));
            game.PlaceNewBug(PlayerColor.Black, BugType.Beetle, new GridCoords(1, 0, -1));
            var queenBeeMoves = game.GameStateData.WhitePlayerMoves;
            Assert.Equal(1, queenBeeMoves.Count);
            var moves = queenBeeMoves[new GridCoords(0, 0, 0)];
            Assert.Equal(2, moves.Count);
            Assert.True(moves.All(move => move.Count == 2));
        }

        [Fact]
        public void GrasshopperMovementTest()
        {
            var game = new Game();
            game.PlaceNewBug(PlayerColor.White, BugType.Grasshopper, new GridCoords(0, 0, 0));
            game.PlaceNewBug(PlayerColor.Black, BugType.Beetle, new GridCoords(1, 0, -1));
            var grasshopperMoves = game.GameStateData.WhitePlayerMoves;
            Assert.Equal(1, grasshopperMoves.Count);
            var moves = grasshopperMoves[new GridCoords(0, 0, 0)];
            Assert.Equal(1, moves.Count);
            Assert.True(moves.All(move => move.Count == 2));
        }

        [Fact] 
        public void PilBugMovementTest()
        {
            var game = new Game();
            game.PlaceNewBug(PlayerColor.White, BugType.PillBug, new GridCoords(0, 0, 0));
            game.PlaceNewBug(PlayerColor.Black, BugType.QueenBee, new GridCoords(1, -1, 0));
            var allMoves = game.GameStateData.WhitePlayerMoves;
        }

        [Fact]
        public void BugsShouldNotDuplicate()
        {
            var game = new Game();
            game.PlaceNewBug(PlayerColor.White, BugType.Beetle, new GridCoords(0, 0, 0));
            game.PlaceNewBug(PlayerColor.Black, BugType.Beetle, new GridCoords(1, -1, 0));
            game.MoveBug(PlayerColor.White, new GridCoords(0, 0, 0), new GridCoords(1, 0, -1));
            game.MoveBug(PlayerColor.Black, new GridCoords(1, -1, 0), new GridCoords(1, 0, -1));
            Assert.Equal(2, game.GameStateData.Bugs.Select(x => x.Value.Count).Sum());
        }
    }
}
