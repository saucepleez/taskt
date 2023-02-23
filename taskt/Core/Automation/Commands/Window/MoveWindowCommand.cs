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
    [Attributes.ClassAttributes.Description("This command moves a window to a specified location on screen.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to move an existing window by name to a certain point on the screen.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class MoveWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please enter or select the window that you want to move.")]
        //[InputSpecification("Input or Type the name of the window that you want to move.")]
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
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("X horizontal coordinate (pixel) for the Window's Location")]
        [InputSpecification("X Window Location", true)]
        //[SampleUsage("**0** or **{{{vXPos}}}** or **%kwd_current_position%**")]
        [PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        [PropertyDetailSampleUsage("**0**", "Specify X Top Position")]
        [PropertyDetailSampleUsage("**100**", PropertyDetailSampleUsage.ValueType.Value, "X Position")]
        [PropertyDetailSampleUsage("**{{{vXPos}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "X Position")]
        [PropertyDetailSampleUsage("**%kwd_current_position%**", "Spcify Current Position for X Position")]
        [Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range could be 0-1920")]
        [PropertyValidationRule("X Position", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "X Position")]
        public string v_XWindowPosition { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Y vertical coordinate (pixel) for the Window's Location")]
        [InputSpecification("Y Window Location", true)]
        //[SampleUsage("**0** or **{{{vYPos}}}** or **%kwd_current_position%**")]
        [PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        [PropertyDetailSampleUsage("**0**", "Specify Y Left Position")]
        [PropertyDetailSampleUsage("**100**", PropertyDetailSampleUsage.ValueType.Value, "Y Position")]
        [PropertyDetailSampleUsage("**{{{vYPos}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Y Position")]
        [PropertyDetailSampleUsage("**%kwd_current_position%**", "Spcify Current Position for Y Position")]
        [Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range could be 0-1080")]
        [PropertyValidationRule("Y Position", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Y Position")]
        public string v_YWindowPosition { get; set; }

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

        public MoveWindowCommand()
        {
            this.CommandName = "MoveWindowCommand";
            this.SelectionName = "Move Window";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //string windowName = v_WindowName.ConvertToUserVariable(sender);
            //string searchMethod = v_SearchMethod.GetUISelectionValue("v_SearchMethod", this, engine);
            //IntPtr wHnd = WindowNameControls.FindWindowHandle(windowName, searchMethod, engine);

            var handles = WindowNameControls.FindWindows(this, nameof(v_WindowName), nameof(v_SearchMethod), nameof(v_MatchMethod), nameof(v_TargetWindowIndex), nameof(v_WaitTime), engine);

            foreach(var whnd in handles)
            {
                User32Functions.RECT pos = User32Functions.GetWindowPosition(whnd);

                var variableXPosition = v_XWindowPosition.ConvertToUserVariable(sender);
                int xPos;
                if ((variableXPosition == engine.engineSettings.CurrentWindowPositionKeyword) || (variableXPosition == engine.engineSettings.CurrentWindowXPositionKeyword))
                {
                    xPos = pos.left;
                }
                else if (variableXPosition == engine.engineSettings.CurrentWindowYPositionKeyword)
                {
                    xPos = pos.top;
                }
                else
                {
                    xPos = v_XWindowPosition.ConvertToUserVariableAsInteger("X Position", engine);
                }

                var variableYPosition = v_YWindowPosition.ConvertToUserVariable(sender);
                int yPos;
                if ((variableYPosition == engine.engineSettings.CurrentWindowPositionKeyword) || (variableYPosition == engine.engineSettings.CurrentWindowYPositionKeyword))
                {
                    yPos = pos.top;
                }
                else if (variableYPosition == engine.engineSettings.CurrentWindowXPositionKeyword)
                {
                    yPos = pos.left;
                }
                else
                {
                    yPos = v_YWindowPosition.ConvertToUserVariableAsInteger("Y Position", engine);
                }

                User32Functions.SetWindowPosition(whnd, xPos, yPos);
            }
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

        //private static void MoveWindow(IntPtr hwnd, int xPos, int yPos, string xKeyword, string yKeyword, EngineSettings setting)
        //{
        //    int xWin, yWin;
        //    User32Functions.RECT rc = User32Functions.GetWindowPosition(hwnd);
        //    xWin = rc.left;
        //    yWin = rc.top;

        //    if ((xKeyword == setting.CurrentWindowPositionKeyword) || (xKeyword == setting.CurrentWindowXPositionKeyword))
        //    {
        //        xPos = xWin;
        //    }
        //    else if (xKeyword == setting.CurrentWindowYPositionKeyword)
        //    {
        //        xPos = yWin;
        //    }
        //    if ((yKeyword == setting.CurrentWindowPositionKeyword) || (yKeyword == setting.CurrentWindowYPositionKeyword))
        //    {
        //        yPos = yWin;
        //    }
        //    else if (yKeyword == setting.CurrentWindowXPositionKeyword)
        //    {
        //        yPos = xWin;
        //    }

        //    User32Functions.SetWindowPosition(hwnd, xPos, yPos);
        //}

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //create window name helper control
        //    //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_WindowName", this));
        //    //WindowNameControl = CommandControls.CreateStandardComboboxFor("v_WindowName", this).AddWindowNames(editor);
        //    //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_WindowName", this, new Control[] { WindowNameControl }, editor));
        //    //RenderedControls.Add(WindowNameControl);

        //    //RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_SearchMethod", this, editor));

        //    //var xGroup = CommandControls.CreateDefaultInputGroupFor("v_XWindowPosition", this, editor);
        //    //var yGroup = CommandControls.CreateDefaultInputGroupFor("v_YWindowPosition", this, editor);
        //    //RenderedControls.AddRange(xGroup);
        //    //RenderedControls.AddRange(yGroup);

        //    //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_XWindowPosition", this));
        //    //var xPositionControl = CommandControls.CreateDefaultInputFor("v_XWindowPosition", this);
        //    //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_XWindowPosition", this, new Control[] { xPositionControl }, editor));
        //    //RenderedControls.Add(xPositionControl);

        //    //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_YWindowPosition", this));
        //    //var yPositionControl = CommandControls.CreateDefaultInputFor("v_YWindowPosition", this);
        //    //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_YWindowPosition", this, new Control[] { yPositionControl }, editor));
        //    //RenderedControls.Add(yPositionControl);

        //    RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));


        //    return RenderedControls;

        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Target Window: " + v_WindowName + ", Target Coordinates (" + v_XWindowPosition + "," + v_YWindowPosition + ")]";
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
        //    if (String.IsNullOrEmpty(this.v_XWindowPosition))
        //    {
        //        this.validationResult += "X is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_YWindowPosition))
        //    {
        //        this.validationResult += "Y is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}

        public override void ConvertToIntermediate(EngineSettings settings, List<Script.ScriptVariable> variables)
        {
            var cnv = new Dictionary<string, string>();
            cnv.Add("v_WindowName", "convertToIntermediateWindowName");
            cnv.Add("v_XWindowPosition", "convertToIntermediateWindowPosition");
            cnv.Add("v_YWindowPosition", "convertToIntermediateWindowPosition");
            ConvertToIntermediate(settings, cnv, variables);
        }

        public override void ConvertToRaw(EngineSettings settings)
        {
            var cnv = new Dictionary<string, string>();
            cnv.Add("v_WindowName", "convertToRawWindowName");
            cnv.Add("v_XWindowPosition", "convertToRawWindowPosition");
            cnv.Add("v_YWindowPosition", "convertToRawWindowPosition");
            ConvertToRaw(settings, cnv);
        }
    }
}