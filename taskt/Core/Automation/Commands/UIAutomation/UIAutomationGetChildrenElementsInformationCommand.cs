using System;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Get")]
    [Attributes.ClassAttributes.Description("This command allows you to get Children Elements Information from AutomationElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Children Elements Information from AutomationElement.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationGetChildrenElementsInformationCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_InputAutomationElementName))]
        public string v_RootElement { get; set; }

        [XmlElement]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_SearchParameters))]
        public DataTable v_SearchParameters { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_ResultVariable { get; set; }

        public UIAutomationGetChildrenElementsInformationCommand()
        {
            this.CommandName = "UIAutomationGetChildrenElementsInformationCommand";
            this.SelectionName = "Get Children Elements Information";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetElement = v_RootElement.GetAutomationElementVariable(engine);

            var elems = AutomationElementControls.GetChildrenElements(targetElement, v_SearchParameters, engine);

            string result = "";

            int counts = elems.Count;
            for (int i = 0; i < counts; i++)
            {
                var elem = elems[i];
                result += "Index: " + i + ", Name: " + elem.Current.Name + ", LocalizedControlType: " + elem.Current.LocalizedControlType + ", ControlType: " + AutomationElementControls.GetControlTypeText(elem.Current.ControlType) + "\n";
            }
            result.Trim().StoreInUserVariable(engine, v_ResultVariable);
        }
    }
}