using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hive.Common;
using Hive.EngineWrapper;
using Newtonsoft.Json;

namespace Hive.IpcServer
{
    class HiveServer
    {
        private readonly NamedPipeServerStream _pipe;
        private readonly Game _game = new Game();

        public HiveServer(string pipeName)
        {
            _pipe = new NamedPipeServerStream(pipeName);

        }

        public void Run()
        {
            _pipe.WaitForConnection();
            while (_pipe.IsConnected)
            {
                try
                {
                    var request = Read();
                    ProcessRequest(request);
                    var response = new IpcResponse();
                    response.GameState = _game.GameStateData;
                    Write(response);
                }
                catch (Exception ex)
                {
                    var response = new IpcResponse();
                    response.Error = true;
                    response.ErrorDetails = ex.ToString();
                    Write(response);
                }
            }
        }

        private void ProcessRequest(IpcRequest request)
        {
            if (request.MethodName == nameof(Game.MoveBug))
            {
                if (request.Args.Length == 2)
                {
                    _game.MoveBug(
                        (PlayerColor)request.Args[0],
                        (List<GridCoords>)request.Args[1]);
                    return;
                }
                if (request.Args.Length == 3)
                {
                    _game.MoveBug(
                        (PlayerColor)request.Args[0],
                        (GridCoords)request.Args[1],
                        (GridCoords)request.Args[2]);
                    return;
                }
            }
            if (request.MethodName == nameof(Game.PlaceNewBug))
            {
                if (request.Args.Length == 3)
                {
                    _game.PlaceNewBug(
                        (PlayerColor)request.Args[0],
                        (BugType)request.Args[1],
                        (GridCoords)request.Args[2]);
                    return;
                }
            }
            throw new Exception("Invalid request");
        }

        private IpcRequest Read()
        {
            var header = new byte[4];
            if (_pipe.Read(header, 0, header.Length) != header.Length)
            {
                throw new Exception("Protocol error");
            }
            var len = Utils.ReadUInt32(header, 0);
            var body = new byte[len];
            if (_pipe.Read(body, 0, body.Length) != body.Length)
            {
                throw new Exception("Protocol error");
            }

            var json = Encoding.UTF8.GetString(body);
            var obj = JsonConvert.DeserializeObject<IpcRequest>(json);
            return obj;
        }

        private void Write(IpcResponse response)
        {
            var json = JsonConvert.SerializeObject(response);
            var bytes = Encoding.UTF8.GetBytes(json);
            var header = new byte[4];
            Utils.WriteUInt32(header, bytes.Length, 0);
            _pipe.Write(header, 0, header.Length);
            _pipe.Write(bytes, 0, bytes.Length);
        }
    }
}
