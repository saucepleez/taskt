using System;

namespace taskt.Core.Settings
{
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
}
