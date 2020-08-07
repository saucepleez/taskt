using Newtonsoft.Json;
using RestSharp;
using Serilog.Core;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using taskt.Core.Command;
using taskt.Core.Common;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.IO;
using taskt.Core.Model.EngineModel;
using taskt.Core.Script;
using taskt.Core.Settings;
using taskt.Core.Utilities.CommonUtilities;

namespace taskt.Server
{
    /// <summary>
    /// Exposes a local API which enables other (taskt) clients to send automation
    /// </summary>
    public static class LocalTCPClient
    {
        public static bool IsListening { get; set; }
        public static string TasktResult { get; set; }
        public static event EventHandler ListeningStarted;
        public static event EventHandler ListeningStopped;
        public static IEngine AutomationInstance;

        private static IfrmScriptEngine _scriptEngine;
        private static IEngine _executeCommandEngine;
        private static Logger _automationLogger;
        private static TcpListener _tcpListener;
        private static LocalListenerSettings _listenerSettings;
        private static IfrmScriptBuilder _associatedBuilder;
        private static int _port;

        static LocalTCPClient()
        {

        }

        public static void Initialize(IfrmScriptBuilder builder, IEngine automationEngine)
        {
            _associatedBuilder = builder;
            _automationLogger = automationEngine.EngineLogger;
            _automationLogger.Information("Automation Listener Initializing");

            if (AutomationInstance == null)
                AutomationInstance = automationEngine;

            var appSettings = new ApplicationSettings();
            appSettings = appSettings.GetOrCreateApplicationSettings();

            _listenerSettings = appSettings.ListenerSettings;

            if (_listenerSettings.LocalListeningEnabled)
            {
                _automationLogger.Information("Local Listening is Enabled");
            }
            else
            {
                _automationLogger.Information("Local Listening is Disabled");
            }

            if (_listenerSettings.StartListenerOnStartup)
            {
                if (_listenerSettings.LocalListeningEnabled)
                {
                    _automationLogger.Information("Automatically Starting Listening Service");
                    StartListening(_listenerSettings.ListeningPort);
                }
                else
                {
                    _automationLogger.Information("Listening Service is not Enabled! Unable to Automatically Start Listening!");
                }

            }

            _automationLogger.Information("Automation Listener Finished Initializing");
        }

