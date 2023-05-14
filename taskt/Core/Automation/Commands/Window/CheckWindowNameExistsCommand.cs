using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.SubGruop("Window State")]
    [Attributes.ClassAttributes.CommandSettings("Check Window Name Exists")]
    [Attributes.ClassAttributes.Description("This command returns a existence of window name.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to check a existence of window name.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CheckWindowNameExistsCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WindowName))]
        public string v_WindowName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_CompareMethod))]
        public string v_SearchMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [Remarks("When Window Exists, Result is **True**")]
        public string v_UserVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WaitTime))]
        [PropertyIsOptional(true, "0")]
        [PropertyFirstValue("0")]
        public string v_WaitTime { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WindowNameResult))]
        public string v_NameResult { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WindowHandleResult))]
        public string v_HandleResult { get; set; }

        public CheckWindowNameExistsCommand()
        {
            //this.CommandName = "CheckWindowNameExistsCommand";
            //this.SelectionName = "Check Window Name Exists";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //try
            //{
            //    var handles = WindowNameControls.FindWindows(this, nameof(v_WindowName), nameof(v_SearchMethod), nameof(v_WaitTime), engine);

            //    (handles.Count > 0).StoreInUserVariable(engine, v_UserVariableName);
            //}
            //catch
            //{
            //    false.StoreInUserVariable(engine, v_UserVariableName);
            //}
            //WindowNameControls.WindowAction(this, nameof(v_WindowName), nameof(v_SearchMethod), nameof(v_WaitTime), engine,
            //    new Action<System.Collections.Generic.List<(IntPtr, string)>>(wins =>
            //    {
            //        (wins.Count > 0).StoreInUserVariable(engine, v_UserVariableName);
            //    }), nameof(v_NameResult), nameof(v_HandleResult), new Action<Exception>(ex =>
            //    {
            //        false.StoreInUserVariable(engine, v_UserVariableName);
            //    })
            //);
            WindowNameControls.WindowAction(this, engine,
                new Action<System.Collections.Generic.List<(IntPtr, string)>>(wins =>
                {
                    (wins.Count > 0).StoreInUserVariable(engine, v_UserVariableName);
                }),
                new Action<Exception>(ex =>
                {
                    false.StoreInUserVariable(engine, v_UserVariableName);
                })
            );
        }

        public override void Refresh(frmCommandEditor editor)
        {
            base.Refresh();
            //ComboBox cmb = (ComboBox)ControlsList[nameof(v_WindowName)];
            //cmb.AddWindowNames();
            ControlsList.GetPropertyControl<ComboBox>(nameof(v_WindowName)).AddWindowNames();
        }
    }
}