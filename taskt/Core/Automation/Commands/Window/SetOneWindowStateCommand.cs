using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("One Window Name Actions")]
    [Attributes.ClassAttributes.CommandSettings("Set One Window State")]
    [Attributes.ClassAttributes.Description("This command Set State one Window.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Set State one Window.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SetOneWindowStateCommand : AOneWindowNameActionCommands, IWindowStateProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WindowState))]
        [PropertyParameterOrder(6500)]
        public string v_WindowState { get; set; }

        public SetOneWindowStateCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WindowNameActionAndWait(engine, new Action<IntPtr, string>((whnd, name) =>
            {
                var setState = new SetWindowStateByWindowHandleCommand()
                {
                    v_WindowHandle = whnd.ToString(),
                    v_WindowState = this.v_WindowState,
                };
                setState.RunCommand(engine);
            }));
        }
    }
}
