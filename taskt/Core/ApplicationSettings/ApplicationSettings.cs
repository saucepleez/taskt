using System;
using System.IO;
using System.Xml.Serialization;
using taskt.Core.IO;

namespace taskt.Core
{
    /// <summary>
    /// Defines settings for the entire application
    /// </summary>
    [Serializable]
    public class ApplicationSettings
    {
        public ServerSettings ServerSettings { get; set; } = new ServerSettings();
        public EngineSettings EngineSettings { get; set; } = new EngineSettings();
        public ClientSettings ClientSettings { get; set; } = new ClientSettings();
        public LocalListenerSettings ListenerSettings { get; set; } = new LocalListenerSettings();
        public ApplicationSettings()
        {

        }

        public void Save(ApplicationSettings appSettings)
        {
            //create settings directory

            //var settingsDir = Folders.GetFolder(Folders.FolderType.SettingsFolder);
            var settingsDir = Folders.GetSettingsFolderPath();

            //if directory does not exist then create directory
            if (!Directory.Exists(settingsDir))
            {
                Directory.CreateDirectory(settingsDir);
            }

            //create file path
            var filePath = Path.Combine(settingsDir, "AppSettings.xml");

            ////create filestream
            //var fileStream = System.IO.File.Create(filePath);

            ////output to xml file
            //XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
            //serializer.Serialize(fileStream, appSettings);
            //fileStream.Close();
            SaveAs(appSettings, filePath);
        }

        public static void SaveAs(ApplicationSettings appSettings, string filePath)
        {
            using (FileStream fileStream = File.Create(filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
                serializer.Serialize(fileStream, appSettings);
                fileStream.Close();
            }       
        }

        public ApplicationSettings GetOrCreateApplicationSettings()
        {
            //create settings directory
            //var settingsDir = Folders.GetFolder(Folders.FolderType.SettingsFolder);
            //var settingsDir = Folders.GetSettingsFolderPath();

            //create file path
            var filePath = Path.Combine(Folders.GetSettingsFolderPath(), "AppSettings.xml");

            ApplicationSettings appSettings;
            if (File.Exists(filePath))
            {
                ////open file and return it or return new settings on error
                //var fileStream = System.IO.File.Open(filePath, FileMode.Open);

                //try
                //{
                //    XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
                //    appSettings = (ApplicationSettings)serializer.Deserialize(fileStream);
                //}
                //catch (Exception)
                //{
                //    appSettings = new ApplicationSettings();
                //}

                //fileStream.Close();
                try
                {
                    appSettings = Open(filePath);
                }
                catch
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

        public static ApplicationSettings Open(string filePath)
        {
            ApplicationSettings appSettings = null;
            using (FileStream fileStream = File.Open(filePath, FileMode.Open))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
                    appSettings = (ApplicationSettings)serializer.Deserialize(fileStream);
                }
                catch (Exception ex)
                {
                    //appSettings = new ApplicationSettings();
                    throw ex;
                }
                finally
                {
                    fileStream.Close();
                }
            }
            return appSettings;
        }

        public string replaceApplicationKeyword(string targetString)
        {
            return this.ClientSettings.replaceClientKeyword(this.EngineSettings.replaceEngineKeyword(targetString));
        }
    }
}
