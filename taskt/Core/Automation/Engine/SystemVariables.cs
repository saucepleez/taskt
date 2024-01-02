using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using taskt.Core.Automation.Commands;
using taskt.Core.IO;
using taskt.Core.Script;

namespace taskt.Core.Automation.Engine
{
    /// <summary>
    /// system variables
    /// </summary>
    public static class SystemVariables
    {
        private static readonly ScriptVariable Folder_Desktop = new ScriptVariable { VariableName = "Folder.Desktop", VariableValue = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) };
        private static readonly ScriptVariable Folder_Documents = new ScriptVariable { VariableName = "Folder.Documents", VariableValue = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) };
        private static readonly ScriptVariable Folder_AppData = new ScriptVariable { VariableName = "Folder.AppData", VariableValue = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) };
        private static readonly ScriptVariable Folder_ScriptPath = new ScriptVariable { VariableName = "Folder.ScriptPath", VariableValue = Folders.GetFolder(Folders.FolderType.ScriptsFolder) };
        private static readonly ScriptVariable Folder_RootPath = new ScriptVariable { VariableName = "Folder.RootPath", VariableValue = Folders.GetFolder(Folders.FolderType.RootFolder) };
        private static readonly ScriptVariable Folder_AttendedTasksPath = new ScriptVariable { VariableName = "Folder.AttendedTasksPath", VariableValue = Folders.GetFolder(Folders.FolderType.AttendedTasksFolder) };

        private static readonly ScriptVariable DateTime_Now = new ScriptVariable { VariableName = "DateTime.Now", VariableValue = "" };
        private static readonly ScriptVariable DateTime_Now_Month = new ScriptVariable { VariableName = "DateTime.Now.Month", VariableValue = "" };
        private static readonly ScriptVariable DateTime_Now_Day = new ScriptVariable { VariableName = "DateTime.Now.Day", VariableValue = "" };
        private static readonly ScriptVariable DateTime_Now_Year = new ScriptVariable { VariableName = "DateTime.Now.Year", VariableValue = "" };
        private static readonly ScriptVariable DateTime_Now_YearLong = new ScriptVariable { VariableName = "DateTime.Now.YearLong", VariableValue = "" };
        private static readonly ScriptVariable DateTime_Now_Hour = new ScriptVariable { VariableName = "DateTime.Now.Hour", VariableValue = "" };
        private static readonly ScriptVariable DateTime_Now_Minute = new ScriptVariable { VariableName = "DateTime.Now.Minute", VariableValue = "" };
        private static readonly ScriptVariable DateTime_Now_Second = new ScriptVariable { VariableName = "DateTime.Now.Second", VariableValue = "" };
        private static readonly ScriptVariable DateTime_Now_FileSafe = new ScriptVariable { VariableName = "DateTime.Now.FileSafe", VariableValue = "" };

        private static readonly ScriptVariable System_InputLanguage = new ScriptVariable { VariableName = "System.InputLanguage", VariableValue = InputLanguage.CurrentInputLanguage.Culture.Name };
        private static readonly ScriptVariable System_KeyboardLayout = new ScriptVariable { VariableName = "System.KeyboardLayout", VariableValue = InputLanguage.CurrentInputLanguage.LayoutName };

        private static readonly ScriptVariable Error_Message = new ScriptVariable { VariableName = "Error.Message", VariableValue = "" };
        private static readonly ScriptVariable Error_Line = new ScriptVariable { VariableName = "Error.Line", VariableValue = "" };
        private static readonly ScriptVariable Error_StackTrace = new ScriptVariable { VariableName = "Error.StackTrace", VariableValue = "" };

        private static readonly ScriptVariable PC_MachineName = new ScriptVariable { VariableName = "PC.MachineName", VariableValue = Environment.MachineName };
        private static readonly ScriptVariable PC_UserName = new ScriptVariable { VariableName = "PC.UserName", VariableValue = Environment.UserName };
        private static readonly ScriptVariable PC_DomainName = new ScriptVariable { VariableName = "PC.DomainName", VariableValue = Environment.UserDomainName };

        private static readonly ScriptVariable Env_ActiveWindowTitle = new ScriptVariable { VariableName = "Env.ActiveWindowTitle", VariableValue = "" };
        private static readonly ScriptVariable Window_CurrentWindowName = new ScriptVariable { VariableName = "Window.CurrentWindowName", VariableValue = "" };
        private static readonly ScriptVariable Window_CurrentWindowHandle = new ScriptVariable { VariableName = "Window.CurrentWindowHandle", VariableValue = "" };

        private static readonly ScriptVariable Taskt_EngineContext = new ScriptVariable { VariableName = "taskt.EngineContext", VariableValue = "" };
        private static readonly ScriptVariable Taskt_Location = new ScriptVariable { VariableName = "taskt.Location", VariableValue = Assembly.GetEntryAssembly().Location };
        
        private static readonly ScriptVariable Char_NewLine = new ScriptVariable { VariableName = "Char.NewLine", VariableValue = Environment.NewLine };
        private static readonly ScriptVariable Char_Cr = new ScriptVariable { VariableName = "Char.Cr", VariableValue = "\r" };
        private static readonly ScriptVariable Char_Lf = new ScriptVariable { VariableName = "Char.Lf", VariableValue = "\n" };
        private static readonly ScriptVariable Char_Tab = new ScriptVariable { VariableName = "Char.Tab", VariableValue = "\t" };
        private static readonly ScriptVariable Char_Space = new ScriptVariable { VariableName = "Char.Space", VariableValue = " " };
        
        private static readonly ScriptVariable FileCounter_F0 = new ScriptVariable { VariableName = "FileCounter.F0", VariableValue = "1" };
        private static readonly ScriptVariable FileCounter_F00 = new ScriptVariable { VariableName = "FileCounter.F00", VariableValue = "01" };
        private static readonly ScriptVariable FileCounter_F000 = new ScriptVariable { VariableName = "FileCounter.F000", VariableValue = "001" };
        private static readonly ScriptVariable File_CurrentScriptPath = new ScriptVariable { VariableName = "File.CurrentScriptFile", VariableValue = "" };

        private static readonly ScriptVariable Loop_CurrentIndex = new ScriptVariable { VariableName = "Loop.CurrentIndex", VariableValue = "0" };

        private static readonly ScriptVariable Math_PI = new ScriptVariable { VariableName = "Math.PI", VariableValue = Math.PI.ToString() };
        private static readonly ScriptVariable Math_E = new ScriptVariable { VariableName = "Math.E", VariableValue = Math.E.ToString() };

        private static readonly List<ScriptVariable> systemVariables = new List<ScriptVariable>()
        {
            // folders
            Folder_Desktop,
            Folder_Documents,
            Folder_AppData,
            Folder_ScriptPath,
            Folder_RootPath,
            Folder_AttendedTasksPath,

            // datetime
            DateTime_Now,
            DateTime_Now_Month,
            DateTime_Now_Day,
            DateTime_Now_Year,
            DateTime_Now_YearLong,
            DateTime_Now_Hour,
            DateTime_Now_Minute,
            DateTime_Now_Second,
            DateTime_Now_FileSafe,

            // system
            System_InputLanguage,
            System_KeyboardLayout,

            // error
            Error_Message,
            Error_Line,
            Error_StackTrace,

            // PC
            PC_MachineName,
            PC_UserName,
            PC_DomainName,

            // Active Window Title (not recommended)
            Env_ActiveWindowTitle,
            // Window
            Window_CurrentWindowName,
            Window_CurrentWindowHandle,

            // taskt
            Taskt_EngineContext,
            Taskt_Location,

            // special chars
            Char_NewLine,
            Char_Cr,
            Char_Lf,
            Char_Tab,
            Char_Space,

            // file
            FileCounter_F0,
            FileCounter_F00,
            FileCounter_F000,
            File_CurrentScriptPath,

            // loop
            Loop_CurrentIndex,

            // math
            Math_PI,
            Math_E,
        };

        /// <summary>
        /// update system variables value
        /// </summary>
        /// <param name="engine"></param>
        private static void UpdateSystemVariables(AutomationEngineInstance engine)
        {
            DateTime_Now.VariableValue = DateTime.Now.ToString();
            DateTime_Now_Month.VariableValue = DateTime.Now.ToString("MM");
            DateTime_Now_Day.VariableValue = DateTime.Now.ToString("dd");
            DateTime_Now_Year.VariableValue = DateTime.Now.ToString("yy");
            DateTime_Now_YearLong.VariableValue = DateTime.Now.ToString("yyyy");
            DateTime_Now_Hour.VariableValue = DateTime.Now.ToString("HH");
            DateTime_Now_Minute.VariableValue = DateTime.Now.ToString("mm");
            DateTime_Now_Second.VariableValue = DateTime.Now.ToString("ss");
            DateTime_Now_FileSafe.VariableValue = DateTime.Now.ToString("MM-dd-yy hh.mm.ss");

            Env_ActiveWindowTitle.VariableValue = WindowNameControls.GetActiveWindowTitle();

            Window_CurrentWindowName.VariableValue = WindowNameControls.GetActiveWindowTitle();
            Window_CurrentWindowHandle.VariableValue = WindowNameControls.GetActiveWindowHandle().ToString();

            Taskt_EngineContext.VariableValue = engine.GetEngineContext();

            File_CurrentScriptPath.VariableValue = engine.FileName;
        }

        /// <summary>
        /// get system variables list
        /// </summary>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static List<ScriptVariable> GetSystemVariables(AutomationEngineInstance engine)
        {
            UpdateSystemVariables(engine);
            return systemVariables;
        }

        /// <summary>
        /// get system variable names
        /// </summary>
        /// <returns></returns>
        public static List<string> GetSystemVariablesName()
        {
            return systemVariables.Select(t => t.VariableName).ToList();
        }

        /// <summary>
        /// update Loop.CurrentIndex value
        /// </summary>
        /// <param name="value"></param>
        public static void Update_LoopCurrentIndex(int value)
        {
            Loop_CurrentIndex.VariableValue = value.ToString();
        }
    }
}
