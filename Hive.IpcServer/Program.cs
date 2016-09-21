﻿using System;
using System.Diagnostics;
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
            catch
            {
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
                        catch (Exception)
                        {
                            Process.GetCurrentProcess().Kill();
                            break;
                        }
                    }
                });
                t.IsBackground = true;
                t.Start();
            }
        }
    }
}