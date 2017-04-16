namespace Whip.Services.Interfaces.Utilities
{
    public interface ILoggingService
    {
        void Info(string message);

        void Warn(string message);

        void Error(string message);

        void Fatal(string message);
    }
}
