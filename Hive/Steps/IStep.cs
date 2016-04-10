namespace Hive.Steps
{
    interface IStep
    {
        GridCoords[] GetPossibleTargets(GameState state);
    }
}