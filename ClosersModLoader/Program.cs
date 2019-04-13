using System;
using System.Timers;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClosersModLoader
{
    class Program
    {
        private static Timer timer;
        static int flag = 0;
        static bool exited = true;
        static string mod_font;
        static string ori_font;
        static string run_font;

        static void Main(string[] args)
        {
            versionCheck();
            log("PROGRAM STARTED");
            log("STARTING LAUNCHER");
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "LAUNCHER.EXE";
            detection();
            p.Start();
            if(p != null)
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
            timer = new Timer(500);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            System.Diagnostics.Process[] p = System.Diagnostics.Process.GetProcessesByName("CW");
            if (p.Length != 0 && flag == 0)
            {
                File.Move(run_font, ori_font);
                File.Move(mod_font, run_font);
                log("LOADED, PROGRAM WILL EXIT IN 5 SECS");
                System.Threading.Thread.Sleep(5000);
                Environment.Exit(0);
                flag = 1;
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
        }
    }
}
