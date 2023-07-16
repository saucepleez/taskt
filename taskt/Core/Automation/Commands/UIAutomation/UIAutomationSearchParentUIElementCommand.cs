using System;
using System.Windows.Automation;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Search UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Search Parent UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to get Parent UIElement from UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Parent UIElement from UIElement.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationSearchParentUIElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_NewOutputUIElementName))]
        [PropertyDescription("UIElement Variable Name to Store Parent UIElement")]
        public string v_AutomationElementVariable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_WaitTime))]
        public string v_WaitTime { get; set; }

        public UIAutomationSearchParentUIElementCommand()
        {
            //this.CommandName = "UIAutomationGetParentElementCommand";
            //this.SelectionName = "Get Parent Element";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var rootElement = v_TargetElement.GetUIElementVariable(engine);

            var waitTime = this.ConvertToUserVariableAsInteger(nameof(v_WaitTime), engine);
            object ret = WaitControls.WaitProcess(waitTime, "Parent Element",
                new Func<(bool, object)>(() =>
                {
                    try
                    {
                        var root = UIElementControls.GetParentUIElement(rootElement);
                        return (true, root);
                    }
                    catch
                    {
                        return (false, null);
                    }
                }), engine
            );
            if (ret is AutomationElement parent)
            {
                parent.StoreInUserVariable(engine, v_AutomationElementVariable);
            }
            else
            {
                throw new Exception("Parent UIElement not Found");
            }
        }
    }
}