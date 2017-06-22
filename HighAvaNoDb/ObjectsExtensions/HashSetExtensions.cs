using System.Collections.Generic;

namespace HighAvaNoDb.ObjectsExtensions
{
    public static class CollectionExtensions
    {
        public static void AddAll<T>(this ICollection<T>  entity, ICollection<T> items)
        {
            foreach (var item in items)
            {
                entity.Add(item);
            }
        }
    }
}
