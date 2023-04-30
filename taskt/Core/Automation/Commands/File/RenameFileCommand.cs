using System;
using System.Xml.Serialization;
using System.IO;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation Commands")]
    [Attributes.ClassAttributes.CommandSettings("Rename File")]
    [Attributes.ClassAttributes.Description("This command renames a file at a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to rename an existing file.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class RenameFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.AllowNoExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport)]
        public string v_SourceFilePath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("New File Name (with Extension)")]
        [InputSpecification("Specify the New File Name including the Extension.")]
        [PropertyDetailSampleUsage("**newfile.txt**", PropertyDetailSampleUsage.ValueType.Value, "File Name")]
        [PropertyDetailSampleUsage("**{{{vNewFileName}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "File Name")]
        [Remarks("Changing the file extension will not automatically convert files.")]
        [PropertyValidationRule("New FileName", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "New FileName")]
        public string v_NewName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("When File Name Same After the Change")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyDetailSampleUsage("**Ignore**", "Nothing to do")]
        [PropertyDetailSampleUsage("**Error**", "Rise a Error")]
        [PropertyIsOptional(true, "Ignore")]
        public string v_IfFileNameSame { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        public string v_WaitTime { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePathResult))]
        [PropertyDescription("Variable Name to Store File Path Before Rename")]
        public string v_BeforeFilePathResult { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePathResult))]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store File Path After Rename")]
        public string v_AfterFilePathResult { get; set; }

        public RenameFileCommand()
        {
            //this.CommandName = "RenameFileCommand";
            //this.SelectionName = "Rename File";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            ////apply variable logic
            //var sourceFile = FilePathControls.WaitForFile(this, nameof(v_SourceFilePath), nameof(v_WaitTime), engine);
            
            //var currentFileName = Path.GetFileName(sourceFile);
            //var newFileName = v_NewName.ConvertToUserVariableAsFileName(engine);

            ////get source file name and info
            //FileInfo sourceFileInfo = new FileInfo(sourceFile);

            ////create destination
            //var destinationPath = Path.Combine(sourceFileInfo.DirectoryName, newFileName);

            //var whenSame = this.GetUISelectionValue(nameof(v_IfFileNameSame), engine);
            //if (currentFileName == newFileName)
            //{
            //    switch (whenSame)
            //    {
            //        case "ignore":
            //            return;

            //        case "error":
            //            throw new Exception("File Name before and after the changes is same. Name '" + newFileName + "'");
            //    }
            //}

            ////rename file
            //File.Move(sourceFile, destinationPath);

            FilePathControls.FileAction(this, engine,
                new Action<string>(sourceFile =>
                {
                    var currentFileName = Path.GetFileName(sourceFile);
                    var newFileName = v_NewName.ConvertToUserVariableAsFileName(engine);

                    //get source file name and info
                    FileInfo sourceFileInfo = new FileInfo(sourceFile);

                    //create destination
                    var destinationPath = Path.Combine(sourceFileInfo.DirectoryName, newFileName);

                    var whenSame = this.GetUISelectionValue(nameof(v_IfFileNameSame), engine);
                    if (currentFileName == newFileName)
                    {
                        switch (whenSame)
                        {
                            case "ignore":
                                return;

                            case "error":
                                throw new Exception("File Name before and after the changes is same. Name '" + newFileName + "'");
                        }
                    }

                    //rename file
                    File.Move(sourceFile, destinationPath);

                    if (!string.IsNullOrEmpty(v_AfterFilePathResult))
                    {
                        destinationPath.StoreInUserVariable(engine, v_AfterFilePathResult);
                    }
                })
            );
        }
    }
}