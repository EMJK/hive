using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Hive.Common;
using Hive.EngineWrapper;
using Julas.Utils;

namespace Hive.IpcServer
{
    class HiveServer
    {
        private readonly NamedPipeServerStream _pipe;
        private readonly Game _game = new Game();
        private BinaryFormatter _formatter;

        public HiveServer(string pipeName)
        {
            _formatter = new BinaryFormatter();
            _formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
            _pipe = new NamedPipeServerStream(pipeName);
        }

        public void Run()
        {
            _pipe.WaitForConnection();
            try
            {
                var initialResponse = new IpcResponse();
                initialResponse.GameState = _game.GameStateData;
                Write(initialResponse);
            }
            catch (Exception ex)
            {
                throw;
            }

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
            try
            {
                if (request.MethodName == nameof(Game.MoveBug))
                {
                    if (request.Args.Length == 3)
                    {
                        var arg1 = (PlayerColor)request.Args[0];
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
                        var arg1 = (PlayerColor)request.Args[0];
                        var arg2 = (BugType)request.Args[1];
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
            var header = new byte[4];
            if (_pipe.Read(header, 0, header.Length) != header.Length)
            {
                throw new Exception("Protocol error");
            }
            var bodyLength = Utils.ReadUInt32(header, 0);
            var body = new byte[bodyLength];
            if (_pipe.Read(body, 0, body.Length) != body.Length)
            {
                throw new Exception("Protocol error");
            }
            using (var ms = new MemoryStream(body))
            {
                var obj = (IpcRequest)_formatter.Deserialize(ms);
                return obj;
            }
        }

        private void Write(IpcResponse obj)
        {
            byte[] data;
            using (var ms = new MemoryStream())
            {
                _formatter.Serialize(ms, obj);
                data = ms.ToArray();
            }
            var header = new byte[4];
            Utils.WriteUInt32(header, data.Length, 0);
            _pipe.Write(header, 0, header.Length);
            _pipe.Write(data, 0, data.Length);
            _pipe.Flush();
            _pipe.WaitForPipeDrain();
        }
    }
}
