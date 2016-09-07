using System;
using System.Collections.Generic;
using System.Text;

namespace Hive.Common
{
    public interface IGameActions
    {
        void MoveBug(PlayerColor color, List<GridCoords> sequence);
        void MoveBug(PlayerColor color, GridCoords from, GridCoords to);
        void PlaceNewBug(PlayerColor color, BugType bug, GridCoords coords);
    }
}
