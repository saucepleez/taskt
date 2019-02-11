using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using taskt.Core.Automation.User32;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to set text to the clipboard.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to copy the data from the clipboard and apply it to a variable.  You can then use the variable to extract the value.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against the VariableList from the scripting engine using System.Windows.Forms.Clipboard.")]
    public class ClipboardSetTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select a target variable or input a value")]
        [Attributes.PropertyAttributes.InputSpecification("Select a variable or provide an input value")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InputValue { get; set; }

        public ClipboardSetTextCommand()
        {
            this.CommandName = "ClipboardSetTextCommand";
            this.SelectionName = "Set Clipboard Text";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            var input = v_InputValue.ConvertToUserVariable(sender);
            User32Functions.SetClipboardText(input);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Apply '" + v_InputValue + "' to Clipboard]";
        }
    }
}
