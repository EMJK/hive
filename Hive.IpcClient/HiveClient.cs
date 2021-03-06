﻿using System;
using System.Diagnostics;
using System.IO;
using Hive.Common;
using Hive.Common.Communication;

namespace Hive.IpcClient
{
    public class HiveClient : IGameActions, IDisposable
    {
        private Process _engineProcess;
        private readonly Server _server;
        private readonly Action<string> _logger;
        private int _moveNumber = 0;

        public GameStateData GameState { get; private set; }

        public HiveClient(Action<string> logger) : this(logger, 0)
        {
        }

        public HiveClient(Action<string> logger, int serverPort)
        {
            _logger = logger ?? (s => { });
            _server = new Server(serverPort);
            if (serverPort == 0)
            {
                StartProcess();
            }
        }

        public void Dispose()
        {
            Try(_engineProcess.Kill);
            Try(_engineProcess.Dispose);
        }

        public void MoveBug(PlayerColor color, GridCoords from, GridCoords to)
        {
            var bug = GameState.GetTopBugAtCoords(from);
            var bugType = bug?.Type.ToString() ?? "NULL";
            var bugColor = bug?.Color.ToString() ?? "NULL";
            _logger($"{color} is trying to move {bugColor} {bugType} from {from} to {to}");

            if (GameState.CurrentPlayer == color && GameState.CheckIfBugCanMove(color, from, to))
            {
                SendMessageAndReadResponse(new IpcRequest(nameof(MoveBug), color, from, to));
                _logger($"{color} moved {bugColor} {bugType} from {from} to {to}");
            }
            else
            {
                throw new InvalidOperationException($"{color} cannot move {bugColor} {bugType} from {from} to {to}");
            }
        }

        public void PlaceNewBug(PlayerColor color, BugType bug, GridCoords coords)
        {
            _logger($"{color} is trying to place new {color} {bug} at {coords}");

            if (GameState.CurrentPlayer == color && GameState.CheckNewBugPlacement(color, bug, coords))
            {
                SendMessageAndReadResponse(new IpcRequest(nameof(PlaceNewBug), color, bug, coords));
                _logger($"{color} placed new {color} {bug} at {coords}");
            }
            else
            {
                throw new InvalidOperationException($"{color} cannot place {color} {bug} at {coords}");
            }
        }

        private void StartProcess()
        {
            _engineProcess = new Process();
            _engineProcess.StartInfo.FileName = Path.Combine(Environment.CurrentDirectory, "Hive.IpcServer.exe");
            _engineProcess.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
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
                throw new InvalidOperationException($"Engine returned error: {response.ErrorDetails}");
            }
            else
            {
                GameState = response.GameState;
                _moveNumber++;
                GameState.MoveNumber = _moveNumber;
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