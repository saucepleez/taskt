using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskt.Core
{
    internal class FilePathControls
    {
        public static bool hasFolderPath(string path)
        {
            return (path != System.IO.Path.GetFileName(path));
        }

        public static bool hasExtension(string path)
        {
            //string file = System.IO.Path.GetFileName(path);
            //return (file != System.IO.Path.GetExtension(path));
            return (System.IO.Path.GetExtension(path).Length > 0);
        }

        public static string formatFilePath(string path, Core.Automation.Engine.AutomationEngineInstance engine)
        {
            if (hasFolderPath(path))
            {
                return path;
            }
            else
            {
                //return engine.engineSettings.;
                return System.IO.Path.GetDirectoryName(engine.FileName) + "\\" + path;
            }
        }

        public static bool containsFileCounter(string path, Core.Automation.Engine.AutomationEngineInstance engine)
        {
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

        //public static string formatFileCounter_NotExists(string path, Core.Automation.Engine.AutomationEngineInstance engine, string extension)
        //{
        //    var settings = engine.engineSettings;
        //    string format = "";
        //    string src = "";
        //    if (path.Contains(settings.wrapVariableMarker("FileCounter.F0")))
        //    {
        //        src = settings.wrapVariableMarker("FileCounter.F0");
        //        format = "0";
        //    }
        //    else if (path.Contains(settings.wrapVariableMarker("FileCounter.F00")))
        //    {
        //        src = settings.wrapVariableMarker("FileCounter.F00");
        //        format = "00";
        //    }
        //    else if (path.Contains(settings.wrapVariableMarker("FileCounter.F000")))
        //    {
        //        src = settings.wrapVariableMarker("FileCounter.F000");
        //        format = "000";
        //    }
        //    else
        //    {
        //        return path;
        //    }

        //    int cnt = 1;
        //    while(true)
        //    {
        //        string trgPath = path.Replace(src, cnt.ToString(format));
        //        trgPath = formatFilePath(trgPath.ConvertToUserVariable(engine), engine);
        //        if (!hasExtension(trgPath))
        //        {
        //            trgPath += (extension.StartsWith(".") ? extension : "." + extension);
        //        }

        //        if (!System.IO.File.Exists(trgPath))
        //        {
        //            return trgPath;
        //        }

        //        cnt++;
        //    }
        //}


        public static string formatFilePath_ContainsFileCounter(string vPath, Automation.Engine.AutomationEngineInstance engine, string extension, bool useExistsFile = false)
        {
            string f0 = engine.engineSettings.wrapVariableMarker("FileCounter.F0");
            string f00 = engine.engineSettings.wrapVariableMarker("FileCounter.F00");
            string f000 = engine.engineSettings.wrapVariableMarker("FileCounter.F000");

            int fileCount = 1;

            while (true)
            {
                string replPath = vPath.Replace(f0, fileCount.ToString()).Replace(f00, fileCount.ToString("00")).Replace(f000, fileCount.ToString("000"));
                string path = replPath.ConvertToUserVariable(engine);

                if (!hasFolderPath(path))
                {
                    path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(engine.FileName), path);
                }

                // extenstion
                if (!hasExtension(path))
                {
                    path = path.EndsWith(".") ? path + extension : path + "." + extension;
                }

                // check existance
                if (System.IO.File.Exists(path))
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

        public static string formatFilePath_NoFileCounter(string vPath, Automation.Engine.AutomationEngineInstance engine, List<string> extensions, bool checkFileExistance = false, bool allowNoExtensionFile = false)
        {
            string path = vPath.ConvertToUserVariable(engine);

            if (!hasFolderPath(path))
            {
                path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(engine.FileName), path);
            }

            // no extension
            if (!hasExtension(path) && allowNoExtensionFile)
            {
                if (checkFileExistance)
                {
                    // no extension & exists
                    if (System.IO.File.Exists(path))
                    {
                        return path;
                    }
                }
                else
                {
                    return path;
                }
            }

            if (!hasExtension(path)) 
            {
                if (checkFileExistance)
                {
                    // add extension loop
                    foreach(var extension in extensions)
                    {
                        string testPath = path.EndsWith(".") ? path + extension : path + "." + extension;
                        if (System.IO.File.Exists(testPath))
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

        public static string formatFilePath_NoFileCounter(string vPath, Automation.Engine.AutomationEngineInstance engine, string extension, bool checkFileExistance = false, bool allowNoExtensionFile = false)
        {
            return formatFilePath_NoFileCounter(vPath, engine, new List<string>() { extension }, checkFileExistance, allowNoExtensionFile);
        }
    }
}
