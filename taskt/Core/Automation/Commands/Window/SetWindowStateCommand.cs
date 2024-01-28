using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.SubGruop("Window Actions")]
    [Attributes.ClassAttributes.CommandSettings("Set Window State")]
    [Attributes.ClassAttributes.Description("This command sets a target window's state.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to change a window's state to minimized, maximized, or restored state")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SetWindowStateCommand : AWindowNameCommand
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WindowName))]
        //public string v_WindowName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_CompareMethod))]
        //public string v_SearchMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("State of the Window")]
        [PropertyUISelectionOption("Maximize")]
        [PropertyUISelectionOption("Minimize")]
        [PropertyUISelectionOption("Restore")]
        [InputSpecification("", true)]
        [PropertyValidationRule("Window State", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "State")]
        [PropertyParameterOrder(6500)]
        public string v_WindowState { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_MatchMethod))]
        //[PropertySelectionChangeEvent(nameof(MatchMethodComboBox_SelectionChangeCommitted))]
        //public string v_MatchMethod { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_TargetWindowIndex))]
        //public string v_TargetWindowIndex { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WaitTime))]
        //public string v_WaitTime { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WindowNameResult))]
        //public string v_NameResult { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_OutputWindowHandle))]
        //public string v_HandleResult { get; set; }

        public SetWindowStateCommand()
        {
            //this.CommandName = "SetWindowStateCommand";
            //this.SelectionName = "Set Window State";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            WindowControls.WindowAction(this, engine,
                new Action<List<(IntPtr, string)>>(wins =>
                {
                    var windowState = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WindowState), engine);
                    var state = WindowControls.WindowState.SW_RESTORE;
                    switch (windowState.ToLower())
                    {
                        case "maximize":
                            state = WindowControls.WindowState.SW_MAXIMIZE;
                            break;
                        case "minimize":
                            state = WindowControls.WindowState.SW_MINIMIZE;
                            break;
                    }

                    foreach (var win in wins)
                    {
                        var whnd = win.Item1;
                        if (WindowControls.IsIconic(whnd) && (state != WindowControls.WindowState.SW_MINIMIZE))
                        {
                            WindowControls.ShowIconicWindow(whnd);
                        }
                        WindowControls.SetWindowState(whnd, state);
                    }
                })
            );
        }

        //private void MatchMethodComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        //{
        //    WindowNameControls.MatchMethodComboBox_SelectionChangeCommitted(ControlsList, (ComboBox)sender, nameof(v_TargetWindowIndex));
        //}

        //public override void Refresh(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        //{
        //    ControlsList.GetPropertyControl<ComboBox>(nameof(v_WindowName)).AddWindowNames();
        //}
    }
}