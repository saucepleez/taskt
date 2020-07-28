using System.Windows.Forms;

namespace taskt.Core.Infrastructure
{
    public interface IfrmScriptBuilder
    {
        string ScriptFilePath { get; set; }
        int DebugLine { get; set; }
        IfrmScriptEngine CurrentEngine { get; set; }
        bool IsScriptRunning { get; set; }
        bool IsScriptPaused { get; set; }
        bool IsScriptSteppedOver { get; set; }
        bool IsScriptSteppedInto { get; set; }
        bool IsUnhandledException { get; set; }
        void Notify(string notificationText);
        void RemoveDebugTab();
        DialogResult LoadErrorForm(string errorMessage);
    }
}
