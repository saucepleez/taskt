using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace taskt.Core.Sockets
{
    /// <summary>
    /// taskt SocketClient wraps WebSocket4Net and manages local client connection to taskt Server
    /// </summary>
   public static class SocketClient
    {
        public static UI.Forms.frmScriptBuilder associatedBuilder;
        private static string publicKey;
        private static string serverURI;
        private static DateTime connectionOpened;
        private static WebSocket4Net.WebSocket webSocket;
        public static string connectionException;
        private static bool retryOnFail;
        private static System.Timers.Timer heartbeatTimer;
        private static bool bypassCertificationValidation;
        static SocketClient()
        {

        }
        /// <summary>
        /// Initializes the Socket Client
        /// </summary>
        public static void Initialize()
        {
            Logging.log.Info("Initializing Socket Client");
            LoadSettings();
        }
        /// <summary>
        /// Loads Settings used for the Socket Client
        /// </summary>
        public static void LoadSettings()
        {
            Logging.log.Info("Loading Socket Client Settings");
            //setup heartbeat to the server
            heartbeatTimer = new System.Timers.Timer();
            heartbeatTimer.Interval = 10000;
            heartbeatTimer.Elapsed += HeartbeatTimer_Elapsed;

            //get app settings
            var appSettings = new Core.ApplicationSettings();
            appSettings = appSettings.GetOrCreateApplicationSettings();

            //pull server settings
            var serverSettings = appSettings.ServerSettings;
            serverURI = serverSettings.ServerURL;
            retryOnFail = serverSettings.RetryServerConnectionOnFail;
            publicKey = serverSettings.ServerPublicKey;
            bypassCertificationValidation = serverSettings.BypassCertificateValidation;

            Logging.log.Info("Socket Client - URI: " + serverURI);
            Logging.log.Info("Socket Client - Retry On Fail: " + retryOnFail);
            Logging.log.Info("Socket Client - Public Key: " + publicKey);
           
            //try to connect to server
            if ((serverSettings.ServerConnectionEnabled) && (serverSettings.ConnectToServerOnStartup))
            {
                Connect(serverSettings.ServerURL);
            }

      

        }

        /// <summary>
        /// Initializes a Web Socket connection to taskt Server
        /// </summary>
        /// <param name="serverUri">URI to the socket server, ex. wss://ip:port/ws</param>
        public static void Connect(string serverUri)
        {
            try
            {
                //var fileName = "Server Connection Logs";
                //taskt.Core.Logging.Setup(fileName, log4net.Core.Level.Debug);
                //log = log4net.LogManager.GetLogger(fileName);

                //var fileName = "Server Connection Logs";
                //var logging = new taskt.Core.Logging();
                //log = logging.Setup(fileName, log4net.Core.Level.Debug);

                if (bypassCertificationValidation)
                {
                    Logging.log.Info("Client bypassing certificate validation");
                    //trust all certificates -- for self signed certificates, etc.
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
                }

                //reset connection exception
                connectionException = string.Empty;

                Logging.log.Info("Client Requesting To Connect to URI: " + serverUri);
                //handle if insecure or invalid connection is defined
                if (!serverUri.ToLower().StartsWith("wss://"))
                {
                    throw new InvalidSocketURI("Socket connections must begin with wss://");
                }

                //create socket connection
                webSocket = new WebSocket4Net.WebSocket(serverUri);
                webSocket.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(ConnectionError);
                webSocket.Opened += new EventHandler(ConnectionOpened);
                webSocket.Closed += new EventHandler(ConnectionClosed);
                webSocket.MessageReceived += new EventHandler<WebSocket4Net.MessageReceivedEventArgs>(MessageReceived);
                webSocket.Open();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Disconnect()
        {
            Logging.log.Info("Client Requested Disconnect");
            webSocket.Close();
        }

        /// <summary>
        /// Event that occurs once a connection is successfully opened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnectionOpened(object sender, EventArgs e)
        {
            //send message
            Logging.log.Info("Server Connection Opened");
            connectionOpened = DateTime.Now;
            SendMessage("CONN_REQUEST");
            heartbeatTimer.Enabled = false;

                          
        }

        private static void HeartbeatTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            
        }

        /// <summary>
        /// Event that occurs once a connection is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnectionClosed(object sender, EventArgs e)
        {
            heartbeatTimer.Enabled = false;
            Logging.log.Info("Server Connection Closed");

            if (retryOnFail)
            {
                Connect(serverURI);
            }


        }
        /// <summary>
        /// Occurs when a connection error happens.  This can fire if taskt server is down or not responding.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnectionError(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            Logging.log.Info("Server Connection Error: " + e.Exception.Message);
            connectionException = e.Exception.Message;

            if (retryOnFail)
            {
                Connect(serverURI);
            }
        }
        /// <summary>
        /// Occurs when a message is received from taskt server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MessageReceived(object sender, WebSocket4Net.MessageReceivedEventArgs e)
        {
            Logging.log.Info("Message Received: " + e.Message);
            //server responded with script
            if (e.Message.Contains("?xml"))
            {
                //execute scripts
                RunXMLScript(e.Message);
            }
            //server wants the client status
            else if (e.Message.Contains("CLIENT_STATUS"))
            {
                SendMessage("CLIENT_STATUS=" + Client.ClientStatus);
            }
            //server send a new public key
            else if (e.Message.Contains("ACCEPT_KEY"))
            {
                var authPublicKey = e.Message.Replace("ACCEPT_KEY=", "");
                publicKey = authPublicKey;
           
                //add public key to app settings and save
                var appSettings = new Core.ApplicationSettings().GetOrCreateApplicationSettings();
                appSettings.ServerSettings.ConnectToServerOnStartup = true;
                appSettings.ServerSettings.ServerConnectionEnabled = true;
                appSettings.ServerSettings.ServerPublicKey = authPublicKey;

                appSettings.Save(appSettings);

           
            }

           

        }

        public static void SendMessage(string message)
        {
            //if connection isnt open don't bother sending
            if ((webSocket.State != WebSocket4Net.WebSocketState.Open))
            {
                return;
            }
    

            //create message package
            var socketPkg = new SocketPackage();
            socketPkg.PUBLIC_KEY = publicKey;
            socketPkg.MESSAGE = message;
            socketPkg.MACHINE_NAME = System.Environment.MachineName;
            socketPkg.USER_NAME = System.Environment.UserName;

            //serialize
            var jsonPackage = Newtonsoft.Json.JsonConvert.SerializeObject(socketPkg);

            Logging.log.Info("Sending Message: " + jsonPackage);

        
            //send message
            webSocket.Send(jsonPackage);
        }

        public static string GetSocketState()
        {
            switch (webSocket.State)
            {
                case WebSocket4Net.WebSocketState.None:
                    return "Disconnected";
                case WebSocket4Net.WebSocketState.Connecting:
                    return "Connecting";
                case WebSocket4Net.WebSocketState.Open:
                    TimeSpan ts = DateTime.Now - connectionOpened;
                    int hours = (int)ts.TotalHours;
                    int minutes = ts.Minutes;
                    int seconds = ts.Seconds;
                    return "Connected - " + hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
                case WebSocket4Net.WebSocketState.Closing:
                    return "Closing";
                case WebSocket4Net.WebSocketState.Closed:
                    return "Closed";
                default:
                    return "Unknown";
            }
        }

        private static void RunXMLScript(string scriptData)
        {


            associatedBuilder.Invoke(new MethodInvoker(delegate ()
            {
                UI.Forms.frmScriptEngine newEngine = new UI.Forms.frmScriptEngine("", null);
                newEngine.xmlInfo = scriptData;
                newEngine.callBackForm = null;
                newEngine.Show();
            }));            

        }
    }

  
    /// <summary>
    /// Model for sending data to taskt Server
    /// </summary>
    public class SocketPackage
    {
        public string PUBLIC_KEY { get; set; }
        public string MACHINE_NAME { get; set; }
        public string USER_NAME { get; set; }
        public string MESSAGE { get; set; }
    }
    /// <summary>
    /// Exception defines Invalid Socket URI passed by user
    /// </summary>
    public class InvalidSocketURI : Exception
    {
        public InvalidSocketURI()
        {
        }

        public InvalidSocketURI(string message)
            : base(message)
        {
        }
    }
}
