using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common;
using Hive.IpcClient;
using Hive.IpcServer;
using Xunit;

namespace Hive.Tests
{
    public class IpcTests
    {
        [Fact]
        public void IpcTest()
        {
            string pipeName = "test_pipe_name";
            Exception threadException = null;
            var thread = new Thread(() =>
            {
                try
                {
                    var server = new HiveServer(pipeName);
                    server.Run();
                }
                catch (Exception ex)
                {
                    threadException = ex;
                }
            });
            thread.Start();
            Thread.Sleep(500);
            var client = new HiveClient(pipeName);

            client.PlaceNewBug(PlayerColor.White, BugType.Beetle, new GridCoords(0, 0, 0));
            client.PlaceNewBug(PlayerColor.Black, BugType.Beetle, new GridCoords(1, -1, 0));
            client.MoveBug(PlayerColor.White, new GridCoords(0, 0, 0), new GridCoords(1, 0, -1));
            client.MoveBug(PlayerColor.Black, new GridCoords(1, -1, 0), new GridCoords(1, 0, -1));
            thread.Abort();
            Assert.True(threadException == null || threadException is ThreadAbortException);
        }
    }
}
