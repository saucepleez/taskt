using System;
using System.IO;

namespace taskt.Core.Automation.Commands
{
    public static class EM_FolderCopyFolderPropertiesExtensionMethods
    {
        /// <summary>
        /// create folder copy action
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static Action<string, string> CreateFolderCopyAction(this IFolderCopyFolderProperties command, Engine.AutomationEngineInstance engine)
        {
            if (command.ToScriptCommand().ExpandValueOrUserVariableAsYesNo(nameof(command.v_CopySubFolder), engine))
            {
                return new Action<string, string>((a, b) =>
                        {
                            CopyFolderProcess(a, b, true);
                        });
            }
            else
            {
                return new Action<string, string>((a, b) =>
                        {
                            CopyFolderProcess(a, b, false);
                        });
            }
        }

        /// <summary>
        /// copy folder process
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        /// <param name="copySubDirs"></param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        private static void CopyFolderProcess(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // If the destination directory doesn't exist, create it.

            if (!Directory.GetParent(destDirName).Exists)
            {
                throw new DirectoryNotFoundException($"Destination directory does not exist or could not be found: '{Directory.GetParent(destDirName)}'");
            }

            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the subdirectories for the specified directory.
            DirectoryInfo sDirInfo = new DirectoryInfo(sourceDirName);
            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = sDirInfo.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                DirectoryInfo[] subDirs = sDirInfo.GetDirectories();
                foreach (DirectoryInfo subdir in subDirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    CopyFolderProcess(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}
