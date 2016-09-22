using System;
using System.Diagnostics;
using Hive.Common;
using Hive.Common.Communication;

namespace Hive.IpcClient
{
    public class HiveClient : IGameActions, IDisposable
    {
        private Process _engineProcess;
        private readonly Server _server;

        public GameStateData GameState { get; private set; }

        public HiveClient()
        {
            _server = new Server();
            StartProcess();
        }

        public void Dispose()
        {
            Try(_engineProcess.Kill);
            Try(_engineProcess.Dispose);
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
            _engineProcess = new Process();
            _engineProcess.StartInfo.FileName = "Hive.IpcServer.exe";
            _engineProcess.StartInfo.CreateNoWindow = true;
            _engineProcess.StartInfo.UseShellExecute = false;
            _engineProcess.StartInfo.RedirectStandardOutput = true;
            _engineProcess.StartInfo.RedirectStandardInput = true;
            _engineProcess.StartInfo.RedirectStandardError = true;
            _engineProcess.StartInfo.Arguments = _server.LocalPort + " " + Process.GetCurrentProcess().Id;
            _engineProcess.Start();
            _server.AcceptConnection();
            ReadResponse();
        }

        private void Write(IpcRequest obj)
        {
            StreamHelper.WriteLine(_server.Writer, obj);
        }

        private IpcResponse Read()
        {
            var obj = StreamHelper.ReadLine<IpcResponse>(_server.Reader);
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

        private delegate void Action();
    }
}