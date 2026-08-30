using System;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation")]
    [Attributes.ClassAttributes.CommandSettings("Remove Invalid File Name Charactors")]
    [Attributes.ClassAttributes.Description("This command removes Invalid File name charactors from specified text")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to removes Invalid File name charactors from specified text")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_files))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class RemoveInvalidFileNameCharactorsCommand : AFileRemoveInvalidFileNameCharactorsCommands
    {
        public RemoveInvalidFileNameCharactorsCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.RemoveInvalidCharactors(engine);
        }
    }
}