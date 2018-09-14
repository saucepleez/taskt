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
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "Folder.Desktop", VariableValue = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "Folder.Documents", VariableValue = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "Folder.AppData", VariableValue = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "Folder.ScriptPath", VariableValue = Core.Common.GetScriptFolderPath() });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "DateTime.Now", VariableValue = DateTime.Now.ToString() });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "DateTime.Now.FileSafe", VariableValue = DateTime.Now.ToString("MM-dd-yy hh.mm.ss") });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "PC.MachineName", VariableValue = Environment.MachineName });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "PC.UserName", VariableValue = Environment.UserName });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "PC.DomainName", VariableValue = Environment.UserDomainName });
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
}