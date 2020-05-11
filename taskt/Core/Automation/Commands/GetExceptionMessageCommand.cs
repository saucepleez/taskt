using System;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using System.Linq;
using System.Xml.Serialization;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Error Handling Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to store an exception error message in a variable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to save an exception error message")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class GetExceptionMessageCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please define where the error text should be stored")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_userVariableName { get; set; }

        public GetExceptionMessageCommand()
        {
            this.CommandName = "GetExceptionMessageCommand";
            this.SelectionName = "Get Exception Message";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var error = engine.ErrorsOccured.OrderByDescending(x => x.LineNumber).FirstOrDefault();
            var errorMessage = engine.FileName + " Line " + error.LineNumber.ToString() + "Error: " + error.ErrorMessage;
            errorMessage.StoreInUserVariable(sender, v_userVariableName);

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_userVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_userVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_userVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);

            return RenderedControls;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Store in: '" + v_userVariableName + "']";
        }
    }
}