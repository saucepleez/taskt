using System;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.SubGruop("Clipboard")]
    [Attributes.ClassAttributes.CommandSettings("Clear Clipboard Text")]
    [Attributes.ClassAttributes.Description("This command allows you to clear text to the clipboard.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to copy the data from the clipboard and apply it to a variable.  You can then use the variable to extract the value.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against the VariableList from the scripting engine using System.Windows.Forms.Clipboard.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true, true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ClearClipboardTextCommand : ScriptCommand
    {
        public ClearClipboardTextCommand()
        {
            //this.CommandName = "ClipboardClearTextCommand";
            //this.SelectionName = "Clear Clipboard Text";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //User32Functions.SetClipboardText("");
            ClipboardControls.SetClipboardText("");
        }
    }
}
