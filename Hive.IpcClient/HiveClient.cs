using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Hive.Common;
using System.IO.Pipes;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using Hive.Common.Communication;

namespace Hive.IpcClient
{
    public class HiveClient : IGameActions, IDisposable
    {
        private Process _engineProcess;
        private Server _server;

        public GameStateData GameState { get; private set; }

        public HiveClient()
        {
            _server = new Server();
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
            _engineProcess = new Process();
            _engineProcess.StartInfo.FileName = "Hive.IpcServer.exe";
            _engineProcess.StartInfo.UseShellExecute = false;
            _engineProcess.StartInfo.RedirectStandardOutput = true;
            _engineProcess.StartInfo.RedirectStandardInput = true;
            _engineProcess.StartInfo.RedirectStandardError = true;
            _engineProcess.StartInfo.Arguments = _server.LocalPort.ToString();
            _engineProcess.Start();
            _server.AcceptConnection();
            ReadResponse();
        }

        private void Write(IpcRequest obj)
        {
            StdInOut.WriteLine(_server.Writer, obj);
        }

        private IpcResponse Read()
        {
            var obj = StdInOut.ReadLine<IpcResponse>(_server.Reader);
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
            Try(_engineProcess.Kill);
            Try(_engineProcess.Dispose);
        }
    }
}
