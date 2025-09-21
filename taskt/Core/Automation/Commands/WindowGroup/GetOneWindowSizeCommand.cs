using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Get From Window Name")]
    [Attributes.ClassAttributes.CommandSettings("Get One Window Size")]
    [Attributes.ClassAttributes.Description("This command returns one window size.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want one window size.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetOneWindowSizeCommand : AOneWindowNameCommands, IWindowSizeProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Recieve the Window Width")]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(6500)]
        public string v_Width { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Recieve the Window Height")]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(6500)]
        public string v_Height { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WhenWindowIsMinimizedForGet))]
        [PropertyParameterOrder(9000)]
        public string v_WhenWindowIsMinimized { get; set; }

        public GetOneWindowSizeCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WindowNameAction(engine, new Action<IntPtr, string>((whnd, name) =>
            {
                var getSize = new GetWindowSizeFromWindowHandleCommand()
                {
                    v_WindowHandle = whnd.ToString(),
                    v_Width = this.v_Width,
                    v_Height = this.v_Height,
                    v_WhenWindowIsMinimized = this.v_WhenWindowIsMinimized,
                };
                getSize.RunCommand(engine);
            }));
        }
    }
}
