using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Db
{
    public struct AsyncTryResult<T>
        where T : class
    {
        public AsyncTryResult(T returnValue)
        {
            ReturnValue = returnValue;
            HasResult = returnValue != null;
        }

        public T ReturnValue { get; }
        public bool HasResult { get; }
    }
}