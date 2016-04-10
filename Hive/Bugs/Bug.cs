using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hive.Steps;

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
            
        }

        private Move[] GetPillbugMoves 
    }
}
