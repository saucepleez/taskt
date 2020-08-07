using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
using System;
using System.IO;
using System.Net;
using System.Timers;
using System.Windows.Forms;
using taskt.Core.Common;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.IO;
using taskt.Core.Model.ServerModel;
using taskt.Core.Settings;
using taskt.Core.Utilities.CommonUtilities;

namespace taskt.Server
{
    /// <summary>
    /// Used by the taskt client for tasktServer integration
    /// </summary>
    public static class HttpServerClient
    {
        public static IfrmScriptBuilder AssociatedBuilder;
        private static IfrmScriptEngine _scriptEngine;
        private static Logger _httpLogger;
        private static System.Timers.Timer _heartbeatTimer;
        private static ApplicationSettings _appSettings;

        static HttpServerClient()
        {
            string httpLoggerFilePath = Path.Combine(Folders.GetFolder(FolderType.LogFolder), "taskt HTTP Logs.txt");
            _httpLogger = new Logging().CreateFileLogger(httpLoggerFilePath, RollingInterval.Day);
            Initialize();
        }

        /// <summary>
        /// Initializes/Reinitializes settings and sets up the check in with tasktServer
        /// </summary>
        public static void Initialize()
        {
            var settingClass = new ApplicationSettings();
            _appSettings = settingClass.GetOrCreateApplicationSettings();

            if (_appSettings.ServerSettings.ServerConnectionEnabled)
            {
                //handle for reinitialization
                if (_heartbeatTimer != null)
                {
                    _heartbeatTimer.Elapsed -= Heartbeat_Elapsed;
                }

                //setup heartbeat to the server
                _heartbeatTimer = new System.Timers.Timer();
                _heartbeatTimer.Interval = 5000;
                _heartbeatTimer.Elapsed += Heartbeat_Elapsed;
                _heartbeatTimer.Enabled = true;
            }
        }

