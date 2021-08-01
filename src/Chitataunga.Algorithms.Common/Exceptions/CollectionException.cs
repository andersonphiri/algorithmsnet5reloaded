using System;
using System.Runtime.Serialization;

namespace Chitataunga.Algorithms.Common.Exceptions
{
    [Serializable]
    public class CollectionException : EmptyCollectionException
    {

        public CollectionException()
        {
        }

        public CollectionException(string message) : base(message)
        {
        }

        public CollectionException(int code, string errorMessage) : base(code,errorMessage)
        {
        }

        public CollectionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CollectionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}
