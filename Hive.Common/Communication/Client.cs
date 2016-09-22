using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Hive.Common.Communication
{
    public class Client
    {
        private readonly TcpClient _client;

        public Client(int port)
        {
            _client = new TcpClient();
            _client.Connect(IPAddress.Loopback, port);
            var stream = _client.GetStream();
            Writer = new StreamWriter(stream);
            Reader = new StreamReader(stream);
        }

        public TextReader Reader { get; private set; }
        public TextWriter Writer { get; private set; }
    }

    public class Server
    {
        private TcpClient _client;
        private readonly TcpListener _listener;

        public Server(int serverPort)
        {
            _listener = new TcpListener(IPAddress.Loopback, serverPort);
            _listener.Start();
            LocalPort = ((IPEndPoint) _listener.LocalEndpoint).Port;
        }

        public int LocalPort { get; }

        public TextReader Reader { get; private set; }
        public TextWriter Writer { get; private set; }

        public void AcceptConnection()
        {
            _client = _listener.AcceptTcpClient();
            var stream = _client.GetStream();
            Writer = new StreamWriter(stream);
            Reader = new StreamReader(stream);
        }
    }
}