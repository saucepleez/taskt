using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    var rootSettings = new Core.ApplicationSettings().GetOrCreateApplicationSettings();
                    var rootFolder = rootSettings.ClientSettings.RootFolder;
                    return rootFolder;
                case FolderType.AttendedTasksFolder:
                    //return attended tasks folder from settings
                    var attendedSettings = new Core.ApplicationSettings().GetOrCreateApplicationSettings();
                    var attentedTasksFolder = attendedSettings.ClientSettings.AttendedTasksFolder;
                    return attentedTasksFolder;
                case FolderType.SettingsFolder:
                    //return app data taskt folder
                    return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\taskt\\";
                case FolderType.ScriptsFolder:
                    //return scripts folder
                    return System.IO.Path.Combine(GetFolder(FolderType.RootFolder), "My Scripts\\");
                case FolderType.LogFolder:
                    //return logs folder
                    return System.IO.Path.Combine(GetFolder(FolderType.RootFolder), "Logs\\");
                case FolderType.TempFolder:
                    //return temp folder
                    return System.IO.Path.GetTempPath() + "\\taskt\\";
                default:
                    //enum is not implemented
                    throw new NotImplementedException("FolderType " + folderType.ToString() + " Not Supported");
            }

        }


        public enum FolderType
        {
            RootFolder,
            SettingsFolder,
            ScriptsFolder,
            LogFolder,
            TempFolder,
            AttendedTasksFolder
        }
    }
}
