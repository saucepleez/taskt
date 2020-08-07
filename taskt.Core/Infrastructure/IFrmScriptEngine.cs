using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using taskt.Core.Enums;
using taskt.Core.Model.ServerModel;
using taskt.Core.Script;

namespace taskt.Core.Infrastructure
{
    public interface IfrmScriptEngine
    {
        string FilePath { get; set; }
        string JsonData { get; set; }
        Task RemoteTask { get; set; }
        bool ServerExecution { get; set; }
        IfrmScriptBuilder CallBackForm { get; set; }
        //AutomationEngineInstance EngineInstance { get; set; }
        string Result { get; set; }        
        bool IsNewTaskSteppedInto { get; set; }
        bool IsNewTaskResumed { get; set; }
        bool IsNewTaskCancelled { get; set; }
        bool IsHiddenTaskEngine { get; set; }
        int DebugLineNumber { get; set; }
        bool IsDebugMode { get; set; }
        bool CloseWhenDone { get; set; }
        bool ClosingAllEngines { get; set; }
        bool IsChildEngine { get; set; }
        Logger ScriptEngineLogger { get; set; }

        void ShowMessage(string message, string title, DialogType dialogType, int closeAfter);
        void ShowEngineContext(string context, int closeAfter);
        void LaunchRDPSession(string machineName, string userName, string password, int width, int height);
        //List<string> ShowInput(IInputCommand inputs);
        List<ScriptVariable> ShowHTMLInput(string htmlTemplate);
        void AddStatus(string text, Color? statusColor = null);
        void uiBtnPause_Click(object sender, EventArgs e);
    }
}
