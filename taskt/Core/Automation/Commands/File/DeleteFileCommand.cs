using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation Commands")]
    [Attributes.ClassAttributes.CommandSettings("Delete File")]
    [Attributes.ClassAttributes.Description("This command deletes a file from a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to detete a file from a specific location.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class DeleteFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.AllowNoExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport)]
        public string v_SourceFilePath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WhenFileDoesNotExists))]
        public string v_WhenFileDoesNotExists { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionControls), nameof(SelectionControls.v_YesNoComboBox))]
        [PropertyDescription("File Move to the Recycle Bin")]
        [PropertyIsOptional(true, "No")]
        public string v_MoveToRecycleBin { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        public string v_WaitTime { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePathResult))]
        public string v_ResultPath { get; set; }

        public DeleteFileCommand()
        {
            //this.CommandName = "DeleteFileCommand";
            //this.SelectionName = "Delete File";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //try
            //{
            //    var targetFile = FilePathControls.WaitForFile(this, nameof(v_SourceFilePath), nameof(v_WaitTime), engine);

            //    if (this.GetYesNoSelectionValue(nameof(v_MoveToRecycleBin), engine))
            //    {
            //        Shell32.MoveToRecycleBin(targetFile);
            //    }
            //    else
            //    {
            //        System.IO.File.Delete(targetFile);
            //    }
            //}
            //catch
            //{
            //    if (this.GetUISelectionValue(nameof(v_WhenFileDoesNotExists), engine) == "error")
            //    {
            //        throw new Exception("File does Not Exists. File Path: '" + v_SourceFilePath + "'");
            //    }
            //}

            FilePathControls.FileAction(this, engine,
                new Action<string>(path =>
                {
                    if (this.GetYesNoSelectionValue(nameof(v_MoveToRecycleBin), engine))
                    {
                        Shell32.MoveToRecycleBin(path);
                    }
                    else
                    {
                        System.IO.File.Delete(path);
                    }
                }),
                new Action<Exception>(ex =>
                {
                    if (this.GetUISelectionValue(nameof(v_WhenFileDoesNotExists), engine) == "error")
                    {
                        throw new Exception("File does Not Exists. File Path: '" + v_SourceFilePath + "'");
                    }
                })
            );
        }
    }
}