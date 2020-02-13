using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
           
            var settingsDir = Core.IO.Folders.GetFolder(Folders.FolderType.SettingsFolder);

            //if directory does not exist then create directory
            if (!System.IO.Directory.Exists(settingsDir))
            {
                System.IO.Directory.CreateDirectory(settingsDir);
            }

            //create file path
            var filePath =  System.IO.Path.Combine(settingsDir, "AppSettings.xml");

            //create filestream
            var fileStream = System.IO.File.Create(filePath);

            //output to xml file
            XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
            serializer.Serialize(fileStream, appSettings);
            fileStream.Close();
        }
        public ApplicationSettings GetOrCreateApplicationSettings()
        {
            //create settings directory
            var settingsDir = Core.IO.Folders.GetFolder(Folders.FolderType.SettingsFolder);

            //create file path
            var filePath = System.IO.Path.Combine(settingsDir, "AppSettings.xml");

            ApplicationSettings appSettings;
            if (System.IO.File.Exists(filePath))
            {
                //open file and return it or return new settings on error
                var fileStream = System.IO.File.Open(filePath, FileMode.Open);

                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
                    appSettings = (ApplicationSettings)serializer.Deserialize(fileStream);
                }
                catch (Exception)
                {
                    appSettings = new ApplicationSettings();
                }

                fileStream.Close();
            }
            else
            {
                appSettings = new ApplicationSettings();
            }

            return appSettings;
        }
    }
    /// <summary>
    /// Defines Server settings for tasktServer if using the server component to manage the client
    /// </summary>
    [Serializable]
    public class ServerSettings
    {
        public bool ServerConnectionEnabled { get; set; }
        public bool ConnectToServerOnStartup { get; set; }
        public bool RetryServerConnectionOnFail { get; set; }
        public bool BypassCertificateValidation { get; set; }
        public string ServerURL { get; set; }
        public string ServerPublicKey { get; set; }

        public string HTTPServerURL { get; set; }
        public Guid HTTPGuid { get; set; }

        public ServerSettings()
        {
            HTTPServerURL = "https://localhost:44377/";
        }
    }
    /// <summary>
    /// Defines engine settings which can be managed by the user
    /// </summary>
    [Serializable]
    public class EngineSettings
    {
        public bool ShowDebugWindow { get; set; }
        public bool AutoCloseDebugWindow { get; set; }
        public bool EnableDiagnosticLogging { get; set; }
        public bool ShowAdvancedDebugOutput { get; set; }
        public bool CreateMissingVariablesDuringExecution { get; set; }
        public bool TrackExecutionMetrics { get; set; }
        public string VariableStartMarker { get; set; }
        public string VariableEndMarker { get; set; }
        public System.Windows.Forms.Keys CancellationKey { get; set; }
        public int DelayBetweenCommands { get; set; }
        public bool OverrideExistingAppInstances { get; set; }
        public bool AutoCloseMessagesOnServerExecution { get; set; }
        public bool AutoCloseDebugWindowOnServerExecution { get; set; }
        public bool AutoCalcVariables { get; set; }
        public EngineSettings()
        {
            ShowDebugWindow = true;
            AutoCloseDebugWindow = true;
            EnableDiagnosticLogging = true;
            ShowAdvancedDebugOutput = false;
            CreateMissingVariablesDuringExecution = true;
            TrackExecutionMetrics = true;
            VariableStartMarker = "{";
            VariableEndMarker = "}";
            CancellationKey = System.Windows.Forms.Keys.Pause;
            DelayBetweenCommands = 250;
            OverrideExistingAppInstances = false;
            AutoCloseMessagesOnServerExecution = true;
            AutoCloseDebugWindowOnServerExecution = true;
            AutoCalcVariables = true;
    }
    }
    /// <summary>
    /// Defines application/client-level settings which can be managed by the user
    /// </summary>
    [Serializable]
    public class ClientSettings
    {
        public bool AntiIdleWhileOpen { get; set; }
        public string RootFolder { get; set; }
        public bool InsertCommandsInline { get; set; }
        public bool EnableSequenceDragDrop { get; set; }
        public bool MinimizeToTray { get; set; }
        public string AttendedTasksFolder { get; set; }
        public string StartupMode { get; set; }
        public bool PreloadBuilderCommands { get; set; }
        public bool UseSlimActionBar { get; set; }
        public ClientSettings()
        {
            MinimizeToTray = false;
            AntiIdleWhileOpen = false;
            InsertCommandsInline = false;
            RootFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "taskt");
            StartupMode = "Builder Mode";
            AttendedTasksFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "taskt", "My Scripts");
            EnableSequenceDragDrop = true;
            PreloadBuilderCommands = false;
            UseSlimActionBar = true;
        }
    }
    /// <summary>
    /// Defines Server settings for tasktServer if using the server component to manage the client
    /// </summary>
    [Serializable]
    public class LocalListenerSettings
    {
        public bool StartListenerOnStartup { get; set; }
        public bool LocalListeningEnabled { get; set; }
        public bool RequireListenerAuthenticationKey { get; set; }
        public int ListeningPort { get; set; }
        public string AuthKey { get; set; }
        public bool EnableWhitelist { get; set; }
        public string IPWhiteList { get; set; }
        public LocalListenerSettings()
        {
            StartListenerOnStartup = false;
            LocalListeningEnabled = false;
            RequireListenerAuthenticationKey = false;
            EnableWhitelist = false;
            ListeningPort = 19312;
            AuthKey = Guid.NewGuid().ToString();
            IPWhiteList = "";
        }
    }

    [Serializable]
    public class WhiteListIP
    {
        string _value;
        public WhiteListIP(string s)
        {
            _value = s;
        }
        public string Value { get { return _value; } set { _value = value; } }
    }
}
