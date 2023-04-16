using System;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Automation;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Search")]
    [Attributes.ClassAttributes.CommandSettings("Check Element Exist")]
    [Attributes.ClassAttributes.Description("This command allows you to to check AutomationElement existence.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to check AutomationElement existence")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationCheckElementExistCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_InputAutomationElementName))]
        public string v_TargetElement { get; set; }

        [XmlElement]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_SearchParameters))]
        public DataTable v_SearchParameters { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [Remarks("When the Element exists, Result value is **True**")]
        public string v_Result { get; set; }

        public UIAutomationCheckElementExistCommand()
        {
            //this.CommandName = "UIAutomationCheckElementExistCommand";
            //this.SelectionName = "Check Element Exist";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var rootElement = v_TargetElement.GetAutomationElementVariable(engine);

            AutomationElement elem = AutomationElementControls.SearchGUIElement(rootElement, v_SearchParameters, engine);

            (elem != null).StoreInUserVariable(engine, v_Result);
        }

        public override void AfterShown()
        {
            AutomationElementControls.RenderSearchParameterDataGridView((DataGridView)ControlsList[nameof(v_SearchParameters)]);
        }
    }
}