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
    [Attributes.ClassAttributes.Description("This command allows you to check AutomationElement existence.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to check AutomationElement existence.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationCheckElementExistByXPathCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please specify AutomationElement Variable")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[SampleUsage("**{{{vElement}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.AutomationElement, true)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        //[PropertyValidationRule("AutomationElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Element")]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_InputAutomationElementName))]
        public string v_TargetElement { get; set; }

        [XmlElement]
        //[PropertyDescription("Please specify search XPath")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyCustomUIHelper("GUI Inspect Tool", "lnkInspectTool_Clicked")]
        //[InputSpecification("")]
        //[SampleUsage("**//Button[@Name=\"OK\"]** or **{{{vXPath}}}**")]
        //[Remarks("XPath does not support to use parent, following-sibling, and preceding-sibling for root element.")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.TextBox)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyValidationRule("XPath", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "XPath")]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_XPath))]
        public string v_SearchXPath { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please specify a Variable to store Result")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[SampleUsage("**vElement** or **{{{vElement}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Boolean, true)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        //[PropertyIsVariablesList(true)]
        //[PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Store")]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [Remarks("When the Element exists, Result value is **True**")]
        public string v_Result { get; set; }

        public UIAutomationCheckElementExistByXPathCommand()
        {
            this.CommandName = "UIAutomationCheckElementExistByXPathCommand";
            this.SelectionName = "Check Element Exist By XPath";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var rootElement = v_TargetElement.GetAutomationElementVariable(engine);

            XElement xml = AutomationElementControls.GetElementXml(rootElement, out _);

            string xpath = v_SearchXPath.ConvertToUserVariable(engine);
            if (!xpath.StartsWith("."))
            {
                xpath = "." + xpath;
            }

            XElement resElem = xml.XPathSelectElement(xpath);
            (resElem != null).StoreInUserVariable(engine, v_Result);
        }

        //private void lnkInspectTool_Clicked(object sender, EventArgs e)
        //{
        //    TextBox txt = (TextBox)((CommandItemControl)sender).Tag;
        //    AutomationElementControls.GUIInspectTool_UsedByXPath_Clicked(txt);
        //}
    }
}