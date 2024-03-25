﻿using System;
using System.Xml.Serialization;
using System.Windows.Automation;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("UIElement Action")]
    [Attributes.ClassAttributes.CommandSettings("Select Item In UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Select a Item in UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to Select a Item in UIElement.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationSelectItemInUIElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Item Value to Select")]
        [InputSpecification("Item Value", true)]
        [PropertyDetailSampleUsage("**Yes**", PropertyDetailSampleUsage.ValueType.Value)]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value)]
        [PropertyDetailSampleUsage("**{{{vItem}}}**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyDisplayText(true, "Item")]
        public string v_Item { get; set; }

        public UIAutomationSelectItemInUIElementCommand()
        {
            //this.CommandName = "UIAutomationSelectItemInElementCommand";
            //this.SelectionName = "Select Item In Element";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var targetElement = v_TargetElement.ExpandUserVariableAsUIElement(engine);

            var itemName = v_Item.ExpandValueOrUserVariable(engine);

            var items = UIElementControls.GetSelectionItems(targetElement);
            bool isSelected = false;
            foreach(var item in items)
            {
                if (item.Current.Name == itemName)
                {
                    SelectionItemPattern selPtn = (SelectionItemPattern)item.GetCurrentPattern(SelectionItemPattern.Pattern);
                    selPtn.Select();
                    isSelected = true;
                    break;
                }
            }

            if (!isSelected)
            {
                throw new Exception("Item '" + v_Item + "' does not exists");
            }
        }
    }
}