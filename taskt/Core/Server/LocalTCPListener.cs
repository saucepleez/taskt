using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taskt.Core.Server
{
    /// <summary>
    /// Exposes a local API which enables other (taskt) clients to send automation
    /// </summary>
    public static class LocalTCPListener
    {
        public static Automation.Engine.AutomationEngineInstance automationInstance;
        public static Automation.Engine.AutomationEngineInstance ExecuteCommandEngine;
        private static Serilog.Core.Logger automationLogger;
        public static TcpListener automationListener;
        private static LocalListenerSettings listenerSettings;
        public static UI.Forms.frmScriptBuilder associatedBuilder;
        public static int Port;
        public static bool IsListening { get; set; }

        public static string TasktResult { get; set; }
        public static event EventHandler ListeningStarted;
        public static event EventHandler ListeningStopped;
        static LocalTCPListener()
        {
            automationLogger = new Core.Logging().CreateLogger("Automation Client", Serilog.RollingInterval.Day);
            ExecuteCommandEngine = new Automation.Engine.AutomationEngineInstance();
        }

        public static void Initialize(UI.Forms.frmScriptBuilder builder)
        {
            associatedBuilder = builder;
            automationLogger.Information("Automation Listener Initializing");

            automationInstance = new Automation.Engine.AutomationEngineInstance();

            var appSettings = new Core.ApplicationSettings();
            appSettings = appSettings.GetOrCreateApplicationSettings();

            listenerSettings = appSettings.ListenerSettings;

            if (listenerSettings.LocalListeningEnabled)
            {
                automationLogger.Information("Local Listening is Enabled");
            }
            else
            {
                automationLogger.Information("Local Listening is Disabled");
            }

            if ((listenerSettings.StartListenerOnStartup) && (listenerSettings.LocalListeningEnabled))
            {
                automationLogger.Information("Automatically Starting Listening Service");
                StartListening(listenerSettings.ListeningPort);
            }
            else if ((listenerSettings.StartListenerOnStartup) && (!listenerSettings.LocalListeningEnabled))
            {
                automationLogger.Information("Listening Service is not Enabled! Unable to Automatically Start Listening!");
            }


            automationLogger.Information("Automation Listener Finished Initializing");

        }
        public static void StartListening(int port)
        {

            if (!listenerSettings.LocalListeningEnabled)
            {
                automationLogger.Information("Listening Service is not Enabled! Unable to Start Listening!");
                return;
            }

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                StartAutomationListener(port);
            }).Start();

        }
        public static void StartAutomationListener(int port)
        {
            try
            {
                // TcpListener server = new TcpListener(port);
                automationListener = new TcpListener(IPAddress.Any, port);
                Port = port;

                // Start listening for client requests.
                automationListener.Start();

                automationLogger.Information($"Automation Listener Endpoint started at {automationListener.LocalEndpoint}");

                // Buffer for reading data
                Byte[] bytes = new Byte[2048];
                String data = null;

                // Enter the listening loop.
                while (true)
                {
                    IsListening = true;


                    ListeningStarted?.Invoke(null, null);
                    automationLogger.Information($"Automation Listener Waiting for Request");

                    TcpClient client = automationListener.AcceptTcpClient();

                    automationLogger.Information($"Client '{client.Client.RemoteEndPoint}' Connected to Automation Listener");

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
                            automationLogger.Information($"Client Message Content: {data}");

                            //break out request content
                            var messageContent = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                            if (listenerSettings.EnableWhitelist)
                            {
                                automationLogger.Information($"Listener requires IP Verification (Whitelist)");

                                //verify that client is allowed to connect
                                var clientAddress = (((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());

                                //get list of ip
                                var enabledIPs = listenerSettings.IPWhiteList.Split(',');

                                if (enabledIPs.Any(s => s.Trim().Contains(clientAddress)))
                                {
                                    automationLogger.Information($"Client '{clientAddress}' verified from WhiteList '{listenerSettings.IPWhiteList}'");
                                }
                                else
                                {
                                    automationLogger.Information($"Closing Client Connection due to IP verification failure");
                                    SendResponse(ResponseCode.Unauthorized, $"Unauthorized", stream);
                                    return;
                                }

                            }
                            else
                            {
                                automationLogger.Information($"Listener does not require IP Verification");
                            }



                            if (listenerSettings.RequireListenerAuthenticationKey)
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
                                    automationLogger.Information($"Closing Client Connection due to Null/Empty Auth Key");
                                    SendResponse(ResponseCode.Unauthorized, $"Invalid Auth Key", stream);
                                    break;

                                }
                                else if (authKey != listenerSettings.AuthKey)
                                {
                                    //auth key invalid   
                                    automationLogger.Information($"Closing Client Connection due to Invalid Auth Key");
                                    SendResponse(ResponseCode.Unauthorized, $"Invalid Auth Key", stream);
                                    break;
                                }
                                else if (authKey == listenerSettings.AuthKey)
                                {
                                    //auth key valid
                                    automationLogger.Information($"Auth Key Verified");
                                    ProcessRequest(data, messageContent, stream);
                                }
                            }
                            else
                            {
                                //verification not required
                                automationLogger.Information($"Auth Key Verification Not Required");
                                ProcessRequest(data, messageContent, stream);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        automationLogger.Information($"Error Occured Reading Stream: {ex.ToString()}");
                    }

                    // Shutdown and end connection
                    client.Close();
                    automationLogger.Information($"Client Connection Closed");
                }
            }
            catch (SocketException e)
            {
                automationLogger.Information("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                automationListener.Stop();
                IsListening = false;
                ListeningStopped?.Invoke(null, null);
            }
        }
        private static void ProcessRequest(string data, string[] messageContent, NetworkStream stream)
        {

            if ((data.StartsWith("POST /ExecuteScript")) || (data.StartsWith("POST /AwaitScript")))
            {
                automationLogger.Information($"Client Requests Script Execution");

                //locate the body content
                string dataParameter = "";
                bool isFileLocation = false;

                //find the script parameter
                foreach (var item in messageContent)
                {
                    if (item.StartsWith("ScriptData: "))
                    {
                        dataParameter = item.Replace("ScriptData: ", "");
                        isFileLocation = false;

                        break;
                    }

                    else if (item.StartsWith("ScriptLocation: "))
                    {
                        dataParameter = item.Replace("ScriptLocation: ", "");
                        isFileLocation = true;
                        break;
                    }

                }

                //check to see if nothing was provided
                if (string.IsNullOrEmpty(dataParameter))
                {
                    automationLogger.Information($"Client Script Data Not Found");
                    return;
                }


                if (dataParameter.TryParseBase64(out var base64SourceString))
                {
                    automationLogger.Information($"Client Passed Base64 String: {base64SourceString}");
                    dataParameter = base64SourceString;
                }
                else
                {
                    automationLogger.Information($"Client Did Not Pass Base64 String");
                }


                //check if data parameter references file location
                if (isFileLocation)
                {
                    if (File.Exists(dataParameter))
                    {
                        //file was found at path provided
                        dataParameter = File.ReadAllText(dataParameter);
                    }
                    else if (File.Exists(Path.Combine(IO.Folders.GetFolder(IO.Folders.FolderType.ScriptsFolder), dataParameter)))
                    {
                        //file was found at fallback to scripts folder
                        dataParameter = Path.Combine(IO.Folders.GetFolder(IO.Folders.FolderType.ScriptsFolder), dataParameter);
                        dataParameter = File.ReadAllText(dataParameter);
                    }
                    else
                    {
                        //file not found
                        automationLogger.Information($"Client Script Location Not Found: {dataParameter}");
                        SendResponse(ResponseCode.InternalServerError, $"Client Script Location Not Found: {dataParameter}", stream);
                        return;
                    }

                }

                //log execution
                automationLogger.Information($"Executing Script: {dataParameter}");


                //invoke builder and pass it script data to execute
                associatedBuilder.Invoke(new MethodInvoker(delegate ()
                {
                    UI.Forms.frmScriptEngine newEngine = new UI.Forms.frmScriptEngine();
                    newEngine.xmlData = dataParameter;
                    newEngine.callBackForm = null;
                    //instance = newEngine.engineInstance;
                    newEngine.Show();
                }));


                if (data.StartsWith("POST /AwaitScript"))
                {
                    //reset result value
                    TasktResult = "";

                    //add reference to script finished event
                    automationInstance.ScriptFinishedEvent += AutomationInstance_ScriptFinishedEvent;

                    //wait for script to finish before returning
                    do
                    {
                        Thread.Sleep(1000);
                    } while (TasktResult == string.Empty);

                    //send response back to client
                    SendResponse(ResponseCode.OK, automationInstance.TasktResult, stream);

                }
                else
                {
                    //return success immediately
                    SendResponse(ResponseCode.OK, "Script Launched Successfully", stream);
                }



            }
            else if (data.StartsWith("POST /ExecuteCommand"))
            {
                automationLogger.Information($"Client Requests Command Execution");

                //locate the body content
                string dataParameter = "";

                //find the script parameter
                foreach (var item in messageContent)
                {
                    if (item.StartsWith("CommandData: "))
                    {
                        dataParameter = item.Replace("CommandData: ", "");
                        break;
                    }
                }

                //check to see if nothing was provided
                if (string.IsNullOrEmpty(dataParameter))
                {
                    automationLogger.Information($"Client Command Data Not Found");
                    return;
                }


                if (dataParameter.TryParseBase64(out var base64SourceString))
                {
                    automationLogger.Information($"Client Passed Base64 String: {base64SourceString}");
                    dataParameter = base64SourceString;
                }
                else
                {
                    automationLogger.Information($"Client Did Not Pass Base64 String");
                }

                try
                {
                    //deserialize command
                    var command = Newtonsoft.Json.JsonConvert.DeserializeObject(dataParameter, new Newtonsoft.Json.JsonSerializerSettings() { TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All });

                    //log execution
                    automationLogger.Information($"Executing Command: {dataParameter}");

                    //define script action
                    var scriptAction = new Script.ScriptAction() { ScriptCommand = (Core.Automation.Commands.ScriptCommand)command };

                    //execute command
                    ExecuteCommandEngine.ExecuteCommand(scriptAction);

                    //send back response
                    SendResponse(ResponseCode.OK, "Command Executed Successfully", stream);
                }
                catch (Exception ex)
                {
                    SendResponse(ResponseCode.InternalServerError, $"An error occured: {ex.ToString()}", stream);
                }


            }

            else if (data.StartsWith("POST /EngineStatus"))
            {
                automationLogger.Information($"Returning Engine Status: {Client.ClientStatus}");
                SendResponse(ResponseCode.OK, Core.Client.ClientStatus, stream);
            }
            else if (data.StartsWith("POST /RestartTaskt"))
            {
                automationLogger.Information($"Restarting taskt");
                SendResponse(ResponseCode.OK, "taskt is being Restarted", stream);
                Application.Restart();
            }
            else
            {
                automationLogger.Information($"Invalid Client Request");
                SendResponse(ResponseCode.InternalServerError, "Invalid Client Request", stream);
            }
        }

        private static void AutomationInstance_ScriptFinishedEvent(object sender, Automation.Engine.ScriptFinishedEventArgs e)
        {
            //set result once script completes
            TasktResult = automationInstance.TasktResult;
        }

        public static void SendResponse(ResponseCode ResponseCode, string content, Stream networkStream)
        {
            System.IO.StreamWriter writer = new System.IO.StreamWriter(networkStream);

            string responseHeader = "";
            if (ResponseCode == ResponseCode.OK)
            {
                responseHeader = "HTTP/1.1 200 OK";
            }
            else if (ResponseCode == ResponseCode.InternalServerError)
            {
                responseHeader = "HTTP/1.1 500 INTERNAL SERVER ERROR";
            }
            else if (ResponseCode == ResponseCode.Unauthorized)
            {
                responseHeader = "HTTP/1.1 401 UNAUTHORIZED";
            }
            else
            {
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
            automationListener.Stop();
        }
        public static string SendAutomationTask(string endpoint, string parameterType, string timeout, string scriptData = "", string awaitPreference = "")
        {

            if (!endpoint.StartsWith("http://"))
            {
                endpoint = $"http://{endpoint}";
            }

            var client = new RestSharp.RestClient(endpoint);
            var request = new RestSharp.RestRequest();
            request.Method = RestSharp.Method.POST;
            request.AddHeader("Content-Type", "text/plain");

            if (listenerSettings.RequireListenerAuthenticationKey)
            {
                request.AddHeader("AuthKey", listenerSettings.AuthKey);
            }

            //check type of execution needed
            if (parameterType == "Run Raw Script Data")
            {


                //handle await preference
                if (awaitPreference == "Await For Result")
                {
                    request.Resource = "/AwaitScript";
                }
                else
                {
                    request.Resource = "/ExecuteScript";
                }

                //add script data
                request.AddParameter("ScriptData", scriptData.ToBase64(), RestSharp.ParameterType.HttpHeader);
            }
            else if (parameterType == "Run Local File")
            {

                //handle await preference
                if (awaitPreference == "Await For Result")
                {
                    request.Resource = "/AwaitScript";
                }
                else
                {
                    request.Resource = "/ExecuteScript";
                }

                if (File.Exists(scriptData))
                {
                    //file was found at path provided
                    scriptData = File.ReadAllText(scriptData);
                }
                else if (File.Exists(Path.Combine(IO.Folders.GetFolder(IO.Folders.FolderType.ScriptsFolder), scriptData)))
                {
                    //file was found at fallback to scripts folder
                    scriptData = Path.Combine(IO.Folders.GetFolder(IO.Folders.FolderType.ScriptsFolder), scriptData);
                    scriptData = File.ReadAllText(scriptData);
                }
                else
                {
                    throw new FileNotFoundException(scriptData);
                }

                //add script data
                request.AddParameter("ScriptData", scriptData.ToBase64(), RestSharp.ParameterType.HttpHeader);

            }
            else if (parameterType == "Run Remote File")
            {

                //handle await preference
                if (awaitPreference == "Await For Result")
                {
                    request.Resource = "/AwaitScript";
                }
                else
                {
                    request.Resource = "/ExecuteScript";
                }

                //add script parameter
                request.AddParameter("ScriptLocation", scriptData, RestSharp.ParameterType.HttpHeader);
            }
            else if (parameterType == "Run Command Json")
            {

                request.Resource = "/ExecuteCommand";

                //add script data
                request.AddParameter("CommandData", scriptData.ToBase64(), RestSharp.ParameterType.HttpHeader);

            }
            else if (parameterType == "Get Engine Status")
            {
                request.Resource = "/EngineStatus";
            }
            else if (parameterType == "Restart taskt")
            {
                request.Resource = "/RestartTaskt";
            }

            request.Timeout = int.Parse(timeout);


            RestSharp.IRestResponse resp = client.Execute(request);

            string content;
            if (resp.ErrorMessage is null)
            {
                content = resp.Content;
            }
            else
            {
                content = resp.ErrorMessage;
            }

            return content;
        }

        public static string GetListeningAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return $"{ip.ToString()}:{Port.ToString()}";
                }
            }

            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
    public enum ResponseCode
    {
        OK = 200,
        InternalServerError = 500,
        Unauthorized = 401
    }
}
