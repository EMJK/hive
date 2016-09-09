using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Hive.Common.Communication
{
    public class Client
    {
        TcpClient _client;
        public TextReader Reader { get; private set; }
        public TextWriter Writer { get; private set; }

        public Client(int port)
        {
            _client = new TcpClient();
            _client.Connect(IPAddress.Loopback, port);
            var stream = _client.GetStream();
            Writer = new StreamWriter(stream);
            Reader = new StreamReader(stream);
        }
    }

    public class Server
    {
        private TcpListener _listener;
        private TcpClient _client;

        public int LocalPort { get; }

        public TextReader Reader { get; private set; }
        public TextWriter Writer { get; private set; }

        public Server()
        {
            _listener = new TcpListener(IPAddress.Loopback, 0);
            _listener.Start();
            LocalPort = ((IPEndPoint)_listener.LocalEndpoint).Port;
        }

        public void AcceptConnection()
        {
            _client = _listener.AcceptTcpClient();
            var stream = _client.GetStream();
            Writer = new StreamWriter(stream);
            Reader = new StreamReader(stream);
        }
    }
}
