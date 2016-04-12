using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hive.Steps;
using Julas.Utils;
using Julas.Utils.Collections;

namespace Hive.Bugs
{
    internal abstract class Bug
    {
        public PlayerColor Color { get; private set; }

        protected Bug(PlayerColor color)
        {
            Color = color;
        }

        protected abstract StepPattern GetStepPattern(GameState state);

        public Move[] GetPossibleMoves(GameState state)
        {
            if (state.PreviousMove.Bug == this) return new Move[0];
        }

        private Move[] GetPillbugMoves(GameState state)
        {
            if (state.PreviousPlayer == PlayerColor.Empty) //nie było wcześniej ruchu
            {
                return new Move[0];
            }
            var currentField = state.Grid.GetFieldWithBug(this);
            //wszystkie sąsiednie stonogi w kolorze obecnego gracza
            var neighborPillBugCoords = currentField.Item1.GetSurroundingCoords()
                .Select(x => Tuple.Create(x, state.Grid.GetBugsForCoords(x)))
                .Where(x => !x.Item2.IsNullOrEmpty() && x.Item2.Peek().Map(b => b.Color == state.CurrentPlayer && b is PillBug))
                .Select(x => x.Item1);

            var availablePillBugs = neighborPillBugCoords.Where(p => state.Grid.CheckFreedomOfMovement(currentField.Item1, p));
            var availableMoves = availablePillBugs
                .SelectMany(p => p.GetSurroundingCoords()
                    .Where(c => !c.Equals(currentField.Item1))
                    .Where(c => state.Grid.GetBugsForCoords(c).IsNullOrEmpty())
                    .Select(c => new Move()
                    {
                        Bug = currentField.Item2.Peek(),
                        Player = state.CurrentPlayer,
                        Sequence = new[] {currentField.Item1, p, c}
                    }))
                .Where(m => m.Sequence
                    .Zip(m.Sequence.Skip(1), Tuple.Create)
                    .All(t => state.Grid.CheckFreedomOfMovement(t.Item1, t.Item2)))
                .ToArray();
            return availableMoves;
        }
    }
}
