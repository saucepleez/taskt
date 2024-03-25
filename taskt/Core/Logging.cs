﻿using Serilog;
using Serilog.Formatting.Compact;
using taskt.Core.IO;

namespace taskt.Core
{
    /// <summary>
    /// Handles functionality for logging to files
    /// </summary>
    public class Logging
    {
        public Serilog.Core.Logger CreateLogger(string componentName, RollingInterval logInterval)
        {
            return new LoggerConfiguration()
                    //.WriteTo.File(Folders.GetFolder(Core.IO.Folders.FolderType.LogFolder) + "\\taskt " + componentName + " Logs.txt", rollingInterval: logInterval)
                    .WriteTo.File(Folders.GetLogsFolderPath() + "\\taskt " + componentName + " Logs.txt", rollingInterval: logInterval)
                    .CreateLogger();
        }

        public Serilog.Core.Logger CreateJsonLogger(string componentName, RollingInterval logInterval)
        {
            return new LoggerConfiguration()
                    //.WriteTo.File(new CompactJsonFormatter(), Folders.GetFolder(Core.IO.Folders.FolderType.LogFolder) + "\\taskt " + componentName + " Logs.txt", rollingInterval: logInterval)
                    .WriteTo.File(new CompactJsonFormatter(), Folders.GetLogsFolderPath() + "\\taskt " + componentName + " Logs.txt", rollingInterval: logInterval)
                    .CreateLogger();
        }
    }
}
