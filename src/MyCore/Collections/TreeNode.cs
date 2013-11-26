using System.Collections.Generic;

namespace MyCore.Collections {
    public class TreeNode<T> {
        private IList<TreeNode<T>> _children;
        public IList<TreeNode<T>> Children {
            get {
                return _children ?? (_children = new List<TreeNode<T>>());
            }
            set {
                _children = value;
            }
        }
        public T Item { get; set; }
        public TreeNode(T item) {
            Item = item;
        }
        public TreeNode<T> AddChild(T item) {
            var nodeItem = new TreeNode<T>(item);
            Children.Add(nodeItem);
            return nodeItem;
        }
    }
}