using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common;
using Hive.Common.Communication;
using Hive.EngineWrapper;
using Julas.Utils;

namespace Hive.IpcServer
{
    public class HiveServer
    {
        private readonly Game _game = new Game();
        private Client _client;

        public void Run(int port)
        {
            while (true)
            {
                try
                {
                    _client = new Client(port);
                    break;
                }
                catch
                {
                    Thread.Sleep(100);
                }
            }
            var initialResponse = new IpcResponse();
            initialResponse.GameState = _game.GameStateData;
            Write(initialResponse);

            while (true)
            {
                var request = Read();
                ProcessRequest(request);
                var response = new IpcResponse();
                response.GameState = _game.GameStateData;
                Write(response);
            }
        }

        private void ProcessRequest(IpcRequest request)
        {
            try
            {
                if (request.MethodName == nameof(Game.MoveBug))
                {
                    if (request.Args.Length == 3)
                    {
                        var arg1 = (PlayerColor)request.Args[0].ConvertTo<PlayerColor>();
                        var arg2 = (GridCoords) request.Args[1];
                        var arg3 = (GridCoords) request.Args[2];
                        _game.MoveBug(arg1, arg2, arg3);
                        return;
                    }
                }
                if (request.MethodName == nameof(Game.PlaceNewBug))
                {
                    if (request.Args.Length == 3)
                    {
                        var arg1 = (PlayerColor)request.Args[0].ConvertTo<PlayerColor>();
                        var arg2 = (BugType)request.Args[1].ConvertTo<BugType>();
                        var arg3 = (GridCoords)request.Args[2];
                        _game.PlaceNewBug(arg1, arg2, arg3);
                        return;
                    }
                }
                throw new Exception("Unknown method");
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid request", ex);
            }
        }

        private IpcRequest Read()
        {
            var obj = StdInOut.ReadLine<IpcRequest>(_client.Reader);
            return obj;
        }

        private void Write(IpcResponse obj)
        {
            StdInOut.WriteLine(_client.Writer, obj);
        }
    }
}
