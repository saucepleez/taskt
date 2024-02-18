using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.SubGruop("Get From Window Handle")]
    [Attributes.ClassAttributes.CommandSettings("Get Window Size From Window Handle")]
    [Attributes.ClassAttributes.Description("This command returns window size.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want window size.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetWindowSizeFromWindowHandleCommand : AWindowHandleCommands, IWindowSizeProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_InputWindowHandle))]
        //public string v_WindowHandle { get; set; }


        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Recieve the Window Width")]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(5500)]
        public string v_Width { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Recieve the Window Height")]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(5500)]
        public string v_Height { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WaitTime))]
        //public string v_WaitTime { get; set; }

        public GetWindowSizeFromWindowHandleCommand()
        {
        }
        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            WindowControls.WindowHandleAction(this, engine,
                new Action<IntPtr>(whnd =>
                {
                    var rct = WindowControls.GetWindowRect(whnd);

                    if (!string.IsNullOrEmpty(v_Width))
                    {
                        (rct.right - rct.left).StoreInUserVariable(engine, v_Width);
                    }
                    if (!string.IsNullOrEmpty(v_Height))
                    {
                        (rct.bottom - rct.top).StoreInUserVariable(engine, v_Height);
                    }
                })
            );
        }
    }
}