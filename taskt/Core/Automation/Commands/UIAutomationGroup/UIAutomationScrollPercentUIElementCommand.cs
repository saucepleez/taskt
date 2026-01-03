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
    [Attributes.ClassAttributes.CommandSettings("Scroll Percent UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Scroll UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to Scroll UIElement.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationScrollPercentUIElementCommand : AUIElementActionCommands, ICanHandleUIElementScrollBar
    {
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
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Scroll Value")]
        [PropertyDetailSampleUsage("**0**", PropertyDetailSampleUsage.ValueType.Value, "Scroll")]
        [PropertyDetailSampleUsage("**{{vScroll}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Scroll")]
        [PropertyValueRange(0, 100)]
        [PropertyValidationRule("Scroll Method", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Scroll Value")]
        [PropertyParameterOrder(6100)]
        public string v_ScrollValue{ get; set; }

        public UIAutomationScrollPercentUIElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.UIElementActionAndWait(engine,
                new Action<AutomationElement, IntPtr>((targetElement, whnd) =>
                {
                    //if (!targetElement.TryGetCurrentPattern(ScrollPattern.Pattern, out object scrollPtn))
                    //{
                    //    if (targetElement.Current.ControlType == ControlType.ScrollBar)
                    //    {
                    //        var parentElement = EM_CanHandleUIElementExtentionMethods.GetParentUIElement(targetElement);
                    //        if (!parentElement.TryGetCurrentPattern(ScrollPattern.Pattern, out scrollPtn))
                    //        {
                    //            this.ActionNotSupportedProcess("Scroll", engine);
                    //            return;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        this.ActionNotSupportedProcess("Scroll", engine);
                    //        return;
                    //    }
                    //}

                    var sp = EM_CanHandleUIElementScrollBarExtensionMethods.GetScrollPattern(targetElement, new Action(() =>
                    {
                        this.ActionNotSupportedProcess("Scroll", engine);
                    }));
                    if (sp == null)
                    {
                        return;
                    }

                    var scrollValue = (double)this.ExpandValueOrUserVariableAsDecimal(nameof(v_ScrollValue), engine);

                    //var sp = (ScrollPattern)scrollPtn;
                    var scrollbarType = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ScrollBarType), engine);
                    switch (scrollbarType)
                    {
                        case "horizonal":
                            sp.SetScrollPercent(scrollValue, ScrollPattern.NoScroll);
                            break;
                        case "vertical":
                            sp.SetScrollPercent(ScrollPattern.NoScroll, scrollValue);
                            break;
                    }
                })
            );
        }
    }
}