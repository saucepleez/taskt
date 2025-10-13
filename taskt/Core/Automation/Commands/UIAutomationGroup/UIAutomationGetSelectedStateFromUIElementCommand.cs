using System;
using System.Xml.Serialization;
using System.Windows.Automation;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.UIAutomationGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation")]
    [Attributes.ClassAttributes.SubGruop("Get From UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Get Selected State From UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to get Selected State from UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Selected State from UIElement.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationGetSelectedStateFromUIElementCommand : AGetFromUIElementCommands, IResultProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        //public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [Remarks("When UIElement is Selected, Result is **True**")]
        [PropertyParameterOrder(6000)]
        public string v_Result { get; set; }

        public UIAutomationGetSelectedStateFromUIElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var targetElement = v_TargetElement.ExpandUserVariableAsUIElement(engine);

            //bool checkState;
            //if (targetElement.TryGetCurrentPattern(TogglePattern.Pattern, out object patternObj))
            //{
            //    checkState = (((TogglePattern)patternObj).Current.ToggleState == ToggleState.On);
            //}
            //else if (targetElement.TryGetCurrentPattern(SelectionItemPattern.Pattern, out patternObj))
            //{
            //    checkState = ((SelectionItemPattern)patternObj).Current.IsSelected;
            //}
            //else
            //{
            //    throw new Exception("Thie UIElement does not have Selected State");
            //}
            //checkState.StoreInUserVariable(engine, v_ResultVariable);

            this.UIElementAction(engine,
                new Action<AutomationElement>((targetElement) =>
                {
                    bool checkState;
                    if (targetElement.TryGetCurrentPattern(TogglePattern.Pattern, out object patternObj))
                    {
                        checkState = (((TogglePattern)patternObj).Current.ToggleState == ToggleState.On);
                    }
                    else if (targetElement.TryGetCurrentPattern(SelectionItemPattern.Pattern, out patternObj))
                    {
                        checkState = ((SelectionItemPattern)patternObj).Current.IsSelected;
                    }
                    else
                    {
                        //throw new Exception("Thie UIElement does not have Selected State");
                        this.ValueCanNotRetrievedProcess("Selected State", new Action(() =>
                        {
                            "".StoreInUserVariable(engine, v_Result);
                        }), engine);
                        return;
                    }
                    checkState.StoreInUserVariable(engine, v_Result);
                })
            );
        }
    }
}