using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Automation;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using System.Xml.Linq;
using System.Xml.XPath;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Search")]
    [Attributes.ClassAttributes.Description("This command allows you to get AutomationElement from AutomationElement using by XPath.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get AutomationElement from AutomationElement. XPath does not support to use parent and sibling for root element.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationGetElementFromElementByXPathCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please specify AutomationElement Variable")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**{{{vElement}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.AutomationElement, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("AutomationElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Element")]
        public string v_TargetElement { get; set; }

        [XmlElement]
        [PropertyDescription("Please specify search XPath")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyCustomUIHelper("GUI Inspect Tool", "lnkInspectTool_Clicked")]
        [InputSpecification("")]
        [SampleUsage("**//Button[@Name=\"OK\"]** or **{{{vXPath}}}**")]
        [Remarks("XPath does not support to use parent, following-sibling, and preceding-sibling for root element.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.TextBox)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("XPath", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "XPath")]
        public string v_SearchXPath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify a Variable to store Result AutomationElement")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**vElement** or **{{{vElement}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.AutomationElement, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Result AutomationElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Store")]
        public string v_AutomationElementVariable { get; set; }

        //[XmlIgnore]
        //[NonSerialized]
        //private TextBox XPathTextBox;

        public UIAutomationGetElementFromElementByXPathCommand()
        {
            this.CommandName = "UIAutomationGetElementFromElementByXPathCommand";
            this.SelectionName = "Get Element From Element By XPath";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var rootElement = v_TargetElement.GetAutomationElementVariable(engine);

            Dictionary<string, AutomationElement> dic;

            XElement xml = AutomationElementControls.GetElementXml(rootElement, out dic);

            string xpath = v_SearchXPath.ConvertToUserVariable(engine);
            if (!xpath.StartsWith("."))
            {
                xpath = "." + xpath;
            }

            XElement resElem = xml.XPathSelectElement(xpath);

            if (resElem == null)
            {
                throw new Exception("AutomationElement not found XPath: " + v_SearchXPath);
            }

            AutomationElement res = dic[resElem.Attribute("Hash").Value];
            res.StoreInUserVariable(engine, v_AutomationElementVariable);
        }
        private void lnkInspectTool_Clicked(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)ControlsList[nameof(v_SearchXPath)];
            AutomationElementControls.GUIInspectTool_UsedByXPath_Clicked(txt);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctrls);

        //    XPathTextBox = (TextBox)ctrls.GetControlsByName("v_SearchXPath", CommandControls.CommandControlType.Body)[0];

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Root Element: '" + v_TargetElement + "', XPath: '" + v_SearchXPath + "', Store: '" + v_AutomationElementVariable + "']";
        //}
    }
}