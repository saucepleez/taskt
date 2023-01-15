using System;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Search")]
    [Attributes.ClassAttributes.Description("This command allows you to get Child Element from AutomationElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Child Element from AutomationElement. Search only for Child Elements.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationGetChildElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_InputAutomationElementName))]
        [PropertyDescription("Root AutomationElement Variable")]
        public string v_RootElement { get; set; }

        [XmlElement]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_SearchParameters))]
        public DataTable v_SearchParameters { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Child Element Index")]
        [InputSpecification("Index", true)]
        [PropertyDetailSampleUsage("**0**", "Specfity the First AutomationElement")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Index")]
        [PropertyDetailSampleUsage("**{{{vIndex}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Index")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Index", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Index")]
        public string v_Index { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_NewOutputAutomationElementName))]
        [PropertyDescription("AutomationElemnet Variable Name to Store Child Element")]
        public string v_AutomationElementVariable { get; set; }

        public UIAutomationGetChildElementCommand()
        {
            this.CommandName = "UIAutomationGetChildElementCommand";
            this.SelectionName = "Get Child Element";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var rootElement = v_RootElement.GetAutomationElementVariable(engine);
            int index = v_Index.ConvertToUserVariableAsInteger("v_Index", engine);

            var elems = AutomationElementControls.GetChildrenElements(rootElement, v_SearchParameters, engine);
            if (elems.Count > 0)
            {
                elems[index].StoreInUserVariable(engine, v_AutomationElementVariable);
            }
            else
            {
                throw new Exception("AutomationElement not found");
            }
        }
    }
}