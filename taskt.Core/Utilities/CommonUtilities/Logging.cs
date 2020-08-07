using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace taskt.Core.Utilities.CommonUtilities
{
    /// <summary>
    /// Handles functionality for logging to files
    /// </summary>
    public class Logging
    {
        public Logger CreateFileLogger(string filePath, RollingInterval logInterval,
            LogEventLevel minLogLevel = LogEventLevel.Verbose)
        {
            var levelSwitch = new LoggingLevelSwitch();
            levelSwitch.MinimumLevel = minLogLevel;

            return new LoggerConfiguration()
                    .MinimumLevel.ControlledBy(levelSwitch)
                    .WriteTo.File(filePath, rollingInterval: logInterval)
                    .CreateLogger();
        }

        public Logger CreateHTTPLogger(string uri, LogEventLevel minLogLevel = LogEventLevel.Verbose)
        {
            var levelSwitch = new LoggingLevelSwitch();
            levelSwitch.MinimumLevel = minLogLevel;

            return new LoggerConfiguration()
                    .MinimumLevel.ControlledBy(levelSwitch)
                    .WriteTo.Http(uri)
                    .CreateLogger();
        }

        public Logger CreateSignalRLogger(string url, string logHub = "LogHub", string[] logGroupNames = null,
            string[] logUserIds = null, LogEventLevel minLogLevel = LogEventLevel.Verbose)
        {
            var levelSwitch = new LoggingLevelSwitch();
            levelSwitch.MinimumLevel = minLogLevel;

            return new LoggerConfiguration()
                    .MinimumLevel.ControlledBy(levelSwitch)
                    .WriteTo.SignalRClient(url,
                                           hub: logHub, // default is LogHub
                                           groupNames: logGroupNames, // default is null
                                           userIds: logUserIds)// default is null
                    .CreateLogger();
        }

        public Logger CreateJsonFileLogger(string jsonFilePath, RollingInterval logInterval,
            LogEventLevel minLogLevel = LogEventLevel.Verbose)
        {
            var levelSwitch = new LoggingLevelSwitch();
            levelSwitch.MinimumLevel = minLogLevel;

            return new LoggerConfiguration()
                    .MinimumLevel.ControlledBy(levelSwitch)
                    .WriteTo.File(new CompactJsonFormatter(), jsonFilePath, rollingInterval: logInterval)
                    .CreateLogger();
        }
    }
}
