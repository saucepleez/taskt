using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace taskt.Core
{
    /// <summary>
    /// Handles functionality for logging to files
    /// </summary>
    public class Logging
    {
        public Serilog.Core.Logger CreateLogger(string componentName)
        {
            return new LoggerConfiguration()
            .WriteTo.File(Core.Common.GetLogFolderPath() + "\\taskt " + componentName + " Logs.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        }

    }
    
}
