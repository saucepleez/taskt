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
    [Attributes.ClassAttributes.CommandSettings("Expand Collapse Items In UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Expand or Collapse Items in UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to Expand or Collapse Items in UIElement.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationExpandCollapseItemsInUIElementCommand : AUIElementActionCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        //public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Items State")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("", true)]
        [PropertyUISelectionOption("Expand")]
        [PropertyUISelectionOption("Collapse")]
        //[Remarks("")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Items State", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "State")]
        [PropertyParameterOrder(6000)]
        public string v_ItemsState { get; set; }

        public UIAutomationExpandCollapseItemsInUIElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var targetElement = v_TargetElement.ExpandUserVariableAsUIElement(engine);
            //var state = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ItemsState), engine);

            //if (targetElement.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out object exColPtn))
            //{
            //    switch (state)
            //    {
            //        case "expand":
            //            ((ExpandCollapsePattern)exColPtn).Expand();
            //            break;
            //        case "collapse":
            //            ((ExpandCollapsePattern)exColPtn).Collapse();
            //            break;
            //    }
            //}
            //else
            //{
            //    throw new Exception($"UIElement '{v_TargetElement}' does not support Expand/Collapse");
            //}

            this.UIElementActionAndWait(engine,
                new Action<AutomationElement, IntPtr>((targetElement, whnd) =>
                {
                    var state = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ItemsState), engine);
                    if (targetElement.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out object exColPtn))
                    {
                        switch (state)
                        {
                            case "expand":
                                ((ExpandCollapsePattern)exColPtn).Expand();
                                break;
                            case "collapse":
                                ((ExpandCollapsePattern)exColPtn).Collapse();
                                break;
                        }
                    }
                    else
                    {
                        //throw new Exception($"UIElement '{v_TargetElement}' does not support Expand/Collapse");
                        this.ActionNotSupportedProcess("Expand/Collapse", engine);
                    }
                })
            );
        }
    }
}