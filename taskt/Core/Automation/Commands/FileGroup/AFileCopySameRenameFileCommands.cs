using System;
using System.Xml.Serialization;
using System.IO;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for copy in same folder/rename file commands
    /// </summary>
    public abstract class AFileCopySameRenameFileCommands : AFileExistsFilePathBeforeAfterResultCommands, ICanHandleFileName
    {
        //[XmlAttribute]
        //public string v_TargetFilePath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("New File Name")]
        [InputSpecification("New File Name", true)]
        [PropertyDetailSampleUsage("**newfile**", PropertyDetailSampleUsage.ValueType.Value, "File Name")]
        [PropertyDetailSampleUsage("**{{{vNewFileName}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "File Name")]
        [PropertyValidationRule("New FileName", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "New FileName")]
        [PropertyParameterOrder(6000)]
        public virtual string v_NewFileName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Extension Option")]
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
        [PropertyDisplayText(false, "")]
        [PropertySelectionChangeEvent(nameof(cmbExtensionOption_SelectionChange))]
        [PropertyParameterOrder(7000)]
        public virtual string v_ExtensionOption { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("New File Extension")]
        [InputSpecification("New File Extension", true)]
        [PropertyDetailSampleUsage("**txt**", PropertyDetailSampleUsage.ValueType.Value, "Extension")]
        [PropertyDetailSampleUsage("**{{{vExtension}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Extension")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("New Extension", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(8000)]
        public virtual string v_NewExtension { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("When File Name Same After the Change")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyDetailSampleUsage("**Ignore**", "Nothing to do")]
        [PropertyDetailSampleUsage("**Error**", "Rise a Error")]
        [PropertyIsOptional(true, "Ignore")]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(9000)]
        public virtual string v_WhenFileNameSame { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("When New File Is Exists")]
        [PropertyUISelectionOption("Error")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Delete")]
        [PropertyUISelectionOption("Delete To Recycle Bin")]
        [PropertyDetailSampleUsage("**Ignore**", "Nothing to do")]
        [PropertyDetailSampleUsage("**Error**", "Rise a Error")]
        [PropertyDetailSampleUsage("**Delete**", "Delete File")]
        [PropertyDetailSampleUsage("**Delete To Recycle Bin**", "Delete the File to Recycle Bin and Copy")]
        [PropertyIsOptional(true, "Error")]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(9000)]
        public virtual string v_WhenNewFileExists { get; set; }

        //[XmlAttribute]
        //public string v_WaitTimeForFile { get; set; }

        //[XmlAttribute]
        //public string v_BeforeFilePathResult { get; set; }

        //[XmlAttribute]
        //public string v_AfterFilePathResult { get; set; }

        /// <summary>
        /// create action func
        /// </summary>
        /// <param name="processFunc">(targetPath, destinationPath)</param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected Func<string, string> CreateActionFunc(Action<string, string> processFunc, Engine.AutomationEngineInstance engine)
        {
            return new Func<string, string>(path =>
            {
                var currentFileName = Path.GetFileName(path);
                var newFileName = this.ExpandValueOrUserVariableAsFileName(nameof(v_NewFileName), engine);

                var newExtension = v_NewExtension.ExpandValueOrUserVariable(engine);
                if (!newExtension.StartsWith("."))
                {
                    newExtension = "." + newExtension;
                }

                var newFileOption = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ExtensionOption), engine);
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

                // get source file name and info
                var sourceFileInfo = new FileInfo(path);

                // create destination
                var destinationPath = Path.Combine(sourceFileInfo.DirectoryName, newFileName);

                var whenSame = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenFileNameSame), engine);
                if (EM_CanHandleFileOrFolderPathExtensionMethods.IsSamePath(path, destinationPath))
                {
                    switch (whenSame)
                    {
                        case "ignore":
                            return destinationPath;

                        case "error":
                            throw new Exception($"File Name before and after the changes is same. Name '{newFileName}'");
                    }
                }

                // check new file exists
                if (File.Exists(destinationPath))
                {
                    void RunDeleteFile(bool recycleBin)
                    {
                        var delFile = new DeleteFileCommand()
                        {
                            v_TargetFilePath = destinationPath,
                            v_MoveToRecycleBin = (recycleBin) ? "Yes" : "No",
                        };
                        delFile.RunCommand(engine);
                    }

                    switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenNewFileExists), engine))
                    {
                        case "error":
                            throw new Exception($"File is already Exists. Path: '{destinationPath}'");
                        case "ignore":
                            return destinationPath;
                        case "delete":
                            RunDeleteFile(false);
                            break;
                        case "delete to recycle bin":
                            RunDeleteFile(true);
                            break;
                    }
                }

                // action
                processFunc(path, destinationPath);

                return destinationPath;
            });
        }

        protected void cmbExtensionOption_SelectionChange(object sender, EventArgs e)
        {
            var cmb = (System.Windows.Forms.ComboBox)sender;
            ControlsList.SecondLabelProcess(nameof(v_ExtensionOption), nameof(v_ExtensionOption), cmb.SelectedItem.ToString());
        }
    }
}
