using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using System.Windows.Automation;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Search")]
    [Attributes.ClassAttributes.CommandSettings("Search Element From Window")]
    [Attributes.ClassAttributes.Description("This command allows you to get AutomationElement from Window Name using by XPath.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get AutomationElement from Window Name. XPath does not support to use parent and sibling for root element.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationSearchElementFromWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please select the Window Name")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[SampleUsage("**Untitled - Notepad** or **%kwd_current_window%** or **{{{vWindowName}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsWindowNamesList(true)]
        //[PropertyValidationRule("Window Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Window Name")]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WindowName))]
        public string v_WindowName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Window name search method")]
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

        public UIAutomationSearchElementFromWindowCommand()
        {
            //this.CommandName = "UIAutomationGetElementFromWindowCommand";
            //this.SelectionName = "Get Element From Window";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            ////create variable window name
            //var variableWindowName = v_WindowName.ConvertToUserVariable(sender);
            //var searchMethod = v_SearchMethod.GetUISelectionValue("v_SearchMethod", this, engine);

            //string windowName = WindowNameControls.GetMatchedWindowName(variableWindowName, searchMethod, engine);

            //var windowElement = AutomationElementControls.GetFromWindowName(windowName, engine);
            //windowElement.StoreInUserVariable(engine, v_AutomationElementVariable);

            WindowNameControls.WindowAction(this, engine,
                new Action<List<(IntPtr, string)>>(wins =>
                {
                    var windowElement = AutomationElement.FromHandle(wins[0].Item1);
                    windowElement.StoreInUserVariable(engine, v_AutomationElementVariable);
                })
            );
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

        //public override void ConvertToIntermediate(EngineSettings settings, List<Script.ScriptVariable> variables)
        //{
        //    var cnv = new Dictionary<string, string>();
        //    cnv.Add("v_WindowName", "convertToIntermediateWindowName");
        //    ConvertToIntermediate(settings, cnv, variables);
        //}

        //public override void ConvertToRaw(EngineSettings settings)
        //{
        //    var cnv = new Dictionary<string, string>();
        //    cnv.Add("v_WindowName", "convertToRawWindowName");
        //    ConvertToRaw(settings, cnv);
        //}
    }
}