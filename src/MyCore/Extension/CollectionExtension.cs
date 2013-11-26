using System;
using System.Collections.Generic;
using System.Linq;

namespace MyCore.Extension {
    public static class CollectionExtension {
        public static int Remove<T>(this ICollection<T> source, Func<T, bool> func) {
            var count = 0;
            var searchResult = source.Where(func).ToList();
            foreach (var item in searchResult) {
                source.Remove(item);
                count++;
            }
            return count;
        }
        public static bool RemoveFirst<T>(this ICollection<T> source, Func<T, bool> func) {
            var item = source.FirstOrDefault(func);
            return source.Remove(item);
        }
        public static bool RemoveLast<T>(this ICollection<T> source, Func<T, bool> func) {
            var item = source.LastOrDefault(func);
            return source.Remove(item);
        }
    }
}