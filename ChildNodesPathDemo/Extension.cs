using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildNodesPathDemo
{
    public static class Extension
    {
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> sequence, Func<T, IEnumerable<T>> childFetcher)
        {
            var itemsToYield = new Queue<T>(sequence);
            while (itemsToYield.Count > 0)
            {
                var item = itemsToYield.Dequeue();
                yield return item;

                var children = childFetcher(item);
                if (children != null)
                {
                    foreach (var child in children)
                    {
                        itemsToYield.Enqueue(child);
                    }
                }
            }
        }
    }
}
