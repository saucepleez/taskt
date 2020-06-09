//Copyright (c) 2019 Jason Bayldon
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
using System;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.Forms.Supplement_Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;

namespace taskt
{
    static class Program
    {
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
        public static frmSplash SplashForm { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //SetProcessDPIAware();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //exception handler
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            //if the exe was passed a filename argument then run the script
            if (args.Length > 0)
            {
                string filePath = args[0];

                if (!File.Exists(filePath))
                {
                    using (EventLog eventLog = new EventLog("Application"))
                    {
                        eventLog.Source = "Application";
                        eventLog.WriteEntry("An attempt was made to run a taskt script file from '" + filePath +
                            "' but the file was not found.  Please verify that the file exists at the path indicated.",
                            EventLogEntryType.Error, 101, 1);
                    }

                    Application.Exit();
                    return;
                }

                Application.Run(new frmScriptEngine(filePath, null, null, true));
            }
            else
            {
                //clean up updater
                var updaterExecutableDestination = Application.StartupPath + "\\taskt-updater.exe";

                if (File.Exists(updaterExecutableDestination))
                {
                    File.Delete(updaterExecutableDestination);
                }

                SplashForm = new frmSplash();
                SplashForm.Show();

                Application.DoEvents();
                Application.Run(new frmScriptBuilder());
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled exception occured: " + (e.ExceptionObject as Exception).ToString(), "Oops");
        }
    }
}