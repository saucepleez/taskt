﻿using System;
using System.Xml.Serialization;
using System.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Folder Operation Commands")]
    [Attributes.ClassAttributes.CommandSettings("Get Folders")]
    [Attributes.ClassAttributes.Description("This command returns a list of folder directories from a specified location")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to return a list of folder directories from a specific location.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetFoldersCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please indicate the path to the source folder")]
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
        [PropertyDescription("Folder Name to Filter")]
        [InputSpecification("Folder Name to Filter", true)]
        //[SampleUsage("**hello** or **{{{vFolderName}}}**")]
        [PropertyDetailSampleUsage("**hello**", PropertyDetailSampleUsage.ValueType.Value, "Filter")]
        [PropertyDetailSampleUsage("**{{{vFolderName}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Filter")]
        [PropertyIsOptional(true, " Empty and searched All Folders")]
        [PropertyValidationRule("", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Filter")]
        public string v_SearchFolderName { get; set; }

        [XmlAttribute]
        [PropertyDetailSampleUsage(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Folder Name Search Method")]
        [PropertyUISelectionOption("Contains")]
        [PropertyUISelectionOption("Starts with")]
        [PropertyUISelectionOption("Ends with")]
        [PropertyUISelectionOption("Exact match")]
        [PropertyIsOptional(true, "Contains")]
        public string v_SearchMethod { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Specify the variable to assign the folder path list")]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        //[PropertyIsVariablesList(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        //[PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Store")]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public string v_UserVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_WaitTime))]
        public string v_WaitForFolder { get; set; }

        public GetFoldersCommand()
        {
            //this.CommandName = "GetFoldersCommand";
            //this.SelectionName = "Get Folders";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //apply variable logic
            //var sourceFolder = v_SourceFolderPath.ConvertToUserVariable(sender);
            var sourceFolder = FolderPathControls.WaitForFolder(this, nameof(v_SourceFolderPath), nameof(v_WaitForFolder), engine);

            //delete folder
            var directoriesList = System.IO.Directory.GetDirectories(sourceFolder).ToList();

            //var searchFolder = v_SearchFolderName.ConvertToUserVariable(engine);
            var searchFolder = v_SearchFolderName.ConvertToUserVariableAsFolderName(engine);
            if (!String.IsNullOrEmpty(searchFolder))
            {
                switch (this.GetUISelectionValue(nameof(v_SearchMethod), engine))
                {
                    case "contains":
                        directoriesList = directoriesList.Where(t => System.IO.Path.GetFileName(t).Contains(searchFolder)).ToList();
                        break;
                    case "starts with":
                        directoriesList = directoriesList.Where(t => System.IO.Path.GetFileName(t).StartsWith(searchFolder)).ToList();
                        break;
                    case "ends with":
                        directoriesList = directoriesList.Where(t => System.IO.Path.GetFileName(t).EndsWith(searchFolder)).ToList();
                        break;
                    case "exact match":
                        directoriesList = directoriesList.Where(t => System.IO.Path.GetFileName(t).Equals(searchFolder)).ToList();
                        break;
                }
            }

            directoriesList.StoreInUserVariable(engine, v_UserVariableName);
        }
    }
}