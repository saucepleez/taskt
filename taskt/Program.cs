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
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace taskt
{
    static class Program
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        /// <summary>
        /// app switch
        /// </summary>
        public static bool AllowChangeSettingsFilePathSwitch { get; private set; } = false;

        /// <summary>
        /// splash form
        /// </summary>
        //public static UI.Forms.Splash.frmSplash SplashForm { get; set; }
        private static UI.Forms.Splash.frmSplash SplashForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            App.UpdateLocationAndVersionInfo();

            // High DPI
            SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //exception handler
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            // create namespace list
            Core.Automation.Commands.PropertyControls.CreateCommandClassDic();

            //if the exe was passed a filename argument then run the script
            if (args.Length > 0)
            {
                string type = "";
                string scriptFilePath = "";
                string settingsFilePath = "";
                if (args.Length == 1)
                {
                    // only file name
                    scriptFilePath = args[0];
                    type = "run";
                }
                else if ((args.Length > 1) && (args.Length % 2 == 0))
                {
                    for (int i = 0; i < args.Length; i += 2)
                    {
                        switch (args[i])
                        {
                            case "-r":
                            case "-e":
                                type = "run";
                                scriptFilePath = args[i + 1];
                                break;
                            case "-o":
                                type = "open";
                                scriptFilePath = args[i + 1];
                                break;
                            case "-oh":
                                type = "open";
                                scriptFilePath = "*" + args[i + 1];
                                break;
                            case "-s":
                                settingsFilePath = args[i + 1];
                                break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Strange parameters", App.Taskt_VersionInfo.ProductName);

                    using (var eventLog = new EventLog("Application"))
                    {
                        eventLog.Source = "Application";
                        eventLog.WriteEntry("Strange parameters", EventLogEntryType.Error, 101, 1);
                    }

                    Application.Exit();
                    return;
                }

                // specify settings file
                if (!string.IsNullOrEmpty(settingsFilePath))
                {
                    if (Path.IsPathRooted(settingsFilePath))
                    {
                        settingsFilePath = Path.Combine(Core.IO.Folders.GetSettingsFolderPath(), settingsFilePath);
                    }
                }

                // load settings file
                if (!UpdateSettingsProcess(settingsFilePath))
                {
                    Application.Exit();
                    return;
                }
                
                if (type == "run" || type == "open")
                {
                    // script file
                    string checkFilePath = scriptFilePath.StartsWith("*") ? scriptFilePath.Substring(1) : scriptFilePath;
                    if (!Path.IsPathRooted(checkFilePath))
                    {
                        checkFilePath = Path.Combine(Core.IO.Folders.GetScriptsFolderPath(), checkFilePath);
                    }

                    if (!File.Exists(checkFilePath))
                    {
                        MessageBox.Show($"taskt Script File does not exits.\r\nPath: {scriptFilePath}", App.Taskt_VersionInfo.ProductName);

                        using (var eventLog = new EventLog("Application"))
                        {
                            eventLog.Source = "Application";
                            eventLog.WriteEntry($"An attempt was made to run a taskt script file from '{scriptFilePath}' but the file was not found.  Please verify that the file exists at the path indicated.", EventLogEntryType.Error, 101, 1);
                        }

                        Application.Exit();
                        return;
                    }
                    else
                    {
                        scriptFilePath = scriptFilePath.StartsWith("*") ? $"*{checkFilePath}" : checkFilePath;
                    }
                }

                switch (type) 
                {
                    case "run":
                        // execute
                        Application.Run(new UI.Forms.ScriptEngine.frmScriptEngine(scriptFilePath, null, null, true));
                        break;

                    case "open":
                        // edit
                        SplashForm = new UI.Forms.Splash.frmSplash();
                        SplashForm.Show();

                        Application.DoEvents();

                        Application.Run(new UI.Forms.ScriptBuilder.frmScriptBuilder(scriptFilePath));
                        break;

                    default:
                        SplashForm = new UI.Forms.Splash.frmSplash();
                        SplashForm.Show();

                        Application.DoEvents();

                        Application.Run(new UI.Forms.ScriptBuilder.frmScriptBuilder());
                        break;
                }
                //if (type == "run")
                //{
                //    // execute
                //    Application.Run(new UI.Forms.ScriptEngine.frmScriptEngine(scriptFilePath, null, null, true));
                //}
                //else
                //{
                //    // edit
                //    SplashForm = new UI.Forms.Splash.frmSplash();
                //    SplashForm.Show();

                //    Application.DoEvents();

                //    Application.Run(new UI.Forms.ScriptBuilder.frmScriptBuilder(scriptFilePath));
                //}
            }
            else
            {
                //clean up updater
                var updaterExecutableDestination = Path.Combine(Application.StartupPath, "taskt-updater.exe");

                if (File.Exists(updaterExecutableDestination))
                {
                    File.Delete(updaterExecutableDestination);
                }

                // load settings file
                if (!UpdateSettingsProcess(""))
                {
                    Application.Exit();
                    return;
                }
                
                SplashForm = new UI.Forms.Splash.frmSplash();
                SplashForm.Show();

                Application.DoEvents();

                Application.Run(new UI.Forms.ScriptBuilder.frmScriptBuilder());
            }
        }

        /// <summary>
        /// Update settings file
        /// </summary>
        /// <param name="settingsFilePath"></param>
        private static bool UpdateSettingsProcess(string settingsFilePath)
        {
            AllowChangeSettingsFilePathSwitch = true;
            var result = App.UpdateSettings(settingsFilePath);
            AllowChangeSettingsFilePathSwitch = false;
            return result;
        }

        /// <summary>
        /// error!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"An unhandled exception occured: {e.ExceptionObject as Exception}", App.Taskt_VersionInfo.ProductName);
        }

        /// <summary>
        /// hide Splash Screen form
        /// </summary>
        public static void HideSplashScreen()
        {
            SplashForm?.Hide();
        }
    }
}