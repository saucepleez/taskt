using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.SubGruop("Window Handle Actions")]
    [Attributes.ClassAttributes.CommandSettings("Set Window State By Window Handle")]
    [Attributes.ClassAttributes.Description("This command sets a target window's state.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to change a window's state to minimized, maximized, or restored state")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SetWindowStateByWindowHandleCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_InputWindowHandle))]
        public string v_WindowHandle { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("State of the Window")]
        [PropertyUISelectionOption("Maximize")]
        [PropertyUISelectionOption("Minimize")]
        [PropertyUISelectionOption("Restore")]
        [InputSpecification("", true)]
        [PropertyValidationRule("Window State", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "State")]
        public string v_WindowState { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WaitTime))]
        public string v_WaitTime { get; set; }

        public SetWindowStateByWindowHandleCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            WindowNameControls.WindowHandleAction(this, engine,
                new Action<IntPtr>(whnd =>
                {
                    var windowState = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WindowState), engine);
                    var state = WindowNameControls.WindowState.SW_RESTORE;
                    switch (windowState.ToLower())
                    {
                        case "maximize":
                            state = WindowNameControls.WindowState.SW_MAXIMIZE;
                            break;
                        case "minimize":
                            state = WindowNameControls.WindowState.SW_MINIMIZE;
                            break;
                    }

                    if (WindowNameControls.IsIconic(whnd) && (state != WindowNameControls.WindowState.SW_MINIMIZE))
                    {
                        WindowNameControls.ShowIconicWindow(whnd);
                    }
                    WindowNameControls.SetWindowState(whnd, state);
                })
            );
        }
    }
}