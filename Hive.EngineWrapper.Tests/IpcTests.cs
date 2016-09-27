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
            }
        }
    }
}