using MyCore.Data;

namespace MyCore.Services {
    public abstract class UniqueObject<TKey> : IdentityObject<TKey> {
        private string _code;
        private string _name;
        protected UniqueObject() {
            _code = string.Empty;
            _name = string.Empty;
        }
        public virtual string Code {
            get { return _code; }
            set { _code = value; }
        }
        public virtual string Name {
            get { return _name; }
            set { _name = value; }
        }
    }

    public interface IUniqueService<T, TKey> : IServiceBase<T, TKey> where T : UniqueObject<TKey> {
        T GetByCode(string code);
    }

    public abstract class UniqueService<T, TKey> : ServiceBase<T, TKey>, IUniqueService<T, TKey> where T : UniqueObject<TKey> {
        private readonly IRepositoryBase<T, TKey> _repository;
        protected UniqueService(IRepositoryBase<T, TKey> baseRepository)
            : base(baseRepository) {
            _repository = baseRepository;
        }

        #region Implementation of IUniqueService<T>

        public virtual T GetByCode(string code) {
            return string.IsNullOrWhiteSpace(code) ? null : _repository.GetSingle(entity => entity.Code.ToUpper() == code.Trim().ToUpper());
        }

        #endregion
    }
}