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
    [Attributes.ClassAttributes.Description("This command resizes a window to a specified size.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to reize a window by name to a specific size on screen.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ResizeWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please enter or select the window that you want to resize.")]
        //[InputSpecification("Input or Type the name of the window that you want to resize.")]
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
        [PropertyDescription("Window Width (Pixcel)")]
        [InputSpecification("Window Width", true)]
        [PropertyDetailSampleUsage("**640**", PropertyDetailSampleUsage.ValueType.Value, "Width")]
        [PropertyDetailSampleUsage("**{{{vWidth}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Width")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Width", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Width")]
        public string v_XWindowSize { get; set; }

        [XmlAttribute]
        [PropertyDescription("Window Height (Pixcel)")]
        [InputSpecification("Window Height", true)]
        [PropertyDetailSampleUsage("**480**", PropertyDetailSampleUsage.ValueType.Value, "Height")]
        [PropertyDetailSampleUsage("**{{{vHeight}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Height")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Height", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Height")]
        public string v_YWindowSize { get; set; }

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

        public ResizeWindowCommand()
        {
            this.CommandName = "ResizeWindowCommand";
            this.SelectionName = "Resize Window";
            this.CommandEnabled = true;
            this.CustomRendering = true;    
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //string windowName = v_WindowName.ConvertToUserVariable(sender);
            //string serachMethod = v_SearchMethod.GetUISelectionValue("v_SearchMethod", this, engine);

            //int xSize = v_XWindowSize.ConvertToUserVariableAsInteger("X Window Size", engine);
            //int ySize = v_YWindowSize.ConvertToUserVariableAsInteger("Y Window Size", engine);

            //IntPtr wHnd = WindowNameControls.FindWindowHandle(windowName, serachMethod, engine);
            //User32Functions.SetWindowSize(wHnd, xSize, ySize);

            var width = this.ConvertToUserVariableAsInteger(nameof(v_XWindowSize), engine);
            var height = this.ConvertToUserVariableAsInteger(nameof(v_YWindowSize), engine);

            var handles = WindowNameControls.FindWindows(this, nameof(v_WindowName), nameof(v_SearchMethod), nameof(v_MatchMethod), nameof(v_TargetWindowIndex), nameof(v_WaitTime), engine);

            foreach (var whnd in handles)
            {
                User32Functions.SetWindowSize(whnd, width, height);
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
        //    //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_WindowName", this));
        //    //WindowNameControl = CommandControls.CreateStandardComboboxFor("v_WindowName", this).AddWindowNames(editor);
        //    //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_WindowName", this, new Control[] { WindowNameControl }, editor));
        //    //RenderedControls.Add(WindowNameControl);

        //    //RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_SearchMethod", this, editor));

        //    ////create standard group controls
        //    //var xGroup = CommandControls.CreateDefaultInputGroupFor("v_XWindowSize", this, editor);
        //    //var yGroup = CommandControls.CreateDefaultInputGroupFor("v_YWindowSize", this, editor);
        //    //RenderedControls.AddRange(xGroup);
        //    //RenderedControls.AddRange(yGroup);

        //    RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));
      
        //    return RenderedControls;

        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Target Window: " + v_WindowName + ", Target Size (" + v_XWindowSize + "," + v_YWindowSize + ")]";
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
        //        this.validationResult += "Windows is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_XWindowSize))
        //    {
        //        this.validationResult += "Width is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_YWindowSize))
        //    {
        //        this.validationResult += "Height is empty.\n";
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