using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dialog/Message Commands")]
    [Attributes.ClassAttributes.CommandSettings("Show Message")]
    [Attributes.ClassAttributes.Description("This command allows you to show a message to the user.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to present or display a value on screen to the user.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'MessageBox' and invokes VariableCommand to find variable data.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ShowMessageCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_MultiLinesTextBox))]
        [PropertyDescription("Message to be Displayed")]
        [InputSpecification("Message", true)]
        [PropertyDetailSampleUsage("**Hello World**", PropertyDetailSampleUsage.ValueType.Value, "Message")]
        [PropertyDetailSampleUsage("**{{{vText}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Message")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Massage", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Message")]
        public string v_Message { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Close After X (Seconds) - 0 to Bypass")]
        [InputSpecification("")]
        [Remarks("Specify how many seconds to display on screen.After the amount of seconds passes, the message box will be automatically closed and script will resume execution. **0** to remain open indefinitely or **5** to stay open for 5 seconds.")]
        [PropertyDetailSampleUsage("**1**", "Close After 1 second")]
        [PropertyDetailSampleUsage("**0**", "Don't Close Automatically")]
        [PropertyDetailSampleUsage("**{{{vTime}}}**", "Close After Value of Variable **vTime** seconds")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "0")]
        [PropertyFirstValue("0")]
        [PropertyDisplayText(false, "")]
        public string v_AutoCloseAfter { get; set; }

        public ShowMessageCommand()
        {
            //this.CommandName = "MessageBoxCommand";
            //this.SelectionName = "Show Message";
            //this.CommandEnabled = true;
            //this.v_AutoCloseAfter = "0";
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
            string variableMessage = v_Message.ConvertToUserVariable(engine);

            variableMessage = variableMessage.Replace("\\n", Environment.NewLine);

            if (engine.tasktEngineUI == null)
            {
                engine.ReportProgress("Complex Messagebox Supported With UI Only");
                MessageBox.Show(variableMessage, "Message Box Command", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var closeAfter = this.ConvertToUserVariableAsInteger(nameof(v_AutoCloseAfter), engine);

            //automatically close messageboxes for server requests
            if (engine.serverExecution && closeAfter <= 0)
            {
                closeAfter = 10;
            }

            // TODO: support OK/cancel etc buttons
            var result = engine.tasktEngineUI.Invoke(new Action(() =>
            {
                engine.tasktEngineUI.ShowMessage(variableMessage, "MessageBox Command", UI.Forms.Supplemental.frmDialog.DialogType.OkOnly, closeAfter);
            }
            ));
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    ////create message controls
        //    //var messageControlSet = CommandControls.CreateDefaultInputGroupFor("v_Message", this, editor);
        //    //RenderedControls.AddRange(messageControlSet);
        //    ////create auto close control set
        //    //var autocloseControlSet = CommandControls.CreateDefaultInputGroupFor("v_AutoCloseAfter", this, editor);
        //    //RenderedControls.AddRange(autocloseControlSet);

        //    var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctrls);

        //    return RenderedControls;

        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Message: " + v_Message + "]";
        //}

        ////controls can be overriden and rendered individually
        //public TextBox v_MessageControl()
        //{
        //    var Textbox = new TextBox();
        //    Textbox.Font = new Font("Segoe UI", 12, FontStyle.Regular);
        //    Textbox.DataBindings.Add("Text", this, "v_Message", false, DataSourceUpdateMode.OnPropertyChanged);
        //    Textbox.Height = 30;
        //    Textbox.Width = 300;

        //    return Textbox;
        //}
    }
}
