using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chitataunga.Algorithms.Common.Locks
{
    /// <summary>
    /// A thread safe boolean class using Interlocked Read and Exchange
    /// if the Flag property is true then this object has been set to true, otherwise false
    /// </summary>
    public class BooleanThreadSafe
    {
        private long _isReading = 0;
        private BooleanThreadSafe(bool flag) => Flag = flag;
        public BooleanThreadSafe()
        {

        }
        public bool Flag
        {
            get => Interlocked.Read(ref _isReading) == 1;
            set => Interlocked.Exchange(ref _isReading, Convert.ToInt64(value));
        }
        public static implicit operator bool(BooleanThreadSafe busy) => busy.Flag;
        public static implicit operator BooleanThreadSafe(bool flag) => new (flag);
    }
}
