using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation Commands")]
    [Attributes.ClassAttributes.Description("This command deletes a file from a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to detete a file from a specific location.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class DeleteFileCommand : ScriptCommand
    {

        [XmlAttribute]
        [PropertyDescription("Please indicate the File Path to Delete.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [InputSpecification("Enter or Select the path to the file.")]
        [SampleUsage("**C:\\temp\\myfile.txt** or **{{{vTextFilePath}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("File Path")]
        [PropertyDisplayText(true, "File")]
        public string v_SourceFilePath { get; set; }

        public DeleteFileCommand()
        {
            this.CommandName = "DeleteFileCommand";
            this.SelectionName = "Delete File";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //apply variable logic
            var sourceFile = v_SourceFilePath.ConvertToUserVariable(sender);

            //delete file
            System.IO.File.Delete(sourceFile);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SourceFilePath", this, editor));

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [delete " + v_SourceFilePath + "']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_SourceFilePath))
        //    {
        //        this.validationResult += "Source file is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}