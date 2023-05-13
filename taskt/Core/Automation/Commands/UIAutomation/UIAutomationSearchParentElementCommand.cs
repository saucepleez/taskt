using System;
using System.Windows.Automation;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Search Element")]
    [Attributes.ClassAttributes.CommandSettings("Search Parent Element")]
    [Attributes.ClassAttributes.Description("This command allows you to get Parent Element from AutomationElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Parent Element from AutomationElement.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationSearchParentElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_InputAutomationElementName))]
        public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_NewOutputAutomationElementName))]
        [PropertyDescription("AutomationElement Variable Name to Store Parent AutomationElement")]
        public string v_AutomationElementVariable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_WaitTime))]
        public string v_WaitTime { get; set; }

        public UIAutomationSearchParentElementCommand()
        {
            //this.CommandName = "UIAutomationGetParentElementCommand";
            //this.SelectionName = "Get Parent Element";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var rootElement = v_TargetElement.GetAutomationElementVariable(engine);

            var waitTime = this.ConvertToUserVariableAsInteger(nameof(v_WaitTime), engine);
            object ret = WaitControls.WaitProcess(waitTime, "Parent Element",
                new Func<(bool, object)>(() =>
                {
                    try
                    {
                        var root = AutomationElementControls.GetParentElement(rootElement);
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
                throw new Exception("Parent AutomationElement not Found");
            }
        }
    }
}