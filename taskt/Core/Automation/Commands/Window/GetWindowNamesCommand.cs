using System;
using System.Collections.Generic;
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

        public GetWindowNamesCommand()
        {
            this.CommandName = "GetWindowNamesCommand";
            this.SelectionName = "Get Window Names";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            try
            {
                var lst = new List<string>();
                var handles = WindowNameControls.FindWindows(this, nameof(v_WindowName), nameof(v_SearchMethod), nameof(v_WaitTime), engine);
                foreach (var whnd in handles)
                {
                    lst.Add(WindowNameControls.GetWindowNameFromHandle(whnd));
                }
                lst.StoreInUserVariable(engine, v_UserVariableName);
            }
            catch (Exception ex)
            {
                var whenNotFound = this.GetUISelectionValue(nameof(v_WhenWindowNotFound), engine);
                switch(whenNotFound)
                {
                    case "ignore":
                        new List<string>().StoreInUserVariable(engine, v_UserVariableName);
                        break;
                    case "error":
                        throw ex;
                }
            }
        }

        public override void Refresh(frmCommandEditor editor)
        {
            base.Refresh();
            ComboBox cmb = (ComboBox)ControlsList[nameof(v_WindowName)];
            cmb.AddWindowNames();
        }
    }
}