namespace PerFinanc.Web.Enums
{
    public enum StepLogLevel
    {
        Debug = 1,
        Info = 2,
        Warn = 3,
        Error = 4,
    }

    public interface IStepLogger
    {
        void Info(string message);
        void Warn(string message);
        void Error(string message, Exception? ex = null);
        void Sucess(string message);
    }
}
