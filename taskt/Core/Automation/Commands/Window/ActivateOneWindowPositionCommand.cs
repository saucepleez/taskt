using System;
using taskt.Core.Automation.Commands.Window;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Window Actions")]
    [Attributes.ClassAttributes.CommandSettings("Activate One Winodw")]
    [Attributes.ClassAttributes.Description("This command Activate one Window.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Activate one Windown.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ActivateOneWindowCommand : AWindowNameActionCommands
    {
        public ActivateOneWindowCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WindowNameActionAndWait(engine, new Action<IntPtr, string>((whnd, name) =>
            {
                var activateCommand = new ActivateWindowByWindowHandleCommand()
                {
                    v_WindowHandle = whnd.ToString(),
                    v_WaitTimeForWindow = this.v_WaitTimeForWindow,
                };
                activateCommand.RunCommand(engine);
            }));
        }
    }
}
