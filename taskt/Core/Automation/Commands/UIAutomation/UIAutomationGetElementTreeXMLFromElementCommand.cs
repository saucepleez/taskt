using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Get")]
    [Attributes.ClassAttributes.CommandSettings("Get Element Tree XML From Element")]
    [Attributes.ClassAttributes.Description("This command allows you to get Element Tree XML from AutomationElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Element Tree XML from AutomationElement. XML content is based on WinAppDriver UI Recorder.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationGetElementTreeXMLFromElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_InputAutomationElementName))]
        public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyDescription("Variable Name to store XML")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Variable Name", true)]
        [PropertyDetailSampleUsage("**vXML**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vXML}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [Remarks("XML content is based on WinAppDriver UI Recorder.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Store")]
        public string v_XMLVariable { get; set; }

        public UIAutomationGetElementTreeXMLFromElementCommand()
        {
            //this.CommandName = "UIAutomationGetElementTreeXMLFromElementCommand";
            //this.SelectionName = "Get Element Tree XML From Element";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetElement = v_TargetElement.GetAutomationElementVariable(engine);
            var xml = AutomationElementControls.GetElementXml(targetElement, out _);
            using(System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                xml.Save(sw);
                sw.ToString().StoreInUserVariable(engine, v_XMLVariable);
            }
        }
    }
}