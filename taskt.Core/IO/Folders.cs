using System;
using System.IO;
using taskt.Core.Enums;
using taskt.Core.Settings;

namespace taskt.Core.IO
{
    public static class Folders
    {
        public static string GetFolder(FolderType folderType)
        {
            switch (folderType)
            {
                case FolderType.RootFolder:
                    //return root folder from settings
                    var rootSettings = new ApplicationSettings().GetOrCreateApplicationSettings();
                    var rootFolder = rootSettings.ClientSettings.RootFolder;
                    return rootFolder;
                case FolderType.AttendedTasksFolder:
                    //return attended tasks folder from settings
                    var attendedSettings = new ApplicationSettings().GetOrCreateApplicationSettings();
                    var attentedTasksFolder = attendedSettings.ClientSettings.AttendedTasksFolder;
                    return attentedTasksFolder;
                case FolderType.SettingsFolder:
                    //return app data taskt folder
                    return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "taskt");
                case FolderType.ScriptsFolder:
                    //return scripts folder
                    return Path.Combine(GetFolder(FolderType.RootFolder), "My Scripts");
                case FolderType.LogFolder:
                    //return logs folder
                    return Path.Combine(GetFolder(FolderType.RootFolder), "Logs");
                case FolderType.TempFolder:
                    //return temp folder
                    return Path.Combine(Path.GetTempPath(), "taskt");
                default:
                    //enum is not implemented
                    throw new NotImplementedException("FolderType " + folderType.ToString() + " Not Supported");
            }
        }
    }
}
