using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace taskt.Core.Server
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
        private static System.Timers.Timer reconnectTimer;
        private static bool bypassCertificationValidation;
        public static Serilog.Core.Logger socketLogger;
        static SocketClient()
        {
            socketLogger = new Core.Logging().CreateLogger("Socket", Serilog.RollingInterval.Day);
        }
        /// <summary>
        /// Initializes the Socket Client
        /// </summary>
        public static void Initialize()
        {
          
            //socketLogger.Information("Socket Client Initialized");
            //LoadSettings();
        }
        /// <summary>
        /// Loads Settings used for the Socket Client
        /// </summary>
        public static void LoadSettings()
        {

            socketLogger.Information("Socket Client Loading Settings");

            //setup heartbeat to the server
            reconnectTimer = new System.Timers.Timer();
            reconnectTimer.Interval = 60000;
            reconnectTimer.Elapsed += ReconnectTimer_Elapsed;

            //get app settings
            var appSettings = new Core.ApplicationSettings();
            appSettings = appSettings.GetOrCreateApplicationSettings();

            //pull server settings
            var serverSettings = appSettings.ServerSettings;
            serverURI = serverSettings.ServerURL;
            retryOnFail = serverSettings.RetryServerConnectionOnFail;
            publicKey = serverSettings.ServerPublicKey;
            bypassCertificationValidation = serverSettings.BypassCertificateValidation;
            
           
            //try to connect to server
            if ((serverSettings.ServerConnectionEnabled) && (serverSettings.ConnectToServerOnStartup))
            {
                socketLogger.Information("Socket Client Connecting on Startup");
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

              

                if (bypassCertificationValidation)
                {
                    socketLogger.Information("Socket Client Bypasses SSL Validation");
                    //trust all certificates -- for self signed certificates, etc.
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
                }

                //reset connection exception
                connectionException = string.Empty;

              
                //handle if insecure or invalid connection is defined
                if (!serverUri.ToLower().StartsWith("wss://"))
                {
                    socketLogger.Information("Socket Client URI Invalid");
                    throw new InvalidSocketURI("Socket connections must begin with wss://");
                }

                //create socket connection
                webSocket = new WebSocket4Net.WebSocket(serverUri);
                webSocket.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(ConnectionError);
                webSocket.Opened += new EventHandler(ConnectionOpened);
                webSocket.Closed += new EventHandler(ConnectionClosed);
                webSocket.MessageReceived += new EventHandler<WebSocket4Net.MessageReceivedEventArgs>(MessageReceived);

                socketLogger.Information("Socket Client Opening Connection To: " + serverUri);
                webSocket.Open();

            }
            catch (Exception ex)
            {
                socketLogger.Information("Connection Error: " + ex.ToString());
            }
        }

        public static void Disconnect()
        {
            socketLogger.Information("Socket Client Disconnecting");
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
            socketLogger.Information("Socket Client Sending Connection Opened Successfully");
            connectionOpened = DateTime.Now;
            SendMessage("CONN_REQUEST");
            reconnectTimer.Enabled = true;

                          
        }

        private static void ReconnectTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            socketLogger.Information("Retrying Socket Connection");
            Connect(serverURI);
        }

        /// <summary>
        /// Event that occurs once a connection is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnectionClosed(object sender, EventArgs e)
        {
            socketLogger.Information("Socket Client Connection Closed");
      
          

            if (retryOnFail)
            {
                reconnectTimer.Enabled = true;          
            }


        }
        /// <summary>
        /// Occurs when a connection error happens.  This can fire if taskt server is down or not responding.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnectionError(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
           
            connectionException = e.Exception.Message;
            socketLogger.Information("Socket Client Connection Error: " + connectionException);

            if (retryOnFail)
            {
                reconnectTimer.Enabled = true;
            }
        }
        /// <summary>
        /// Occurs when a message is received from taskt server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MessageReceived(object sender, WebSocket4Net.MessageReceivedEventArgs e)
        {

            socketLogger.Information("Socket Message Received: " + e.Message);

            //server responded with script
            if (e.Message.Contains("?xml"))
            {
                //execute scripts
                RunXMLScript(e.Message);
            }
            //server wants the client status
            else if (e.Message.Contains("CLIENT_STATUS"))
            {
                SendMessage("CLIENT_STATUS=Ping Request Received, " + Client.ClientStatus);
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
        public static void SendExecutionLog(string executionLog)
        {
            //new Thread(() =>
            //{
            //    Thread.CurrentThread.IsBackground = true;

            try
            {
                if (SocketClient.webSocket.State == WebSocket4Net.WebSocketState.Open)
                {
                    using (WebClient client = new WebClient())
                    {
                        try
                        {

                            client.QueryString.Add("ClientName", SocketClient.publicKey);
                            client.QueryString.Add("LogData", executionLog);

                            //create server uri for logging
                            var apiUri = SocketClient.serverURI;
                            apiUri = apiUri.Replace("wss://", "https://").Replace("/ws", "/api/WriteLog");


                            byte[] responsebytes = client.UploadValues(apiUri, "POST", client.QueryString);
                            string responsebody = Encoding.UTF8.GetString(responsebytes);
                        }
                        catch (Exception)
                        {
                            //throw
                        }
                    }
                }
            }
            catch (Exception)
            {
                //throw;
            }


                


            //}).Start();

        }

        public static void SendMessage(string message)
        {
        
            if (webSocket == null)
            {
                return;
            }

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

            socketLogger.Information("Sending Socket Message: " + jsonPackage);
            //send message
            webSocket.Send(jsonPackage);
        }

        public static string GetSocketState()
        {
            if (webSocket == null)
            {
                return "Unknown";
            }

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
                UI.Forms.frmScriptEngine newEngine = new UI.Forms.frmScriptEngine();
                newEngine.xmlData = scriptData;
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
