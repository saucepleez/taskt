using System;
using System.Xml.Serialization;
using System.IO;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    public abstract class AFolderCopySameRenameFolderCommands : AFolderExistsFolderBeforeAfterResultCommands, ICanHandleFolderName
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        //public string v_TargetFolderPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("New Folder Name")]
        [InputSpecification("New Folder Name", true)]
        [PropertyDetailSampleUsage("**myFolder2**", PropertyDetailSampleUsage.ValueType.Value, "New Folder")]
        [PropertyDetailSampleUsage("**{{{vNewFolder}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "New Folder")]
        [PropertyValidationRule("New Folder", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "New Folder")]
        [PropertyParameterOrder(5100)]
        public string v_NewFolderName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("When Folder Name Same After the Change")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyDetailSampleUsage("**Ignore**", "Nothing to do")]
        [PropertyDetailSampleUsage("**Error**", "Rise a Error")]
        [PropertyIsOptional(true, "Ignore")]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(5200)]
        public string v_WhenFolderNameSame { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("When Destination Folder Is Already Exists")]
        [PropertyUISelectionOption("Error")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Delete")]
        [PropertyUISelectionOption("Delete To Recycle Bin")]
        [PropertyDetailSampleUsage("**Error**", "Rise an Error")]
        [PropertyDetailSampleUsage("**Ignore**", "Nothing to do")]
        [PropertyDetailSampleUsage("**Delete**", "Delete the Folder")]
        [PropertyDetailSampleUsage("**Delete To Recycle Bin**", "Delete the Folder to Recycle Bin")]
        [PropertyIsOptional(true, "Error")]
        [PropertyParameterOrder(5400)]
        public virtual string v_WhenDestinationExists { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_WaitTime))]
        //public string v_WaitTimeForFolder { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPathResult))]
        //[PropertyDescription("Variable Name to Store Folder Path Before Rename")]
        //public string v_BeforeFolderPathResult { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        //[PropertyDescription("Variable Name to Store Folder Path After Rename")]
        //[PropertyIsOptional(true)]
        //[PropertyValidationRule("", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(false, "")]
        //public string v_AfterFolderPathResult { get; set; }

        /// <summary>
        /// create action func
        /// </summary>
        /// <param name="processFunc"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected Func<string, string> CreateActionFunc(Action<string, string> processFunc, Engine.AutomationEngineInstance engine)
        {
            return new Func<string, string>(path =>
            {
                var newFolderName = this.ExpandValueOrUserVariableAsFolderName(nameof(v_NewFolderName), engine);

                // get source folder name and info
                var sourceFolderInfo = new DirectoryInfo(path);

                // create destination
                var destinationPath = Path.Combine(sourceFolderInfo.Parent.FullName, newFolderName);

                // check path is same
                if (EM_CanHandleFileOrFolderPathExtensionMethods.IsSamePath(path, destinationPath))
                {
                    switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenFolderNameSame), engine))
                    {
                        case "ignore":
                            return path;

                        case "error":
                            throw new Exception($"Folder Name before and after the changes is same. Name '{newFolderName}'");
                    }
                }

                // if it already exists
                if (Directory.Exists(destinationPath))
                {
                    void RunDeleteFolder(bool recycleBin)
                    {
                        var delFile = new DeleteFolderCommand()
                        {
                            v_TargetFolderPath = destinationPath,
                            v_MoveToRecycleBin = (recycleBin) ? "Yes" : "No",
                        };
                        delFile.RunCommand(engine);
                    }

                    switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenDestinationExists), engine))
                    {
                        case "error":
                            throw new Exception($"Destination Folder is Already Exists. Path: '{destinationPath}'");

                        case "ignore":
                            // nothing todo
                            return destinationPath;

                        case "delete":
                            RunDeleteFolder(false);
                            break;

                        case "delete to recycle bin":
                            RunDeleteFolder(true);
                            break;
                    }
                }

                // action
                processFunc(path, destinationPath);

                return destinationPath;
            });
        }
    }
}
