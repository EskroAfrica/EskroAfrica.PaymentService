using Serilog.Events;

namespace EskroAfrica.PaymentService.Application.Interfaces
{
    public interface ILogService
    {
        void LogEvent(LogEventLevel level, string message, params object?[]? propertyValues);
    }
}
