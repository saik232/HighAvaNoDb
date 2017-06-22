using System.Collections.Generic;
using System.Linq;

namespace HighAvaNoDb.ObjectsExtensions
{
    public static class SortedSetExtensions
    {
        public static IEnumerable<T> HeadSet<T>(this SortedSet<T> entity, T item)
        {
            var headSet = new SortedSet<T>();
            IEnumerable<T> result = entity.TakeWhile(key => !key.Equals(item));
            return result;
        }
    }
}
