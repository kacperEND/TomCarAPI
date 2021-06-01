using System;

namespace Application.Exceptions
{
    [Serializable]
    public class TomCarException : ApplicationException
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public TomCarException(Exception ex)
        {
            Type = ex.GetType().Name;
            Message = ex.Message;
        }

        public TomCarException()
        {
        }

        public TomCarException(string message) : base(message)
        {
        }

        public TomCarException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}