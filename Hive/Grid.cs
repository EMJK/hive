using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Hive.Bugs;
using Julas.Utils;
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

        public List<Bug> GetAllBugs => _bugs.SelectMany(x => x.Value).ToList();

        public void PlaceBug(Bug bug, GridCoords coords)
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

        public Stack<Bug> GetBugsForCoords(GridCoords coords)
        {
            return new Stack<Bug>(GetStackForField(coords));
        }

        public Tuple<GridCoords, Stack<Bug>> GetFieldWithBug(Bug bug)
        {
            var field = _bugs.First(x => x.Value.Contains(bug)).Key;
            return Tuple.Create(field, _bugs[field]);
        }

        public PlayerColor GetTopColor(GridCoords coords)
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
                    .Where(c => GetTopColor(c) == color) // na wierzchu których jest pionek danego gracza
                    .SelectMany(c => c.GetSurroundingCoords()) // policz współrzędne sąsiednich pól dla każdego z nich
                    .Distinct() // wywal duplikaty
                    .Where(c => GetTopColor(c) == PlayerColor.Empty) // zostaw tylko puste pola
                    .Where(c => !c.GetSurroundingCoords().Any(x => GetTopColor(x) == color.Opposite())) // i usuń te do których przylegają pionki przeciwnika
                    .ToArray();
                return ret;
            }
            return new[] {GridCoords.Zero};
        }

        public List<Tuple<GridCoords, Stack<Bug>>> GetAllPopulatedFields()
        {
            return _bugs
                .Where(x => !x.Value.IsNullOrEmpty())
                .Select(x => Tuple.Create(x.Key, new Stack<Bug>(x.Value)))
                .ToList();
        }

        public bool CheckFreedomOfMovement(GridCoords from, GridCoords to)
        {
            if(!from.IsNeighborOf(to)) 
                throw new ArgumentException($@"""{nameof(from)}"" ({from}) and ""{nameof(to)}"" ({to}) are not neighbors");
            if(!GetStackForField(from).Any())
                throw new InvalidOperationException("Cannot move from an empty field");    

            var topLevel = Math.Max(GetStackForField(from).Count - 1, GetStackForField(to).Count);
            var obstacles = new[] {from.LeftOf(to), from.RightOf(to)};
            return obstacles.Any(x => GetStackForField(x).Count < topLevel);
        }

        public void Trim()
        {
            _bugs.Where(x => x.Value.IsNullOrEmpty()).Select(x => x.Key).ForEach(x => _bugs.Remove(x));
        }

        private Stack<Bug> GetStackForField(GridCoords coords)
        {
            if (!_bugs.ContainsKey(coords) || _bugs[coords] == null)
            {
                var stack = new Stack<Bug>();
                _bugs[coords] = stack;
                return stack;
            }
            return _bugs[coords];
        }


    }
}
