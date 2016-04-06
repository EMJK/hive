using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hive.Bugs
{
    enum PlayerColor
    {
        Empty,
        Black,
        White
    }

    internal abstract class Bug
    {
        public PlayerColor Color { get; private set; }
        protected Bug(PlayerColor color)
        {
            Color = color;
        }

        public abstract GridCoords[] GetPossibleMoves(GameState state);
    }
}
