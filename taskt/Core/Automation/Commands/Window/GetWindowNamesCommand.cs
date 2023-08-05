using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using System.Linq;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.SubGruop("Window State")]
    [Attributes.ClassAttributes.CommandSettings("Get Window Names")]
    [Attributes.ClassAttributes.Description("This command returns window names.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want window names.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetWindowNamesCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WindowName))]
        public string v_WindowName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_CompareMethod))]
        public string v_SearchMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public string v_UserVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("When Window Not Found")]
        [InputSpecification("", true)]
        [Remarks("")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyDetailSampleUsage("**Ignore**", "Nothing to do. Get Empty LIST")]
        [PropertyDetailSampleUsage("**Error**", "Rise a Error")]
        [PropertyIsOptional(true, "Ignore")]
        public string v_WhenWindowNotFound { get; set; }

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

        public GetWindowNamesCommand()
        {
            //this.CommandName = "GetWindowNamesCommand";
            //this.SelectionName = "Get Window Names";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //try
            //{
            //    var wins = WindowNameControls.FindWindows(this, nameof(v_WindowName), nameof(v_SearchMethod), nameof(v_WaitTime), engine);
            //    wins.Select(w => w.Item2).ToList().StoreInUserVariable(engine, v_UserVariableName);
            //}
            //catch (Exception ex)
            //{
            //    var whenNotFound = this.GetUISelectionValue(nameof(v_WhenWindowNotFound), engine);
            //    switch(whenNotFound)
            //    {
            //        case "ignore":
            //            new List<string>().StoreInUserVariable(engine, v_UserVariableName);
            //            break;
            //        case "error":
            //            throw ex;
            //    }
            //}
            //WindowNameControls.WindowAction(this, nameof(v_WindowName), nameof(v_SearchMethod), nameof(v_WaitTime), engine,
            //    new Action<List<(IntPtr, string)>>(wins =>
            //    {
            //        wins.Select(w => w.Item2).ToList().StoreInUserVariable(engine, v_UserVariableName);
            //    }), nameof(v_NameResult), nameof(v_HandleResult),
            //    new Action<Exception>(ex =>
            //    {
            //        var whenNotFound = this.GetUISelectionValue(nameof(v_WhenWindowNotFound), engine);
            //        switch (whenNotFound)
            //        {
            //            case "ignore":
            //                new List<string>().StoreInUserVariable(engine, v_UserVariableName);
            //                break;
            //            case "error":
            //                throw ex;
            //        }
            //    })
            //);
            WindowNameControls.WindowAction(this, engine,
                new Action<List<(IntPtr, string)>>(wins =>
                {
                    wins.Select(w => w.Item2).ToList().StoreInUserVariable(engine, v_UserVariableName);
                }), 
                new Action<Exception>(ex =>
                {
                    var whenNotFound = this.GetUISelectionValue(nameof(v_WhenWindowNotFound), engine);
                    switch (whenNotFound)
                    {
                        case "ignore":
                            new List<string>().StoreInUserVariable(engine, v_UserVariableName);
                            break;
                        case "error":
                            throw ex;
                    }
                })
            );
        }

        public override void Refresh(frmCommandEditor editor)
        {
            base.Refresh();
            ControlsList.GetPropertyControl<ComboBox>(nameof(v_WindowName)).AddWindowNames();
        }
    }
}