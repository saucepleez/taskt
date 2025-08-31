using System;
using System.Collections.Generic;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Window Actions")]
    [Attributes.ClassAttributes.CommandSettings("Activate Windows")]
    [Attributes.ClassAttributes.Description("This command activates windows")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to active a windows by name")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ActivateWindowsCommand : AWindowNamesCommands
    {
        public ActivateWindowsCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WindowNamesAction(engine, new Action<List<(IntPtr, string)>>(wins =>
            {
                foreach((var whnd, _) in wins)
                {
                    var activateWindow = new ActivateWindowByWindowHandleCommand()
                    {
                        v_WindowHandle = whnd.ToString(),
                    };
                    activateWindow.RunCommand(engine);
                }
            }));
        }
    }
}
