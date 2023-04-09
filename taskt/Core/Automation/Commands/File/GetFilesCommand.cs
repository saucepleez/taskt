using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation Commands")]
    [Attributes.ClassAttributes.CommandSettings("Get Files")]
    [Attributes.ClassAttributes.Description("This command returns a list of file paths from a specified location")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to return a list of file paths from a specific location.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetFilesCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Path to the Source Folder")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        //[InputSpecification("Enter or Select the path to the folder.")]
        //[SampleUsage("**C:\\temp\\myfolder** or **{{{vTextFolderPath}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyValidationRule("Folder", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Folder")]
        [PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        public string v_SourceFolderPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("File Name Filter")]
        [InputSpecification("File Name Filter", true)]
        [PropertyDetailSampleUsage("**hello**", PropertyDetailSampleUsage.ValueType.Value, "File Name Filter")]
        [PropertyDetailSampleUsage("**{{{vName}}}**", PropertyDetailSampleUsage.ValueType.VariableName, "File Name Filter")]
        [PropertyIsOptional(true, "Empty and Search All Files")]
        [PropertyValidationRule("", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Name")]
        public string v_SearchFileName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("File Name Search Method")]
        [PropertyUISelectionOption("Contains")]
        [PropertyUISelectionOption("Starts with")]
        [PropertyUISelectionOption("Ends with")]
        [PropertyUISelectionOption("Exact match")]
        [PropertyIsOptional(true, "Contains")]
        [PropertyDisplayText(true, "Search Method")]
        public string v_SearchMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Extension")]
        [InputSpecification("Extention", true)]
        [PropertyDetailSampleUsage("**txt**", "Specify text file for Extension")]
        [PropertyDetailSampleUsage("**{{{vExtension}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Extension")]
        [PropertyIsOptional(true, "Empty and Search All Files")]
        [PropertyValidationRule("", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        public string v_SearchExtension { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        [PropertyDescription("List Variable Name to Store Result")]
        public string v_UserVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_WaitTime))]
        public string v_WaitForFolder { get; set; }

        public GetFilesCommand()
        {
            //this.CommandName = "GetFilesCommand";
            //this.SelectionName = "Get Files";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //apply variable logic
            var sourceFolder = FolderPathControls.WaitForFolder(this, nameof(v_SourceFolderPath), nameof(v_WaitForFolder), engine);

            var searchFile = v_SearchFileName.ConvertToUserVariableAsFileName(engine);

            var ext = v_SearchExtension.ConvertToUserVariable(engine).ToLower();

            // get all files
            List<string> filesList;
            filesList = System.IO.Directory.GetFiles(sourceFolder).ToList();

            if (!String.IsNullOrEmpty(searchFile))
            {
                var searchMethod = this.GetUISelectionValue(nameof(v_SearchMethod), engine);
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