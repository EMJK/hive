using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Hive.Common;
using System.IO.Pipes;
using Newtonsoft.Json;

namespace Hive.IpcClient
{
    public class HiveClient : IGameActions, IDisposable
    {
        private Process _engineProcess;
        private NamedPipeClientStream _pipe;

        public GameStateData GameState { get; private set; }

        public HiveClient()
        {
            StartProcess();
        }

        public void MoveBug(PlayerColor color, List<GridCoords> sequence)
        {
            SendMessageAndReadResponse(new IpcRequest(nameof(MoveBug), color, sequence));
        }

        public void MoveBug(PlayerColor color, GridCoords from, GridCoords to)
        {
            SendMessageAndReadResponse(new IpcRequest(nameof(MoveBug), color, from, to));
        }

        public void PlaceNewBug(PlayerColor color, BugType bug, GridCoords coords)
        {
            SendMessageAndReadResponse(new IpcRequest(nameof(PlaceNewBug), color, bug, coords));
        }

        private void StartProcess()
        {
            var pipeId = CreatePipeId();
            _engineProcess = new Process();
            _engineProcess.StartInfo.FileName = "Hive.IpcServer.exe";
            _engineProcess.StartInfo.CreateNoWindow = true;
            _engineProcess.StartInfo.UseShellExecute = false;
            _engineProcess.StartInfo.RedirectStandardOutput = true;
            _engineProcess.StartInfo.Arguments = pipeId;
            _engineProcess.Start();
            OpenPipe(pipeId);
        }

        private void OpenPipe(string pipeId)
        {
            _pipe = new NamedPipeClientStream(pipeId);
            _pipe.Connect();
        }

        private string CreatePipeId()
        {
            return "HiveIpcPipe_" + Guid.NewGuid().ToString("N");
        }

        private void Write(IpcRequest obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var bytes = Encoding.UTF8.GetBytes(json);
            var header = new byte[4];
            Utils.WriteUInt32(header, bytes.Length, 0);
            _pipe.Write(header, 0, header.Length);
            _pipe.Write(bytes, 0, bytes.Length);
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
            var json = Encoding.UTF8.GetString(body);
            var obj = JsonConvert.DeserializeObject<IpcResponse>(json);
            return obj;
        }

        private void SendMessageAndReadResponse(IpcRequest message)
        {
            Write(message);
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
