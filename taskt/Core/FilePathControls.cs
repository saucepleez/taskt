using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskt.Core
{
    public class FilePathControls
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

        public static string formatFileCounter_NotExists(string path, Core.Automation.Engine.AutomationEngineInstance engine, string extension)
        {
            var settings = engine.engineSettings;
            string format = "";
            string src = "";
            if (path.Contains(settings.wrapVariableMarker("FileCounter.F0")))
            {
                src = settings.wrapVariableMarker("FileCounter.F0");
                format = "0";
            }
            else if (path.Contains(settings.wrapVariableMarker("FileCounter.F00")))
            {
                src = settings.wrapVariableMarker("FileCounter.F00");
                format = "00";
            }
            else if (path.Contains(settings.wrapVariableMarker("FileCounter.F000")))
            {
                src = settings.wrapVariableMarker("FileCounter.F000");
                format = "000";
            }
            else
            {
                string newPath = formatFilePath(path.ConvertToUserVariable(engine), engine);
                if (!hasExtension(newPath))
                {
                    newPath += (extension.StartsWith(".") ? extension : "." + extension);
                }
                return newPath;
            }

            int cnt = 1;
            while(true)
            {
                string trgPath = path.Replace(src, cnt.ToString(format));
                trgPath = formatFilePath(trgPath.ConvertToUserVariable(engine), engine);
                if (!hasExtension(trgPath))
                {
                    trgPath += (extension.StartsWith(".") ? extension : "." + extension);
                }

                if (!System.IO.File.Exists(trgPath))
                {
                    return trgPath;
                }

                cnt++;
            }
        }
    }
}
