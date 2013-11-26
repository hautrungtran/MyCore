using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using Autofac;
using Autofac.Integration.Mef;
using MyCore.Caching;
using MyCore.Configuration;
using MyCore.IoC;
using MyCore.Logging;
using MyCore.Mapping;

namespace MyCore {
    public class ApplicationManager {
        private readonly static ApplicationManager CurrentContext = new ApplicationManager();
        public static ApplicationManager Current { get { return CurrentContext; } }
        public IFactory Factory { get; set; }
        public IConfigurationManager AppConfig { get; set; }
        public ILogger Logger { get; set; }
        public ICache Cache { get; set; }
        public virtual void Initialize() {

            #region Config

            if (AppConfig == null) {
                InitAppConfig();
            }

            #endregion

            #region Log

            if (Logger == null) {
                InitLogger();
            }

            #endregion

            #region Cache

            if (Cache == null) {
                InitCache();
            }

            #endregion

            #region Factory

            if (Factory == null) {
                InitFactory();
            }

            #endregion

            #region Mapping

            InitMapping();

            #endregion

        }
        protected virtual void InitAppConfig() {
            AppConfig = new AppConfig();
        }
        protected virtual void InitLogger() {
            Logger = new DefaultLogger("DefaultLogger");
        }
        protected virtual void InitCache() {
            Cache = new MemoryCache();
        }
        protected virtual void InitFactory() {
            var builder = new ContainerBuilder();
            builder.RegisterComposablePartCatalog(new AssemblyCatalog(Assembly.GetCallingAssembly()));
            builder.RegisterInstance(Logger);
            builder.RegisterInstance(Cache);
            var factory = new DefaultFactory(builder);

            builder = new ContainerBuilder();
            if (!factory.Container.IsRegistered<IDefaultRegister>()) return;
            var registers = factory.Container.Resolve<IEnumerable<IDefaultRegister>>();
            foreach (var register in registers) {
                register.RegisterIoC(builder);
            }
            builder.Update(factory.Container);

            Factory = factory;
        }
        protected virtual void InitMapping() {
            if (!Factory.IsRegistered<IMappingRegister>()) return;
            foreach (var register in Factory.ResolveMany<IMappingRegister>()) {
                register.CreateMap();
            }
        }
    }
}