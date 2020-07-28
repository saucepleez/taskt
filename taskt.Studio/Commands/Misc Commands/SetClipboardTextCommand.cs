using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.User32;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Misc Commands")]
    [Description("This command allows you to set text to the clipboard.")]
    [UsesDescription("Use this command when you want to copy the data from the clipboard and apply it to a variable.  You can then use the variable to extract the value.")]
    [ImplementationDescription("This command implements actions against the VariableList from the scripting engine using System.Windows.Forms.Clipboard.")]
    public class SetClipboardTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select a target variable or input a value")]
        [InputSpecification("Select a variable or provide an input value")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InputValue { get; set; }

        [XmlIgnore]
        [NonSerialized]
        public ComboBox VariableNameControl;

        public SetClipboardTextCommand()
        {
            CommandName = "SetClipboardTextCommand";
            SelectionName = "Set Clipboard Text";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var input = v_InputValue.ConvertToUserVariable(engine);
            User32Functions.SetClipboardText(input);
        }
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            //create window name helper control
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_InputValue", this));
            VariableNameControl = CommandControls.CreateStandardComboboxFor("v_InputValue", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_InputValue", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);

            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Apply '" + v_InputValue + "' to Clipboard]";
        }
    }
}
