using EskroAfrica.PaymentService.Application.Interfaces;
using Serilog;
using Serilog.Events;

namespace EskroAfrica.PaymentService.Application.Implementations
{
    public class LogService : ILogService
    {
        private readonly string _logRef = Guid.NewGuid().ToString();

        public void LogEvent(LogEventLevel level, string message, params object?[]? propertyValues)
        {
            message = $"{_logRef}: {message}";
            Log.Write(level, message, propertyValues);
        }
    }
}
