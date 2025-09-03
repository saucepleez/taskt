using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
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

        /// <summary>
        /// get process id from window handle
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpdwProcessId"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        ///// <summary>
        ///// open process from pid
        ///// </summary>
        ///// <param name="processAccess"></param>
        ///// <param name="bInheritHandle"></param>
        ///// <param name="processId"></param>
        ///// <returns></returns>
        //[DllImport("kernel32.dll")]
        //public static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, uint processId);

        ///// <summary>
        ///// https://www.pinvoke.net/default.aspx/kernel32.openprocess
        ///// </summary>
        //private const uint AccessMode = 0x0410; // 0x0400 | 0x0010

        ///// <summary>
        ///// get process name
        ///// </summary>
        ///// <param name="hProcess"></param>
        ///// <param name="hModule"></param>
        ///// <param name="lpBaseName"></param>
        ///// <param name="nSize"></param>
        ///// <returns></returns>
        //[DllImport("psapi.dll", CharSet = CharSet.Ansi)]
        //static extern uint GetModuleBaseName(IntPtr hProcess, IntPtr hModule, [MarshalAs(UnmanagedType.LPStr), Out] StringBuilder lpBaseName, uint nSize);

        ///// <summary>
        ///// close handle
        ///// </summary>
        ///// <param name="handle"></param>
        ///// <returns></returns>
        //[DllImport("kernel32.dll")]
        //private static extern bool CloseHandle(IntPtr handle);

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

            this.WindowHandleAction(engine, new Action<IntPtr>((whnd) =>
            {
                //var ps = Process.GetProcesses();
                //// DBG
                //foreach(var p in ps)
                //{
                //    Console.WriteLine(p.MainWindowHandle);
                //}
                //var proc = ps.Where(p => (p.MainWindowHandle == whnd)).First();
                ////var proc = Process.GetProcesses().Where(p => (p.MainWindowHandle == whnd)).First();
                //proc.ProcessName.StoreInUserVariable(engine, v_Result);

                GetWindowThreadProcessId(whnd, out uint pid);
                if (pid != 0)
                {
                    var p = Process.GetProcessById((int)pid);
                    p.ProcessName.StoreInUserVariable(engine, v_Result);
                }
                else
                {
                    throw new Exception($"Error. Can not find Process Name. Handle: '{v_WindowHandle}', Expand Value: '{whnd}'");
                }
            }));
        }
    }
}