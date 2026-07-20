using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Folder Operation")]
    [Attributes.ClassAttributes.CommandSettings("Remove Invalid Folder Name Charactors")]
    [Attributes.ClassAttributes.Description("This command removes Invalid Folder name charactors from specified text")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to removes Invalid Folder name charactors from specified text")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_files))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class RemoveInvalidFolderNameCharactorsCommand : AFileRemoveInvalidFileNameCharactorsCommands
    {
        [XmlAttribute]
        [PropertyDescription("Folder Name")]
        public override string v_TargetName { get; set; }

        public RemoveInvalidFolderNameCharactorsCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //this.RemoveInvalidCharactors(engine);
            // same Remove Invalid File Name Charactors command
            var rmv = new RemoveInvalidFileNameCharactorsCommand()
            {
                v_TargetName = this.v_TargetName,
                v_Result = this.v_Result,
            };
            rmv.RunCommand(engine);
        }
    }
}