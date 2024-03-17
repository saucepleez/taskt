using System;
using System.IO;
using System.Reflection;

namespace taskt.Core.IO
{
    public static class Folders
    {
        /// <summary>
        /// taskt excecute file path
        /// </summary>
        private readonly static string TASKT_EXECUTE_FOLDER_PATH = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        /// <summary>
        /// scripts folder name
        /// </summary>
        private const string SCRIPTS_FOLDER_NAME = "My Scripts";
        /// <summary>
        /// logs folder name
        /// </summary>
        private const string LOGS_FOLDER_NAME = "Logs";
        /// <summary>
        /// taskt settings folder name
        /// </summary>
        private const string SETTINGS_FOLDER_NAME = "taskt";
        /// <summary>
        /// taskt temp folder name
        /// </summary>
        private const string TEMP_FOLDER_NAME = "taskt";
        /// <summary>
        /// Resouces folder name
        /// </summary>
        private const string RESOURCES_FOLDER_NAME = "Resources";
        /// <summary>
        /// Samples folder name
        /// </summary>
        private const string SAMPLES_FOLDER_NAME = "Samples";
        /// <summary>
        /// AutoSave folder name
        /// </summary>
        private const string AUTOSAVE_FOLDER_NAME = "AutoSave";
        /// <summary>
        /// RunWithoutSaving folder name
        /// </summary>
        private const string RUN_WITHOUT_SAVING_FOLDER_NAME = "RunWithoutSaving";
        /// <summary>
        /// BeforeConverted folder name
        /// </summary>
        private const string BEFORE_CONVERTED_FOLDER_NAME = "BeforeConverted";

        public enum FolderType
        {
            RootFolder,
            SettingsFolder,
            ScriptsFolder,
            LogFolder,
            TempFolder,
            AttendedTasksFolder
        }

        /// <summary>
        /// get some taskt folders path
        /// </summary>
        /// <param name="folderType"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static string GetFolder(FolderType folderType)
        {
            // TODO: eventually stop using
            switch (folderType)
            {
                case FolderType.RootFolder:
                    //return root folder from settings
                    //var rootSettings = new Core.ApplicationSettings().GetOrCreateApplicationSettings();
                    //var rootFolder = rootSettings.ClientSettings.RootFolder;
                    //return rootFolder;
                    return GetRootFolderPath();

                case FolderType.AttendedTasksFolder:
                    //return attended tasks folder from settings
                    //var attendedSettings = new Core.ApplicationSettings().GetOrCreateApplicationSettings();
                    //var attentedTasksFolder = attendedSettings.ClientSettings.AttendedTasksFolder;
                    //return attentedTasksFolder;
                    return GetAttendedTasksFolderPath();

                case FolderType.SettingsFolder:
                    //return app data taskt folder
                    //return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\taskt\\";
                    return GetSettingsFolderPath();

                case FolderType.ScriptsFolder:
                    //return scripts folder
                    //return System.IO.Path.Combine(GetFolder(FolderType.RootFolder), "My Scripts\\");
                    return GetScriptsFolderPath();

                case FolderType.LogFolder:
                    //return logs folder
                    //return System.IO.Path.Combine(GetFolder(FolderType.RootFolder), "Logs\\");
                    return GetLogsFolderPath();

                case FolderType.TempFolder:
                    //return temp folder
                    //return System.IO.Path.GetTempPath() + "\\taskt\\";
                    return GetTempFolderPath();

                default:
                    //enum is not implemented
                    throw new NotImplementedException("FolderType " + folderType.ToString() + " Not Supported");
            }
        }

        /// <summary>
        /// get taskt root folder in Documents
        /// </summary>
        /// <returns></returns>
        public static string GetRootFolderPath()
        {
            var rootSettings = new Core.ApplicationSettings().GetOrCreateApplicationSettings();
            return rootSettings.ClientSettings.RootFolder;
        }

        /// <summary>
        /// get taskt scripts folder in Documents
        /// </summary>
        /// <returns></returns>
        public static string GetAttendedTasksFolderPath()
        {
            var attendedSettings = new ApplicationSettings().GetOrCreateApplicationSettings();
            return attendedSettings.ClientSettings.AttendedTasksFolder;
        }

        /// <summary>
        /// get taskt settings folder in Users\%username%\...
        /// </summary>
        /// <returns></returns>
        public static string GetSettingsFolderPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), $"{SETTINGS_FOLDER_NAME}\\");
        }

        /// <summary>
        /// get scripts folder path
        /// </summary>
        /// <returns></returns>
        public static string GetScriptsFolderPath()
        {
            return Path.Combine(GetRootFolderPath(), $"{SCRIPTS_FOLDER_NAME}\\");
        }

        /// <summary>
        /// get logs folder path
        /// </summary>
        /// <returns></returns>
        public static string GetLogsFolderPath()
        {
            return Path.Combine(GetRootFolderPath(), $"{LOGS_FOLDER_NAME}\\");
        }

        /// <summary>
        /// get temp folder path
        /// </summary>
        /// <returns></returns>
        public static string GetTempFolderPath()
        {
            return Path.Combine(Path.GetTempPath(), $"{TEMP_FOLDER_NAME}\\");
        }

        /// <summary>
        /// get Resouces folder path in taskt excecution file folder
        /// </summary>
        /// <returns></returns>
        public static string GetResourcesFolderPath()
        {
            return Path.Combine(TASKT_EXECUTE_FOLDER_PATH, $"{RESOURCES_FOLDER_NAME}\\");
        }

        /// <summary>
        /// get Samples folder path in taskt excecution file folder
        /// </summary>
        /// <returns></returns>
        public static string GetSamplesFolderPath()
        {
            return Path.Combine(TASKT_EXECUTE_FOLDER_PATH, $"{SAMPLES_FOLDER_NAME}\\");
        }

        /// <summary>
        /// get AutoSave folder path in taskt execution file folder
        /// </summary>
        /// <returns></returns>
        public static string GetAutoSaveFolderPath()
        {
            return Path.Combine(TASKT_EXECUTE_FOLDER_PATH, $"{AUTOSAVE_FOLDER_NAME}\\");
        }

        /// <summary>
        /// get RunWithoutSaving folder path in taskt execution file folder
        /// </summary>
        /// <returns></returns>
        public static string GetRunWithoutSavingFolderPath()
        {
            return Path.Combine(TASKT_EXECUTE_FOLDER_PATH, $"{RUN_WITHOUT_SAVING_FOLDER_NAME}\\");
        }

        /// <summary>
        /// get BeforeConverted folder path in taskt execution file folder
        /// </summary>
        /// <returns></returns>
        public static string GetBeforeConvertedFolderPath()
        {
            return Path.Combine(TASKT_EXECUTE_FOLDER_PATH, $"{BEFORE_CONVERTED_FOLDER_NAME}\\");
        }
    }
}
