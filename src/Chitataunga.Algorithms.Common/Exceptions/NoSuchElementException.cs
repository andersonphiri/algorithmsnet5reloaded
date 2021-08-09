using System;
using System.Runtime.Serialization;

namespace Chitataunga.Algorithms.Common.Exceptions
{
    [Serializable]
    public class NoSuchElementException : CollectionException
    {
        public NoSuchElementException()
        {
        }

        public NoSuchElementException(string message) : base(message)
        {
        }

        public NoSuchElementException(int code, string errorMessage) : base(code, errorMessage)
        {
        }

        public NoSuchElementException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoSuchElementException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}
