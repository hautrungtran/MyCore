using System.Collections.Generic;

namespace MyCore.IoC {
    public interface IFactory {
        T Resolve<T>(object key = null) where T : class;
        IEnumerable<T> ResolveMany<T>(object key = null) where T : class;
        bool IsRegistered<T>() where T : class;
    }
}