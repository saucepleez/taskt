using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("One Window Name Actions")]
    [Attributes.ClassAttributes.CommandSettings("Move One Winodw")]
    [Attributes.ClassAttributes.Description("This command Move one Window.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Move one Window.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class MoveOneWindowCommand : AOneWindowNameActionCommands, IWindowPositionProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_InputXPosition))]
        [PropertyParameterOrder(5500)]
        public string v_XPosition { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_InputYPosition))]
        [PropertyParameterOrder(5500)]
        public string v_YPosition { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WhenWindowIsMinimizedForSet))]
        [PropertyParameterOrder(9000)]
        public string v_WhenWindowIsMinimized { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WhenWindowIsMaximizedForSet))]
        [PropertyParameterOrder(9001)]
        public string v_WhenWindowIsMaximized { get; set; }

        public MoveOneWindowCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WindowNameActionAndWait(engine, new Action<IntPtr, string>((whnd, name) =>
            {
                var moveWindow = new MoveWindowByWindowHandleCommand()
                {
                    v_WindowHandle = whnd.ToString(),
                    v_XPosition = this.v_XPosition,
                    v_YPosition = this.v_YPosition,
                    v_WhenWindowIsMaximized = this.v_WhenWindowIsMaximized,
                    v_WhenWindowIsMinimized = this.v_WhenWindowIsMinimized,
                };
                moveWindow.RunCommand(engine);
            }));
        }
    }
}
