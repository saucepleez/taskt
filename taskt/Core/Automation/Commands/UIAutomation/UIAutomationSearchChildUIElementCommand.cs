using System;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using System.Windows.Forms;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Search UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Search Child UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to get Child Element from UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Child UIElement from UIElement. Search only for Child UIElements.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationSearchChildUIElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        [PropertyDescription("Root UIElement Variable")]
        public string v_RootElement { get; set; }

        [XmlElement]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_SearchParameters))]
        public DataTable v_SearchParameters { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Child UIElement Index")]
        [InputSpecification("Index", true)]
        [PropertyDetailSampleUsage("**0**", "Specfity the First UIElement")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Index")]
        [PropertyDetailSampleUsage("**{{{vIndex}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Index")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Index", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Index")]
        public string v_Index { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_NewOutputUIElementName))]
        [PropertyDescription("UIElement Variable Name to Store Child UIElement")]
        public string v_AutomationElementVariable { get; set; }

        public UIAutomationSearchChildUIElementCommand()
        {
            //this.CommandName = "UIAutomationGetChildElementCommand";
            //this.SelectionName = "Get Child Element";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var rootElement = v_RootElement.GetUIElementVariable(engine);
            int index = v_Index.ConvertToUserVariableAsInteger("v_Index", engine);

            var elems = UIElementControls.GetChildrenUIElements(rootElement, v_SearchParameters, engine);
            if (elems.Count > 0)
            {
                elems[index].StoreInUserVariable(engine, v_AutomationElementVariable);
            }
            else
            {
                throw new Exception("UIElement not found");
            }
        }

        public override void AfterShown()
        {
            //AutomationElementControls.RenderSearchParameterDataGridView((DataGridView)ControlsList[nameof(v_SearchParameters)]);
            UIElementControls.RenderSearchParameterDataGridView(ControlsList.GetPropertyControl<DataGridView>(nameof(v_SearchParameters)));
        }
    }
}