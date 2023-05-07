using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using System.Xml.Linq;
using System.Xml.XPath;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Search")]
    [Attributes.ClassAttributes.CommandSettings("Wait For Element Exist By XPath")]
    [Attributes.ClassAttributes.Description("This command allows you to Wait until the AutomationElement exists using by XPath.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to Wait until the AutomationElement exists using by XPath.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationWaitForElementExistByXPathCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_InputAutomationElementName))]
        public string v_TargetElement { get; set; }

        [XmlElement]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_XPath))]
        public string v_SearchXPath { get; set; }

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

        public UIAutomationWaitForElementExistByXPathCommand()
        {
            //this.CommandName = "UIAutomationWaitForElementExistByXPathCommand";
            //this.SelectionName = "Wait For Element Exist By XPath";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var rootElement = v_TargetElement.GetAutomationElementVariable(engine);

            string xpath = v_SearchXPath.ConvertToUserVariable(engine);
            if (!xpath.StartsWith("."))
            {
                xpath = "." + xpath;
            }

            int pauseTime = v_WaitTime.ConvertToUserVariableAsInteger("Wait Time", engine);
            var stopWaiting = DateTime.Now.AddSeconds(pauseTime);

            bool elementFound = false;
            while (!elementFound)
            {
                //XElement xml = AutomationElementControls.GetElementXml(rootElement, out _);
                (var xml, _) = AutomationElementControls.GetElementXml(rootElement);
                XElement resElem = xml.XPathSelectElement(xpath);
                if (resElem != null)
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

        //private void lnkInspectTool_Clicked(object sender, EventArgs e)
        //{
        //    TextBox txt = (TextBox)((CommandItemControl)sender).Tag;
        //    AutomationElementControls.GUIInspectTool_UsedByXPath_Clicked(txt);
        //}
    }
}