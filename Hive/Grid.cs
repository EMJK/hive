using System;
using System.Collections.Generic;
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

        public List<Tuple<GridCoords, Bug[]>> GetAllPopulatedFields()
        {
            return _bugs
                .Where(kvp => !kvp.Value.IsNullOrEmpty())
                .Select(kvp => Tuple.Create(kvp.Key, kvp.Value.ToArray()))
                .ToList();
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
