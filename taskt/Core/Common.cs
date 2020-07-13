//Copyright (c) 2019 Jason Bayldon
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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using taskt.Core.IO;

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


            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Context = new StreamingContext(StreamingContextStates.Clone);
                formatter.Serialize(ms, source);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }




            ////output to xml file
            //XmlSerializer serializer = new XmlSerializer(typeof(T));
            //var settings = new XmlWriterSettings
            //{
            //    NewLineHandling = NewLineHandling.Entitize,
            //    Indent = true
            //};

            //StringBuilder xml = new StringBuilder();
            //XmlWriter xmlWriter = XmlWriter.Create(xml);

            //serializer.Serialize(xmlWriter, source);


            //using (TextReader reader = new StringReader(xml.ToString()))
            //{
            //    T deserializedData = (T)serializer.Deserialize(reader);
            //    return deserializedData;
            //}

            //System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            //Stream stream = new MemoryStream();
            //using (stream)
            //{
            //    formatter.Serialize(stream, source);
            //    stream.Seek(0, SeekOrigin.Begin);
            //    return (T)formatter.Deserialize(stream);
            //}
        }
        ///// <summary>
        ///// Returns a path to the underlying Script folder where script file objects are loaded and saved. Used when saved or loading files.
        ///// </summary>
        //public static string GetScriptFolderPath()
        //{
        //    return GetAppFolderPath() + "My Scripts\\";
        //}
        ///// <summary>
        ///// Returns a path to the storage path for taskt objects. Used when accessing the base storage path.
        ///// </summary>
        //public static string GetAppFolderPath()
        //{
        //    return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\taskt\\";
        //}
        ///// <summary>
        ///// Returns a path to the storage path for log objects. Used when accessing the base storage path.
        ///// </summary>
        //public static string GetLogFolderPath()
        //{
        //    return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\taskt\\Logs";
        //}

        //public static string GetAppDataPath()
        //{
        //    return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\taskt\\";
        //}

        /// <summary>
        /// Returns commands from the AutomationCommands.cs file grouped by Custom 'Group' attribute.
        /// </summary>
        public static List<IGrouping<Attribute, Type>> GetGroupedCommands()
        {
            var groupedCommands = Assembly.GetExecutingAssembly().GetTypes()
                          .Where(t => t.Namespace == "taskt.Core.Automation.Commands")
                          .Where(t => t.Name != "ScriptCommand")
                          .Where(t => t.IsAbstract == false)
                          .Where(t => t.BaseType.Name == "ScriptCommand")
                          .Where(t => CommandEnabled(t))
                          .GroupBy(t => t.GetCustomAttribute(typeof(Core.Automation.Attributes.ClassAttributes.Group)))
                          .ToList();

            return groupedCommands;
        }
        /// <summary>
        /// Returns boolean indicating if the current command is enabled for use in automation.
        /// </summary>
        private static bool CommandEnabled(Type cmd)
        {
            var scriptCommand = (Core.Automation.Commands.ScriptCommand)Activator.CreateInstance(cmd);
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
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "Folder.ScriptPath", VariableValue = Core.IO.Folders.GetFolder(Folders.FolderType.ScriptsFolder) });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "Folder.RootPath", VariableValue = Core.IO.Folders.GetFolder(Folders.FolderType.RootFolder)});
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "Folder.AttendedTasksPath", VariableValue = Core.IO.Folders.GetFolder(Folders.FolderType.AttendedTasksFolder) });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "DateTime.Now", VariableValue = DateTime.Now.ToString() });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "DateTime.Now.Month", VariableValue = DateTime.Now.ToString("MM")});
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "DateTime.Now.Day", VariableValue = DateTime.Now.ToString("dd") });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "DateTime.Now.Year", VariableValue = DateTime.Now.ToString("yy") });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "DateTime.Now.YearLong", VariableValue = DateTime.Now.ToString("yyyy") });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "DateTime.Now.Hour", VariableValue = DateTime.Now.ToString("HH") });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "DateTime.Now.Minute", VariableValue = DateTime.Now.ToString("mm") });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "DateTime.Now.Second", VariableValue = DateTime.Now.ToString("ss") });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "DateTime.Now.FileSafe", VariableValue = DateTime.Now.ToString("MM-dd-yy hh.mm.ss") });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "System.InputLanguage", VariableValue = InputLanguage.CurrentInputLanguage.Culture.Name });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "System.KeyboardLayout", VariableValue = InputLanguage.CurrentInputLanguage.LayoutName });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "Error.Message", VariableValue = "An Error Occured!" });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "Error.Line", VariableValue = "1" });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "Error.StackTrace", VariableValue = "An Error Occured + StackTrace" });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "PC.MachineName", VariableValue = Environment.MachineName });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "PC.UserName", VariableValue = Environment.UserName });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "PC.DomainName", VariableValue = Environment.UserDomainName });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "Env.ActiveWindowTitle", VariableValue = Core.Automation.User32.User32Functions.GetActiveWindowTitle() });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "taskt.EngineContext", VariableValue = "{JsonContext}" });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "taskt.Location", VariableValue = System.Reflection.Assembly.GetEntryAssembly().Location });
            systemVariableList.Add(new Core.Script.ScriptVariable { VariableName = "Loop.CurrentIndex", VariableValue = "0" });
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

        public static List<string> GetAvailableWindowNames()
        {
            List<string> windowList = new List<string>();
            //get all running processes
            Process[] processlist = Process.GetProcesses();
            //pull the main window title for each
            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    //add to the control list of available windows
                    windowList.Add(process.MainWindowTitle);
                }
            }


            SHDocVw.ShellWindows shellWindows = new SHDocVw.ShellWindows();

            foreach (SHDocVw.InternetExplorer window in shellWindows)
            {
                windowList.Add("Windows Explorer - " + window.LocationName);
            }


            windowList.Sort();

            return windowList;
        }

        private static string GetKeyDescription(Keys key)
        {
            switch (key)
            {
                //letters
                case Keys.A:
                case Keys.B:
                case Keys.C:
                case Keys.D:
                case Keys.E:
                case Keys.F:
                case Keys.G:
                case Keys.H:
                case Keys.I:
                case Keys.J:
                case Keys.K:
                case Keys.L:
                case Keys.M:
                case Keys.N:
                case Keys.O:
                case Keys.P:
                case Keys.Q:
                case Keys.R:
                case Keys.S:
                case Keys.T:
                case Keys.U:
                case Keys.V:
                case Keys.W:
                case Keys.X:
                case Keys.Y:
                case Keys.Z:
                    return Enum.GetName(typeof(Keys), key);

                //digits
                case Keys.D0:
                    return "0";
                case Keys.NumPad0:
                    return "Number Pad 0";
                case Keys.D1:
                    return "1";
                case Keys.NumPad1:
                    return "Number Pad 1";
                case Keys.D2:
                    return "2";
                case Keys.NumPad2:
                    return "Number Pad 2";
                case Keys.D3:
                    return "3";
                case Keys.NumPad3:
                    return "Number Pad 3";
                case Keys.D4:
                    return "4";
                case Keys.NumPad4:
                    return "Number Pad 4";
                case Keys.D5:
                    return "5";
                case Keys.NumPad5:
                    return "Number Pad 5";
                case Keys.D6:
                    return "6";
                case Keys.NumPad6:
                    return "Number Pad 6";
                case Keys.D7:
                    return "7";
                case Keys.NumPad7:
                    return "Number Pad 7";
                case Keys.D8:
                    return "8";
                case Keys.NumPad8:
                    return "Number Pad 8";
                case Keys.D9:
                    return "9";
                case Keys.NumPad9:
                    return "Number Pad 9";

                //punctuation
                case Keys.Add:
                    return "Number Pad +";
                case Keys.Subtract:
                    return "Number Pad -";
                case Keys.Divide:
                    return "Number Pad /";
                case Keys.Multiply:
                    return "Number Pad *";
                case Keys.Space:
                    return "Spacebar";
                case Keys.Decimal:
                    return "Number Pad .";

                //function
                case Keys.F1:
                case Keys.F2:
                case Keys.F3:
                case Keys.F4:
                case Keys.F5:
                case Keys.F6:
                case Keys.F7:
                case Keys.F8:
                case Keys.F9:
                case Keys.F10:
                case Keys.F11:
                case Keys.F12:
                case Keys.F13:
                case Keys.F14:
                case Keys.F15:
                case Keys.F16:
                case Keys.F17:
                case Keys.F18:
                case Keys.F19:
                case Keys.F20:
                case Keys.F21:
                case Keys.F22:
                case Keys.F23:
                case Keys.F24:
                    return Enum.GetName(typeof(Keys), key);

                //navigation
                case Keys.Up:
                    return "Up Arrow";
                case Keys.Down:
                    return "Down Arrow";
                case Keys.Left:
                    return "Left Arrow";
                case Keys.Right:
                    return "Right Arrow";
                case Keys.Prior:
                    return "Page Up";
                case Keys.Next:
                    return "Page Down";
                case Keys.Home:
                    return "Home";
                case Keys.End:
                    return "End";

                //control keys
                case Keys.Back:
                    return "Backspace";
                case Keys.Tab:
                    return "Tab";
                case Keys.Escape:
                    return "Escape";
                case Keys.Enter:
                    return "Enter";
                case Keys.Shift:
                case Keys.ShiftKey:
                    return "Shift";
                case Keys.LShiftKey:
                    return "Shift (Left)";
                case Keys.RShiftKey:
                    return "Shift (Right)";
                case Keys.Control:
                case Keys.ControlKey:
                    return "Control";
                case Keys.LControlKey:
                    return "Control (Left)";
                case Keys.RControlKey:
                    return "Control (Right)";
                case Keys.Menu:
                case Keys.Alt:
                    return "Alt";
                case Keys.LMenu:
                    return "Alt (Left)";
                case Keys.RMenu:
                    return "Alt (Right)";
                case Keys.Pause:
                    return "Pause";
                case Keys.CapsLock:
                    return "Caps Lock";
                case Keys.NumLock:
                    return "Num Lock";
                case Keys.Scroll:
                    return "Scroll Lock";
                case Keys.PrintScreen:
                    return "Print Screen";
                case Keys.Insert:
                    return "Insert";
                case Keys.Delete:
                    return "Delete";
                case Keys.Help:
                    return "Help";
                case Keys.LWin:
                    return "Windows (Left)";
                case Keys.RWin:
                    return "Windows (Right)";
                case Keys.Apps:
                    return "Context Menu";

                //browser keys
                case Keys.BrowserBack:
                    return "Browser Back";
                case Keys.BrowserFavorites:
                    return "Browser Favorites";
                case Keys.BrowserForward:
                    return "Browser Forward";
                case Keys.BrowserHome:
                    return "Browser Home";
                case Keys.BrowserRefresh:
                    return "Browser Refresh";
                case Keys.BrowserSearch:
                    return "Browser Search";
                case Keys.BrowserStop:
                    return "Browser Stop";

                //media keys
                case Keys.VolumeDown:
                    return "Volume Down";
                case Keys.VolumeMute:
                    return "Volume Mute";
                case Keys.VolumeUp:
                    return "Volume Up";
                case Keys.MediaNextTrack:
                    return "Next Track";
                case Keys.Play:
                case Keys.MediaPlayPause:
                    return "Play";
                case Keys.MediaPreviousTrack:
                    return "Previous Track";
                case Keys.MediaStop:
                    return "Stop";
                case Keys.SelectMedia:
                    return "Select Media";

                //IME keys
                case Keys.HanjaMode:
                case Keys.JunjaMode:
                case Keys.HangulMode:
                case Keys.FinalMode:    //duplicate values: Hanguel, Kana, Kanji  
                case Keys.IMEAccept:
                case Keys.IMEConvert:   //duplicate: IMEAceept
                case Keys.IMEModeChange:
                case Keys.IMENonconvert:
                    return null;

                //special keys
                case Keys.LaunchMail:
                    return "Launch Mail";
                case Keys.LaunchApplication1:
                    return "Launch Favorite Application 1";
                case Keys.LaunchApplication2:
                    return "Launch Favorite Application 2";
                case Keys.Zoom:
                    return "Zoom";

                //oem keys 
                case Keys.OemSemicolon: //oem1
                    return ";";
                case Keys.OemQuestion:  //oem2
                    return "?";
                case Keys.Oemtilde:     //oem3
                    return "~";
                case Keys.OemOpenBrackets:  //oem4
                    return "[";
                case Keys.OemPipe:  //oem5
                    return "|";
                case Keys.OemCloseBrackets:    //oem6
                    return "]";
                case Keys.OemQuotes:        //oem7
                    return "'";
                case Keys.OemBackslash: //oem102
                    return "/";
                case Keys.Oemplus:
                    return "+";
                case Keys.OemMinus:
                    return "-";
                case Keys.Oemcomma:
                    return ",";
                case Keys.OemPeriod:
                    return ".";

                //unsupported oem keys
                case Keys.Oem8:
                case Keys.OemClear:
                    return null;

                //unsupported other keys
                case Keys.None:
                case Keys.LButton:
                case Keys.RButton:
                case Keys.MButton:
                case Keys.XButton1:
                case Keys.XButton2:
                case Keys.Clear:
                case Keys.Sleep:
                case Keys.Cancel:
                case Keys.LineFeed:
                case Keys.Select:
                case Keys.Print:
                case Keys.Execute:
                case Keys.Separator:
                case Keys.ProcessKey:
                case Keys.Packet:
                case Keys.Attn:
                case Keys.Crsel:
                case Keys.Exsel:
                case Keys.EraseEof:
                case Keys.NoName:
                case Keys.Pa1:
                case Keys.KeyCode:
                case Keys.Modifiers:
                    return null;

                default:
                    throw new NotSupportedException(Enum.GetName(typeof(Keys), key));
            }
        }

        public static List<string> GetAvailableKeys()
        {
            var keyDescriptionList = new List<string>();

            foreach (string name in Enum.GetNames(typeof(Keys)))
            {

                object value = Enum.Parse(typeof(Keys), name);

                var keyValue = (Keys)value;

               
                string description = Core.Common.GetKeyDescription((Keys)value);

                if (description != null)
                {
                    keyDescriptionList.Add(string.Concat(description, " [", keyValue, "]"));
                }

            }

            return keyDescriptionList;
        }

        public static string ConvertObjectToJson(object obj)
        {
           
                //set json settings
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.Error = (serializer, err) =>
                {
                    err.ErrorContext.Handled = true;
                };

                settings.Formatting = Newtonsoft.Json.Formatting.Indented;

                return JsonConvert.SerializeObject(obj, settings);
         
        }

    }
}

