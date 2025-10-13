using System;
using System.Windows.Automation;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.UIAutomationGroup;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation")]
    [Attributes.ClassAttributes.SubGruop("UIElement Action")]
    [Attributes.ClassAttributes.CommandSettings("Set Text To UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to set Text Value from UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to set Text Value from UIElement.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationSetTextToUIElementCommand : AUIElementActionCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        //public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_OneLineTextBox))]
        [PropertyDescription("Text to Set")]
        [InputSpecification("Text", true)]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value)]
        [PropertyDetailSampleUsage("**{{{vText}}}**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyDisplayText(true, "Text")]
        [PropertyParameterOrder(6000)]
        public string v_TextToSet { get; set; }

        public UIAutomationSetTextToUIElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var targetElement = v_TargetElement.ExpandUserVariableAsUIElement(engine);

            //var ct = targetElement.GetCurrentPropertyValue(AutomationElement.ControlTypeProperty) as ControlType;
            //if (ct == ControlType.Spinner)
            //{
            //    targetElement = UIElementControls.SearchGUIElementByXPath(targetElement, "/Edit[1]", 10, engine);
            //    targetElement = targetElement.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
            //}

            //string textValue = v_TextVariable.ExpandValueOrUserVariable(engine);

            //if (targetElement.TryGetCurrentPattern(ValuePattern.Pattern, out object valPtn))
            //{
            //    ((ValuePattern)valPtn).SetValue(textValue);
            //}
            //else if (targetElement.TryGetCurrentPattern(TextPattern.Pattern, out _))
            //{
            //    targetElement.SetFocus();
            //    System.Threading.Thread.Sleep(100);
            //    SendKeys.SendWait("^{HOME}");
            //    SendKeys.SendWait("^+{END}");
            //    SendKeys.SendWait("{DEL}");
            //    SendKeys.SendWait(textValue);
            //}
            //else
            //{
            //    throw new Exception("UIElement '" + v_TargetElement + "' can not set Text");
            //}

            this.UIElementActionAndWait(engine,
                new Action<AutomationElement, IntPtr>((targetElement, whnd) =>
                {
                    var ct = targetElement.GetCurrentPropertyValue(AutomationElement.ControlTypeProperty) as ControlType;
                    if (ct == ControlType.Spinner)
                    {
                        targetElement = targetElement.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
                    }

                    string textValue = v_TextToSet.ExpandValueOrUserVariable(engine);

                    if (targetElement.TryGetCurrentPattern(ValuePattern.Pattern, out object valPtn))
                    {
                        ((ValuePattern)valPtn).SetValue(textValue);
                    }
                    else if (targetElement.TryGetCurrentPattern(TextPattern.Pattern, out _))
                    {
                        targetElement.SetFocus();
                        System.Threading.Thread.Sleep(100);
                        //SendKeys.SendWait("^{HOME}");
                        //SendKeys.SendWait("^+{END}");
                        //SendKeys.SendWait("{DEL}");
                        //SendKeys.SendWait(textValue);

                        var sendKey = new EnterKeysFromWindowHandleCommand()
                        {
                            v_WindowHandle = whnd.ToString(),
                            v_TextToSend = "^{HOME}",
                        };
                        sendKey.RunCommand(engine);
                        sendKey.v_TextToSend = "^+{END}";
                        sendKey.RunCommand(engine);
                        sendKey.v_TextToSend = "{DEL}";
                        sendKey.RunCommand(engine);
                        sendKey.v_TextToSend = textValue;
                        sendKey.RunCommand(engine);
                    }
                    else
                    {
                        //throw new Exception("UIElement '" + v_TargetElement + "' can not set Text");
                        this.ActionNotSupportedProcess("Set Text", engine);
                    }
                })
            );
        }
    }
}