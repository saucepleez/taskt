using System;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Get From Window Handle")]
    [Attributes.ClassAttributes.CommandSettings("Get Process Name From Window Handle")]
    [Attributes.ClassAttributes.Description("This command returns process name.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get process name.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetProcessNameFromWindowHandleCommand : AWindowHandleCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_InputWindowHandle))]
        //public string v_WindowHandle { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Process Name")]
        [PropertyParameterOrder(5500)]
        public string v_Result { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WaitTime))]
        //[PropertyIsOptional(true, "0")]
        //[PropertyFirstValue("0")]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WaitTime))]
        public override string v_WaitTimeForWindow { get; set; }

        //public string v_WindowTitleResult {get;set;}

        public GetProcessNameFromWindowHandleCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //WindowControls.WindowHandleAction(this, engine,
            //    new Action<IntPtr>(whnd =>
            //    {
            //        var proc = Process.GetProcesses().Where(p => (p.MainWindowHandle == whnd)).First();
            //        proc.ProcessName.StoreInUserVariable(engine, v_Result);
            //    })
            //);
            //var whnd = this.GetWindowHandle(engine);
            //var proc = Process.GetProcesses().Where(p => (p.MainWindowHandle == whnd)).First();
            //proc.ProcessName.StoreInUserVariable(engine, v_Result);

            this.GetWindowHandle(engine, new Action<IntPtr>((whnd) =>
            {
                var proc = Process.GetProcesses().Where(p => (p.MainWindowHandle == whnd)).First();
                proc.ProcessName.StoreInUserVariable(engine, v_Result);
            }));
        }
    }
}