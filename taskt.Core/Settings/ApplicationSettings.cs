using Newtonsoft.Json;
using System;
using System.IO;
using taskt.Core.Enums;
using taskt.Core.IO;

namespace taskt.Core.Settings
{
    /// <summary>
    /// Defines settings for the entire application
    /// </summary>
    [Serializable]
    public class ApplicationSettings
    {
        public ServerSettings ServerSettings { get; set; }
        public EngineSettings EngineSettings { get; set; }
        public ClientSettings ClientSettings { get; set; }
        public LocalListenerSettings ListenerSettings { get; set; }

        public ApplicationSettings()
        {
            ServerSettings = new ServerSettings();
            EngineSettings = new EngineSettings();
            ClientSettings = new ClientSettings();
            ListenerSettings = new LocalListenerSettings();
        }

        public void Save(ApplicationSettings appSettings)
        {
            //create settings directory
            var settingsDir = Folders.GetFolder(FolderType.SettingsFolder);

            if (!Directory.Exists(settingsDir))
            {
                Directory.CreateDirectory(settingsDir);
            }

            //create file path
            var filePath = Path.Combine(settingsDir, "AppSettings.json");

            var serializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects
            };
            JsonSerializer serializer = JsonSerializer.Create(serializerSettings);

            //output to json file
            //if output path was provided
            using (StreamWriter sw = new StreamWriter(filePath))
            using (JsonWriter writer = new JsonTextWriter(sw){ Formatting = Formatting.Indented })
            {
                serializer.Serialize(writer, appSettings, typeof(ApplicationSettings));
            }
        }

        public ApplicationSettings GetOrCreateApplicationSettings()
        {
            //create settings directory
            var settingsDir = Folders.GetFolder(FolderType.SettingsFolder);

            //create file path
            var filePath = Path.Combine(settingsDir, "AppSettings.json");

            ApplicationSettings appSettings;
            if (File.Exists(filePath))
            {
                //open file and return it or return new settings on error
                try
                {
                    using (StreamReader file = File.OpenText(filePath))
                    {
                        var serializerSettings = new JsonSerializerSettings()
                        {
                            TypeNameHandling = TypeNameHandling.Objects
                        };

                        JsonSerializer serializer = JsonSerializer.Create(serializerSettings);
                        appSettings = (ApplicationSettings)serializer.Deserialize(file, typeof(ApplicationSettings));
                    }
                }
                catch (Exception)
                {
                    appSettings = new ApplicationSettings();
                }
            }
            else
            {
                appSettings = new ApplicationSettings();
            }

            return appSettings;
        }
    }
}
