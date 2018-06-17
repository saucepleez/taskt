//Copyright (c) 2018 Jason Bayldon
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace taskt.Core
{
    public static class Common
    {
        /// <summary>
        /// Creates a unique 'clone' of an item. Used to create unique clones of commands when changing/updating new parameters.
        /// </summary>
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            if (source == null)
            {
                return default(T);
            }

            System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
        /// <summary>
        /// Returns a path to the underlying Script folder where script file objects are loaded and saved. Used when saved or loading files.
        /// </summary>
        public static string GetScriptFolderPath()
        {
            return GetAppFolderPath() + "My Scripts\\";
        }
        /// <summary>
        /// Returns a path to the storage path for taskt objects. Used when accessing the base storage path.
        /// </summary>
        public static string GetAppFolderPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\taskt\\";
        }
        /// <summary>
        /// Returns a path to the storage path for log objects. Used when accessing the base storage path.
        /// </summary>
        public static string GetLogFolderPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\taskt\\Logs";
        }
        /// <summary>
        /// Returns commands from the AutomationCommands.cs file grouped by Custom 'Group' attribute.
        /// </summary>
        public static List<IGrouping<Attribute, Type>> GetGroupedCommands()
        {
            var groupedCommands = Assembly.GetExecutingAssembly().GetTypes()
                          .Where(t => t.Namespace == "taskt.Core.AutomationCommands")
                          .Where(t => t.Name != "ScriptCommand")
                          .Where(t => t.IsAbstract == false)
                          .Where(t => t.BaseType.Name == "ScriptCommand")
                          .Where(t => CommandEnabled(t))
                          .GroupBy(t => t.GetCustomAttribute(typeof(Core.AutomationCommands.Attributes.ClassAttributes.Group)))
                          .ToList();

            return groupedCommands;
        }
        /// <summary>
        /// Returns boolean indicating if the current command is enabled for use in automation.
        /// </summary>
        private static bool CommandEnabled(Type cmd)
        {
            var scriptCommand = (Core.AutomationCommands.ScriptCommand)Activator.CreateInstance(cmd);
            return scriptCommand.CommandEnabled;
        }
        /// <summary>
        /// Returns a list of system-generated variables for use with automation.
        /// </summary>
        public static List<Core.Script.ScriptVariable> GenerateSystemVariables()
        {
            List<Core.Script.ScriptVariable> systemVariableList = new List<Core.Script.ScriptVariable>();
            systemVariableList.Add(new Core.Script.ScriptVariable { variableName = "Folder.Desktop", variableValue = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) });
            systemVariableList.Add(new Core.Script.ScriptVariable { variableName = "Folder.Documents", variableValue = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) });
            systemVariableList.Add(new Core.Script.ScriptVariable { variableName = "Folder.AppData", variableValue = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) });
            systemVariableList.Add(new Core.Script.ScriptVariable { variableName = "Folder.ScriptPath", variableValue = Core.Common.GetScriptFolderPath() });
            systemVariableList.Add(new Core.Script.ScriptVariable { variableName = "DateTime.Now", variableValue = DateTime.Now.ToString() });
            systemVariableList.Add(new Core.Script.ScriptVariable { variableName = "DateTime.Now.FileSafe", variableValue = DateTime.Now.ToString("MM-dd-yy hh.mm.ss") });
            systemVariableList.Add(new Core.Script.ScriptVariable { variableName = "PC.MachineName", variableValue = Environment.MachineName });
            systemVariableList.Add(new Core.Script.ScriptVariable { variableName = "PC.UserName", variableValue = Environment.UserName });
            systemVariableList.Add(new Core.Script.ScriptVariable { variableName = "PC.DomainName", variableValue = Environment.UserDomainName });
            return systemVariableList;
        }
        public static string ImageToBase64(Image image)
        {

            using (MemoryStream m = new MemoryStream())
            {
                image.Save(m, System.Drawing.Imaging.ImageFormat.Bmp);
                byte[] imageBytes = m.ToArray();
                var base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }

        }
        public static Image Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            return image;
        }

    }

    public static class ExtensionMethods
    {
        /// <summary>
        /// Replaces variable placeholders ([variable]) with variable text.
        /// </summary>
        /// <param name="sender">The script engine instance (frmScriptEngine) which contains session variables.</param>
        public static string ConvertToUserVariable(this String str, object sender)
        {
            if (str == null)
                return string.Empty;

            var engineForm = (UI.Forms.frmScriptEngine)sender; ;

            var variableList = engineForm.variableList;
            var systemVariables = Core.Common.GenerateSystemVariables();

            var searchList = new List<Core.Script.ScriptVariable>();
            searchList.AddRange(variableList);
            searchList.AddRange(systemVariables);

            string[] potentialVariables = str.Split('[', ']');

            foreach (var potentialVariable in potentialVariables)
            {
                var varCheck = (from vars in searchList
                                where vars.variableName == potentialVariable
                                select vars).FirstOrDefault();

               // break here; //todo -- this needs to resolve a variable with the name Row.Item(0);

                if (varCheck != null)
                {
                    var searchVariable = "[" + potentialVariable + "]";

                    if (str.Contains(searchVariable))
                    {
                        str = str.Replace(searchVariable, (string)varCheck.GetDisplayValue());
                    }
                    else if (str.Contains(potentialVariable))
                    {
                        str = str.Replace(potentialVariable, (string)varCheck.GetDisplayValue());
                    }
                }

                else if ((potentialVariable.Contains("ds") && (potentialVariable.Contains("."))))
                {
                    //peform dataset check
                    var splitVariable = potentialVariable.Split('.');
                    string dsleading = splitVariable[0];
                    string datasetName = splitVariable[1];
                    string columnRequired = splitVariable[2];
                    int columnNumber;

                    var datasetVariable = variableList.Where(f => f.variableName == datasetName).FirstOrDefault();

                    if (datasetVariable == null)
                        continue;

                    DataTable dataTable = (DataTable)datasetVariable.variableValue;

                    if (datasetVariable == null)
                        continue;

                    if ((dsleading == "ds") && (int.TryParse(columnRequired, out columnNumber)))
                    {
                        //get by column index
                        str = (string)dataTable.Rows[datasetVariable.currentPosition][columnNumber];
                    }
                    else if (dsleading == "ds")
                    {
                            //get by column index
                        str = (string)dataTable.Rows[datasetVariable.currentPosition][columnRequired];
                    }



                }


            }

            //test if math is required
            try
            {
                DataTable dt = new DataTable();
                var v = dt.Compute(str, "");
                return v.ToString();
            }
            catch (Exception)
            {
                return str;
            }
        }
        /// <summary>
        /// Stores value of the string to a user-defined variable.
        /// </summary>
        /// <param name="sender">The script engine instance (frmScriptEngine) which contains session variables.</param>
        /// <param name="targetVariable">the name of the user-defined variable to override with new value</param>
        public static void StoreInUserVariable(this String str, object sender, string targetVariable)
        {
            AutomationCommands.VariableCommand newVariableCommand = new AutomationCommands.VariableCommand();
            newVariableCommand.v_userVariableName = targetVariable;
            newVariableCommand.v_Input = str;
            newVariableCommand.RunCommand(sender);
        }


        public static string ApplyVariableFormatting(this String str)
        {
            return str.Insert(0, "[").Insert(str.Length + 1, "]");
        }
    }

    public class ImageRecognitionFingerPrint
    {
        public int pixelID { get; set; }
        public Color PixelColor { get; set; }
        public int xLocation { get; set; }
        public int yLocation { get; set; }
        bool matchFound { get; set; }

    }

    public class ProgressUpdate
    {
        public int LineNumber { get; set; }
        public string UpdateText { get; set; }
    }




    [Serializable]
    public class ApplicationSettings
    {
        public ServerSettings ServerSettings { get; set; } = new ServerSettings();
        public EngineSettings EngineSettings { get; set; } = new EngineSettings();
        public ClientSettings ClientSettings { get; set; } = new ClientSettings();
        public ApplicationSettings()
        {

        }


        public void Save(ApplicationSettings appSettings)
        {
            var savePath = Core.Common.GetAppFolderPath() + "AppSettings.xml";
            var fileStream = System.IO.File.Create(savePath);

            //output to xml file
            XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
            serializer.Serialize(fileStream, appSettings);
            fileStream.Close();
        }
        public ApplicationSettings GetOrCreateApplicationSettings()
        {
            var savePath = Core.Common.GetAppFolderPath() + "AppSettings.xml";

            ApplicationSettings appSettings;
            if (System.IO.File.Exists(savePath))
            {
                //open file and return it or return new settings on error
                var fileStream = System.IO.File.Open(savePath, FileMode.Open);

                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
                    appSettings = (ApplicationSettings)serializer.Deserialize(fileStream);
                }
                catch (Exception)
                {
                    appSettings = new ApplicationSettings();
                }

                fileStream.Close();
            }
            else
            {
                appSettings = new ApplicationSettings();
            }

            return appSettings;
        }
    }
    [Serializable]
    public class ServerSettings
    {
        public bool ServerConnectionEnabled { get; set; }
        public bool ConnectToServerOnStartup { get; set; }
        public bool RetryServerConnectionOnFail { get; set; }
        public string ServerURL { get; set; }
        public string ServerPublicKey { get; set; }
    }

    [Serializable]
    public class EngineSettings
    {
        public bool ShowDebugWindow { get; set; }
        public bool AutoCloseDebugWindow { get; set; }
        public bool EnableDiagnosticLogging { get; set; }

        public EngineSettings()
            {
            ShowDebugWindow = true;
            AutoCloseDebugWindow = true;
            EnableDiagnosticLogging = true;
            }
    }
    [Serializable]
    public class ClientSettings
    {
        public bool AntiIdleWhileOpen { get; set; }

        public ClientSettings()
        {
            AntiIdleWhileOpen = false;
        }
    }
}