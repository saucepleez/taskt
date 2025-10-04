using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Folder Operation")]
    [Attributes.ClassAttributes.CommandSettings("Delete Folder")]
    [Attributes.ClassAttributes.Description("This command deletes a folder from a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to delete a folder from a specific location.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_files))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class DeleteFolderCommand : AFolderExistsFolderPathPathResultCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        //public string v_TargetFolderPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Folder Move to the Recycle Bin")]
        [PropertyIsOptional(true, "No")]
        [PropertyParameterOrder(6000)]
        public string v_MoveToRecycleBin { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_WaitTime))]
        //public string v_WaitTimeForFolder { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPathResult))]
        //public string v_ResultPath { get; set; }

        public DeleteFolderCommand()
        {
            //this.CommandName = "DeleteFolderCommand";
            //this.SelectionName = "Delete Folder";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            ////apply variable logic
            //var sourceFolder = FolderPathControls.WaitForFolder(this, nameof(v_SourceFolderPath), nameof(v_WaitForFolder), engine);

            ////delete folder
            //if (this.GetYesNoSelectionValue(nameof(v_MoveToRecycleBin), engine))
            //{
            //    Shell32.MoveToRecycleBin(sourceFolder);
            //}
            //else
            //{
            //    System.IO.Directory.Delete(sourceFolder, true);
            //}

            //FolderPathControls.FolderAction(this, engine,
            //    new Action<string>(path =>
            //    {
            //        //delete folder
            //        if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_MoveToRecycleBin), engine))
            //        {
            //            Shell32.MoveToRecycleBin(path);
            //        }
            //        else
            //        {
            //            System.IO.Directory.Delete(path, true);
            //        }
            //    })
            //);

            this.FolderAction(engine,
                new Func<string, string>(path =>
                {
                    // delete folder
                    if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_MoveToRecycleBin), engine))
                    {
                        Shell32.MoveToRecycleBin(path);
                    }
                    else
                    {
                        System.IO.Directory.Delete(path, true);
                    }
                    return path;
                })
            );
        }
    }
}