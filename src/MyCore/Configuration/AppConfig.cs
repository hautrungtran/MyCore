using System.Configuration;

namespace MyCore.Configuration {
    public interface IConfigurationManager {
        ISettingManager Settings { get; }
        T GetSection<T>(string name) where T : ConfigurationSection;
    }
    public class AppConfig : IConfigurationManager {
        public virtual ISettingManager Settings { get; set; }
        public virtual T GetSection<T>(string name) where T : ConfigurationSection {
            return ConfigurationManager.GetSection(name) as T;
        }
    }
}