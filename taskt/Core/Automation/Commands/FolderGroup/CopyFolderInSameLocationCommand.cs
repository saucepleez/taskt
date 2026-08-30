using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Folder Operation")]
    [Attributes.ClassAttributes.CommandSettings("Copy Folder In Same Location")]
    [Attributes.ClassAttributes.Description("This command Copy Folder in Same Location Path.")]
    [Attributes.ClassAttributes.UsesDescription("This command Copy Folder in Same Location Path.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_files))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class CopyFolderInSamLocationCommand : AFolderCopySameRenameFolderCommands, IFolderCopyFolderProperties
    {
        //[XmlAttribute]
        //public string v_TargetFolderPath { get; set; }

        //[XmlAttribute]
        //public string v_NewFolderName { get; set; }

        //[XmlAttribute]
        //public string v_WhenFolderNameSame { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Copy SubFolders")]
        [PropertyIsOptional(true, "Yes")]
        [PropertyParameterOrder(5300)]
        public string v_CopySubFolder { get; set; }

        //[XmlAttribute]
        //public string v_WaitTimeForFolder { get; set; }

        //[XmlAttribute]
        //public string v_BeforeFolderPathResult { get; set; }

        //[XmlAttribute]
        //public string v_AfterFolderPathResult { get; set; }

        public CopyFolderInSamLocationCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.FolderAction(engine,
                this.CreateActionFunc(
                    this.CreateFolderCopyAction(engine)
                , engine)
            );
        }
    }
}
