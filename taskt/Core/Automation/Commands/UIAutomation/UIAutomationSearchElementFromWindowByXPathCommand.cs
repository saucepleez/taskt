using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.UI.Forms;
using System.Windows.Forms;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Search")]
    [Attributes.ClassAttributes.CommandSettings("Search Element From Window By XPath")]
    [Attributes.ClassAttributes.Description("This command allows you to get AutomationElement from Window Name using by XPath.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get AutomationElement from Window Name. XPath does not support to use parent and sibling for root element.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationSearchElementFromWindowByXPathCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please specify Window Name")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[SampleUsage("**{{{vElement}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsWindowNamesList(true)]
        //[PropertyValidationRule("Window Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Window Name")]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WindowName))]
        public string v_WindowName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please select Window name search method")]
        //[InputSpecification("")]
        //[PropertyUISelectionOption("Contains")]
        //[PropertyUISelectionOption("Starts with")]
        //[PropertyUISelectionOption("Ends with")]
        //[PropertyUISelectionOption("Exact match")]
        //[SampleUsage("**Contains** or **Starts with** or **Ends with** or **Exact match**")]
        //[Remarks("")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsOptional(true, "Contains")]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_CompareMethod))]
        public string v_SearchMethod { get; set; }

        [XmlElement]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_XPath))]
        public string v_SearchXPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_OutputAutomationElementName))]
        public string v_AutomationElementVariable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_MatchMethod_Single))]
        [PropertySelectionChangeEvent(nameof(MatchMethodComboBox_SelectionChangeCommitted))]
        public string v_MatchMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_TargetWindowIndex))]
        public string v_TargetWindowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WaitTime))]
        public string v_WindowWaitTime { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_WaitTime))]
        public string v_ElementWaitTime { get; set; }

        public UIAutomationSearchElementFromWindowByXPathCommand()
        {
            //this.CommandName = "UIAutomationGetElementFromWindowByXPathCommand";
            //this.SelectionName = "Get Element From Window By XPath";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var winElem = AutomationElementControls.GetWindowAutomationElement(this, engine);

            //(var xml, var dic) = AutomationElementControls.GetElementXml(winElem);

            //var xpath = v_SearchXPath.ConvertToUserVariableAsXPath(engine);

            //XElement resElem = xml.XPathSelectElement(xpath) ?? throw new Exception("AutomationElement not found XPath: " + v_SearchXPath);

            //AutomationElement res = dic[resElem.Attribute("Hash").Value];
            //res.StoreInUserVariable(engine, v_AutomationElementVariable);

            var elem = AutomationElementControls.SearchGUIElementByXPath(this, winElem, nameof(v_SearchXPath), nameof(v_ElementWaitTime), engine);
            elem.StoreInUserVariable(engine, v_AutomationElementVariable);
        }

        private void MatchMethodComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            WindowNameControls.MatchMethodComboBox_SelectionChangeCommitted(ControlsList, (ComboBox)sender, nameof(v_TargetWindowIndex));
        }

        public override void Refresh(frmCommandEditor editor)
        {
            base.Refresh();
            var cmb = (ComboBox)ControlsList[nameof(v_WindowName)];
            cmb.AddWindowNames();
        }
    }
}