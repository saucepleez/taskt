using System;
using System.Collections.Generic;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Multi Window Actions")]
    [Attributes.ClassAttributes.CommandSettings("Close Windows")]
    [Attributes.ClassAttributes.Description("This command closes windows.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to close an existing windows by name.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window_close))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class CloseWindowsCommand : AWindowNamesActionCommnads
    {
        public CloseWindowsCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WindowNamesAction(engine, new Action<List<(IntPtr, string)>>(wins =>
            {
                foreach ((var whnd, _) in wins)
                {
                    var closeWindow = new CloseWindowByWindowHandle()
                    {
                        v_WindowHandle = whnd.ToString(),
                        v_WaitTimeBetweenFindAndAction = this.v_WaitTimeBetweenFindAndAction,
                        v_ActivateBeforeAction = this.v_ActivateBeforeAction,
                    };
                    closeWindow.RunCommand(engine);
                }
            }));
        }
    }
}