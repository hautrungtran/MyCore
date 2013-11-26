using System.Collections.Generic;
using Autofac;

namespace MyCore.IoC {
    public class DefaultFactory : IFactory {
        public virtual IContainer Container { get; private set; }
        public DefaultFactory(ContainerBuilder builder) {
            Container = builder.Build();
        }

        #region Implementation of IDependencyFactory
        public virtual void Update(ContainerBuilder builder) {
            builder.Update(Container);
        }
        public virtual T Resolve<T>(object key = null) where T : class {
            if (Container != null && Container.IsRegistered<T>()) {
                return key != null ? Container.ResolveKeyed<T>(key) : Container.Resolve<T>();
            }
            return default(T);
        }
        public virtual IEnumerable<T> ResolveMany<T>(object key = null) where T : class {
            if (Container != null && Container.IsRegistered<T>()) {
                return key != null ? Container.ResolveKeyed<IEnumerable<T>>(key) : Container.Resolve<IEnumerable<T>>();
            }
            return new List<T>();
        }
        public virtual bool IsRegistered<T>() where T : class {
            return Container.IsRegistered<T>();
        }

        #endregion

    }
}