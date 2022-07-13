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
    [Attributes.ClassAttributes.Description("This command allows you to get AutomationElement from Window Name using by XPath.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get AutomationElement from Window Name. XPath does not support to use parent and sibling for root element.")]
    public class UIAutomationGetElementFromWindowByXPathCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please specify Window Name")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**{{{vElement}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsWindowNamesList(true)]
        [PropertyValidationRule("Window Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_WindowName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select Window name search method")]
        [InputSpecification("")]
        [PropertyUISelectionOption("Contains")]
        [PropertyUISelectionOption("Starts with")]
        [PropertyUISelectionOption("Ends with")]
        [PropertyUISelectionOption("Exact match")]
        [SampleUsage("**Contains** or **Starts with** or **Ends with** or **Exact match**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsOptional(true, "Contains")]
        public string v_SearchMethod { get; set; }

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
        public string v_AutomationElementVariable { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private TextBox XPathTextBox;

        public UIAutomationGetElementFromWindowByXPathCommand()
        {
            this.CommandName = "UIAutomationGetElementFromWindowByXPathCommand";
            this.SelectionName = "Get Element From Window By XPath";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            string searchMethod = v_SearchMethod.GetUISelectionValue("v_SearchMethod", this, engine);
            string windowName = WindowNameControls.GetMatchedWindowName(v_WindowName, searchMethod, engine);
            var rootElement = AutomationElementControls.GetFromWindowName(windowName);

            if (rootElement == null)
            {
                throw new Exception("Window Name '" + v_WindowName + "' not found");
            }

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

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            XPathTextBox = (TextBox)ctrls.GetControlsByName("v_SearchXPath", CommandControls.CommandControlType.Body)[0];

            return RenderedControls;
        }

        private void lnkInspectTool_Clicked(object sender, EventArgs e)
        {
            AutomationElementControls.GUIInspectTool_UsedByXPath_Clicked(XPathTextBox);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Window Name: '" + v_WindowName + "', XPath: '" + v_SearchXPath + "', Store: '" + v_AutomationElementVariable + "']";
        }

    }
}