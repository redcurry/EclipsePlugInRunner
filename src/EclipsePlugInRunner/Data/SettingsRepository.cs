using System.IO;
using System.Reflection;
using YamlDotNet.Serialization;

namespace EclipsePlugInRunner.Data
{
    internal class SettingsRepository
    {
        private const string SettingsFileName = "settings.yaml";

        private readonly string _settingsFilePath;

        public SettingsRepository()
        {
            _settingsFilePath = GetSettingsFilePath();
        }

        private string GetSettingsFilePath()
        {
            return Path.Combine(GetEntryAssemblyDirectory(), SettingsFileName);
        }

        private string GetEntryAssemblyDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

        public Settings ReadSettings()
        {
            if (!File.Exists(_settingsFilePath))
            {
                return new Settings();
            }

            var deserializer = new Deserializer();

            using (var reader = File.OpenText(_settingsFilePath))
            {
                return deserializer.Deserialize<Settings>(reader);
            }
        }

        public void WriteSettings(Settings settings)
        {
            var serializer = new Serializer();

            using (var writer = File.CreateText(_settingsFilePath))
            {
                serializer.Serialize(writer, settings);
            }
        }
    }
}
