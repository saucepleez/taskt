using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using taskt.Core;
using taskt.Core.Automation.Commands;

namespace taskt
{
    /// <summary>
    /// Location, FileVersionInfo, AppSettings
    /// </summary>
    public static class App
    {
        /// <summary>
        /// taskt location
        /// </summary>
        public static string Taskt_Location { get; private set; }

        /// <summary>
        /// taskt version info
        /// </summary>
        public static FileVersionInfo Taskt_VersionInfo { get; private set; }

        /// <summary>
        /// taskt settings file path
        /// </summary>
        public static string Taskt_Settings_File_Path { get; private set; }

        /// <summary>
        /// UNSAFE application settings
        /// </summary>
        private static ApplicationSettings Taskt_UNSAFE_Settings { get; set; }

        /// <summary>
        /// application settings
        /// </summary>
        public static SafeApplicationSettings Taskt_Settings { get; private set; }

        /// <summary>
        /// all commands info, etc
        /// </summary>
        public static List<ScriptCommandInformation> AllCommandsInfo { get; private set; }

        /// <summary>
        /// update location, version info
        /// </summary>
        public static void UpdateLocationAndVersionInfo()
        {
            Taskt_Location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            Taskt_VersionInfo = FileVersionInfo.GetVersionInfo(Taskt_Location);
        }

        /// <summary>
        /// update settings
        /// </summary>
        /// <param name="settingsFilePath"></param>
        /// <returns></returns>
        public static bool UpdateSettings(string settingsFilePath)
        {
            // update settings file path
            if (Program.AllowChangeSettingsFilePathSwitch)
            {
                if (!string.IsNullOrEmpty(settingsFilePath))
                {
                    Taskt_Settings_File_Path = settingsFilePath;
                }
                else
                {
                    Taskt_Settings_File_Path = Core.IO.Files.GetSettigsFilePath();
                }
            }

            // update settings
            try
            {
                // when the settings file does not exists, create the file automatically
                Taskt_UNSAFE_Settings = ApplicationSettings.GetOrCreateApplicationSettings(Taskt_Settings_File_Path);
                Taskt_Settings = new SafeApplicationSettings(Taskt_UNSAFE_Settings);
                return true;
            }
            catch
            {
                MessageBox.Show($"Fail load settings file.\r\nPath: {Taskt_Settings_File_Path}", Taskt_VersionInfo.ProductName);
                return false;
            }
        }

        /// <summary>
        /// update all
        /// </summary>
        /// <param name="settingsFilePath"></param>
        public static bool UpdateAll(string settingsFilePath)
        {
            UpdateLocationAndVersionInfo();
            return UpdateSettings(settingsFilePath);
        }

        /// <summary>
        /// save current AppSettings
        /// </summary>
        public static void SaveSettings()
        {
            Taskt_UNSAFE_Settings.Save(Taskt_Settings_File_Path);
        }

        /// <summary>
        /// get AppSettings for DocumentGeneration
        /// </summary>
        /// <returns></returns>
        public static SafeDocumentGenerationApplicationSettings GetSafeDocumentGenerationApplicationSettings()
        {
            return new SafeDocumentGenerationApplicationSettings(Taskt_UNSAFE_Settings);
        }

        /// <summary>
        /// get AppSettings for WebSocket
        /// </summary>
        /// <returns></returns>
        public static SafeWebSocketApplicationSettings GetSafeWebSocketApplicationSettings()
        {
            return new SafeWebSocketApplicationSettings(Taskt_UNSAFE_Settings);
        }

        /// <summary>
        /// get AppSettings for HttpServerClient
        /// </summary>
        /// <returns></returns>
        public static SafeHttpServerClientApplicationSettings GetHttpServerClientApplicationSettings()
        {
            return new SafeHttpServerClientApplicationSettings(Taskt_UNSAFE_Settings);
        }

        /// <summary>
        /// get AppSettings for frmScriptBuilder
        /// </summary>
        /// <returns></returns>
        public static SafeFrmScriptBuilderApplicationSettings GetFrmScriptBuilderApplicationSettings()
        {
            return new SafeFrmScriptBuilderApplicationSettings(Taskt_UNSAFE_Settings);
        }

        /// <summary>
        /// get AppSettings for AutomationEngineInstance
        /// </summary>
        /// <returns></returns>
        public static SafeAutomationEngineInstanceApplicationSettings GetAutomationEngineInstanceApplicationSettings()
        {
            return (new SafeAutomationEngineInstanceApplicationSettings(Taskt_UNSAFE_Settings)).Clone();
        }

        /// <summary>
        /// create all commands info
        /// </summary>
        public static void CreateAllCommandsInfo()
        {
            AllCommandsInfo = ScriptCommandInformation.CreateScriptCommandInformations();
        }
    }
}
