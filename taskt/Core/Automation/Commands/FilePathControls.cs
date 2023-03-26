using System.Collections.Generic;
using System.IO;
using taskt.Core.Automation.Attributes.ClassAttributes;

namespace taskt.Core.Automation.Commands
{
    internal class FilePathControls
    {
        /// <summary>
        /// check file path has folder path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool HasFolderPath(string path)
        {
            return (path != Path.GetFileName(path));
        }

        /// <summary>
        /// check file path has extension
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool HasExtension(string path)
        {
            return (Path.GetExtension(path).Length > 0);
        }

        /// <summary>
        /// check file path contains FileCounter variable
        /// </summary>
        /// <param name="path"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static bool ContainsFileCounter(string path, Engine.AutomationEngineInstance engine)
        {
            path = path ?? "";

            var settings = engine.engineSettings;
            if (path.Contains(settings.wrapVariableMarker("FileCounter.F0")))
            {
                return true;
            }
            else if (path.Contains(settings.wrapVariableMarker("FileCounter.F00")))
            {
                return true;
            }
            else if (path.Contains(settings.wrapVariableMarker("FileCounter.F000")))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// convert file path that contains FileCounter variable
        /// </summary>
        /// <param name="vPath"></param>
        /// <param name="engine"></param>
        /// <param name="extension"></param>
        /// <param name="useExistsFile"></param>
        /// <returns></returns>
        public static string FormatFilePath_ContainsFileCounter(string vPath, Engine.AutomationEngineInstance engine, string extension, bool useExistsFile = false)
        {
            vPath = vPath ?? "";

            string f0 = engine.engineSettings.wrapVariableMarker("FileCounter.F0");
            string f00 = engine.engineSettings.wrapVariableMarker("FileCounter.F00");
            string f000 = engine.engineSettings.wrapVariableMarker("FileCounter.F000");

            int fileCount = 1;

            while (true)
            {
                string replPath = vPath.Replace(f0, fileCount.ToString()).Replace(f00, fileCount.ToString("00")).Replace(f000, fileCount.ToString("000"));
                string path = replPath.ConvertToUserVariable(engine);

                if (!HasFolderPath(path))
                {
                    path = Path.Combine(Path.GetDirectoryName(engine.FileName), path);
                }

                // extenstion
                if (!HasExtension(path))
                {
                    path = path.EndsWith(".") ? path + extension : path + "." + extension;
                }

                // check existance
                if (File.Exists(path))
                {
                    // use if exists
                    if (useExistsFile)
                    {
                        return path;
                    }
                }
                else
                {
                    // use if not exists
                    if (!useExistsFile)
                    {
                        return path;
                    }
                }

                fileCount++;    // next count
            }
        }

        /// <summary>
        /// convert file path that does NOT contains FileCounter variable
        /// </summary>
        /// <param name="vPath"></param>
        /// <param name="engine"></param>
        /// <param name="extensions"></param>
        /// <param name="checkFileExistance"></param>
        /// <param name="allowNoExtensionFile"></param>
        /// <returns></returns>
        public static string FormatFilePath_NoFileCounter(string vPath, Engine.AutomationEngineInstance engine, List<string> extensions, bool checkFileExistance = false, bool allowNoExtensionFile = false)
        {
            vPath = vPath ?? "";
            string path = vPath.ConvertToUserVariable(engine);

            if (!HasFolderPath(path))
            {
                path = Path.Combine(Path.GetDirectoryName(engine.FileName), path);
            }

            // no extension
            if (!HasExtension(path) && allowNoExtensionFile)
            {
                if (checkFileExistance)
                {
                    // no extension & exists
                    if (File.Exists(path))
                    {
                        return path;
                    }
                }
                else
                {
                    return path;
                }
            }

            if (!HasExtension(path)) 
            {
                if (checkFileExistance)
                {
                    // add extension loop
                    foreach(var extension in extensions)
                    {
                        string testPath = path.EndsWith(".") ? path + extension : path + "." + extension;
                        if (File.Exists(testPath))
                        {
                            return testPath;
                        }
                    }
                    // no exists
                    return path.EndsWith(".") ? path + extensions[0] : path + "." + extensions[0];
                }
                else
                {
                    return path.EndsWith(".") ? path + extensions[0] : path + "." + extensions[0];
                }
            }
            else
            {
                return path;
            }
        }

        /// <summary>
        /// convert file path that does NOT contains FileCounter variable
        /// </summary>
        /// <param name="vPath"></param>
        /// <param name="engine"></param>
        /// <param name="extension"></param>
        /// <param name="checkFileExistance"></param>
        /// <param name="allowNoExtensionFile"></param>
        /// <returns></returns>
        public static string FormatFilePath_NoFileCounter(string vPath, Engine.AutomationEngineInstance engine, string extension, bool checkFileExistance = false, bool allowNoExtensionFile = false)
        {
            return FormatFilePath_NoFileCounter(vPath, engine, new List<string>() { extension }, checkFileExistance, allowNoExtensionFile);
        }

        /// <summary>
        /// check file path is URL
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsURL(string path)
        {
            return (path.StartsWith("http:") || path.StartsWith("https:"));
        }

        /// <summary>
        /// format file/folder path to specified format
        /// </summary>
        /// <param name="path"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string FormatFileFolderPath(string path, string format)
        {
            switch (format.ToLower())
            {
                case "file":
                case "filename":
                case "fn":
                    return Path.GetFileName(path);

                case "folder":
                case "directory":
                case "dir":
                    return Path.GetDirectoryName(path);

                case "filewithoutextension":
                case "filenamewithoutextension":
                case "fnwoext":
                    return Path.GetFileNameWithoutExtension(path);

                case "extension":
                case "ext":
                    return Path.GetExtension(path);

                case "drive":
                case "drivename":
                case "root":
                    return Path.GetPathRoot(path);

                default:
                    return "";
            }
        }

        /// <summary>
        /// get format information
        /// </summary>
        /// <returns></returns>
        public static string GetFormatHelp()
        {
            string help =
                @"File
FileName
fn
    File name.
Folder
Directory
dir
    Folder name.
FileWithoutExtension
FileNameWithoutExtension
fnwoext
    File name without extension.
Extension
ext
    File extension.
Drive
DriveName
root
    Drive name.

!!These are Case insensitive!!";
            return help;
        }
    }
}
