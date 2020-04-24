using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Db
{
    // Inspired by https://stackoverflow.com/questions/18716928/how-to-write-a-async-method-with-out-parameter
    // See Jerry Nixon's options on how to get around the fact out arguments are not ok with async methods.

    public struct AsyncTryFindResult<T>
        where T : class
    {
        public AsyncTryFindResult(T returnValue)
        {
            Value = returnValue;
            Found = returnValue != null;
        }

        /// <summary>
        /// The entity that was retrieved from the database if it was found.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// A flag indicating if the Find operation succeeded and we have an instance in the property Value.
        /// </summary>
        public bool Found { get; }
    }
}