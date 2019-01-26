using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace taskt.Core
{
    /// <summary>
    /// Used by the taskt client for tasktServer integration
    /// </summary>
    public static class HttpServerClient
    {
        private static Serilog.Core.Logger httpLogger;
        private static Timer heartbeatTimer { get; set; }
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
                heartbeatTimer.Interval = 30000;
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
                if (appSettings.ServerSettings.ServerConnectionEnabled)
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
                if (!(appSettings.ServerSettings.ServerConnectionEnabled))
                    return false;

                httpLogger.Information("Client is attempting to check in with the server");


                var workerID = appSettings.ServerSettings.HTTPGuid;

                if (workerID != Guid.Empty)
                {
                    httpLogger.Information("Requesting to check in with the server");
                    var client = new WebClient();
                    var content = client.DownloadString("https://localhost:44377/api/Workers/CheckIn?workerID=" + workerID);
                    httpLogger.Information("Received /api/Workers/CheckIn response: " + content);
                    var deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<Worker>(content);
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
        public static Task UpdateTask(Guid taskID, string status)
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
                var content = client.DownloadString(appSettings.ServerSettings.HTTPServerURL + "/api/Tasks/Update?taskID=" + taskID + "&workerID=" + workerID + "&status=" + status);
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
        public string TaskName { get; set; }
        public string Status { get; set; }
        public DateTime TaskStarted { get; set; }
        public DateTime TaskFinished { get; set; }

    }

    #endregion
}
