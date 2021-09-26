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
    }
}
