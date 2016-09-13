using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Hive.Common;
using System.IO.Pipes;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

namespace Hive.IpcClient
{
    public class HiveClient : IGameActions, IDisposable
    {
        private Process _engineProcess;
        private NamedPipeClientStream _pipe;

        public GameStateData GameState { get; private set; }

        public HiveClient()
        {
            StartProcess(null);
        }

        public HiveClient(string debugPipeName)
        {
            StartProcess(debugPipeName);
        }

        public void MoveBug(PlayerColor color, GridCoords from, GridCoords to)
        {
            SendMessageAndReadResponse(new IpcRequest(nameof(MoveBug), color, from, to));
        }

        public void PlaceNewBug(PlayerColor color, BugType bug, GridCoords coords)
        {
            SendMessageAndReadResponse(new IpcRequest(nameof(PlaceNewBug), color, bug, coords));
        }

        private void StartProcess(string debugPipeName)
        {
            var pipeId = debugPipeName ?? CreatePipeId();
            if (debugPipeName == null)
            {
                _engineProcess = new Process();
                _engineProcess.StartInfo.FileName = "Hive.IpcServer.exe";
                _engineProcess.StartInfo.CreateNoWindow = true;
                _engineProcess.StartInfo.UseShellExecute = false;
                _engineProcess.StartInfo.RedirectStandardOutput = true;
                _engineProcess.StartInfo.Arguments = pipeId;
                _engineProcess.Start();
            }
            OpenPipe(pipeId);
        }

        private void OpenPipe(string pipeId)
        {
            _pipe = new NamedPipeClientStream(pipeId);
            _pipe.Connect();
            ReadResponse();
        }

        private string CreatePipeId()
        {
            return "HiveIpcPipe_" + Guid.NewGuid().ToString("N");
        }

        private void Write(IpcRequest obj)
        {
            var data = Json.Serialize(obj);
            var header = new byte[4];
            Utils.WriteUInt32(header, data.Length, 0);
            _pipe.Write(header, 0, header.Length);
            _pipe.Write(data, 0, data.Length);
            _pipe.Flush();
            _pipe.WaitForPipeDrain();
        }

        private IpcResponse Read()
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
            var obj = Json.Deserialize<IpcResponse>(body);
            return obj;
        }

        private void SendMessageAndReadResponse(IpcRequest message)
        {
            Write(message);
            ReadResponse();
        }

        private void ReadResponse()
        {
            var response = Read();
            if (response.Error)
            {
                //something wrong happened
            }
            else
            {
                GameState = response.GameState;
            }
        }

        private delegate void Action();

        private void Try(Action action)
        {
            try
            {
                action();
            }
            catch
            {
            }
        }

        public void Dispose()
        {
            Try(_pipe.Close);
            Try(_pipe.Dispose);
            Try(_engineProcess.Kill);
            Try(_engineProcess.Dispose);
        }
    }
}
