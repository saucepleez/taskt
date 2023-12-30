using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.SubGruop("Get From Window Handle")]
    [Attributes.ClassAttributes.CommandSettings("Get Window State From Window Handle")]
    [Attributes.ClassAttributes.Description("This command returns a state of window name.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a window state.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetWindowStateFromWindowHandleCommand : AWindowHandleCommand
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_InputWindowHandle))]
        //public string v_WindowHandle { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [Remarks("Restore is **1**, Minimize is **2**, Maximize is **3**")]
        [PropertyParameterOrder(5500)]
        public string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WaitTime))]
        //public string v_WaitTime { get; set; }

        public GetWindowStateFromWindowHandleCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            WindowNameControls.WindowHandleAction(this, engine,
                new Action<IntPtr>(whnd =>
                {
                    var state = WindowNameControls.GetWindowState(whnd);
                    state.StoreInUserVariable(engine, v_Result);
                })
            );
        }
    }
}