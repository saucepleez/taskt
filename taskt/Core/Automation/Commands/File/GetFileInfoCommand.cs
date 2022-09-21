using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using System.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation Commands")]
    [Attributes.ClassAttributes.Description("This command returns a list of file paths from a specified location")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to return a list of file paths from a specific location.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetFileInfoCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the file name")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [InputSpecification("Enter or Select the file name.")]
        [SampleUsage("**C:\\temp\\myfile.txt** or **{{{vFileName}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("File", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "File")]
        public string v_TargetFileName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the information type.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyUISelectionOption("File size")]
        [PropertyUISelectionOption("Readonly file")]
        [PropertyUISelectionOption("Hidden file")]
        [PropertyUISelectionOption("Creation time")]
        [PropertyUISelectionOption("Last write time")]
        [PropertyUISelectionOption("Last access time")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Type")]
        public string v_InfoType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Specify the variable to assign the result")]
        [InputSpecification("")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Store")]
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
            var engine = (Engine.AutomationEngineInstance)sender;
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

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctrls);

        //    return RenderedControls;
        //}
        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Get File '" + v_InfoType + "' From: '" + v_TargetFileName + "', Store In: '" + v_UserVariableName + "']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_TargetFileName))
        //    {
        //        this.validationResult += "File is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_InfoType))
        //    {
        //        this.validationResult += "Information type is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_UserVariableName))
        //    {
        //        this.validationResult += "Variable is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}

        public override void addInstance(InstanceCounter counter)
        {
            string type = (String.IsNullOrEmpty(v_InfoType) ? "" : v_InfoType.ToLower());

            switch(type)
            {
                case "readonly file":
                case "hidden file":
                    var boolType = new Automation.Attributes.PropertyAttributes.PropertyInstanceType(PropertyInstanceType.InstanceType.Boolean, true);
                    var ins = (String.IsNullOrEmpty(v_UserVariableName) ? "" : v_UserVariableName);
                    counter.addInstance(ins, boolType, true);
                    counter.addInstance(ins, boolType, false);
                    break;
            }
        }

        public override void removeInstance(InstanceCounter counter)
        {
            string type = (String.IsNullOrEmpty(v_InfoType) ? "" : v_InfoType.ToLower());

            switch (type)
            {
                case "readonly file":
                case "hidden file":
                    var boolType = new Automation.Attributes.PropertyAttributes.PropertyInstanceType(PropertyInstanceType.InstanceType.Boolean, true);
                    var ins = (String.IsNullOrEmpty(v_UserVariableName) ? "" : v_UserVariableName);
                    counter.removeInstance(ins, boolType, true);
                    counter.removeInstance(ins, boolType, false);
                    break;
            }
        }
    }
}