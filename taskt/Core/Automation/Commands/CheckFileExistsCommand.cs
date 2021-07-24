using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using System.Linq;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation Commands")]
    [Attributes.ClassAttributes.Description("This command returns a existence of file paths from a specified location")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to return a existence of file paths from a specific location.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class CheckFileExistsCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Specify the path of the file you want to check for existence (ex. C:\\temp\\myfile.txt, {{{vFilePath}}})")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the file.")]
        [Attributes.PropertyAttributes.SampleUsage("**C:\\temp\\myfile.txt** or **{{{vFilePath}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_TargetFileName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Specify the variable to assign the result")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("Result is **TRUE** or **FALSE**")]
        public string v_UserVariableName { get; set; }

        public CheckFileExistsCommand()
        {
            this.CommandName = "CheckFileExistsCommand";
            this.SelectionName = "Check File Exists";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            //apply variable logic
            var targetFile = v_TargetFileName.ConvertToUserVariable(sender);

            (System.IO.File.Exists(targetFile) ? "TRUE" : "FALSE").StoreInUserVariable(sender, v_UserVariableName);
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_TargetFileName", this, editor));

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_UserVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_UserVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_UserVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);

            return RenderedControls;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Check: '" + v_TargetFileName + "', Result In: '" + v_UserVariableName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_TargetFileName))
            {
                this.validationResult += "Target file is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_UserVariableName))
            {
                this.validationResult += "Variable is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}