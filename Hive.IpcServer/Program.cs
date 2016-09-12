using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Hive.IpcServer
{
    static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                if (args.Length < 1) throw new ArgumentException("Invalid args");
                TryMonitorParent(args);
                new HiveServer().Run(Int32.Parse(args[0]));
            }
            catch
            {
                Process.GetCurrentProcess().Kill();
            }
        }

        private static void TryMonitorParent(string[] args)
        {
            if (args.Length >= 2)
            {
                var parent_proc = Process.GetProcessById(Int32.Parse(args[1]));
                var this_proc = Process.GetCurrentProcess();
                Task.Run(() =>
                {
                    while (!parent_proc.HasExited)
                    {
                        Task.Delay(100);
                    }
                    this_proc.Kill();
                });
            }
        }
    }
}
