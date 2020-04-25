using System.Collections.Generic;

namespace MatchBox.Data.Extensions
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