        public static void StartListening(int port)
        {
            if (!_listenerSettings.LocalListeningEnabled)
            {
                _automationLogger.Information("Listening Service is not Enabled! Unable to Start Listening!");
                return;
            }

            new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    StartAutomationListener(port);
                }
            ).Start();
        }

        public static void StartAutomationListener(int port)
        {
            try
            {
                // TcpListener server = new TcpListener(port);
                _tcpListener = new TcpListener(IPAddress.Any, port);
                _port = port;

                // Start listening for client requests.
                _tcpListener.Start();

                _automationLogger.Information($"Automation Listener Endpoint started at {_tcpListener.LocalEndpoint}");

                // Buffer for reading data
                Byte[] bytes = new Byte[2048];
                String data = null;

                // Enter the listening loop.
                while (true)
                {
                    IsListening = true;

                    ListeningStarted?.Invoke(null, null);
                    _automationLogger.Information($"Automation Listener Waiting for Request");

                    TcpClient client = _tcpListener.AcceptTcpClient();

                    _automationLogger.Information($"Client '{client.Client.RemoteEndPoint}' Connected to Automation Listener");

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();
                    int i;

                    try
                    {
                        // Loop to receive all the data sent by the client.
                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            // Translate data bytes to a ASCII string.
                            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                            _automationLogger.Information($"Client Message Content: {data}");

                            //break out request content
                            var messageContent = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                            if (_listenerSettings.EnableWhitelist)
                            {
                                _automationLogger.Information($"Listener requires IP Verification (Whitelist)");

                                //verify that client is allowed to connect
                                var clientAddress = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();

                                //get list of ip
                                var enabledIPs = _listenerSettings.IPWhiteList.Split(',');

                                if (enabledIPs.Any(s => s.Trim().Contains(clientAddress)))
                                {
                                    _automationLogger.Information($"Client '{clientAddress}' verified from WhiteList '{_listenerSettings.IPWhiteList}'");
                                }
                                else
                                {
                                    _automationLogger.Information($"Closing Client Connection due to IP verification failure");
                                    SendResponse(HttpStatusCode.Unauthorized, $"Unauthorized", stream);
                                    return;
                                }
                            }
                            else
                            {
                                _automationLogger.Information($"Listener does not require IP Verification");
                            }

                            if (_listenerSettings.RequireListenerAuthenticationKey)
                            {
                                string authKey = "";
                                foreach (var item in messageContent)
                                {
                                    if (item.StartsWith("AuthKey: "))
                                    {
                                        authKey = item.Replace("AuthKey: ", "");
                                        break;
                                    }
                                }

                                //auth key check
                                if (string.IsNullOrEmpty(authKey))
                                {
                                    //auth key not provided
                                    _automationLogger.Information($"Closing Client Connection due to Null/Empty Auth Key");
                                    SendResponse(HttpStatusCode.Unauthorized, $"Invalid Auth Key", stream);
                                    break;

                                }
                                else if (authKey != _listenerSettings.AuthKey)
                                {
                                    //auth key invalid
                                    _automationLogger.Information($"Closing Client Connection due to Invalid Auth Key");
                                    SendResponse(HttpStatusCode.Unauthorized, $"Invalid Auth Key", stream);
                                    break;
                                }
                                else if (authKey == _listenerSettings.AuthKey)
                                {
                                    //auth key valid
                                    _automationLogger.Information($"Auth Key Verified");
                                    ProcessRequest(data, messageContent, stream);
                                }
                            }
                            else
                            {
                                //verification not required
                                _automationLogger.Information($"Auth Key Verification Not Required");
                                ProcessRequest(data, messageContent, stream);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        _automationLogger.Information($"Error Occured Reading Stream: {ex}");
                    }

                    // Shutdown and end connection
                    client.Close();
                    _automationLogger.Information($"Client Connection Closed");
                }
            }
            catch (SocketException e)
            {
                _automationLogger.Information("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                _tcpListener.Stop();
                IsListening = false;
                ListeningStopped?.Invoke(null, null);
            }
        }

        private static void ProcessRequest(string data, string[] messageContent, NetworkStream stream)
        {
            string dataParameter = "";
            string error = "Client Script Data Not Found";
            bool isFileLocation = false;

            // Before handling specific request endpoint logic, do process data
            foreach (var item in messageContent)
            {
                if (item.StartsWith("ScriptData: "))
                {
                    dataParameter = item.Replace("ScriptData: ", "");
                    break;
                }
                else if (item.StartsWith("ScriptLocation: "))
                {
                    dataParameter = item.Replace("ScriptLocation: ", "");
                    isFileLocation = true;
                    break;
                }
                else if (item.StartsWith("CommandData: "))
                {
                    dataParameter = item.Replace("CommandData: ", "");
                    error = "Client Command Data Not Found";
                    break;
                }
            }

            if (string.IsNullOrEmpty(dataParameter))
            {
                _automationLogger.Information(error);
                return;
            }

            if (dataParameter.TryParseBase64(out var base64SourceString))
            {
                _automationLogger.Information($"Client Passed Base64 String: {base64SourceString}");
                dataParameter = base64SourceString;
            }
            else
            {
                _automationLogger.Information($"Client Did Not Pass Base64 String");
            }

            // Begind handling endpoint logic
            if (data.StartsWith("POST /ExecuteScript") || data.StartsWith("POST /AwaitScript"))
            {
                _automationLogger.Information($"Client Requests Script Execution");

                //check if data parameter references file location
                if (isFileLocation)
                {
                    var fallbackPath = Path.Combine(Folders.GetFolder(FolderType.ScriptsFolder), dataParameter);
                    if (File.Exists(dataParameter))
                    {
                        //file was found at path provided
                        dataParameter = File.ReadAllText(dataParameter);
                    }
                    else if (File.Exists(fallbackPath))
                    {
                        //file was found at fallback to scripts folder
                        dataParameter = File.ReadAllText(fallbackPath);
                    }
                    else
                    {
                        //file not found
                        _automationLogger.Information($"Client Script Location Not Found: {dataParameter}");
                        SendResponse(HttpStatusCode.InternalServerError, $"Client Script Location Not Found: {dataParameter}", stream);
                        return;
                    }
                }

                //log execution
                _automationLogger.Information($"Executing Script: {dataParameter}");

                //invoke builder and pass it script data to execute
                ((Form)_associatedBuilder).Invoke(new MethodInvoker(delegate ()
                    {
                        _scriptEngine.JsonData = dataParameter;
                        _scriptEngine.CallBackForm = null;
                        ((Form)_scriptEngine).Show();
                    })
                );

                if (data.StartsWith("POST /AwaitScript"))
                {
                    //reset result value
                    TasktResult = "";

                    //add reference to script finished event
                    AutomationInstance.ScriptFinishedEvent += AutomationInstance_ScriptFinishedEvent;

                    //wait for script to finish before returning
                    do
                    {
                        Thread.Sleep(1000);
                    } while (TasktResult == string.Empty);

                    //send response back to client
                    SendResponse(HttpStatusCode.OK, AutomationInstance.TasktResult, stream);
                }
                else
                {
                    //return success immediately
                    SendResponse(HttpStatusCode.OK, "Script Launched Successfully", stream);
                }
            }
            else if (data.StartsWith("POST /ExecuteCommand"))
            {
                _automationLogger.Information($"Client Requests Command Execution");

                try
                {
                    //deserialize command
                    var serializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
                    var command = JsonConvert.DeserializeObject(dataParameter, serializerSettings);

                    //log execution
                    _automationLogger.Information($"Executing Command: {dataParameter}");

                    //define script action
                    var scriptAction = new ScriptAction() { ScriptCommand = (ScriptCommand)command };

                    //execute command
                    _executeCommandEngine.ExecuteCommand(scriptAction);

                    //send back response
                    SendResponse(HttpStatusCode.OK, "Command Executed Successfully", stream);
                }
                catch (Exception ex)
                {
                    SendResponse(HttpStatusCode.InternalServerError, $"An error occured: {ex}", stream);
                }
            }
            else if (data.StartsWith("POST /EngineStatus"))
            {
                _automationLogger.Information($"Returning Engine Status: {Client.ClientStatus}");
                SendResponse(HttpStatusCode.OK, Client.ClientStatus, stream);
            }
            else if (data.StartsWith("POST /RestartTaskt"))
            {
                _automationLogger.Information($"Restarting taskt");
                SendResponse(HttpStatusCode.OK, "taskt is being Restarted", stream);
                Application.Restart();
            }
            else
            {
                _automationLogger.Information($"Invalid Client Request");
                SendResponse(HttpStatusCode.InternalServerError, "Invalid Client Request", stream);
            }
        }

        private static void AutomationInstance_ScriptFinishedEvent(object sender, ScriptFinishedEventArgs e)
        {
            //set result once script completes
            TasktResult = AutomationInstance.TasktResult;
        }

        public static void SendResponse(HttpStatusCode ResponseCode, string content, Stream networkStream)
        {
            StreamWriter writer = new StreamWriter(networkStream);
            string responseHeader;

            switch (ResponseCode)
            {
                case HttpStatusCode.OK:
                    responseHeader = "HTTP/1.1 200 OK";
                    break;
                case HttpStatusCode.Unauthorized:
                    responseHeader = "HTTP/1.1 401 UNAUTHORIZED";
                    break;
                case HttpStatusCode.InternalServerError:
                    responseHeader = "HTTP/1.1 500 INTERNAL SERVER ERROR";
                    break;
                default:
                    throw new NotImplementedException();
            }

            writer.Write(responseHeader);
            writer.Write(Environment.NewLine);
            writer.Write("Content-Type: text/plain; charset=UTF-8");
            writer.Write(Environment.NewLine);
            writer.Write("Content-Length: " + content.Length);
            writer.Write(Environment.NewLine);
            writer.Write(Environment.NewLine);
            writer.Write(content);
            writer.Flush();
        }

        public static void StopAutomationListener()
        {
            _tcpListener.Stop();
        }

        public static string SendAutomationTask(string endpoint, string parameterType, string timeout,
                                                string scriptData = "", string awaitPreference = "")
        {
            if (!endpoint.StartsWith("http://"))
            {
                endpoint = $"http://{endpoint}";
            }

            var client = new RestClient(endpoint);
            var request = new RestRequest();

            // Endpoint/Values for await preference
            var awaitForResult = "Await For Result";
            var awaitEndpoint = "/AwaitScript";
            var executeEndpoint = "/ExecuteScript";

            request.Method = Method.POST;
            request.AddHeader("Content-Type", "text/plain");
            request.Resource = (awaitPreference == awaitForResult) ? awaitEndpoint : executeEndpoint;
            request.Timeout = int.Parse(timeout);

            if (_listenerSettings.RequireListenerAuthenticationKey)
            {
                request.AddHeader("AuthKey", _listenerSettings.AuthKey);
            }

            switch (parameterType)
            {
                case "Run Raw Script Data":
                    //Add script data
                    request.AddParameter("ScriptData", scriptData.ToBase64(), ParameterType.HttpHeader);
                    break;
                case "Run Local File":
                    string fallbackPath = Path.Combine(Folders.GetFolder(FolderType.ScriptsFolder), scriptData);

                    try
                    {
                        if (File.Exists(scriptData))
                        {
                            //file was found at path provided
                            scriptData = File.ReadAllText(scriptData);
                        }
                        else if (File.Exists(fallbackPath))
                        {
                            //file was found at fallback to scripts folder
                            scriptData = File.ReadAllText(fallbackPath);
                        }
                    }
                    catch
                    {
                        throw new FileNotFoundException(scriptData);
                    }

                    request.AddParameter("ScriptData", scriptData.ToBase64(), ParameterType.HttpHeader);
                    break;
                case "Run Remote File":
                    //Add script to be executed location
                    request.AddParameter("ScriptLocation", scriptData, ParameterType.HttpHeader);
                    break;
                case "Run Command Json":
                    request.Resource = "/ExecuteCommand";
                    // Add command data
                    request.AddParameter("CommandData", scriptData.ToBase64(), ParameterType.HttpHeader);
                    break;
                case "Get Engine Status":
                    request.Resource = "/EngineStatus";
                    break;
                case "Restart taskt":
                    request.Resource = "/RestartTaskt";
                    break;
                default:
                    break;
            }

            IRestResponse resp = client.Execute(request);

            string content = resp.ErrorMessage is null ? resp.Content : resp.ErrorMessage;
            return content;
        }

        public static string GetListeningAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return $"{ip}:{_port}";
                }
            }

            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static void InitializeScriptEngine(IfrmScriptEngine newEngine)
        {
            if (_scriptEngine == null)
                _scriptEngine = newEngine;
        }

        public static void InitializeAutomationEngine(IEngine executeCommandEngine)
        {
            _executeCommandEngine = executeCommandEngine;
        }
    }
}
