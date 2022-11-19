using System;
using System.Collections.Generic;
using System.Xml.Serialization;
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
    public class GetFilesCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the path to the source folder.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        [InputSpecification("Enter or Select the path to the folder.")]
        [SampleUsage("**C:\\temp\\myfolder** or **{{{vTextFolderPath}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Folder", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Folder")]
        public string v_SourceFolderPath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please indicate the file name filter")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter or Select the file name filter.")]
        [SampleUsage("**hello** or **{{{vFileName}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyIsOptional(true, "empty and search all files")]
        [PropertyDisplayText(true, "Name")]
        public string v_SearchFileName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please indicate the file name search method")]
        [InputSpecification("")]
        [PropertyUISelectionOption("Contains")]
        [PropertyUISelectionOption("Starts with")]
        [PropertyUISelectionOption("Ends with")]
        [PropertyUISelectionOption("Exact match")]
        [SampleUsage("**Contains** or **Starts with** or **Ends with** or **Exact match**")]
        [Remarks("")]
        [PropertyIsOptional(true, "Contains")]
        public string v_SearchMethod { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please indicate the extension")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter or Select the extension.")]
        [SampleUsage("**txt** or **{{{vExtension}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyIsOptional(true, "empty and search all files")]
        public string v_SearchExtension { get; set; }

        [XmlAttribute]
        [PropertyDescription("Specify the variable to assign the file path list")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyIsVariablesList(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyValidationRule("List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Store")]
        public string v_UserVariableName { get; set; }

        public GetFilesCommand()
        {
            this.CommandName = "GetFilesCommand";
            this.SelectionName = "Get Files";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
            //apply variable logic
            var sourceFolder = v_SourceFolderPath.ConvertToUserVariable(sender);

            var searchFile = v_SearchFileName.ConvertToUserVariable(sender);

            var ext = v_SearchExtension.ConvertToUserVariable(sender).ToLower();

            // get all files
            List<string> filesList;
            filesList = System.IO.Directory.GetFiles(sourceFolder).ToList();

            if (!String.IsNullOrEmpty(searchFile))
            {
                //var searchMethod = v_SearchMethod.ConvertToUserVariable(sender);
                //if (String.IsNullOrEmpty(searchMethod))
                //{
                //    searchMethod = "Contains";
                //}
                var searchMethod = this.GetUISelectionValue(nameof(v_SearchMethod), "Search Method", engine);
                switch (searchMethod)
                {
                    case "contains":
                        filesList = filesList.Where(t => System.IO.Path.GetFileNameWithoutExtension(t).Contains(searchFile)).ToList();
                        break;
                    case "starts with":
                        filesList = filesList.Where(t => System.IO.Path.GetFileNameWithoutExtension(t).StartsWith(searchFile)).ToList();
                        break;
                    case "ends with":
                        filesList = filesList.Where(t => System.IO.Path.GetFileNameWithoutExtension(t).EndsWith(searchFile)).ToList();
                        break;
                    case "exact match":
                        filesList = filesList.Where(t => System.IO.Path.GetFileNameWithoutExtension(t).Equals(searchFile)).ToList();
                        break;
                }
            }

            if (!String.IsNullOrEmpty(ext))
            {
                ext = "." + ext;
                filesList = filesList.Where(t => System.IO.Path.GetExtension(t).ToLower() == ext).ToList();
            }

            filesList.StoreInUserVariable(engine, v_UserVariableName);
        }
    }
}