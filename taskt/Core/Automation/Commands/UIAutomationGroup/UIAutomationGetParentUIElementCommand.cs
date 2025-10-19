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
    [Attributes.ClassAttributes.CommandSettings("Get Parent UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to get Parent UIElement from UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Parent UIElement from UIElement.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationGetParentUIElementCommand : AGetFromUIElementCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        //public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_NewOutputUIElementName))]
        [PropertyDescription("UIElement Variable Name to Store Parent UIElement")]
        [PropertyParameterOrder(6000)]
        public string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_WaitTime))]
        //public string v_WaitTime { get; set; }

        public UIAutomationGetParentUIElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var rootElement = v_TargetElement.ExpandUserVariableAsUIElement(engine);

            //var waitTime = this.ExpandValueOrUserVariableAsInteger(nameof(v_WaitTime), engine);
            //object ret = WaitControls.WaitProcess(waitTime, "Parent Element",
            //    new Func<(bool, object)>(() =>
            //    {
            //        try
            //        {
            //            var root = UIElementControls.GetParentUIElement(rootElement);
            //            return (true, root);
            //        }
            //        catch
            //        {
            //            return (false, null);
            //        }
            //    }), engine
            //);
            //if (ret is AutomationElement parent)
            //{
            //    parent.StoreInUserVariable(engine, v_AutomationElementVariable);
            //}
            //else
            //{
            //    throw new Exception("Parent UIElement not Found");
            //}

            this.UIElementAction(engine,
                new Action<AutomationElement>((targetElement) =>
                {
                    try
                    {
                        var p = EM_CanHandleUIElementExtentionMethods.GetParentUIElement(targetElement);
                        p.StoreInUserVariable(engine, v_Result);
                    }
                    catch
                    {
                        this.ValueCanNotRetrievedProcess("Parent Element", new Action(() =>
                        {
                            "".StoreInUserVariable(engine, v_Result);
                        }), engine);
                    }
                })
            );
        }
    }
}