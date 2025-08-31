using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Window Actions")]
    [Attributes.ClassAttributes.CommandSettings("Resize Windows")]
    [Attributes.ClassAttributes.Description("This command resizes windows.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to reize windows.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ResizeWindowsCommand : AWindowNamesActionCommnads, IWindowSizeProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_InputWidth))]
        [PropertyParameterOrder(5500)]
        public string v_Width { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_InputHeight))]
        [PropertyParameterOrder(5500)]
        public string v_Height { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WhenWindowIsMinimizedForSet))]
        [PropertyParameterOrder(9000)]
        public string v_WhenWindowIsMinimized { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WhenWindowIsMinimizedForSet))]
        [PropertyParameterOrder(9001)]
        public string v_WhenWindowIsMaximized { get; set; }

        public ResizeWindowsCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WindowNamesAction(engine, new Action<List<(IntPtr, string)>>(wins =>
            {
                foreach ((var whnd, _) in wins)
                {
                    var resizeWindow = new ResizeWindowByWindowHandleCommand()
                    {
                        v_WindowHandle = whnd.ToString(),
                        v_Width = this.v_Width,
                        v_Height = this.v_Height,
                        v_WhenWindowIsMaximized = this.v_WhenWindowIsMaximized,
                        v_WhenWindowIsMinimized = this.v_WhenWindowIsMinimized,
                        v_WaitTimeBetweenFindAndAction = this.v_WaitTimeBetweenFindAndAction,
                    };
                    resizeWindow.RunCommand(engine);
                }
            }));
        }
    }
}