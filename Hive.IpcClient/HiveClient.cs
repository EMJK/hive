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
        private BinaryFormatter _formatter;

        public GameStateData GameState { get; private set; }

        public HiveClient()
        {
            _formatter = new BinaryFormatter();
            _formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
            StartProcess();
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
            ReadResponse();
        }

        private string CreatePipeId()
        {
#if DEBUG
            return "DEBUG_PIPE_NAME";
#else
            return "HiveIpcPipe_" + Guid.NewGuid().ToString("N");
#endif
        }

        private void Write(IpcRequest obj)
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
            using (var ms = new MemoryStream(body))
            {
                var obj = (IpcResponse)_formatter.Deserialize(ms);
                return obj;
            }
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
