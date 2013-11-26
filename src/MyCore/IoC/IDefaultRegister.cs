using Autofac;

namespace MyCore.IoC {
    public interface IDefaultRegister {
        void RegisterIoC(ContainerBuilder builder);
    }
}