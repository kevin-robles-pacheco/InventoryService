using Serilog.Core;
using Serilog.Events;

namespace InventoryService.API.Serilog
{
    public class LoggingLevelSwitchFromConfig : LoggingLevelSwitch
    {
        private readonly string _confName;
        private readonly IConfiguration _configuration;

        public LoggingLevelSwitchFromConfig(string confName, IConfiguration configuration)
        {
            _confName = confName;
            _configuration = configuration;
            SetLoggingLevel();
        }

        public void UpdateLoggingLevel()
        {
            SetLoggingLevel();
        }

        private void SetLoggingLevel()
        {
            if (Enum.TryParse<LogEventLevel>(_configuration[_confName] ?? "Warning", true, out var level))
            {
                MinimumLevel = level;
            }
        }
    }
}
