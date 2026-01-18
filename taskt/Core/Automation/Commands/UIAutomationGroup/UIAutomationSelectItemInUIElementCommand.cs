using System;
using System.Xml.Serialization;
using System.Windows.Automation;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.UIAutomationGroup;
using System.Collections.Generic;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation")]
    [Attributes.ClassAttributes.SubGruop("UIElement Action")]
    [Attributes.ClassAttributes.CommandSettings("Select Item In UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Select a Item in UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to Select a Item in UIElement.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationSelectItemInUIElementCommand : AUIElementActionCommands, IUIElementSelectionItemsProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        //public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Item Value to Select")]
        [InputSpecification("Item Value", true)]
        [PropertyDetailSampleUsage("**Yes**", PropertyDetailSampleUsage.ValueType.Value)]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value)]
        [PropertyDetailSampleUsage("**{{{vItem}}}**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value)]
        [PropertyDisplayText(true, "Item")]
        [PropertyParameterOrder(6000)]
        public string v_Item { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Item Value Type")]
        [PropertyUISelectionOption("Text Value")]
        [PropertyUISelectionOption("Index")]
        [PropertyIsOptional(true, "Text Value")]
        [PropertyValidationRule("Value Type", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "Value Type")]
        [PropertyParameterOrder(6100)]
        public string v_ItemValueType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_ExpandWhenItemsNotFound))]
        [PropertyParameterOrder(7000)]
        public string v_ExpandWhenItemsNotFound { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_WaitTimeAfterExpand))]
        [PropertyParameterOrder(7100)]
        public string v_WaitTimeAfterExpand { get; set; }

        public UIAutomationSelectItemInUIElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.SelectionItemsAction(engine,
                new Action<List<AutomationElement>>((items) =>
                {
                    bool isSelected = false;

                    AutomationElement targetItem = null;
                    switch(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ItemValueType), engine))
                    {
                        case "text value":
                            var itemName = v_Item.ExpandValueOrUserVariable(engine);
                            foreach (var item in items)
                            {
                                if (item.Current.Name == itemName)
                                {
                                    targetItem = item;
                                    break;
                                }
                            }
                            break;
                        case "index":
                            var itemIndex = this.ExpandValueOrUserVariableAsInteger(nameof(v_Item), engine);
                            if (itemIndex < 0)
                            {
                                itemIndex += items.Count;
                            }
                            if (itemIndex >= 0 && itemIndex < items.Count)
                            {
                                targetItem = items[itemIndex];
                            }
                            break;
                    }

                    if (targetItem != null)
                    {
                        if (targetItem.TryGetCurrentPattern(SelectionItemPattern.Pattern, out object siPtn))
                        {
                            ((SelectionItemPattern)siPtn).Select();
                            isSelected = true;
                        }
                    }

                    if (!isSelected)
                    {
                        this.ActionNotSupportedProcess("Select Item", engine);
                    }
                }),
                new Action(() =>
                {
                    this.ActionNotSupportedProcess("Select Item", engine);
                })
            );
        }
    }
}