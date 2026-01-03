using System;
using System.Windows.Automation;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.UIAutomationGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation")]
    [Attributes.ClassAttributes.SubGruop("Get From UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Get ScrollBar Information From UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to get ScrollBar Information from UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get ScrollBar Information from UIElement.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationGetScrollBarInformationFromUIElementCommand : AGetFromUIElementCommands, IResultProperties, ICanHandleUIElementScrollBar
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        //public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Information Type")]
        [PropertyUISelectionOption("Horizontally Scrollable")]
        [PropertyUISelectionOption("Horizontal Scroll Percent")]
        [PropertyUISelectionOption("Vertically Scrollable")]
        [PropertyUISelectionOption("Vertical Scroll Percent")]
        [PropertyValidationRule("Information", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Information")]
        [PropertyParameterOrder(6000)]
        public string v_InformationType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(7000)]
        public string v_Result { get; set; }

        public UIAutomationGetScrollBarInformationFromUIElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.UIElementAction(engine,
                new Action<AutomationElement>((targetElement) =>
                {
                    //if (!targetElement.TryGetCurrentPattern(ScrollPattern.Pattern, out object scrollPtn))
                    //{
                    //    if (targetElement.Current.ControlType == ControlType.ScrollBar)
                    //    {
                    //        var parentElement = EM_CanHandleUIElementExtentionMethods.GetParentUIElement(targetElement);
                    //        if (!parentElement.TryGetCurrentPattern(ScrollPattern.Pattern, out scrollPtn))
                    //        {
                    //            this.ValueCanNotRetrievedProcess($"ScrollBar Value", new Action(() =>
                    //            {
                    //                "".StoreInUserVariable(engine, v_Result);
                    //            }), engine);
                    //            return;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        this.ValueCanNotRetrievedProcess($"ScrollBar Value", new Action(() =>
                    //        {
                    //            "".StoreInUserVariable(engine, v_Result);
                    //        }), engine);
                    //        return;
                    //    }
                    //}
                    var sp = EM_CanHandleUIElementScrollBarExtensionMethods.GetScrollPattern(targetElement, new Action(() =>
                    {
                        this.ValueCanNotRetrievedProcess($"ScrollBar Value", new Action(() =>
                        {
                            "".StoreInUserVariable(engine, v_Result);
                        }), engine);
                    }));
                    if (sp == null)
                    {
                        return;
                    }

                    //var sp = (ScrollPattern)scrollPtn;
                    switch(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_InformationType), engine))
                    {
                        case "horizontally scrollable":
                            sp.Current.HorizontallyScrollable.StoreInUserVariable(engine, v_Result);
                            break;
                        case "horizontal scroll percent":
                            sp.Current.HorizontalScrollPercent.StoreInUserVariable(engine, v_Result);
                            break;
                        case "vertically scrollable":
                            sp.Current.VerticallyScrollable.StoreInUserVariable(engine, v_Result);
                            break;
                        case "vertical scroll percent":
                            sp.Current.VerticalScrollPercent.StoreInUserVariable(engine, v_Result);
                            break;
                    }
                })
            );
        }
    }
}