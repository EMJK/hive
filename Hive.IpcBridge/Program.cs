using System;
using System.Diagnostics;

namespace Hive.IpcServer
{
    static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                if (args.Length != 1) throw new ArgumentException("Invalid args");
                new HiveServer(args[0]).Run();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}
