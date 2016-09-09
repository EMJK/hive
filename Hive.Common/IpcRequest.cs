using System;
using System.Collections.Generic;
using System.Text;

namespace Hive.Common
{
    public class IpcRequest
    {
        public string MethodName { get; set; }
        public object[] Args { get; set; }

        public IpcRequest()
        {
            
        }

        public IpcRequest(string methodName, params object[] args)
        {
            MethodName = methodName;
            Args = args;
        }
    }

    public class IpcMoveBugRequest
    {
        private PlayerColor Color { get; set; }
        GridCoords From { get; set; }
        GridCoords To { get; set; }
    }

    public class IpcPlaceNewBugRequest
    {
        PlayerColor Color { get; set; }
        BugType Bug { get; set; }
        GridCoords Coords { get; set; }
    }
}
