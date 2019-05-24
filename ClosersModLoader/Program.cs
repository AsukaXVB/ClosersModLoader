using System;
using System.Timers;
using System.IO;
using System.Windows.Forms;
using System.Text;

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
            System.Diagnostics.Process p = new System.Diagnostics.Process();
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
            System.Diagnostics.Process[] p = System.Diagnostics.Process.GetProcessesByName("CW");
            if (p.Length != 0)
            {
                File.Move(run_font, ori_font);
                File.Move(mod_font, run_font);
                log("LOADED, PROGRAM WILL EXIT IN 5 SECS");
                System.Threading.Thread.Sleep(5000);
                SaveLog();
                Environment.Exit(0);
            }
        }

        public static void versionCheck()
        {
            try
            {
                System.Diagnostics.Process[] p = System.Diagnostics.Process.GetProcessesByName("CW");
                foreach (System.Diagnostics.Process ps in p)
                {
                    ps.Kill();
                    log("PROCESSES CLEARED");
                    System.Threading.Thread.Sleep(300);
                }
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
                log("FONT FILE NOT EXISTS!! PLEASE DO A CLIENT VERIFY.");
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
        }

        public static void SaveLog(bool Append = false, string Path = "./Loader.log")
        {
            if (LogString != null && LogString.Length > 0)
            {
                if (Append)
                {
                    using (StreamWriter file = System.IO.File.AppendText(Path))
                    {
                        file.Write(LogString.ToString());
                        file.Close();
                        file.Dispose();
                    }
                }
                else
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(Path))
                    {
                        file.Write(LogString.ToString());
                        file.Close();
                        file.Dispose();
                    }
                }
            }
        }
    }
}
