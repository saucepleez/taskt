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
    [Attributes.ClassAttributes.CommandSettings("Get Text From UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to get Text Value from UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Text Value from UIElement.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationGetTextFromUIElementCommand : AGetFromUIElementCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        //public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(6000)]
        public string v_Result { get; set; }

        public UIAutomationGetTextFromUIElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var targetElement = v_TargetElement.ExpandUserVariableAsUIElement(engine);

            //string res = UIElementControls.GetTextValue(targetElement);
            //res.StoreInUserVariable(engine, v_Result);

            this.UIElementAction(engine,
                new Action<AutomationElement>(targetElement =>
                {
                    string text;
                    //object patternObj;
                    if (targetElement.TryGetCurrentPattern(RangeValuePattern.Pattern, out object rPtn))
                    {
                        // bar
                        text = ((RangeValuePattern)rPtn).Current.Value.ToString();
                    }
                    else if (targetElement.TryGetCurrentPattern(ValuePattern.Pattern, out object vPtn))
                    {
                        // TextBox
                        text = ((ValuePattern)vPtn).Current.Value;
                    }
                    else if (targetElement.TryGetCurrentPattern(TextPattern.Pattern, out object tPtn))
                    {
                        // TextBox Multilune
                        text = ((TextPattern)tPtn).DocumentRange.GetText(-1);
                    }
                    else if (targetElement.TryGetCurrentPattern(SelectionPattern.Pattern, out object sPtn))
                    {
                        // combobox
                        AutomationElement selElem = ((SelectionPattern)sPtn).Current.GetSelection()[0];
                        text = selElem.Current.Name;
                    }
                    else
                    {
                        // others
                        text = targetElement.Current.Name;
                    }

                    text.StoreInUserVariable(engine, v_Result);
                })
            );
        }
    }
}