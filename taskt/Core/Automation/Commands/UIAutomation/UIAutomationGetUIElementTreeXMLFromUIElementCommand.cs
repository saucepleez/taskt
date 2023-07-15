using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Get From UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Get UIElement Tree XML From UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to get UIElement Tree XML from UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get UIElement Tree XML from UIElement. XML content is based on WinAppDriver UI Recorder.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationGetUIElementTreeXMLFromUIElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
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

        public UIAutomationGetUIElementTreeXMLFromUIElementCommand()
        {
            //this.CommandName = "UIAutomationGetElementTreeXMLFromElementCommand";
            //this.SelectionName = "Get Element Tree XML From Element";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetElement = v_TargetElement.GetUIElementVariable(engine);
            //var xml = AutomationElementControls.GetElementXml(targetElement, out _);
            (var xml, _) = UIElementControls.GetElementXml(targetElement);
            using(System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                xml.Save(sw);
                sw.ToString().StoreInUserVariable(engine, v_XMLVariable);
            }
        }
    }
}