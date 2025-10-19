using System;
using System.Xml.Serialization;
using System.Windows.Automation;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.UIAutomationGroup;

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
        [PropertyDisplayText(true, "Item")]
        [PropertyParameterOrder(6000)]
        public string v_Item { get; set; }

        public UIAutomationSelectItemInUIElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var targetElement = v_TargetElement.ExpandUserVariableAsUIElement(engine);

            //var itemName = v_Item.ExpandValueOrUserVariable(engine);

            //var items = UIElementControls.GetSelectionItems(targetElement);
            //bool isSelected = false;
            //foreach(var item in items)
            //{
            //    if (item.Current.Name == itemName)
            //    {
            //        SelectionItemPattern selPtn = (SelectionItemPattern)item.GetCurrentPattern(SelectionItemPattern.Pattern);
            //        selPtn.Select();
            //        isSelected = true;
            //        break;
            //    }
            //}

            //if (!isSelected)
            //{
            //    throw new Exception("Item '" + v_Item + "' does not exists");
            //}

            this.SelectionItemsAction(engine,
                new Action<System.Collections.Generic.List<AutomationElement>>((items) =>
                {
                    var itemName = v_Item.ExpandValueOrUserVariable(engine);

                    bool isSelected = false;
                    foreach (var item in items)
                    {
                        if (item.Current.Name == itemName)
                        {
                            if (item.TryGetCurrentPattern(SelectionItemPattern.Pattern, out object ptn))
                            {
                                var selPtn = (SelectionItemPattern)ptn;
                                selPtn.Select();
                                isSelected = true;
                            }
                            break;
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