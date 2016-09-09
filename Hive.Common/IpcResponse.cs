using System;

namespace Hive.Common
{
    public class IpcResponse
    {
        public bool Error { get; set; }
        public string ErrorDetails { get; set; }
        public GameStateData GameState { get; set; }
    }
}