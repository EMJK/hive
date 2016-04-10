using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hive.Bugs;
using Julas.Utils.Collections;

namespace Hive
{
    class Grid
    {
        private readonly Dictionary<GridCoords, Stack<Bug>> _bugs;

        public Grid()
        {
            _bugs = new Dictionary<GridCoords, Stack<Bug>>();
        }

        public void PutBug(Bug bug, GridCoords coords)
        {
            if (bug == null) throw new ArgumentNullException(nameof(bug));
            GetStackForField(coords).Push(bug);
        }

        public Bug PickBug(GridCoords coords)
        {
            if (!_bugs.ContainsKey(coords) || _bugs[coords].IsNullOrEmpty())
            {
                return null;
            }
            return _bugs[coords].Pop();
        }

        public Bug[] GetFieldContent(GridCoords coords)
        {
            if (!_bugs.ContainsKey(coords) || _bugs[coords].IsNullOrEmpty())
            {
                return new Bug[0];
            }
            return _bugs[coords].ToArray();
        }

        public PlayerColor GetColor(GridCoords coords)
        {
            Stack<Bug> stack = null;
            if (_bugs.TryGetValue(coords, out stack))
            {
                if (!stack.IsNullOrEmpty())
                {
                    return stack.Peek().Color;
                }
            }
            return PlayerColor.Empty;
        }

        public GridCoords[] GetFieldsForNewBug(PlayerColor color)
        {
            if (_bugs.Any())
            {
                var ret = _bugs.Keys // weź współrzędne wszystkich stosów pionków
                    .Where(c => GetColor(c) == color) // na wierzchu których jest pionek danego gracza
                    .SelectMany(c => c.GetSurroundingCoords()) // policz współrzędne sąsiednich pól dla każdego z nich
                    .Distinct() // wywal duplikaty
                    .Where(c => GetColor(c) == PlayerColor.Empty) // zostaw tylko puste pola
                    .Where(c => !c.GetSurroundingCoords().Any(x => GetColor(x) == color.Opposite())) // i usuń te do których przylegają pionki przeciwnika
                    .ToArray();
                return ret;
            }
            return new[] {GridCoords.Zero};
        }

        public Tuple<GridCoords, Bug[]>[] GetAllPopulatedFields()
        {
            return _bugs
                .Where(kvp => !kvp.Value.IsNullOrEmpty())
                .Select(kvp => Tuple.Create(kvp.Key, kvp.Value.ToArray()))
                .ToArray();
        }

        private Stack<Bug> GetStackForField(GridCoords coords)
        {
            if (!_bugs.ContainsKey(coords) || _bugs[coords] == null)
            {
                var stack = new Stack<Bug>(2);
                _bugs[coords] = stack;
                return stack;
            }
            return _bugs[coords];
        }
    }
}
