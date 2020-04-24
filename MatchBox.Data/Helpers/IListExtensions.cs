using System;
using System.Collections.Generic;
using System.Text;

namespace MatchBox.Data.Helpers
{
    public static class IListExtensions
    {
        public static T AddAndReturn<T>(this IList<T> list, T item)
        {
            list.Add(item);
            return item;
        }
    }
}
