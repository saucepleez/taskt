using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Repository.Hierarchy;
using log4net.Core;
using log4net.Appender;
using log4net.Layout;

namespace taskt.Core
{
    /// <summary>
    /// Handles functionality for logging to files
    /// </summary>
    public static class Logging
    {
        /// <summary>
        /// Sets up a new logging instance
        /// </summary>
        public static void Setup(string logName)
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

            PatternLayout patternLayout = new PatternLayout
            {
                ConversionPattern = "%date [%thread] %-5level - %message%newline"
            };
            patternLayout.ActivateOptions();

            RollingFileAppender roller = new RollingFileAppender
            {
                AppendToFile = true,
                File = Core.Common.GetLogFolderPath() + "\\" + logName + ".txt",
                Layout = patternLayout,
                MaxSizeRollBackups = 5,
                MaximumFileSize = "1GB",
                RollingStyle = RollingFileAppender.RollingMode.Size,
                StaticLogFileName = true
            };
            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);

            MemoryAppender memory = new MemoryAppender();
            memory.ActivateOptions();
            hierarchy.Root.AddAppender(memory);

            hierarchy.Root.Level = Level.Info;
            hierarchy.Configured = true;
        }
    }
}
