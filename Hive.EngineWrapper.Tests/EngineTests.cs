using System.Linq;
using Hive.Common;
using Xunit;

namespace Hive.EngineWrapper.Tests
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
            var moves = beetleMoves.Where(x => x.FirstOrDefault()?.Equals(new GridCoords(0, 0, 0)) == true).ToList();
            Assert.Equal(3, moves.Count);
            Assert.True(moves.All(move => move.Count == 2));
        }

        [Fact]
        public void BugsShouldNotDuplicate()
        {
            var game = new Game();
            game.PlaceNewBug(PlayerColor.White, BugType.Beetle, new GridCoords(0, 0, 0));
            game.PlaceNewBug(PlayerColor.Black, BugType.Beetle, new GridCoords(1, -1, 0));
            game.MoveBug(PlayerColor.White, new GridCoords(0, 0, 0), new GridCoords(1, 0, -1));
            game.MoveBug(PlayerColor.Black, new GridCoords(1, -1, 0), new GridCoords(1, 0, -1));
            Assert.Equal(2, game.GameStateData.Bugs.Select(x => x.Item2.Count).Sum());
        }

        [Fact]
        public void GrasshopperMovementTest()
        {
            var game = new Game();
            game.PlaceNewBug(PlayerColor.White, BugType.Grasshopper, new GridCoords(0, 0, 0));
            game.PlaceNewBug(PlayerColor.Black, BugType.Beetle, new GridCoords(1, 0, -1));
            var grasshopperMoves = game.GameStateData.WhitePlayerMoves;
            var moves =
                grasshopperMoves.Where(x => x.FirstOrDefault()?.Equals(new GridCoords(0, 0, 0)) == true).ToList();
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
        public void QueenBeeMovementTest()
        {
            var game = new Game();
            game.PlaceNewBug(PlayerColor.White, BugType.QueenBee, new GridCoords(0, 0, 0));
            game.PlaceNewBug(PlayerColor.Black, BugType.Beetle, new GridCoords(1, 0, -1));
            var queenBeeMoves = game.GameStateData.WhitePlayerMoves;
            var moves = queenBeeMoves.Where(x => x.FirstOrDefault()?.Equals(new GridCoords(0, 0, 0)) == true).ToList();
            Assert.Equal(2, moves.Count);
            Assert.True(moves.All(move => move.Count == 2));
        }

        [Fact]
        public void NewBugSequence()
        {
            var game = new Game();
            game.PlaceNewBug(PlayerColor.White, BugType.Grasshopper, new GridCoords(0, 0, 0));
            game.PlaceNewBug(PlayerColor.Black, BugType.Beetle, new GridCoords(-1, 1, 0));
            game.PlaceNewBug(PlayerColor.White, BugType.SoldierAnt, new GridCoords(1, -1, 0));
        }

        [Fact]
        public void GridCoordsCubeToAxialTranslationTest()
        {
            GridCoords c;

            c = new GridCoords(0, 0, 0);
            Assert.Equal(10, c.OX);
            Assert.Equal(10, c.OY);

            c = new GridCoords(1, 0, -1);
            Assert.Equal(10, c.OX);
            Assert.Equal(11, c.OY);

            c = new GridCoords(1, -1, 0);
            Assert.Equal(11, c.OX);
            Assert.Equal(10, c.OY);

            c = new GridCoords(0, -1, 1);
            Assert.Equal(10, c.OX);
            Assert.Equal(9, c.OY);

            c = new GridCoords(-1, 0, 1);
            Assert.Equal(9, c.OX);
            Assert.Equal(9, c.OY);

            c = new GridCoords(-1, 1, 0);
            Assert.Equal(9, c.OX);
            Assert.Equal(10, c.OY);

            c = new GridCoords(0, 1, -1);
            Assert.Equal(9, c.OX);
            Assert.Equal(11, c.OY);
        }

        [Fact]
        public void GridCoordsAxialToCubeTranslationTest()
        {
            GridCoords c;

            c = new GridCoords(10, 10);
            Assert.Equal(0, c.CX);
            Assert.Equal(0, c.CY);
            Assert.Equal(0, c.CZ);

            c = new GridCoords(10, 11);
            Assert.Equal(1, c.CX);
            Assert.Equal(0, c.CY);
            Assert.Equal(-1, c.CZ);

            c = new GridCoords(9, 11);
            Assert.Equal(0, c.CX);
            Assert.Equal(1, c.CY);
            Assert.Equal(-1, c.CZ);

            c = new GridCoords(9, 10);
            Assert.Equal(-1, c.CX);
            Assert.Equal(1, c.CY);
            Assert.Equal(0, c.CZ);

            c = new GridCoords(9, 9);
            Assert.Equal(-1, c.CX);
            Assert.Equal(0, c.CY);
            Assert.Equal(1, c.CZ);

            c = new GridCoords(10, 9);
            Assert.Equal(0, c.CX);
            Assert.Equal(-1, c.CY);
            Assert.Equal(1, c.CZ);

            c = new GridCoords(11, 10);
            Assert.Equal(1, c.CX);
            Assert.Equal(-1, c.CY);
            Assert.Equal(0, c.CZ);
        }
    }
}