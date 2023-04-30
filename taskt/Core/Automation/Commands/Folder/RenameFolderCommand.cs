using System;
using System.Xml.Serialization;
using System.IO;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Folder Operation Commands")]
    [Attributes.ClassAttributes.CommandSettings("Rename Folder")]
    [Attributes.ClassAttributes.Description("This command renames a folder at a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to rename an existing folder.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class RenameFolderCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        public string v_SourceFolderPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("New Folder Name")]
        [InputSpecification("New Folder Name", true)]
        //[SampleUsage("**newFolderName** or **{{{vNewFolderName}}}**")]
        [PropertyDetailSampleUsage("**myFolder2**", PropertyDetailSampleUsage.ValueType.Value, "New Folder")]
        [PropertyDetailSampleUsage("**{{{vNewFolder}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "New Folder")]
        [PropertyValidationRule("New Folder", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "New Folder")]
        public string v_NewName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("When Folder Name Same After the Change")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyDetailSampleUsage("**Ignore**", "Nothing to do")]
        [PropertyDetailSampleUsage("**Error**", "Rise a Error")]
        [PropertyIsOptional(true, "Ignore")]
        public string v_IfFolderNameSame { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_WaitTime))]
        public string v_WaitForFolder { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPathResult))]
        [PropertyDescription("Variable Name to Store Folder Path Before Rename")]
        public string v_BeforeFolderPathResult { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Folder Path After Rename")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        public string v_AfterFolderPathResult { get; set; }

        public RenameFolderCommand()
        {
            //this.CommandName = "RenameFolderCommand";
            //this.SelectionName = "Rename Folder";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            ////apply variable logic
            //var sourceFolder = FolderPathControls.WaitForFolder(this, nameof(v_SourceFolderPath), nameof(v_WaitForFolder), engine);
            //var currentFolderName = Path.GetFileName(sourceFolder);

            //var newFolderName = v_NewName.ConvertToUserVariableAsFolderName(engine);

            ////get source folder name and info
            //DirectoryInfo sourceFolderInfo = new DirectoryInfo(sourceFolder);

            ////create destination
            //var destinationPath = Path.Combine(sourceFolderInfo.Parent.FullName, newFolderName);

            //var whenSame = this.GetUISelectionValue(nameof(v_IfFolderNameSame), engine);
            //if (currentFolderName == newFolderName)
            //{
            //    switch (whenSame)
            //    {
            //        case "ignore":
            //            return; 

            //        case "error":
            //            throw new Exception("Folder Name before and after the changes is same. Name '" + newFolderName + "'");
            //    }
            //}

            ////rename folder
            //Directory.Move(sourceFolder, destinationPath);

            FolderPathControls.FolderAction(this, engine,
                new Action<string>(path =>
                {
                    var currentFolderName = Path.GetFileName(path);

                    var newFolderName = v_NewName.ConvertToUserVariableAsFolderName(engine);

                    //get source folder name and info
                    DirectoryInfo sourceFolderInfo = new DirectoryInfo(path);

                    //create destination
                    var destinationPath = Path.Combine(sourceFolderInfo.Parent.FullName, newFolderName);

                    var whenSame = this.GetUISelectionValue(nameof(v_IfFolderNameSame), engine);
                    if (currentFolderName == newFolderName)
                    {
                        switch (whenSame)
                        {
                            case "ignore":
                                return;

                            case "error":
                                throw new Exception("Folder Name before and after the changes is same. Name '" + newFolderName + "'");
                        }
                    }

                    //rename folder
                    Directory.Move(path, destinationPath);

                    if (!string.IsNullOrEmpty(v_AfterFolderPathResult))
                    {
                        destinationPath.StoreInUserVariable(engine, v_AfterFolderPathResult);
                    }
                })
            );
        }
    }
}