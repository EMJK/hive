using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Hive;

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
            var beetleMoves = game.WhitePlayerMoves;
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
            var queenBeeMoves = game.WhitePlayerMoves;
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
            var grasshopperMoves = game.WhitePlayerMoves;
            Assert.Equal(1, grasshopperMoves.Count);
            var moves = grasshopperMoves[new GridCoords(0, 0, 0)];
            Assert.Equal(1, moves.Count);
            Assert.True(moves.All(move => move.Count == 2));
        }
    }
}
