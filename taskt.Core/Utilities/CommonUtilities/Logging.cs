using Serilog;
using Serilog.Core;
using Serilog.Formatting.Compact;
using taskt.Core.Enums;
using taskt.Core.IO;

namespace taskt.Core.Utilities.CommonUtilities
{
    /// <summary>
    /// Handles functionality for logging to files
    /// </summary>
    public class Logging
    {
        public Logger CreateLogger(string componentName, RollingInterval logInterval)
        {
            return new LoggerConfiguration()
                .WriteTo.File(
                Folders.GetFolder(FolderType.LogFolder) + "\\taskt " + componentName + " Logs.txt",
                rollingInterval: logInterval
                )
                .CreateLogger();
        }
        public Logger CreateJsonLogger(string componentName, RollingInterval logInterval)
        {
            return new LoggerConfiguration()
                .WriteTo.File(
                new CompactJsonFormatter(),
                Folders.GetFolder(FolderType.LogFolder) + "\\taskt " + componentName + " Logs.txt",
                rollingInterval: logInterval
                )
                .CreateLogger();
        }
    }
}
