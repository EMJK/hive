using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Hive
{
    class GameState
    {
        public Grid Grid { get; }
        public IList<Move> Moves { get; } = new List<Move>();
        public Move PreviousMove => Moves.Last();

        public GameState()
        {
            Grid = new Grid();
        }

        public void ApplyMove(Move move)
        {
            Moves.Add(move);
        }
    }
}
