using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.User32;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.SubGruop("Window Actions")]
    [Attributes.ClassAttributes.Description("This command closes an open window.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to close an existing window by name.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CloseWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please enter or select the window that you want to close. (ex. Notepad, %kwd_current_window%, {{{vWindow}}})")]
        //[InputSpecification("Input or Type the name of the window that you want to close.")]
        //[SampleUsage("**Untitled - Notepad** or **%kwd_current_window%** or **{{{vWindow}}}**")]
        //[Remarks("")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyCustomUIHelper("Up-to-date", "lnkUpToDate_Click")]
        //[PropertyIsWindowNamesList(true)]
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
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsOptional(true, "Contains")]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_CompareMethod))]
        public string v_SearchMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_MatchMethod))]
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

        public CloseWindowCommand()
        {
            this.CommandName = "CloseWindowCommand";
            this.SelectionName = "Close Window";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Automation.Engine.AutomationEngineInstance)sender;

            //string windowName = v_WindowName.ConvertToUserVariable(sender);
            //string searchMethod = v_SearchMethod.GetUISelectionValue("v_SearchMethod", this, engine);

            //var handles = WindowNameControls.FindWindowsHandles(windowName, searchMethod, engine);
            //foreach (var handle in handles)
            //{
            //    User32Functions.CloseWindow(handle);
            //}

            var handles = WindowNameControls.FindWindows(this, nameof(v_WindowName), nameof(v_SearchMethod), nameof(v_MatchMethod), nameof(v_TargetWindowIndex), nameof(v_WaitTime), engine);

            foreach(var whnd in handles)
            {
                User32Functions.CloseWindow(whnd);
            }
        }

        private void MatchMethodComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            WindowNameControls.MatchMethodComboBox_SelectionChangeCommitted(ControlsList, (ComboBox)sender, nameof(v_TargetWindowIndex));
        }

        public override void Refresh(frmCommandEditor editor)
        {
            base.Refresh();
            //WindowNameControl.AddWindowNames();
            ComboBox cmb = (ComboBox)ControlsList[nameof(v_WindowName)];
            cmb.AddWindowNames();
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //create window name helper control
        //    //RenderedControls.Add(UI.CustomControls.CommandControls.CreateDefaultLabelFor("v_WindowName", this));
        //    //WindowNameControl = UI.CustomControls.CommandControls.CreateStandardComboboxFor("v_WindowName", this).AddWindowNames(editor);
        //    //RenderedControls.AddRange(UI.CustomControls.CommandControls.CreateUIHelpersFor("v_WindowName", this, new Control[] { WindowNameControl }, editor));
        //    //RenderedControls.Add(WindowNameControl);

        //    //RenderedControls.AddRange(UI.CustomControls.CommandControls.CreateDefaultDropdownGroupFor("v_SearchMethod", this, editor));

        //    // DBG
        //    //User32Functions.GetWindowNames();


        //    RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Target Window: " + v_WindowName + "]";
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

        //    return this.IsValid;
        //}

        public override void ConvertToIntermediate(EngineSettings settings, List<Core.Script.ScriptVariable> variables)
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