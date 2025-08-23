using System;
using System.Runtime.InteropServices;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Window Handle Actions")]
    [Attributes.ClassAttributes.CommandSettings("Activate Window By Window Handle")]
    [Attributes.ClassAttributes.Description("This command activates a window and brings it to the front.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to active a window by name or bring it to attention.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ActivateWindowByWindowHandleCommand : AWindowHandleCommands
    {
        /// <summary>
        /// set window state
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nCmdShow"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        private static extern bool SetWindowState(IntPtr hWnd, int nCmdShow);

        /// <summary>
        /// set window is ForeGround
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        private static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// window state is normal
        /// </summary>
        private const int WINDOW_NORMAL = 1;

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_InputWindowHandle))]
        //public string v_WindowHandle { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WaitTime))]
        //public string v_WaitTime { get; set; }

        //public string v_WindowTitleResult {get;set;}

        public ActivateWindowByWindowHandleCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var whnd = this.GetWindowHandle(engine);
            //if (IsIconic(whnd)) 
            //{
            //    SetWindowState(whnd, WINDOW_NORMAL);
            //}
            //SetForegroundWindow(whnd);
            this.GetWindowHandle(engine, new Action<IntPtr>((whnd) =>
            {
                if (EM_CanHandleWindowHandleExtentionMethods.IsWindowMinimized(whnd))
                {
                    SetWindowState(whnd, WINDOW_NORMAL);
                }
                SetForegroundWindow(whnd);
            }));
        }
    }
}
