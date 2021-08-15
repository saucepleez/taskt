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
    [Attributes.ClassAttributes.Description("This command returns a list of file paths from a specified location")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to return a list of file paths from a specific location.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class GetFileInfoCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the file name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the file name.")]
        [Attributes.PropertyAttributes.SampleUsage("**C:\\temp\\myfile.txt** or **{{{vFileName}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_TargetFileName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please specify the informationtype.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**C:\\temp\\myfile.txt** or **{{{vFileName}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("File size")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Readonly file")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Hidden file")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Creation time")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Last write time")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Last access time")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_InfoType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Specify the variable to assign the result")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        public string v_UserVariableName { get; set; }

        public GetFileInfoCommand()
        {
            this.CommandName = "GetFileInfoCommand";
            this.SelectionName = "Get File Info";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            //apply variable logic
            var targetFile = v_TargetFileName.ConvertToUserVariable(sender);
            var infoType = v_InfoType.ConvertToUserVariable(sender);

            string ret = "";
            var fileInfo = new System.IO.FileInfo(targetFile);
            switch (infoType)
            {
                case "File size":
                    ret = fileInfo.Length.ToString();
                    break;
                case "Readonly file":
                    ret = fileInfo.IsReadOnly ? "TRUE" : "FALSE";
                    break;
                case "Hidden file":
                    ret = (((System.IO.FileAttributes)fileInfo.Attributes & System.IO.FileAttributes.Hidden) == System.IO.FileAttributes.Hidden) ? "TRUE": "FALSE";
                    break;
                case "Creation time":
                    ret = fileInfo.CreationTime.ToString();
                    break;
                case "Last write time":
                    ret = fileInfo.LastWriteTime.ToString();
                    break;
                case "Last access time":
                    ret = fileInfo.LastAccessTime.ToString();
                    break;
                default:
                    throw new Exception(infoType + " is not support.");
                    break;
            }

            ret.StoreInUserVariable(sender, v_UserVariableName);
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            return RenderedControls;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Get File '" + v_InfoType + "' From: '" + v_TargetFileName + "', Store In: '" + v_UserVariableName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_TargetFileName))
            {
                this.validationResult += "File is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_InfoType))
            {
                this.validationResult += "Information type is empty.\n";
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