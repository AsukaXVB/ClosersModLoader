using System;
using System.Timers;
using System.IO;
using System.Windows.Forms;
using System.Text;
using System.Diagnostics;

namespace ClosersModLoader
{
    class Program
    {
        private static System.Timers.Timer timer;
        public static StringBuilder LogString = new StringBuilder();
        static string mod_font;
        static string ori_font;
        static string run_font;

        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new fake());
        }

        public static void start()
        {
            versionCheck();
            log("PROGRAM STARTED");
            log("STARTING LAUNCHER");
            Process p = new Process();
            p.StartInfo.FileName = "LAUNCHER.EXE";
            detection();
            p.Start();
            if (p != null)
            {
                p.EnableRaisingEvents = true;
                p.Exited += new EventHandler(proc_Exited);
            }
            Console.ReadLine();
        }

        static void proc_Exited(object sender, EventArgs e)
        {
            log("CLOSER LAUNCHER STARTED, LOADING MODS...");
            setTimer();
        }

        static void setTimer()
        {
            timer = new System.Timers.Timer(500);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Process[] client = Process.GetProcessesByName("CW");
            Process[] launcher = Process.GetProcessesByName("CLOSERS");
            if (client.Length != 0)
            {
                File.Move(run_font, ori_font);
                File.Move(mod_font, run_font);
                log("LOADED, PROGRAM WILL EXIT IN 5 SECS");
                System.Threading.Thread.Sleep(5000);
                Environment.Exit(0);
            }

            if(launcher.Length == 0 && client.Length == 0)
            {
                log("USERS CLOSED LAUNCHER, EXITING...");
                Environment.Exit(0);
            }
        }

        public static void versionCheck()
        {

            try
            {
                EndTask("CW");
                EndTask("Blackcipher");
                log("PROCESSES CLEARED");
                System.Threading.Thread.Sleep(100);
            }
            catch (Exception ex)
            {
            throw ex;
            }

            if (File.Exists("SIMHEI.TTF"))
            {
                log("DETECTED VERSION: CN");
                run_font = "SIMHEI.TTF";
            }
            else if (File.Exists("MSJH.TTF"))
            {
                log("DETECTED VERSION: TW");
                run_font = "MSJH.TTF";
            }
            else
            {
                log("FONT FILE NOT EXISTS!! PLEASE DO A CLIENT VERIFY.");
                log("EXITING IN 5 SECONDS...");
                System.Threading.Thread.Sleep(5000);
                Environment.Exit(0);
            }
            ori_font = run_font + ".ori";
            mod_font = run_font + ".mod";
        }

        static void detection()
        {
            log("CHECKING IF MOD LOADED");
            if (!File.Exists(mod_font))
            {
                File.Move(run_font, mod_font);
                File.Move(ori_font, run_font);
                log("MOD LOADED RESTORED");
            }
            else
                log("MOD NOT LOADED");
        }

        static void log(string str)
        {
            Console.Write("[" + DateTime.Now.ToString() + "]" + str + "\r\n");
            LogString.Append("[" + DateTime.Now.ToString() + "]" + str + "\r\n");
            SaveLog();
        }

        public static void SaveLog(bool Append = false, string Path = "./Loader.log")
        {
            if (LogString != null && LogString.Length > 0)
            {
                if (Append)
                {
                    using (StreamWriter file = File.AppendText(Path))
                    {
                        file.Write(LogString.ToString());
                        file.Close();
                        file.Dispose();
                    }
                }
                else
                {
                    using (StreamWriter file = new StreamWriter(Path))
                    {
                        file.Write(LogString.ToString());
                        file.Close();
                        file.Dispose();
                    }
                }
            }
        }

        public static void EndTask(string processName)
        {
            foreach (Process process in Process.GetProcessesByName(processName))
            {
                process.Kill();
            }
        }
    }
}
