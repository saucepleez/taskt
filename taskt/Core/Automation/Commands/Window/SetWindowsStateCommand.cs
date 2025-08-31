using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Window Actions")]
    [Attributes.ClassAttributes.CommandSettings("Set Windows State")]
    [Attributes.ClassAttributes.Description("This command sets a target windows state.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to change a windows state to minimized, maximized, or restored state")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SetWindowsStateCommand : AWindowNamesActionCommnads, IWindowStateProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WindowState))]
        [PropertyParameterOrder(6500)]
        public string v_WindowState { get; set; }

        public SetWindowsStateCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WindowNamesAction(engine, new Action<List<(IntPtr, string)>>(wins =>
            {
                foreach ((var whnd, _) in wins)
                {
                    var setState = new SetWindowStateByWindowHandleCommand()
                    {
                        v_WindowHandle = whnd.ToString(),
                        v_WindowState = this.v_WindowState,
                        v_WaitTimeBetweenFindAndAction = this.v_WaitTimeBetweenFindAndAction,
                    };
                    setState.RunCommand(engine);
                }
            }));
        }
    }
}