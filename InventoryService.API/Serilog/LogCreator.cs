using Serilog;

namespace InventoryService.API.Serilog
{
    public class LogCreator
    {
        private static LoggingLevelSwitchFromConfig? _levelSwitchFromConfig;
        private static LoggingLevelSwitchFromConfig? _aspLoggingLevel;

        public LogCreator(IConfiguration configuration)
        {
            _levelSwitchFromConfig = new LoggingLevelSwitchFromConfig("LoggingLevel", configuration);
            _aspLoggingLevel = new LoggingLevelSwitchFromConfig("AspLoggingLevel", configuration);
        }

        public static void UpdateLogLevel()
        {
            _levelSwitchFromConfig?.UpdateLoggingLevel();
            _aspLoggingLevel?.UpdateLoggingLevel();
        }

        public static void ConfigureLogging(LoggerConfiguration loggerConfiguration)
        {
            loggerConfiguration
                .MinimumLevel.ControlledBy(_levelSwitchFromConfig)
                .Enrich.WithThreadId()
                .WriteTo.Async(
                    (write) => write.Console(
                        outputTemplate: "{Timestamp:HH:mm:ss.fff} ({ThreadId}) [{Level}]  {Message}, {Exception} {NewLine}"));
        }
    }
}
