using System;
using System.Runtime.InteropServices;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Window Handle Actions")]
    [Attributes.ClassAttributes.CommandSettings("Close Window By Window Handle")]
    [Attributes.ClassAttributes.Description("This command closes an open window.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to close an existing window by name.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window_close))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class CloseWindowByWindowHandle : AWindowHandleActionBaseCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_InputWindowHandle))]
        //public string v_WindowHandle { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WaitTime))]
        //public string v_WaitTime { get; set; }

        //public string v_WindowTitleResult {get;set;}

        /// <summary>
        /// for close window by whnd
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// close value
        /// </summary>
        private static readonly UInt32 WM_CLOSE = 0x0010;

        public CloseWindowByWindowHandle()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //WindowControls.WindowHandleAction(this, engine, 
            //    new Action<IntPtr>(whnd =>
            //    {
            //        WindowControls.CloseWindow(whnd);
            //    })
            //);
            //var whnd = this.GetWindowHandle(engine);
            //SendMessage(whnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

            this.WindowHandleActionBeforeWait(engine, new Action<IntPtr>((whnd) =>
            {
                SendMessage(whnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }));
        }
    }
}