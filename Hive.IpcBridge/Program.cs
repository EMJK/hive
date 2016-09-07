using System;

namespace Hive.IpcServer
{
    static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            if (args.Length != 1) throw new ArgumentException("Invalid args");
            new HiveServer(args[0]).Run();
        }
    }
}
