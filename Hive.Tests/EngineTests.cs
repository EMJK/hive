﻿using System;
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
            Assert.Equal(3, beetleMoves.Count);
        }
    }
}