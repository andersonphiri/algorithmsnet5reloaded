using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chitataunga.Algorithms.Common.Exceptions
{
    [Serializable]
    public class EmptyCollectionException : Exception
    {
        public int StatusCode { get; protected set; }

        public EmptyCollectionException()
        {
        }

        public EmptyCollectionException(string message) : base(message)
        {
        }

        public EmptyCollectionException(int code, string errorMessage) : this(errorMessage)
        {
            StatusCode = code;
        }

        public EmptyCollectionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmptyCollectionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}
