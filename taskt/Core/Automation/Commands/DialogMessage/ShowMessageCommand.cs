using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.UI.Forms.General;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dialog/Message")]
    [Attributes.ClassAttributes.CommandSettings("Show Message")]
    [Attributes.ClassAttributes.Description("This command allows you to show a message to the user.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to present or display a value on screen to the user.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'MessageBox' and invokes VariableCommand to find variable data.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_input))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ShowMessageCommand : ScriptCommand, IDialogResultProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_MultiLinesTextBox))]
        [PropertyDescription("Message to be Displayed")]
        [InputSpecification("Message", true)]
        [PropertyDetailSampleUsage("**Hello World**", PropertyDetailSampleUsage.ValueType.Value, "Message")]
        [PropertyDetailSampleUsage("**{{{vText}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Message")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Message", PropertyValidationRule.ValidationRuleFlags.None)]
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

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Font Name")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDetailSampleUsage("**MS Gothic**", "Specify MS Gothic")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        public string v_FontName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Font Size")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDetailSampleUsage("**12**", PropertyDetailSampleUsage.ValueType.Value, "Font Size")]
        [PropertyDetailSampleUsage("**{{{vFont}}}**", PropertyDetailSampleUsage.ValueType.VariableName, "Font Size")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        public string v_FontSize { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Dialog Type")]
        [PropertyUISelectionOption("OkOnly")]
        [PropertyUISelectionOption("YesNo")]
        [PropertyUISelectionOption("OkCancel")]
        [PropertyUISelectionOption("Close")]
        [PropertyUISelectionOption("Nothing")]
        [PropertyValidationRule("Dialog Type", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Dialog Type")]
        [PropertyIsOptional(true, "OkOnly")]
        public string v_DialogType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Dialog Title")]
        [InputSpecification("Text")]
        [PropertyIsOptional(true, "ShowMessage Command")]
        [PropertyValidationRule("DialogTitle", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyFirstValue("ShowMessage Command")]
        [PropertyDisplayText(false, "Dialog Title")]
        public string v_DialogTitle { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Wait for answer")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [PropertyIsOptional(true, "Yes")]
        [PropertyFirstValue("Yes")]
        [PropertyDisplayText(false, "Wait For Answer")]
        public string v_WaitForAnswer { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        //[PropertyDescription("Variable Name to Store Dialog Result")]
        //[PropertyIsOptional(true)]
        //[PropertyValidationRule("Dialog Result", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(false, "Dialog Result")]
        [PropertyVirtualProperty(nameof(ShowDialogControls), nameof(ShowDialogControls.v_DialogResult))]
        public string v_DialogResult { get; set; }

        public ShowMessageCommand()
        {
            //this.CommandName = "MessageBoxCommand";
            //this.SelectionName = "Show Message";
            //this.CommandEnabled = true;
            //this.v_AutoCloseAfter = "0";
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            string variableMessage = v_Message.ExpandValueOrUserVariable(engine);

            variableMessage = variableMessage.Replace("\\n", Environment.NewLine);

            if (engine.tasktEngineUI == null)
            {
                engine.ReportProgress("Complex Messagebox Supported With UI Only");
                MessageBox.Show(variableMessage, "Message Box Command", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var closeAfter = this.ExpandValueOrUserVariableAsInteger(nameof(v_AutoCloseAfter), engine);

            // automatically close messageboxes for server requests
            if (engine.serverExecution && closeAfter <= 0)
            {
                closeAfter = 10;
            }

            string fontName = "";
            if (!string.IsNullOrEmpty(v_FontName))
            {
                fontName = this.ExpandValueOrUserVariable(nameof(v_FontName), "Font Name", engine);
            }
            float fontSize = 0F;
            if (!string.IsNullOrEmpty(v_FontSize))
            {
                fontSize = (float)this.ExpandValueOrUserVariableAsDecimal(nameof(v_FontSize), engine);
            }
            if (string.IsNullOrEmpty(v_DialogType))
            {
                v_DialogType = "OkOnly";
            }
            var dialogType = (frmDialog.DialogType)Enum.Parse(typeof(frmDialog.DialogType), this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_DialogType), engine), true);

            if (string.IsNullOrEmpty(v_DialogTitle))
            {
                v_DialogTitle = "ShowMessage Command";
            }
            var dialogTitle = v_DialogTitle.ExpandValueOrUserVariable(engine);

            //// TODO: support OK/cancel etc buttons
            //var result = engine.tasktEngineUI.Invoke(new Action(() =>
            //{
            //    engine.tasktEngineUI.ShowMessage(variableMessage, "MessageBox Command", dialogType, closeAfter);
            //}
            //));

            engine.tasktEngineUI.Invoke(new Action(() =>
            {
                using (var confirmationForm = new frmDialog(variableMessage, dialogTitle, dialogType, closeAfter, true, fontName, fontSize))
                {
                    var res = confirmationForm.ShowDialog();
                    //if (!string.IsNullOrEmpty(v_DialogResult))
                    //{
                    //    //res.ToString().StoreInUserVariable(engine, v_DialogResult);
                    //}
                    this.StoreDialogResultInUserVariable(res.ToString(), engine);
                }
            }));
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
