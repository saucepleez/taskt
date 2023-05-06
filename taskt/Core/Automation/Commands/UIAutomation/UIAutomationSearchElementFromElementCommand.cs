using System;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Automation;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using System.Windows.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Search")]
    [Attributes.ClassAttributes.CommandSettings("Search Element From Element")]
    [Attributes.ClassAttributes.Description("This command allows you to get AutomationElement from AutomationElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get AutomationElement from AutomationElement. Search for Descendants Elements.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationSearchElementFromElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_InputAutomationElementName))]
        [PropertyDescription("AutomationElement Variable Name to Search")]
        public string v_TargetElement { get; set; }

        [XmlElement]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_SearchParameters))]
        public DataTable v_SearchParameters { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_NewOutputAutomationElementName))]
        public string v_AutomationElementVariable { get; set; }

        public UIAutomationSearchElementFromElementCommand()
        {
            //this.CommandName = "UIAutomationGetElementFromElementCommand";
            //this.SelectionName = "Get Element From Element";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var rootElement = v_TargetElement.GetAutomationElementVariable(engine);

            AutomationElement elem = AutomationElementControls.SearchGUIElement(rootElement, v_SearchParameters, engine);
            if (elem != null)
            {
                elem.StoreInUserVariable(engine, v_AutomationElementVariable);
            }
            else
            {
                throw new Exception("AutomationElement not found");
            }
        }

        public override void AfterShown()
        {
            AutomationElementControls.RenderSearchParameterDataGridView((DataGridView)ControlsList[nameof(v_SearchParameters)]);
        }
    }
}