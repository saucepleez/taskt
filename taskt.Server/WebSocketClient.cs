using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using taskt.Core.Common;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.IO;
using taskt.Core.Model.ServerModel;
using taskt.Core.Settings;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Server.Exceptions;
using WebSocket4Net; // Used for WebSocket and MessageReceivedEventArgs
using ErrorEventArgs = SuperSocket.ClientEngine.ErrorEventArgs;

namespace taskt.Server
{
    /// <summary>
    /// taskt SocketClient wraps WebSocket4Net and manages local client connection to taskt Server
    /// </summary>
    public static class SocketClient
    {
        public static IfrmScriptBuilder AssociatedBuilder;
        public static string ConnectionException;
        public static Logger SocketLogger;

        private static IfrmScriptEngine _scriptEngine;
        private static string _publicKey;
        private static string _serverURI;
        private static DateTime _connectionOpened;
        private static WebSocket _webSocket;
        private static bool _retryOnFail;
        private static System.Timers.Timer _reconnectTimer;
        private static bool _bypassCertificationValidation;

        static SocketClient()
        {
            string socketLoggerFilePath = Path.Combine(Folders.GetFolder(FolderType.LogFolder), "taskt Socket Logs.txt");
            SocketLogger = new Logging().CreateFileLogger(socketLoggerFilePath, RollingInterval.Day);
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
            SocketLogger.Information("Socket Client Loading Settings");

            //setup heartbeat to the server
            _reconnectTimer = new System.Timers.Timer();
            _reconnectTimer.Interval = 60000;
            _reconnectTimer.Elapsed += ReconnectTimer_Elapsed;

            //get app settings
            var appSettings = new ApplicationSettings();
            appSettings = appSettings.GetOrCreateApplicationSettings();

            //pull server settings
            var serverSettings = appSettings.ServerSettings;

            _serverURI = serverSettings.ServerURL;
            _retryOnFail = serverSettings.RetryServerConnectionOnFail;
            _publicKey = serverSettings.ServerPublicKey;
            _bypassCertificationValidation = serverSettings.BypassCertificateValidation;

            //try to connect to server
            if (serverSettings.ServerConnectionEnabled && serverSettings.ConnectToServerOnStartup)
            {
                SocketLogger.Information("Socket Client Connecting on Startup");
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
                if (_bypassCertificationValidation)
                {
                    SocketLogger.Information("Socket Client Bypasses SSL Validation");
                    //trust all certificates -- for self signed certificates, etc.
                    ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
                }

                //reset connection exception
                ConnectionException = string.Empty;

                //handle if insecure or invalid connection is defined
                if (!serverUri.ToLower().StartsWith("wss://"))
                {
                    SocketLogger.Information("Socket Client URI Invalid");
                    throw new InvalidSocketUriException("Socket connections must begin with wss://");
                }

                //create socket connection
                _webSocket = new WebSocket(serverUri);
                _webSocket.Error += new EventHandler<ErrorEventArgs>(ConnectionError);
                _webSocket.Opened += new EventHandler(ConnectionOpened);
                _webSocket.Closed += new EventHandler(ConnectionClosed);
                _webSocket.MessageReceived += new EventHandler<MessageReceivedEventArgs>(MessageReceived);

                SocketLogger.Information("Socket Client Opening Connection To: " + serverUri);
                _webSocket.Open();
            }
            catch (Exception ex)
            {
                SocketLogger.Information("Connection Error: " + ex.ToString());
            }
        }

        public static void Disconnect()
        {
            SocketLogger.Information("Socket Client Disconnecting");
            _webSocket.Close();
        }

        /// <summary>
        /// Event that occurs once a connection is successfully opened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnectionOpened(object sender, EventArgs e)
        {
            //send message
            SocketLogger.Information("Socket Client Sending Connection Opened Successfully");
            _connectionOpened = DateTime.Now;
            SendMessage("CONN_REQUEST");
            _reconnectTimer.Enabled = true;
        }

