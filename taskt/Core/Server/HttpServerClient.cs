using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace taskt.Core.Server
{
    /// <summary>
    /// Used by the taskt client for tasktServer integration
    /// </summary>
    public static class HttpServerClient
    {
        public static UI.Forms.frmScriptBuilder associatedBuilder;
        private static Serilog.Core.Logger httpLogger;
        private static System.Timers.Timer heartbeatTimer { get; set; }
        private static ApplicationSettings appSettings { get; set; }

        static HttpServerClient()
        {
            httpLogger = new Core.Logging().CreateLogger("HTTP", Serilog.RollingInterval.Day);
            Initialize();
        }

        /// <summary>
        /// Initializes/Reinitializes settings and sets up the check in with tasktServer
        /// </summary>
        public static void Initialize()
        {
            var settingClass = new Core.ApplicationSettings();
            appSettings = settingClass.GetOrCreateApplicationSettings();

            if (appSettings.ServerSettings.ServerConnectionEnabled)
            {

                //handle for reinitialization
                if (heartbeatTimer != null)
                {
                    heartbeatTimer.Elapsed -= Heartbeat_Elapsed;
                }

                //setup heartbeat to the server
                heartbeatTimer = new System.Timers.Timer();
                heartbeatTimer.Interval = 5000;
                heartbeatTimer.Elapsed += Heartbeat_Elapsed;
                heartbeatTimer.Enabled = true;
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
                if (appSettings.ServerSettings.ServerConnectionEnabled && associatedBuilder != null)
                {
                    CheckIn();
                }
                else
                {
                    heartbeatTimer.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                httpLogger.Information("Heartbeat Error:" + ex.ToString());
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
                if (!(appSettings.ServerSettings.ServerConnectionEnabled))
                    return false;

                httpLogger.Information("Client attempting to retrieve a GUID from the server, override " + overrideExisting);





                if (appSettings.ServerSettings.HTTPGuid == Guid.Empty || overrideExisting)
                {
                    httpLogger.Information("Requesting GUID from the application server");
                    var client = new WebClient();

                    var userName = Environment.UserName;
                    var machineName = Environment.MachineName;
                    var content = client.DownloadString(appSettings.ServerSettings.HTTPServerURL + "/api/Workers/New?userName=" + userName + "&machineName=" + machineName);

                    httpLogger.Information("Received /api/Workers/New response: " + content);
                    var deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<Worker>(content);

                    appSettings.ServerSettings.HTTPGuid = deserialized.WorkerID;
                    new ApplicationSettings().Save(appSettings);
                    return true;
                }
                else
                {
                    httpLogger.Information("Client has an assigned GUID or override existing GUID set to false. To retrieve a new GUID, the existing setting must be cleared.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                httpLogger.Information("Error Getting GUID: " + ex.ToString());
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
                if (!(appSettings.ServerSettings.ServerConnectionEnabled) || associatedBuilder == null)
                    return false;

                httpLogger.Information("Client is attempting to check in with the server");


                var workerID = appSettings.ServerSettings.HTTPGuid;

                if (workerID != Guid.Empty)
                {
                    httpLogger.Information("Requesting to check in with the server");
                    var client = new WebClient();
                    var content = client.DownloadString(appSettings.ServerSettings.HTTPServerURL + "/api/Workers/CheckIn?workerID=" + workerID + "&engineBusy=" + Core.Client.EngineBusy);
                    httpLogger.Information("Received /api/Workers/CheckIn response: " + content);
                    var deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<CheckInResponse>(content);

                    if (deserialized.ScheduledTask != null)
                    {

                        associatedBuilder.Invoke(new MethodInvoker(delegate ()
                        {


                            if (deserialized.ScheduledTask.ExecutionType == "Local")
                            {
                                UI.Forms.frmScriptEngine newEngine = new UI.Forms.frmScriptEngine(deserialized.PublishedScript.ScriptData, null);
                                newEngine.remoteTask = deserialized.ScheduledTask;
                                newEngine.serverExecution = true;
                                newEngine.Show();
                            }
                            else
                            {
                                UI.Forms.frmScriptEngine newEngine = new UI.Forms.frmScriptEngine();
                                newEngine.xmlData = deserialized.PublishedScript.ScriptData;
                                newEngine.remoteTask = deserialized.ScheduledTask;
                                newEngine.serverExecution = true;
                                newEngine.Show();
                            }
        
                           
                        }));
                 
                    }


                    return true;
                }
                else
                {
                    httpLogger.Information("Client unable to check in because the client does not have a GUID");
                    return false;
                }
            }
            catch (Exception ex)
            {
                httpLogger.Information("Error Checking In: " + ex.ToString());
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
                if (!(appSettings.ServerSettings.ServerConnectionEnabled))
                    return false;

                httpLogger.Information("Client is testing connection to server");


                var client = new WebClient();
                var content = client.DownloadString(HTTPServerURL + "/api/Test");

                httpLogger.Information("Received /api/Test response: " + content);

                var deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                appSettings.ServerSettings.HTTPServerURL = HTTPServerURL;
                new ApplicationSettings().Save(appSettings);

                return true;
            }
            catch (Exception ex)
            {
                httpLogger.Information("Error Testing Connection: " + ex.ToString());
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
                if (!(appSettings.ServerSettings.ServerConnectionEnabled))
                    return null;

                httpLogger.Information("Client is attempting to add task data to server");



                var workerID = appSettings.ServerSettings.HTTPGuid;

                var userName = Environment.UserName;
                var machineName = Environment.MachineName;


                var client = new WebClient();
                var content = client.DownloadString(appSettings.ServerSettings.HTTPServerURL + "/api/Tasks/New?workerID=" + workerID + "&taskName=" + taskName + "&userName=" + userName + "&machineName=" + machineName);
                httpLogger.Information("Received /api/Tasks/New response: " + content);

                var deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<Task>(content);
                return deserialized;
            }
            catch (Exception ex)
            {
                httpLogger.Information("Error Adding Task: " + ex.ToString());
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
                if (!(appSettings.ServerSettings.ServerConnectionEnabled))
                    return null;

                httpLogger.Information("Client is update a previously added task to server");

                var workerID = appSettings.ServerSettings.HTTPGuid;

                var userName = Environment.UserName;
                var machineName = Environment.MachineName;

                var client = new WebClient();
                var content = client.DownloadString(appSettings.ServerSettings.HTTPServerURL + "/api/Tasks/Update?taskID=" + taskID + "&workerID=" + workerID + "&status=" + status + "&userName=" + userName + "&machineName=" + machineName + "&remark=" + remark);
                httpLogger.Information("Received /api/Tasks/Update response: " + content);
                var deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<Task>(content);

                return deserialized;
            }
            catch (Exception ex)
            {
                httpLogger.Information("Error Updating Task: " + ex.ToString());
                return null;
            }


        }

       public static void PublishScript(string scriptPath, PublishedScript.PublishType publishType)
        {
            try
            {
                if (!(appSettings.ServerSettings.ServerConnectionEnabled))
                    return;

                httpLogger.Information("Client is publishing a script to the server");


                var script = new PublishedScript();
                script.WorkerID = appSettings.ServerSettings.HTTPGuid;
                script.ScriptType = publishType;
                script.FriendlyName = new System.IO.FileInfo(scriptPath).Name;



                WebClient webClient = new WebClient();
                var content = webClient.DownloadString(appSettings.ServerSettings.HTTPServerURL + "/api/Scripts/Exists?workerID=" + script.WorkerID + "&friendlyName=" + script.FriendlyName);

                var scriptExists = Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(content);

                if (scriptExists)
                {
                    var overwritePreference = MessageBox.Show("It appears this task has already been published.  Should we overwrite the existing task on the server?", "Overwrite Existing Task?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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
                if (publishType == PublishedScript.PublishType.ClientReference)
                {
                    script.ScriptData = scriptPath;
                }
                else
                {
                    script.ScriptData = System.IO.File.ReadAllText(scriptPath);
                }

                var scriptJson = Newtonsoft.Json.JsonConvert.SerializeObject(script);

                httpLogger.Information("Posting Json to Publish API: " + scriptJson);
                //create webclient and upload
                webClient.Headers["Content-Type"] = "application/json";
                webClient.UploadStringCompleted +=
                    new UploadStringCompletedEventHandler(PublishTaskCompleted);
               
                var api = new Uri(appSettings.ServerSettings.HTTPServerURL + "/api/Scripts/Publish");

                webClient.UploadStringAsync(api, "POST", scriptJson);


                return;

            }
            catch (Exception ex)
            {
                httpLogger.Information("Publish Error: " + ex.ToString());
                MessageBox.Show("Publish Error: " + ex.ToString());
            }


        }
       private static void PublishTaskCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            try
            {
               var result = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(e.Result);
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
            var botStoreModel = new BotStoreModel();
            botStoreModel.BotStoreName = key;
            botStoreModel.BotStoreValue = value;
            botStoreModel.LastUpdatedBy = appSettings.ServerSettings.HTTPGuid;
            botStoreModel.LastUpdatedOn = DateTime.Now;

            var scriptJson = Newtonsoft.Json.JsonConvert.SerializeObject(botStoreModel);

            httpLogger.Information("Posting Json to Upload API: " + scriptJson);
            
            //create webclient and upload
            WebClient webClient = new WebClient();
            webClient.Headers["Content-Type"] = "application/json";
           var api = new Uri(appSettings.ServerSettings.HTTPServerURL + "/api/BotStore/Add");

           return webClient.UploadString(api, "POST", scriptJson);

        }
        public static string GetData(string key, BotStoreRequest.RequestType requestType)
        {
            var botStoreRequest = new BotStoreRequest();
            botStoreRequest.BotStoreName = key;
            botStoreRequest.workerID = appSettings.ServerSettings.HTTPGuid;
            botStoreRequest.requestType = requestType;

            var scriptJson = Newtonsoft.Json.JsonConvert.SerializeObject(botStoreRequest);

            //create webclient and upload
            WebClient webClient = new WebClient();
            webClient.Headers["Content-Type"] = "application/json";
            var api = new Uri(appSettings.ServerSettings.HTTPServerURL + "/api/BotStore/Get");

            return webClient.UploadString(api, "POST", scriptJson);



        }


    }

    #region tasktServer Models

    public class Worker
    {
        public Guid WorkerID { get; set; }
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public DateTime LastCheckIn { get; set; }
        public WorkerStatus Status { get; set; }
    }

    public enum WorkerStatus
    {
        Pending = 0,
        Authorized = 1,
        Revoked = 2
    }

    public class Task
    {
        public Guid TaskID { get; set; }
        public Guid WorkerID { get; set; }
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public string IPAddress { get; set; }
        public string ExecutionType { get; set; }
        public string Script { get; set; }
        public string Status { get; set; }
        public DateTime TaskStarted { get; set; }
        public DateTime TaskFinished { get; set; }

    }

    public class CheckInResponse
    {
        public Task ScheduledTask { get; set; }
        public PublishedScript PublishedScript { get; set; }
        public Worker Worker { get; set; }
    }

    public class PublishedScript
    {
        public Guid WorkerID { get; set; }
        public PublishType ScriptType { get; set; }
        public string ScriptData { get; set; }
        public string FriendlyName { get; set; }
        public bool OverwriteExisting { get; set; }
        public enum PublishType
        {
            ClientReference,
            ServerReference,
        }
    }

    public class BotStoreModel
    {
        public Guid StoreID { get; set; }
        public string BotStoreName { get; set; }
        public string BotStoreValue { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public Guid LastUpdatedBy { get; set; }
    }

    public class BotStoreRequest
    {
        public Guid workerID { get; set; }
        public string BotStoreName { get; set; }
        public RequestType requestType { get; set; }
       public enum RequestType
        {
            BotStoreValue,
            BotStoreModel
        }
    }

    #endregion
}
