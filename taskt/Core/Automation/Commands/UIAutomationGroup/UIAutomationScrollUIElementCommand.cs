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
    [Attributes.ClassAttributes.CommandSettings("Scroll UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Scroll UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to Scroll UIElement.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationScrollUIElementCommand : AUIElementActionCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        //public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("ScrollBar Type")]
        [PropertyUISelectionOption("Vertical")]
        [PropertyUISelectionOption("Horizonal")]
        [PropertyValidationRule("ScrollBar Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Type")]
        [PropertyParameterOrder(6000)]
        public string v_ScrollBarType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Scroll Method")]
        [PropertyUISelectionOption("Scroll Small Down or Right")]
        [PropertyUISelectionOption("Scroll Large Down or Right")]
        [PropertyUISelectionOption("Scroll Small Up or Left")]
        [PropertyUISelectionOption("Scroll Large Up or Left")]
        [PropertyValidationRule("Scroll Method", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Method")]
        [PropertyParameterOrder(6100)]
        public string v_DirectionAndAmount{ get; set; }

        public UIAutomationScrollUIElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var targetElement = v_TargetElement.ExpandUserVariableAsUIElement(engine);
            //var scrollbarType = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ScrollBarType), engine);

            //var dirAndAmo = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_DirectionAndAmount), engine);
            //var amount = ScrollAmount.NoAmount;
            //switch (dirAndAmo)
            //{
            //    case "scroll small down or right":
            //        amount = ScrollAmount.SmallIncrement;
            //        break;
            //    case "scroll large down or right":
            //        amount = ScrollAmount.LargeIncrement;
            //        break;
            //    case "scroll small up or left":
            //        amount = ScrollAmount.SmallDecrement;
            //        break;
            //    case "scroll large up or left":
            //        amount = ScrollAmount.LargeDecrement;
            //        break;
            //}

            //if (!targetElement.TryGetCurrentPattern(ScrollPattern.Pattern, out object scrollPtn))
            //{
            //    if (targetElement.Current.ControlType == ControlType.ScrollBar)
            //    {
            //        var parentElement = UIElementControls.GetParentUIElement(targetElement);
            //        if (!parentElement.TryGetCurrentPattern(ScrollPattern.Pattern, out scrollPtn))
            //        {
            //            throw new Exception($"UIElement '{v_TargetElement}' does not have ScrollBar");
            //        }
            //    }
            //    else
            //    {
            //        throw new Exception($"UIElement '{v_TargetElement}' is not ScrollBar and does not have ScrollBar");
            //    }
            //}
            //var sp = (ScrollPattern)scrollPtn;
            //switch (scrollbarType)
            //{
            //    case "horizonal":
            //        sp.ScrollHorizontal(amount);
            //        break;
            //    case "vertical":
            //        sp.ScrollVertical(amount);
            //        break;
            //}

            this.UIElementActionAndWait(engine,
                new Action<AutomationElement, IntPtr>((targetElement, whnd) =>
                {
                    var dirAndAmo = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_DirectionAndAmount), engine);
                    var amount = ScrollAmount.NoAmount;
                    switch (dirAndAmo)
                    {
                        case "scroll small down or right":
                            amount = ScrollAmount.SmallIncrement;
                            break;
                        case "scroll large down or right":
                            amount = ScrollAmount.LargeIncrement;
                            break;
                        case "scroll small up or left":
                            amount = ScrollAmount.SmallDecrement;
                            break;
                        case "scroll large up or left":
                            amount = ScrollAmount.LargeDecrement;
                            break;
                    }

                    if (!targetElement.TryGetCurrentPattern(ScrollPattern.Pattern, out object scrollPtn))
                    {
                        if (targetElement.Current.ControlType == ControlType.ScrollBar)
                        {
                            //var parentElement = UIElementControls.GetParentUIElement(targetElement);
                            var parentElement = EM_CanHandleUIElementExtentionMethods.GetParentUIElement(targetElement);
                            if (!parentElement.TryGetCurrentPattern(ScrollPattern.Pattern, out scrollPtn))
                            {
                                //throw new Exception($"UIElement '{v_TargetElement}' does not have ScrollBar");
                                this.ActionNotSupportedProcess("Scroll", engine);
                                return;
                            }
                        }
                        else
                        {
                            //throw new Exception($"UIElement '{v_TargetElement}' is not ScrollBar and does not have ScrollBar");
                            this.ActionNotSupportedProcess("Scroll", engine);
                            return;
                        }
                    }

                    var sp = (ScrollPattern)scrollPtn;
                    var scrollbarType = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ScrollBarType), engine);
                    switch (scrollbarType)
                    {
                        case "horizonal":
                            sp.ScrollHorizontal(amount);
                            break;
                        case "vertical":
                            sp.ScrollVertical(amount);
                            break;
                    }
                })
            );
        }
    }
}