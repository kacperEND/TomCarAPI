using System;

namespace Application.Exceptions
{
    [Serializable]
    public class RecordNotFoundException : TomCarException
    {
        public RecordNotFoundException()
        {
        }

        public RecordNotFoundException(string message) : base(message)
        {
        }

        public RecordNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}