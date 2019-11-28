namespace EducationApp.BusinessLogic.Common.Interfaces
{
    public interface ILoggerNLog
    {
        void Information(string message);
        void Warning(string message);
        void Debug(string message);
        void Error(string message);
    }
}