        private static void ReconnectTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SocketLogger.Information("Retrying Socket Connection");
            Connect(_serverURI);
        }

        /// <summary>
        /// Event that occurs once a connection is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnectionClosed(object sender, EventArgs e)
        {
            SocketLogger.Information("Socket Client Connection Closed");

            if (_retryOnFail)
            {
                _reconnectTimer.Enabled = true;
            }
        }
        /// <summary>
        /// Occurs when a connection error happens.  This can fire if taskt server is down or not responding.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnectionError(object sender, ErrorEventArgs e)
        {
            ConnectionException = e.Exception.Message;
            SocketLogger.Information("Socket Client Connection Error: " + ConnectionException);

            if (_retryOnFail)
            {
                _reconnectTimer.Enabled = true;
            }
        }

        /// <summary>
        /// Occurs when a message is received from taskt server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            SocketLogger.Information("Socket Message Received: " + e.Message);

            //server responded with script
            if (e.Message.Contains("taskt.Core.Script.Script"))
            {
                //execute scripts
                RunJsonScript(e.Message);
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
                _publicKey = authPublicKey;

                //add public key to app settings and save
                var appSettings = new ApplicationSettings().GetOrCreateApplicationSettings();
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
            if (_webSocket?.State == WebSocketState.Open)
            {
                try
                {
                    //create server uri for logging
                    var apiUri = _serverURI;
                    apiUri = apiUri.Replace("wss://", "https://").Replace("/ws", "/api/WriteLog");

                    using (WebClient client = new WebClient())
                    {
                        client.QueryString.Add("ClientName", _publicKey);
                        client.QueryString.Add("LogData", executionLog);

                        byte[] responseBytes = client.UploadValues(apiUri, "POST", client.QueryString);
                        string responseBody = Encoding.UTF8.GetString(responseBytes);
                    }
                }
                catch (Exception)
                {
                    //throw
                }
            }
            //}).Start();
        }

        public static void SendMessage(string message)
        {
            // If the connection isn't open don't send
            if (_webSocket == null || _webSocket.State != WebSocketState.Open)
            {
                return;
            }

            //create message package
            var socketPkg = new SocketPackage();
            socketPkg.PublicKey = _publicKey;
            socketPkg.Message = message;
            socketPkg.MachineName = Environment.MachineName;
            socketPkg.UserName = Environment.UserName;

            //serialize
            var jsonPackage = JsonConvert.SerializeObject(socketPkg);

            SocketLogger.Information("Sending Socket Message: " + jsonPackage);
            //send message
            _webSocket.Send(jsonPackage);
        }

        public static string GetSocketState()
        {
            if (_webSocket == null)
            {
                return "Unknown";
            }

            switch (_webSocket.State)
            {
                case WebSocketState.None:
                    return "Disconnected";
                case WebSocketState.Connecting:
                    return "Connecting";
                case WebSocketState.Open:
                    TimeSpan ts = DateTime.Now - _connectionOpened;
                    int hours = (int)ts.TotalHours;
                    int minutes = ts.Minutes;
                    int seconds = ts.Seconds;
                    return "Connected - " + hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
                case WebSocketState.Closing:
                    return "Closing";
                case WebSocketState.Closed:
                    return "Closed";
                default:
                    return "Unknown";
            }
        }

        private static void RunJsonScript(string scriptData)
        {
            ((Form)AssociatedBuilder).Invoke(new MethodInvoker(delegate ()
                {
                    _scriptEngine.JsonData = scriptData;
                    _scriptEngine.CallBackForm = null;
                    ((Form)_scriptEngine).Show();
                })
            );
        }

        public static void InitializeScriptEngine(IfrmScriptEngine newEngine)
        {
            if (_scriptEngine == null)
                _scriptEngine = newEngine;
        }
    }
}
