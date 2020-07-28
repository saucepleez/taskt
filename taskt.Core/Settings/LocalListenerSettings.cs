using System;

namespace taskt.Core.Settings
{
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
}
