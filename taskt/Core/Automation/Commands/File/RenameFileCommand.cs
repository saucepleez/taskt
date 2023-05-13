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
        [PropertyDescription("New File Name")]
        [InputSpecification("New File Name", true)]
        [PropertyDetailSampleUsage("**newfile**", PropertyDetailSampleUsage.ValueType.Value, "File Name")]
        [PropertyDetailSampleUsage("**{{{vNewFileName}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "File Name")]
        [PropertyValidationRule("New FileName", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "New FileName")]
        public string v_NewName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyUISelectionOption("Auto")]
        [PropertyUISelectionOption("Force Combine New Extension")]
        [PropertyUISelectionOption("Contains New File Name")]
        [PropertyUISelectionOption("Use Before Rename Path")]
        [PropertyAddtionalParameterInfo("Auto", "If the New File Name does not contain an Extension and Not specified New Extension, it will automatically be given the extension of the path before the Rename.")]
        [PropertyAddtionalParameterInfo("Force Combine New Extension", "Forces combining the specified extensions with the New Extension")]
        [PropertyAddtionalParameterInfo("Contains New File Name", "Determine that New File Name contains the Extension and Do NOT add the Extension to the New File Name")]
        [PropertyAddtionalParameterInfo("Use Before Rename Path", "Forces before Rename File Path extensions to be combined")]
        [PropertySecondaryLabel(true)]
        [PropertyIsOptional(true, "Auto")]
        [PropertySelectionChangeEvent(nameof(cmbExtensionOption_SelectionChange))]
        public string v_ExtentionOption { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("New File Extention")]
        [InputSpecification("New File Extention", true)]
        [PropertyDetailSampleUsage("**txt**", PropertyDetailSampleUsage.ValueType.Value, "Extension")]
        [PropertyDetailSampleUsage("**{{{vExtension}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Extension")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("New Extension", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        public string v_NewExtention { get; set; }

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
        [PropertyIsOptional(true)]
        [PropertyValidationRule("", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
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

                    var newExtension = v_NewExtention.ConvertToUserVariable(engine);
                    if (!newExtension.StartsWith("."))
                    {
                        newExtension = "." + newExtension;
                    }

                    var newFileOption = this.GetUISelectionValue(nameof(v_ExtentionOption), engine);
                    switch (newFileOption)
                    {
                        case "auto":
                            if (!Path.HasExtension(newFileName))
                            {
                                if (newExtension == ".")
                                {
                                    newFileName += Path.GetExtension(currentFileName);
                                }
                                else
                                {
                                    newFileName += newExtension;
                                }
                            }
                            break;
                        case "force combine new extension":
                            newFileName += newExtension;
                            break;
                        case "contains new file name":
                            // nothing to do
                            break;
                        case "use before rename path":
                            newFileName += Path.GetExtension(currentFileName);
                            break;
                    }

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

        private void cmbExtensionOption_SelectionChange(object sender, EventArgs e)
        {
            var cmb = (System.Windows.Forms.ComboBox)sender;
            ControlsList.SecondLabelProcess(nameof(v_ExtentionOption), nameof(v_ExtentionOption), cmb.SelectedItem.ToString());
        }
    }
}