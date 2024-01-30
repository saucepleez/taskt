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
        #region normal system variables
        public static readonly ScriptVariable Folder_Desktop = new ScriptVariable { VariableName = "Folder.Desktop", VariableValue = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) };
        public static readonly ScriptVariable Folder_Documents = new ScriptVariable { VariableName = "Folder.Documents", VariableValue = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) };
        public static readonly ScriptVariable Folder_AppData = new ScriptVariable { VariableName = "Folder.AppData", VariableValue = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) };
        public static readonly ScriptVariable Folder_ScriptPath = new ScriptVariable { VariableName = "Folder.ScriptPath", VariableValue = Folders.GetFolder(Folders.FolderType.ScriptsFolder) };
        public static readonly ScriptVariable Folder_RootPath = new ScriptVariable { VariableName = "Folder.RootPath", VariableValue = Folders.GetFolder(Folders.FolderType.RootFolder) };
        public static readonly ScriptVariable Folder_AttendedTasksPath = new ScriptVariable { VariableName = "Folder.AttendedTasksPath", VariableValue = Folders.GetFolder(Folders.FolderType.AttendedTasksFolder) };

        public static readonly ScriptVariable DateTime_Now = new ScriptVariable { VariableName = "DateTime.Now", VariableValue = "" };
        public static readonly ScriptVariable DateTime_Now_Month = new ScriptVariable { VariableName = "DateTime.Now.Month", VariableValue = "" };
        public static readonly ScriptVariable DateTime_Now_Day = new ScriptVariable { VariableName = "DateTime.Now.Day", VariableValue = "" };
        public static readonly ScriptVariable DateTime_Now_Year = new ScriptVariable { VariableName = "DateTime.Now.Year", VariableValue = "" };
        public static readonly ScriptVariable DateTime_Now_YearLong = new ScriptVariable { VariableName = "DateTime.Now.YearLong", VariableValue = "" };
        public static readonly ScriptVariable DateTime_Now_Hour = new ScriptVariable { VariableName = "DateTime.Now.Hour", VariableValue = "" };
        public static readonly ScriptVariable DateTime_Now_Minute = new ScriptVariable { VariableName = "DateTime.Now.Minute", VariableValue = "" };
        public static readonly ScriptVariable DateTime_Now_Second = new ScriptVariable { VariableName = "DateTime.Now.Second", VariableValue = "" };
        public static readonly ScriptVariable DateTime_Now_FileSafe = new ScriptVariable { VariableName = "DateTime.Now.FileSafe", VariableValue = "" };

        public static readonly ScriptVariable System_InputLanguage = new ScriptVariable { VariableName = "System.InputLanguage", VariableValue = InputLanguage.CurrentInputLanguage.Culture.Name };
        public static readonly ScriptVariable System_KeyboardLayout = new ScriptVariable { VariableName = "System.KeyboardLayout", VariableValue = InputLanguage.CurrentInputLanguage.LayoutName };

        public static readonly ScriptVariable Error_Message = new ScriptVariable { VariableName = "Error.Message", VariableValue = "" };
        public static readonly ScriptVariable Error_Line = new ScriptVariable { VariableName = "Error.Line", VariableValue = "" };
        public static readonly ScriptVariable Error_StackTrace = new ScriptVariable { VariableName = "Error.StackTrace", VariableValue = "" };

        public static readonly ScriptVariable PC_MachineName = new ScriptVariable { VariableName = "PC.MachineName", VariableValue = Environment.MachineName };
        public static readonly ScriptVariable PC_UserName = new ScriptVariable { VariableName = "PC.UserName", VariableValue = Environment.UserName };
        public static readonly ScriptVariable PC_DomainName = new ScriptVariable { VariableName = "PC.DomainName", VariableValue = Environment.UserDomainName };

        public static readonly ScriptVariable Env_ActiveWindowTitle = new ScriptVariable { VariableName = "Env.ActiveWindowTitle", VariableValue = "" };
        public static readonly ScriptVariable Window_CurrentWindowName = new ScriptVariable { VariableName = "Window.CurrentWindowName", VariableValue = "" };
        public static readonly ScriptVariable Window_CurrentWindowHandle = new ScriptVariable { VariableName = "Window.CurrentWindowHandle", VariableValue = "" };

        public static readonly ScriptVariable Taskt_EngineContext = new ScriptVariable { VariableName = "taskt.EngineContext", VariableValue = "" };    // post expand
        public static readonly ScriptVariable Taskt_Location = new ScriptVariable { VariableName = "taskt.Location", VariableValue = Assembly.GetEntryAssembly().Location };
        
        public static readonly ScriptVariable Char_NewLine = new ScriptVariable { VariableName = "Char.NewLine", VariableValue = Environment.NewLine };
        public static readonly ScriptVariable Char_Cr = new ScriptVariable { VariableName = "Char.Cr", VariableValue = "\r" };
        public static readonly ScriptVariable Char_Lf = new ScriptVariable { VariableName = "Char.Lf", VariableValue = "\n" };
        public static readonly ScriptVariable Char_Tab = new ScriptVariable { VariableName = "Char.Tab", VariableValue = "\t" };
        public static readonly ScriptVariable Char_Space = new ScriptVariable { VariableName = "Char.Space", VariableValue = " " };
        
        public static readonly ScriptVariable FileCounter_F0 = new ScriptVariable { VariableName = "FileCounter.F0", VariableValue = "1" };
        public static readonly ScriptVariable FileCounter_F00 = new ScriptVariable { VariableName = "FileCounter.F00", VariableValue = "01" };
        public static readonly ScriptVariable FileCounter_F000 = new ScriptVariable { VariableName = "FileCounter.F000", VariableValue = "001" };
        public static readonly ScriptVariable File_CurrentScriptPath = new ScriptVariable { VariableName = "File.CurrentScriptFile", VariableValue = "" };

        public static readonly ScriptVariable Loop_CurrentIndex = new ScriptVariable { VariableName = "Loop.CurrentIndex", VariableValue = "0" };

        public static readonly ScriptVariable Math_PI = new ScriptVariable { VariableName = "Math.PI", VariableValue = Math.PI.ToString() };
        public static readonly ScriptVariable Math_E = new ScriptVariable { VariableName = "Math.E", VariableValue = Math.E.ToString() };
        #endregion

        #region limited system variables
        
        /// <summary>
        /// specify desktop
        /// </summary>
        public static readonly ScriptVariable Window_Desktop = new ScriptVariable { VariableName = "Window.Desktop", VariableValue = "" };

        /// <summary>
        /// specify all windows
        /// </summary>
        public static readonly ScriptVariable Window_AllWindows = new ScriptVariable { VariableName = "Window.AllWindows", VariableValue = "" };

        /// <summary>
        /// specify current window position
        /// </summary>
        public static readonly ScriptVariable Window_CurrentPosition = new ScriptVariable { VariableName = "Window.CurrentPosition", VariableValue = "" };
        /// <summary>
        /// specify current window x position
        /// </summary>
        public static readonly ScriptVariable Window_CurrentXPosition = new ScriptVariable { VariableName = "Window.CurrentPosition", VariableValue = "" };
        /// <summary>
        /// specify current window y position
        /// </summary>
        public static readonly ScriptVariable Window_CurrentYPosition = new ScriptVariable { VariableName = "Window.CurrentPosition", VariableValue = "" };
        /// <summary>
        /// specify current window size
        /// </summary>
        public static readonly ScriptVariable Window_CurrentSize = new ScriptVariable { VariableName = "Window.CurrentSize", VariableValue = "" };
        /// <summary>
        /// specify current window width size
        /// </summary>
        public static readonly ScriptVariable Window_CurrentWidth = new ScriptVariable { VariableName = "Window.CurrentWidth", VariableValue = "" };
        /// <summary>
        /// specify current window height size
        /// </summary>
        public static readonly ScriptVariable Window_CurrentHeight = new ScriptVariable { VariableName = "Window.CurrentHeight", VariableValue = "" };
        #endregion

        #region Enum

        public enum LimitedSystemVariableNames
        {
            None,
            Window_Desktop,
            Window_AllWindows,
            Window_CurrentPosition,
            Window_CurrentXPosition,
            Window_CurrentYPosition,
            Window_CurrentSize,
            Window_CurrentWidth,
            Window_CurrentHeight,
        };

        #endregion

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

            Env_ActiveWindowTitle.VariableValue = WindowControls.GetActiveWindowTitle();

            Window_CurrentWindowName.VariableValue = WindowControls.GetActiveWindowTitle();
            Window_CurrentWindowHandle.VariableValue = WindowControls.GetActiveWindowHandle().ToString();

            // NOTE: Keep it commented out as this is where it slows down the script execution.
            //Taskt_EngineContext.VariableValue = engine.GetEngineContext();

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
