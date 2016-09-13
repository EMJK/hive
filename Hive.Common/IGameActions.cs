namespace Hive.Common
{
    public interface IGameActions
    {
        void MoveBug(PlayerColor color, GridCoords from, GridCoords to);
        void PlaceNewBug(PlayerColor color, BugType bug, GridCoords coords);
    }
}