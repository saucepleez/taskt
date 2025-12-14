using System;
using System.Windows.Automation;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.UIAutomationGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation")]
    [Attributes.ClassAttributes.SubGruop("Search UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Search Child UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to get Child Element from UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Child UIElement from UIElement. Search only for Child UIElements.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationSearchChildUIElementCommand : AChildrenSearchUIElementsFromUIElementByTreeWalkerCommands, IUIElementIndexProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        //[PropertyDescription("Search Start UIElement Variable")]
        //public string v_TargetElement { get; set; }

        //[XmlElement]
        //[PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_SearchParameters))]
        //[PropertyParameterOrder(6000)]
        //public DataTable v_SearchParameters { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_TargetUIElementIndex))]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        //[PropertyDescription("Child UIElement Index")]
        //[InputSpecification("Number", true)]
        //[PropertyDetailSampleUsage("**0**", "Specfity the First UIElement")]
        //[PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Index")]
        //[PropertyDetailSampleUsage("**{{{vIndex}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Index")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyValidationRule("Index", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        //[PropertyDisplayText(true, "Index")]
        [PropertyParameterOrder(6100)]
        public string v_TargetUIElementIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_NewOutputUIElementName))]
        [PropertyDescription("UIElement Variable Name to Store Child UIElement")]
        [PropertyParameterOrder(7000)]
        public string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_WaitTimeForUIElement))]
        //[PropertyParameterOrder(7100)]
        //public string v_WaitTimeForUIElement { get; set; }

        public UIAutomationSearchChildUIElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.UIElementAction(engine, new Action<AutomationElement>((elem) =>
            {
                var children = this.SearchChildrenUIElements(elem, engine);

                //var index = v_TargetUIElementIndex.ExpandValueOrUserVariableAsInteger("v_Index", engine);
                //var index = this.ExpandValueOrUserVariableAsUIElementIndex(engine);
                //if (index < 0)
                //{
                //    index += children.Count;
                //}
                //if (index >= 0 && index < children.Count)
                //{
                //    children[index].StoreInUserVariable(engine, v_Result);
                //}
                //else
                //{
                //    throw new Exception($"UIElement not found. Index: '{v_TargetUIElementIndex}', Expand Value: '{index}'");
                //}

                var e = this.GetUIElementFromList(children, engine);
                e.StoreInUserVariable(engine, v_Result);
            }));
        }
    }
}