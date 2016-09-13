namespace Hive.Common
{
    public class IpcRequest
    {
        public IpcRequest()
        {
        }

        public IpcRequest(string methodName, params object[] args)
        {
            MethodName = methodName;
            Args = args;
        }

        public string MethodName { get; set; }
        public object[] Args { get; set; }
    }

    public class IpcMoveBugRequest
    {
        private PlayerColor Color { get; set; }
        private GridCoords From { get; set; }
        private GridCoords To { get; set; }
    }

    public class IpcPlaceNewBugRequest
    {
        private PlayerColor Color { get; set; }
        private BugType Bug { get; set; }
        private GridCoords Coords { get; set; }
    }
}