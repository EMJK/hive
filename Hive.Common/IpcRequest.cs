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
}
