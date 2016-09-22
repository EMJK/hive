using System;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Hive.IpcServer
{
    internal static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                if (args.Length < 1) throw new ArgumentException("Invalid args");
                TryMonitorParent(args);
                new HiveServer().Run(int.Parse(args[0]));
            }
            catch(Exception ex)
            {
                Log(ex.ToString());
                Process.GetCurrentProcess().Kill();
            }
        }

        private static void TryMonitorParent(string[] args)
        {
            if (args.Length >= 2)
            {
                int parentPid = Int32.Parse(args[1]);
                Thread t = new Thread(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(200);
                        try
                        {
                            var proc = Process.GetProcessById(parentPid);
                            if (proc.HasExited)
                            {
                                Process.GetCurrentProcess().Kill();
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Log(ex.ToString());
                            Process.GetCurrentProcess().Kill();
                            break;
                        }
                    }
                });
                t.IsBackground = true;
                t.Start();
            }
        }

        public static void Log(string s)
        {
            File.AppendAllLines("HiveServerLog.txt", new[] {s});
        }
    }
}