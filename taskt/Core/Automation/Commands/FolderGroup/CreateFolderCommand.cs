using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Folder Operation")]
    [Attributes.ClassAttributes.CommandSettings("Create Folder")]
    [Attributes.ClassAttributes.Description("This command creates a folder in a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to create a folder in a specific location.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_files))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class CreateFolderCommand : AFolderExistsFolderPathPathResultCommands, ICanHandleFolderName
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Name of the New Folder")]
        [InputSpecification("Name of the New Folder", true)]
        //[SampleUsage("**myFolderName** or **{{{vFolderName}}}**")]
        [PropertyDetailSampleUsage("**myFolder**", PropertyDetailSampleUsage.ValueType.Value, "Folder Name")]
        [PropertyDetailSampleUsage("**{{{vFolderName}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Folder Name")]
        [PropertyValidationRule("New Folder Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "New Folder Name")]
        [PropertyParameterOrder(4000)]
        public string v_NewFolderName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        //[PropertyDescription("Location where you want to Create the Folder")]
        //public string v_TargetFolderPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("When Folder Exists")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Delete")]
        [PropertyUISelectionOption("Delete To Recycle Bin")]
        [PropertyUISelectionOption("Error")]
        [PropertyDetailSampleUsage("**Ignore**", "Nothing to do")]
        [PropertyDetailSampleUsage("**Delete**", "Delete Folder and Create Folder")]
        [PropertyDetailSampleUsage("**Delete To Recycle Bin**", "Delete Folder to Recycle Bin and Create Folder")]
        [PropertyDetailSampleUsage("**Error**", "Rise an Error")]
        [PropertyIsOptional(true, "Error")]
        [PropertyValidationRule("When Folder Exists", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyParameterOrder(5100)]
        public string v_WhenFolderExists { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_WaitTime))]
        //public string v_WaitTimeForFolder { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        //[PropertyDescription("Variable Name to Store Created Folder Path")]
        //[PropertyIsOptional(true)]
        //[PropertyValidationRule("", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(false, "")]
        //public string v_ResultPath { get; set; }

        public CreateFolderCommand()
        {
            //this.CommandName = "CreateFolderCommand";
            //this.SelectionName = "Create Folder";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            ////apply variable logic
            //var destinationDirectory = FolderPathControls.WaitForFolder(this, nameof(v_DestinationDirectory), nameof(v_WaitForFolder), engine);

            //var newFolder = v_NewFolderName.ConvertToUserVariableAsFolderName(engine);

            //var finalPath = System.IO.Path.Combine(destinationDirectory, newFolder);
            //if (System.IO.Directory.Exists(finalPath)) { }
            //{
            //    if (this.GetYesNoSelectionValue(nameof(v_DeleteExisting), engine))
            //    {
            //        System.IO.Directory.Delete(finalPath, true);
            //    }
            //}

            ////create folder if it doesn't exist
            //if (!System.IO.Directory.Exists(finalPath))
            //{
            //    System.IO.Directory.CreateDirectory(finalPath);
            //}

            //FolderPathControls.FolderAction(this, engine,
            //    new Action<string>(path =>
            //    {
            //        var newFolder = v_NewFolderName.ExpandValueOrUserVariableAsFolderName(engine);

            //        var finalPath = System.IO.Path.Combine(path, newFolder);
            //        if (System.IO.Directory.Exists(finalPath)) { }
            //        {
            //            if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_DeleteExisting), engine))
            //            {
            //                System.IO.Directory.Delete(finalPath, true);
            //            }
            //        }

            //        //create folder if it doesn't exist
            //        if (!System.IO.Directory.Exists(finalPath))
            //        {
            //            System.IO.Directory.CreateDirectory(finalPath);
            //        }

            //        if (!string.IsNullOrEmpty(v_ResultPath))
            //        {
            //            finalPath.StoreInUserVariable(engine, v_ResultPath);
            //        }
            //    })
            //);

            this.FolderAction(engine,
                new Func<string, string>(path =>
                {
                    var newFolderName = this.ExpandValueOrUserVariableAsFolderName(nameof(v_NewFolderName), engine);

                    // create folder path
                    var createdFolderPath = System.IO.Path.Combine(path, newFolderName);
                    if (System.IO.Directory.Exists(createdFolderPath))
                    {
                        //if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_DeleteExisting), engine))
                        //{
                        //    System.IO.Directory.Delete(createdFolderPath, true);
                        //}
                        void DeleteFolderProcess(string fPath, bool isRecycleBin)
                        {
                            var delFolder = new DeleteFolderCommand()
                            {
                                v_TargetFolderPath = fPath,
                                v_MoveToRecycleBin = (isRecycleBin) ? "Yes" : "No",
                            };
                            delFolder.RunCommand(engine);
                        }

                        switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenFolderExists), engine))
                        {
                            case "error":
                                throw new Exception($"Specified Folder is Already Exists. Path: '{createdFolderPath}'");

                            case "ignore":
                                break;

                            case "delete":
                                DeleteFolderProcess(createdFolderPath, false);
                                break;

                            case "delete to recycle bin":
                                DeleteFolderProcess(createdFolderPath, true);
                                break;
                        }
                    }

                    // create folder if it doesn't exist
                    if (!System.IO.Directory.Exists(createdFolderPath))
                    {
                        System.IO.Directory.CreateDirectory(createdFolderPath);
                    }

                    return createdFolderPath;
                })
            );
        }
    }
}
