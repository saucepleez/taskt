using System;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Automation;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Search")]
    [Attributes.ClassAttributes.CommandSettings("Wait For Element Exist")]
    [Attributes.ClassAttributes.Description("This command allows you to Wait until the AutomationElement exists.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to Wait until the AutomationElement exists.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationWaitForElementExistCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_InputAutomationElementName))]
        public string v_TargetElement { get; set; }

        [XmlElement]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_SearchParameters))]
        public DataTable v_SearchParameters { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify how many seconds to wait for the AutomationElement to exist")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**10** or **{{{vWait}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Wait Time", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Wait", "s")]
        public string v_WaitTime { get; set; }

        public UIAutomationWaitForElementExistCommand()
        {
            //this.CommandName = "UIAutomationWaitForElementExistCommand";
            //this.SelectionName = "Wait For Element Exist";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var rootElement = v_TargetElement.GetAutomationElementVariable(engine);

            int pauseTime = v_WaitTime.ConvertToUserVariableAsInteger("Wait Time", engine);
            var stopWaiting = DateTime.Now.AddSeconds(pauseTime);

            bool elementFound = false;
            while (!elementFound)
            {
                AutomationElement elem = AutomationElementControls.SearchGUIElement(rootElement, v_SearchParameters, engine);
                if (elem != null)
                {
                    elementFound = true;
                }

                if (DateTime.Now > stopWaiting)
                {
                    throw new Exception("AutomationElement was not found in time!");
                }

                engine.ReportProgress("AutomationElement Not Yet Found... " + (int)((stopWaiting - DateTime.Now).TotalSeconds) + "s remain");
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}