using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.User32;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.SubGruop("Window State")]
    [Attributes.ClassAttributes.Description("This command returns a state of window name.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a window state.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetWindowStateCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please enter or select the window that you want to check existence.")]
        //[InputSpecification("Input or Type the name of the window that you want to check existence.")]
        //[SampleUsage("**Untitled - Notepad** or **%kwd_current_window%** or **{{{vWindow}}}**")]
        //[Remarks("")]
        //[PropertyIsWindowNamesList(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyCustomUIHelper("Up-to-date", "lnkUpToDate_Click")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyValidationRule("Window Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WindowName))]
        public string v_WindowName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Window title search method")]
        //[InputSpecification("")]
        //[PropertyUISelectionOption("Contains")]
        //[PropertyUISelectionOption("Starts with")]
        //[PropertyUISelectionOption("Ends with")]
        //[PropertyUISelectionOption("Exact match")]
        //[SampleUsage("**Contains** or **Starts with** or **Ends with** or **Exact match**")]
        //[Remarks("")]
        //[PropertyIsOptional(true, "Contains")]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_CompareMethod))]
        public string v_SearchMethod { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Specify the variable to assign the result")]
        //[InputSpecification("")]
        //[SampleUsage("**vSomeVariable**")]
        //[PropertyIsVariablesList(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [Remarks("Restore is **1**, Minimize is **2**, Maximize is **3**")]
        public string v_UserVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_MatchMethod_Single))]
        [PropertySelectionChangeEvent(nameof(MatchMethodComboBox_SelectionChangeCommitted))]
        public string v_MatchMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_TargetWindowIndex))]
        public string v_TargetWindowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WaitTime))]
        public string v_WaitTime { get; set; }

        //[XmlIgnore]
        //[NonSerialized]
        //public ComboBox WindowNameControl;

        public GetWindowStateCommand()
        {
            this.CommandName = "GetWindowStateCommand";
            this.SelectionName = "Get Window State";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //string windowName = v_WindowName.ConvertToUserVariable(sender);
            //string searchMethod = v_SearchMethod.GetUISelectionValue("v_SearchMethod", this, engine);
            //IntPtr hWnd = WindowNameControls.FindWindowHandle(windowName, searchMethod, engine);

            var handles = WindowNameControls.FindWindows(this, nameof(v_WindowName), nameof(v_SearchMethod), nameof(v_MatchMethod), nameof(v_TargetWindowIndex), nameof(v_WaitTime), engine);
            var hWnd = handles[0];

            User32Functions.WINDOWPLACEMENT wInfo = new User32Functions.WINDOWPLACEMENT();
            User32Functions.GetWindowPlacement(hWnd, ref wInfo);
            wInfo.showCmd.ToString().StoreInUserVariable(engine, v_UserVariableName);
        }

        public override void Refresh(frmCommandEditor editor)
        {
            base.Refresh();
            //WindowNameControl.AddWindowNames();
            ComboBox cmb = (ComboBox)ControlsList[nameof(v_MatchMethod)];
            cmb.AddWindowNames();
        }

        private void MatchMethodComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            WindowNameControls.MatchMethodComboBox_SelectionChangeCommitted(ControlsList, (ComboBox)sender, nameof(v_TargetWindowIndex));
        }

        //private int GetWindowState(IntPtr whnd)
        //{
        //    User32Functions.WINDOWPLACEMENT wInfo = new User32Functions.WINDOWPLACEMENT();
        //    User32Functions.GetWindowPlacement(whnd, ref wInfo);
        //    return wInfo.showCmd;
        //}

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    ////create window name helper control
        //    //RenderedControls.Add(UI.CustomControls.CommandControls.CreateDefaultLabelFor("v_WindowName", this));
        //    //WindowNameControl = UI.CustomControls.CommandControls.CreateStandardComboboxFor("v_WindowName", this).AddWindowNames(editor);
        //    //RenderedControls.AddRange(UI.CustomControls.CommandControls.CreateUIHelpersFor("v_WindowName", this, new Control[] { WindowNameControl }, editor));
        //    //RenderedControls.Add(WindowNameControl);

        //    //RenderedControls.AddRange(UI.CustomControls.CommandControls.CreateDefaultDropdownGroupFor("v_SearchMethod", this, editor));

        //    //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_UserVariableName", this));
        //    //var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_UserVariableName", this).AddVariableNames(editor);
        //    //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_UserVariableName", this, new Control[] { VariableNameControl }, editor));
        //    //RenderedControls.Add(VariableNameControl);

        //    //RenderedControls.AddRange(UI.CustomControls.CommandControls.CreateInferenceDefaultControlGroupFor("v_WindowName", this, editor));
        //    //RenderedControls.AddRange(UI.CustomControls.CommandControls.CreateInferenceDefaultControlGroupFor("v_UserVariableName", this, editor));

        //    RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

        //    return RenderedControls;

        //}


        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Check: " + v_WindowName + "', Result In: '" + v_UserVariableName + "']";
        //}

        //private void lnkUpToDate_Click(object sender, EventArgs e)
        //{
        //    ComboBox cmb = (ComboBox)((CommandItemControl)sender).Tag;
        //    WindowNameControls.UpdateWindowTitleCombobox(cmb);
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_WindowName))
        //    {
        //        this.validationResult += "Window is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_UserVariableName))
        //    {
        //        this.validationResult += "Variable is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}

        public override void ConvertToIntermediate(EngineSettings settings, List<Script.ScriptVariable> variables)
        {
            var cnv = new Dictionary<string, string>();
            cnv.Add("v_WindowName", "convertToIntermediateWindowName");
            ConvertToIntermediate(settings, cnv, variables);
        }

        public override void ConvertToRaw(EngineSettings settings)
        {
            var cnv = new Dictionary<string, string>();
            cnv.Add("v_WindowName", "convertToRawWindowName");
            ConvertToRaw(settings, cnv);
        }
    }
}