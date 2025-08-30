using System;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("One Window Name Actions")]
    [Attributes.ClassAttributes.CommandSettings("Activate One Winodw")]
    [Attributes.ClassAttributes.Description("This command Activate one Window.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Activate one Windown.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ActivateOneWindowCommand : AOneWindowNameCommands
    {
        public ActivateOneWindowCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WindowNameAction(engine, new Action<IntPtr, string>((whnd, name) =>
            {
                var activateCommand = new ActivateWindowByWindowHandleCommand()
                {
                    v_WindowHandle = whnd.ToString(),
                };
                activateCommand.RunCommand(engine);
            }));
        }
    }
}
