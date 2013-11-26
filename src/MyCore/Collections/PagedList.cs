using System;
using System.Collections.Generic;

namespace MyCore.Collections {
    public class PagedList<T> {

        private readonly Lazy<int> _total;
        public IList<T> Items { get; set; }
        public int Total {
            get {
                return _total.Value;
            }
        }
        public PagedList(IList<T> items, Func<int> getTotal) {
            Items = items;
            _total = new Lazy<int>(getTotal);
        }
    }
}