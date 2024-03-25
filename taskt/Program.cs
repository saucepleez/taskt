﻿//Copyright (c) 2019 Jason Bayldon
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace taskt
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // High DPI
            SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //exception handler
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            //if the exe was passed a filename argument then run the script
            if (args.Length > 0)
            {
                string type = "run";
                string filePath;
                if (args.Length == 1)
                {
                    filePath = args[0];
                }
                else
                {
                    switch (args[0])
                    {
                        case "-r":
                        case "-e":
                            filePath = args[1];
                            break;
                        case "-o":
                            type = "open";
                            filePath = args[1];
                            break;
                        case "-oh":
                            type = "open";
                            filePath = "*" + args[1];
                            break;
                        default:
                            using (System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog("Application"))
                            {
                                eventLog.Source = "Application";
                                eventLog.WriteEntry("Strange parameter", System.Diagnostics.EventLogEntryType.Error, 101, 1);
                            }

                            Application.Exit();
                            return;
                    }
                }

                string checkFilePath = filePath.StartsWith("*") ? filePath.Substring(1) : filePath;
                if (!System.IO.File.Exists(checkFilePath))
                {
                    using (System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog("Application"))
                    {
                        eventLog.Source = "Application";
                        eventLog.WriteEntry("An attempt was made to run a taskt script file from '" + filePath + "' but the file was not found.  Please verify that the file exists at the path indicated.", System.Diagnostics.EventLogEntryType.Error, 101, 1);
                    }

                    Application.Exit();
                    return;
                }

                if (type == "run")
                {
                    Application.Run(new UI.Forms.ScriptEngine.frmScriptEngine(filePath, null, null, true));
                }
                else
                {
                    SplashForm = new UI.Forms.Splash.frmSplash();
                    SplashForm.Show();

                    Application.DoEvents();

                    Application.Run(new UI.Forms.ScriptBuilder.frmScriptBuilder(filePath));
                }
            }
            else
            {
                //clean up updater
                var updaterExecutableDestination = Application.StartupPath + "\\taskt-updater.exe";

                if (System.IO.File.Exists(updaterExecutableDestination))
                {
                    System.IO.File.Delete(updaterExecutableDestination);
                }

                SplashForm = new UI.Forms.Splash.frmSplash();
                SplashForm.Show();

                Application.DoEvents();

                Application.Run(new UI.Forms.ScriptBuilder.frmScriptBuilder());
            }
        }
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled exception occured: " + (e.ExceptionObject as Exception).ToString(), "Oops");
        }

        public static UI.Forms.Splash.frmSplash SplashForm { get; set; }
    }
}