using System;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("One Window Name Actions")]
    [Attributes.ClassAttributes.CommandSettings("Close One Window")]
    [Attributes.ClassAttributes.Description("This command Close one Window.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Close one Windown.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class CloseOneWindowCommand : AOneWindowNameActionCommands
    {
        public CloseOneWindowCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WindowNameActionAndWaitActivate(engine, new Action<IntPtr, string>((whnd, name) =>
            {
                var closeWindow = new CloseWindowByWindowHandle()
                {
                    v_WindowHandle = whnd.ToString(),
                    v_WaitTimeBetweenFindAndAction = this.v_WaitTimeBetweenFindAndAction,
                };
                closeWindow.RunCommand(engine);
            }));
        }
    }
}
