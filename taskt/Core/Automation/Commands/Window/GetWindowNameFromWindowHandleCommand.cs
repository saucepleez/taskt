using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Get From Window Handle")]
    [Attributes.ClassAttributes.CommandSettings("Get Window Name From Window Handle")]
    [Attributes.ClassAttributes.Description("This command returns window names.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want window names.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetWindowNameFromWindowHandleCommand : AWindowHandleCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_InputWindowHandle))]
        //public string v_WindowHandle { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Window Name")]
        [PropertyParameterOrder(5500)]
        public string v_Result { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WaitTime))]
        //[PropertyIsOptional(true, "0")]
        //[PropertyFirstValue("0")]
        //[PropertyValidationRule("WaitTime", PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WaitTime))]
        public override string v_WaitTimeForWindow { get; set; }



        public GetWindowNameFromWindowHandleCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //WindowControls.WindowHandleAction(this, engine,
            //    new Action<IntPtr>(whnd =>
            //    {
            //        var n = WindowControls.GetWindowTitle(whnd);
            //        n.StoreInUserVariable(engine, v_Result);
            //    })
            //);

            //var whnd = this.GetWindowHandle(engine);
            //int titleLengthA = GetWindowTextLengthW(whnd);
            //StringBuilder title = new StringBuilder(titleLengthA + 1);
            //GetWindowTextW(whnd, title, title.Capacity);
            //title.ToString().StoreInUserVariable(engine, v_Result);

            this.GetWindowHandle(engine, new Action<IntPtr>((whnd) =>
            {
                v_WindowNameResult.StoreInUserVariable(engine, v_Result);
            }));
        }
    }
}