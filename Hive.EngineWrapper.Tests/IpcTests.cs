using Hive.Common;
using Hive.IpcClient;
using Xunit;

namespace Hive.EngineWrapper.Tests
{
    public class IpcTests
    {
        [Fact]
        public void IpcTest()
        {
            using (var client = new HiveClient(null))
            {
                client.PlaceNewBug(PlayerColor.White, BugType.Beetle, new GridCoords(0, 0, 0));
                client.PlaceNewBug(PlayerColor.Black, BugType.Beetle, new GridCoords(1, -1, 0));
                client.MoveBug(PlayerColor.White, new GridCoords(0, 0, 0), new GridCoords(1, 0, -1));
                client.MoveBug(PlayerColor.Black, new GridCoords(1, -1, 0), new GridCoords(1, 0, -1));
            }
        }
    }
}