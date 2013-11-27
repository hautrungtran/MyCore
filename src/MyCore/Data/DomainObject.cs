namespace MyCore.Data {
    public abstract class DomainObject<TKey> {
        public virtual TKey Id { get; set; }
        public override int GetHashCode() {
            return Id.GetHashCode();
        }
        public override bool Equals(object obj) {
            return Equals(obj as DomainObject<TKey>);
        }
        private bool Equals(DomainObject<TKey> obj) {
            if (obj != null) {
                var type = GetType();
                var otherType = obj.GetType();
                if (type.IsAssignableFrom(otherType) || otherType.IsAssignableFrom(type)) {
                    return Id.Equals(obj.Id);
                }
            }
            return false;
        }
        public static bool operator ==(DomainObject<TKey> left, DomainObject<TKey> right) {
            return (ReferenceEquals(left, null) && ReferenceEquals(right, null)) || (!ReferenceEquals(left, null) && left.Equals(right));
        }
        public static bool operator !=(DomainObject<TKey> left, DomainObject<TKey> right) {
            return !(left == right);
        }
    }
}