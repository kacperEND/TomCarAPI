using System;

namespace Application.Exceptions
{
    [Serializable]
    public class ValidationException : TomCarException
    {
        public ValidationException()
        {
        }

        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public string PermissionName { get; set; }
    }
}