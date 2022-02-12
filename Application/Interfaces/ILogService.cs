namespace Application.Interfaces
{
    public interface ILogService
    {
        void Error(string errorMessage, string user, string description, string code = null);
        void Info(string message, string user, string description);
    }
}