using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.SubGruop("Window Actions")]
    [Attributes.ClassAttributes.CommandSettings("Close Window")]
    [Attributes.ClassAttributes.Description("This command closes an open window.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to close an existing window by name.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CloseWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WindowName))]
        public string v_WindowName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_CompareMethod))]
        public string v_SearchMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_MatchMethod))]
        [PropertySelectionChangeEvent(nameof(MatchMethodComboBox_SelectionChangeCommitted))]
        public string v_MatchMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_TargetWindowIndex))]
        public string v_TargetWindowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WaitTime))]
        public string v_WaitTime { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WindowNameResult))]
        public string v_NameResult { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WindowHandleResult))]
        public string v_HandleResult { get; set; }

        public CloseWindowCommand()
        {
            //this.CommandName = "CloseWindowCommand";
            //this.SelectionName = "Close Window";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Automation.Engine.AutomationEngineInstance)sender;

            //var wins = WindowNameControls.FindWindows(this, nameof(v_WindowName), nameof(v_SearchMethod), nameof(v_MatchMethod), nameof(v_TargetWindowIndex), nameof(v_WaitTime), engine);

            //foreach(var win in wins)
            //{
            //    WindowNameControls.CloseWindow(win.Item1);
            //}

            //WindowNameControls.WindowAction(this, nameof(v_WindowName), nameof(v_SearchMethod), nameof(v_MatchMethod), nameof(v_TargetWindowIndex), nameof(v_WaitTime), engine,
            //    new Action<System.Collections.Generic.List<(IntPtr, string)>>(wins =>
            //    {
            //        foreach (var win in wins)
            //        {
            //            WindowNameControls.CloseWindow(win.Item1);
            //        }
            //    }), nameof(v_NameResult), nameof(v_HandleResult)
            //);
            WindowNameControls.WindowAction(this, engine,
                    new Action<System.Collections.Generic.List<(IntPtr, string)>>(wins =>
                {
                    foreach (var win in wins)
                    {
                        WindowNameControls.CloseWindow(win.Item1);
                    }
                })
            );
        }

        private void MatchMethodComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            WindowNameControls.MatchMethodComboBox_SelectionChangeCommitted(ControlsList, (ComboBox)sender, nameof(v_TargetWindowIndex));
        }

        public override void Refresh(frmCommandEditor editor)
        {
            base.Refresh();
            ControlsList.GetPropertyControl<ComboBox>(nameof(v_WindowName)).AddWindowNames();
        }
    }
}