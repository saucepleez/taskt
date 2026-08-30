using System;
using System.Xml.Serialization;
using System.IO;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for copy/move folder commands
    /// </summary>
    public abstract class AFolderCopyMoveFolderCommands : AFolderExistsFolderBeforeAfterResultCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        //[PropertyDescription("Target Folder")]
        //[PropertyValidationRule("Target Folder", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Target Folder")]
        //public string v_TargetFolderPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        [PropertyDescription("Destination Folder for Action")]
        [PropertyValidationRule("Destination Folder", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Destination Folder")]
        [PropertyParameterOrder(5100)]
        public virtual string v_DestinationFolderPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Create Folder when the Destination Folder does not Exists")]
        [PropertyIsOptional(true, "No")]
        [PropertyParameterOrder(5200)]
        public virtual string v_CreateDirectory { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("When Description Folder Path is Same as Target Folder Path")]
        [PropertyUISelectionOption("Error")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyShowSampleUsageInDescription(false)]
        [PropertyDetailSampleUsage("**Error**", "Rise an Error")]
        [PropertyDetailSampleUsage("**Ignore**", "Do Nothing and move to Next Process")]
        [PropertyIsOptional(true, "Error")]
        [PropertyValidationRule("When Path Is Same", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(5300)]
        public virtual string v_WhenDestinationIsSame { get; set; }

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
        //[PropertyDescription("Wait Time for the Target Folder to Exist (sec)")]
        //[PropertyDisplayText(false, "")]
        //public string v_WaitTimeForFolder { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPathResult))]
        //[PropertyDescription("Variable Name to Store Folder Path Before Copy")]
        //public string v_BeforeFolderPathResult { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePathResult))]
        //[PropertyDescription("Variable Name to Store Folder Path After Copy")]
        //public string v_AfterFolderPathResult { get; set; }

        /// <summary>
        /// create action func for copy/move folder
        /// </summary>
        /// <param name="processFunc">(source, destination)</param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected Func<string, string> CreateActionFunc(Action<string, string> processFunc, Engine.AutomationEngineInstance engine)
        {
            return new Func<string, string>(path =>
            {
                var destinationFolder = this.ExpandValueOrUserVariableAsFolderPath(nameof(v_DestinationFolderPath), engine);
                using (var checkExists = new InnerScriptVariable(engine))
                {
                    var checkFolder = new CheckFolderExistsCommand()
                    {
                        v_TargetFolderPath = this.v_DestinationFolderPath,
                        v_WaitTimeForFolder = this.v_WaitTimeForFolder,
                        v_Result = checkExists.VariableName,
                    };
                    checkFolder.RunCommand(engine);
                    if (!bool.Parse(checkExists.VariableValue.ToString()))
                    {
                        if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_CreateDirectory), engine))
                        {
                            Directory.CreateDirectory(destinationFolder);
                        }
                        else
                        {
                            throw new Exception($"Destination Folder does not Exists. Folder Path: '{v_DestinationFolderPath}', Expand Folder Path: '{destinationFolder}'");
                        }
                    }
                }

                // get source folder name and info
                var sourceFolderInfo = new DirectoryInfo(path);

                // create final path
                var newFolderPath = Path.Combine(destinationFolder, sourceFolderInfo.Name);

                if (EM_CanHandleFileOrFolderPathExtensionMethods.IsSamePath(path, newFolderPath) ||
                    EM_CanHandleFileOrFolderPathExtensionMethods.IsSubItem(newFolderPath, path))
                {
                    switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenDestinationIsSame), engine))
                    {
                        case "error":
                            throw new Exception($"Target Folder Path and Destination Path are Same. Path: '{path}'");

                        case "ignore":
                            return path;
                    }
                }

                // if it already exists
                if (Directory.Exists(newFolderPath))
                {
                    void RunDeleteFolder(bool recycleBin)
                    {
                        var delFile = new DeleteFolderCommand()
                        {
                            v_TargetFolderPath = newFolderPath,
                            v_MoveToRecycleBin = (recycleBin) ? "Yes" : "No",
                        };
                        delFile.RunCommand(engine);
                    }

                    switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenDestinationExists), engine))
                    {
                        case "error":
                            throw new Exception($"Destination Folder is Already Exists. Path: '{newFolderPath}'");

                        case "ignore":
                            // nothing todo
                            return newFolderPath;

                        case "delete":
                            RunDeleteFolder(false);
                            break;

                        case "delete to recycle bin":
                            RunDeleteFolder(true);
                            break;
                    }
                }

                processFunc(path, newFolderPath);

                return newFolderPath;
            });
        }
    }
}
