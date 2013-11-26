using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Configuration;

namespace MyCore.Configuration {
    public interface ISettingManager {
        NameValueCollection Settings { get; }
        string this[string key] { get; }
        bool ContainsKey(string key);
        string GetValue(string key);
        string AddOrGetExits(string key, string defaultValue);
        void AddOrUpdate(string key, string value);
        void Remove(string key);
    }
    [Export(typeof(ISettingManager))]
    public class SettingManager : ISettingManager {
        public virtual NameValueCollection Settings { get { return ConfigurationManager.AppSettings; } }
        public virtual string this[string key] { get { return GetValue(key); } }
        public virtual bool ContainsKey(string key) { return Settings[key] != null; }
        public virtual string GetValue(string key) { return Settings[key]; }
        public virtual string AddOrGetExits(string key, string defaultValue) {
            if (!ContainsKey(key)) {
                AddOrUpdate(key, defaultValue);
            }
            return GetValue(key);
        }
        public virtual void AddOrUpdate(string key, string value) {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = config.AppSettings.Settings;
            if (ContainsKey(key)) {
                settings[key].Value = value;
            } else {
                settings.Add(key, value);
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
        }
        public virtual void Remove(string key) {
            if (!ContainsKey(key)) return;
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove(key);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
        }
    }
}