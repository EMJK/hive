using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Hive.Bugs;

namespace Hive
{
    class GameState
    {
        public Grid Grid { get; }
        public IList<Move> Moves { get; } = new List<Move>();
        public Move PreviousMove => Moves.LastOrDefault();
        public PlayerColor PreviousPlayer => PreviousMove?.Player ?? PlayerColor.Empty;
        public PlayerColor CurrentPlayer => PreviousPlayer.Opposite();

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
