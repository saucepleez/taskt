using System;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Get From Window Handle")]
    [Attributes.ClassAttributes.CommandSettings("Get Window State From Window Handle")]
    [Attributes.ClassAttributes.Description("This command returns a state of window name.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a window state.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetWindowStateFromWindowHandleCommand : AWindowHandleCommands, IWindowStateProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_InputWindowHandle))]
        //public string v_WindowHandle { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Window State Text")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Window State", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "State")]
        [Remarks("Restore is **1**, Minimize is **2**, Maximize is **3**")]
        [PropertyParameterOrder(5500)]
        public string v_WindowState { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Window State Text")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Window State Text", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "State Text")]
        [PropertyParameterOrder(5501)]
        public string v_WindowStateText { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WaitTime))]
        //public string v_WaitTime { get; set; }

        private struct WINDOWPLACEMENT
        {
            uint length;
            uint flags;
            public uint showCmd;
            System.Drawing.Point ptMinPosition;
            System.Drawing.Point ptMaxPosition;
            RECT rcNormalPosition;
            RECT rcDevice;
        }

        /// <summary>
        /// get window state
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpwndpl"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        public GetWindowStateFromWindowHandleCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //WindowControls.WindowHandleAction(this, engine,
            //    new Action<IntPtr>(whnd =>
            //    {
            //        var state = WindowControls.GetWindowState(whnd);
            //        state.StoreInUserVariable(engine, v_WindowState);
            //    })
            //);
            //var whnd = this.GetWindowHandle(engine);
            //var info = new WINDOWPLACEMENT();
            //GetWindowPlacement(whnd, ref info);

            //if (!string.IsNullOrEmpty(v_WindowState))
            //{
            //    ((int)info.showCmd).StoreInUserVariable(engine, v_WindowState);
            //}
            
            //if (!string.IsNullOrEmpty(v_WindowStateText))
            //{
            //    string txt;
            //    switch (info.showCmd)
            //    {
            //        case 1:
            //            txt = "Restore";
            //            break;
            //        case 2:
            //            txt = "Minimize";
            //            break;
            //        case 3:
            //            txt = "Maximize";
            //            break;
            //        default:
            //            txt = "Unknown";
            //            break;
            //    }
            //    txt.StoreInUserVariable(engine, v_WindowStateText);
            //}

            this.WindowHandleAction(engine, new Action<IntPtr>((whnd) =>
            {
                var info = new WINDOWPLACEMENT();
                GetWindowPlacement(whnd, ref info);

                if (!string.IsNullOrEmpty(v_WindowState))
                {
                    ((int)info.showCmd).StoreInUserVariable(engine, v_WindowState);
                }

                if (!string.IsNullOrEmpty(v_WindowStateText))
                {
                    string txt;
                    switch (info.showCmd)
                    {
                        case 1:
                            txt = "Restore";
                            break;
                        case 2:
                            txt = "Minimize";
                            break;
                        case 3:
                            txt = "Maximize";
                            break;
                        default:
                            txt = "Unknown";
                            break;
                    }
                    txt.StoreInUserVariable(engine, v_WindowStateText);
                }
            }));
        }
    }
}