        /// <summary>
        /// Occurs when the heartbeat timer has elapsed and a check in with server is required
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Heartbeat_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (_appSettings.ServerSettings.ServerConnectionEnabled && AssociatedBuilder != null)
                {
                    CheckIn();
                }
                else
                {
                    _heartbeatTimer.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                _httpLogger.Information("Heartbeat Error:" + ex.ToString());
            }


        }
        /// <summary>
        /// Attempts to retrieve a GUID from tasktServer.
        /// </summary>
        /// <param name="overrideExisting">If true, override existing GUID. Warning: This will create a new client entry on the server!</param>
        /// <returns></returns>
        public static bool GetGuid(bool overrideExisting = false)
        {
            try
            {
                if (!_appSettings.ServerSettings.ServerConnectionEnabled)
                    return false;

                _httpLogger.Information("Client attempting to retrieve a GUID from the server, override " + overrideExisting);

                if (_appSettings.ServerSettings.HTTPGuid == Guid.Empty || overrideExisting)
                {
                    _httpLogger.Information("Requesting GUID from the application server");
                    var client = new WebClient();

                    var userName = Environment.UserName;
                    var machineName = Environment.MachineName;

                    string uri = _appSettings.ServerSettings.HTTPServerURL +
                        "/api/Workers/New?userName=" + userName +
                        "&machineName=" + machineName;

                    var content = client.DownloadString(uri);

                    _httpLogger.Information("Received /api/Workers/New response: " + content);
                    var deserialized = JsonConvert.DeserializeObject<Worker>(content);

                    _appSettings.ServerSettings.HTTPGuid = deserialized.WorkerID;
                    new ApplicationSettings().Save(_appSettings);
                    return true;
                }
                else
                {
                    _httpLogger.Information(
                        "Client has an assigned GUID or override existing GUID set to false." +
                        "To retrieve a new GUID, the existing setting must be cleared."
                        );
                    return false;
                }
            }
            catch (Exception ex)
            {
                _httpLogger.Information("Error Getting GUID: " + ex.ToString());
                return false;
            }
        }
        /// <summary>
        /// Checks in the client with the server
        /// </summary>
        /// <returns>Whether or not the call was successful</returns>
        public static bool CheckIn()
        {
            try
            {
                if (!_appSettings.ServerSettings.ServerConnectionEnabled || AssociatedBuilder == null)
                    return false;

                _httpLogger.Information("Client is attempting to check in with the server");
                var workerID = _appSettings.ServerSettings.HTTPGuid;

                if (workerID != Guid.Empty)
                {
                    _httpLogger.Information("Requesting to check in with the server");
                    var client = new WebClient();

                    string uri = _appSettings.ServerSettings.HTTPServerURL +
                        "/api/Workers/CheckIn?workerID=" + workerID +
                        "&engineBusy=" + Client.EngineBusy;

                    var content = client.DownloadString(uri);
                    _httpLogger.Information("Received /api/Workers/CheckIn response: " + content);
                    var deserialized = JsonConvert.DeserializeObject<CheckInResponse>(content);

                    if (deserialized.ScheduledTask != null)
                    {
                        var scheduledTask = new MethodInvoker(delegate ()
                        {
                            if (deserialized.ScheduledTask.ExecutionType == "Local")
                            {
                                _scriptEngine.FilePath = deserialized.PublishedScript.ScriptData;
                                _scriptEngine.CallBackForm = null;
                            }
                            else
                            {
                                _scriptEngine.FilePath = null;
                                _scriptEngine.JsonData = deserialized.PublishedScript.ScriptData;
                            }

                            _scriptEngine.RemoteTask = deserialized.ScheduledTask;
                            _scriptEngine.ServerExecution = true;
                            ((Form)_scriptEngine).Show();
                        });

                        ((Form)AssociatedBuilder).Invoke(scheduledTask);
                    }
                    return true;
                }
                else
                {
                    _httpLogger.Information("Client unable to check in because the client does not have a GUID");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _httpLogger.Information("Error Checking In: " + ex.ToString());
                return false;
            }

        }
        /// <summary>
        /// Used to test the connection to the API ensuring provided credentials were correct
        /// </summary>
        /// <param name="HTTPServerURL">The URL to the taskt server</param>
        /// <returns></returns>
        public static bool TestConnection(string HTTPServerURL)
        {
            try
            {
                if (!_appSettings.ServerSettings.ServerConnectionEnabled)
                    return false;

                _httpLogger.Information("Client is testing connection to server");

                var client = new WebClient();
                var content = client.DownloadString(HTTPServerURL + "/api/Test");

                _httpLogger.Information("Received /api/Test response: " + content);

                var deserialized = JsonConvert.DeserializeObject(content);

                _appSettings.ServerSettings.HTTPServerURL = HTTPServerURL;
                new ApplicationSettings().Save(_appSettings);

                return true;
            }
            catch (Exception ex)
            {
                _httpLogger.Information("Error Testing Connection: " + ex.ToString());
                return false;
            }

        }
        /// <summary>
        /// Adds a new Task to the server
        /// </summary>
        /// <param name="taskName">The name of the task being executed</param>
        /// <returns></returns>
        public static Task AddTask(string taskName)
        {
            try
            {
                if (!_appSettings.ServerSettings.ServerConnectionEnabled)
                    return null;

                _httpLogger.Information("Client is attempting to add task data to server");

                var workerID = _appSettings.ServerSettings.HTTPGuid;

                var userName = Environment.UserName;
                var machineName = Environment.MachineName;

                var client = new WebClient();

                string uri = _appSettings.ServerSettings.HTTPServerURL +
                    "/api/Tasks/New?workerID=" + workerID +
                    "&taskName=" + taskName +
                    "&userName=" + userName +
                    "&machineName=" + machineName;

                var content = client.DownloadString(uri);
                _httpLogger.Information("Received /api/Tasks/New response: " + content);

                var deserialized = JsonConvert.DeserializeObject<Task>(content);
                return deserialized;
            }
            catch (Exception ex)
            {
                _httpLogger.Information("Error Adding Task: " + ex.ToString());
                return null;
            }

        }
        /// <summary>
        /// Updates an existing task on the server
        /// </summary>
        /// <param name="taskID">The ID of the previously created task</param>
        /// <param name="status">The result of the engine such as success or error</param>
        /// <returns></returns>
        public static Task UpdateTask(Guid taskID, string status, string remark)
        {
            try
            {
                if (!_appSettings.ServerSettings.ServerConnectionEnabled)
                    return null;

                _httpLogger.Information("Client is update a previously added task to server");

                var workerID = _appSettings.ServerSettings.HTTPGuid;

                var userName = Environment.UserName;
                var machineName = Environment.MachineName;

                var client = new WebClient();

                string uri = _appSettings.ServerSettings.HTTPServerURL +
                    "/api/Tasks/Update?taskID=" + taskID +
                    "&workerID=" + workerID +
                    "&status=" + status +
                    "&userName=" + userName +
                    "&machineName=" + machineName +
                    "&remark=" + remark;

                var content = client.DownloadString(uri);
                _httpLogger.Information("Received /api/Tasks/Update response: " + content);
                var deserialized = JsonConvert.DeserializeObject<Task>(content);

                return deserialized;
            }
            catch (Exception ex)
            {
                _httpLogger.Information("Error Updating Task: " + ex.ToString());
                return null;
            }
        }

        public static void PublishScript(string scriptPath, PublishType publishType)
        {
            try
            {
                if (!_appSettings.ServerSettings.ServerConnectionEnabled)
                    return;

                _httpLogger.Information("Client is publishing a script to the server");

                var script = new PublishedScript();
                script.WorkerID = _appSettings.ServerSettings.HTTPGuid;
                script.ScriptType = publishType;
                script.FriendlyName = new System.IO.FileInfo(scriptPath).Name;

                WebClient webClient = new WebClient();

                string uri = _appSettings.ServerSettings.HTTPServerURL +
                    "/api/Scripts/Exists?workerID=" + script.WorkerID +
                    "&friendlyName=" + script.FriendlyName;

                var content = webClient.DownloadString(uri);
                var scriptExists = JsonConvert.DeserializeObject<bool>(content);

                if (scriptExists)
                {
                    var messageText = "It appears this task has already been published.  Should we overwrite the existing task on the server?";
                    var messageCaption = "Overwrite Existing Task?";
                    var overwritePreference = MessageBox.Show(messageText, messageCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (overwritePreference == DialogResult.Yes)
                    {
                        script.OverwriteExisting = true;
                    }
                    else
                    {
                        script.OverwriteExisting = false;
                    }

                }
                else
                {
                    script.OverwriteExisting = false;
                }

                //based on publish type upload local reference or whole task
                if (publishType == PublishType.ClientReference)
                {
                    script.ScriptData = scriptPath;
                }
                else
                {
                    script.ScriptData = System.IO.File.ReadAllText(scriptPath);
                }

                var scriptJson = JsonConvert.SerializeObject(script);

                _httpLogger.Information("Posting Json to Publish API: " + scriptJson);
                //create webclient and upload
                webClient.Headers["Content-Type"] = "application/json";
                webClient.UploadStringCompleted +=
                    new UploadStringCompletedEventHandler(PublishTaskCompleted);

                var api = new Uri(_appSettings.ServerSettings.HTTPServerURL + "/api/Scripts/Publish");

                webClient.UploadStringAsync(api, "POST", scriptJson);

                return;
            }
            catch (Exception ex)
            {
                _httpLogger.Information("Publish Error: " + ex.ToString());
                MessageBox.Show("Publish Error: " + ex.ToString());
            }
        }

        private static void PublishTaskCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<string>(e.Result);
                MessageBox.Show(result, "Task Published", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //e.result fetches you the response against your POST request.
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Publish Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        public static string UploadData(string key, string value)
        {
            var botStoreModel = new BotStore();
            botStoreModel.BotStoreName = key;
            botStoreModel.BotStoreValue = value;
            botStoreModel.LastUpdatedBy = _appSettings.ServerSettings.HTTPGuid;
            botStoreModel.LastUpdatedOn = DateTime.Now;

            var scriptJson = JsonConvert.SerializeObject(botStoreModel);

            _httpLogger.Information("Posting Json to Upload API: " + scriptJson);

            //create webclient and upload
            WebClient webClient = new WebClient();
            webClient.Headers["Content-Type"] = "application/json";
            var api = new Uri(_appSettings.ServerSettings.HTTPServerURL + "/api/BotStore/Add");

            return webClient.UploadString(api, "POST", scriptJson);
        }

        public static string GetData(string key, BotStoreRequestType requestType)
        {
            var botStoreRequest = new BotStoreRequest();
            botStoreRequest.BotStoreName = key;
            botStoreRequest.WorkerID = _appSettings.ServerSettings.HTTPGuid;
            botStoreRequest.RequestType = requestType;

            var scriptJson = JsonConvert.SerializeObject(botStoreRequest);

            //create webclient and upload
            WebClient webClient = new WebClient();
            webClient.Headers["Content-Type"] = "application/json";
            var api = new Uri(_appSettings.ServerSettings.HTTPServerURL + "/api/BotStore/Get");

            return webClient.UploadString(api, "POST", scriptJson);
        }

        public static void InitializeScriptEngine(IfrmScriptEngine newEngine)
        {
            if (_scriptEngine == null)
                _scriptEngine = newEngine;
        }
    }
}
