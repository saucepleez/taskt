using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.SubGruop("Window Actions")]
    [Attributes.ClassAttributes.Description("This command waits for a window to exist.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to explicitly wait for a window to exist before continuing script execution.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'ShowWindow' from user32.dll to achieve automation.")]
    public class WaitForWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please enter or select the window name that you are waiting for to exist.")]
        [InputSpecification("Input or Type the name of the window that you want to wait to exist.")]
        [SampleUsage("**Untitled - Notepad** or **%kwd_current_window%** or **{{{vWindow}}}**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyCustomUIHelper("Up-to-date", nameof(lnkUpToDate_Click))]
        [PropertyIsWindowNamesList(true)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Window Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [PropertyDescription("Window title search method")]
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
        [XmlAttribute]
        [PropertyDescription("Indicate how many seconds to wait before an error should be raised.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Specify how many seconds to wait before an error should be invoked")]
        [SampleUsage("**5** or **{{{vWaitTime}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Wait Time", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        public string v_LengthToWait { get; set; }

        [XmlIgnore]
        [NonSerialized]
        public ComboBox WindowNameControl;

        public WaitForWindowCommand()
        {
            this.CommandName = "WaitForWindowCommand";
            this.SelectionName = "Wait For Window To Exist";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            string windowName = v_WindowName.ConvertToUserVariable(sender);
            string searchMethod = v_SearchMethod.GetUISelectionValue("v_SearchMethod", this, engine);

            //var waitUntil = v_LengthToWait.ConvertToUserVariableAsInteger("Length to Wait", engine);
            //var endDateTime = DateTime.Now.AddSeconds(waitUntil);

            //bool isFind = false;
            //while (DateTime.Now < endDateTime)
            //{
            //    try
            //    {
            //        IntPtr wHnd = WindowNameControls.FindWindow(windowName, searchMethod, engine);
            //        isFind = true;
            //        break;
            //    }
            //    catch
            //    {

            //    }
            //    engine.ReportProgress("Window Not Yet Found... " + (int)((endDateTime - DateTime.Now).TotalSeconds) + "s remain");
            //    System.Threading.Thread.Sleep(1000);
            //}

            //if (!isFind)
            //{
            //    throw new Exception("Window was not found in the allowed time!");
            //}

            Func<bool> windowWaitFunc = new Func<bool>(() =>
            {
                try
                {
                    IntPtr wHnd = WindowNameControls.FindWindowHandle(windowName, searchMethod, engine);
                    return true;
                }
                catch
                {
                    return false;
                }
            });
            this.WaitProcess(nameof(v_LengthToWait), "Window", windowWaitFunc, engine);
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

            return RenderedControls;

        }

        public override void Refresh(frmCommandEditor editor)
        {
            base.Refresh();
            WindowNameControl.AddWindowNames();
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: '" + v_WindowName + "', Wait Up To " + v_LengthToWait + " seconds]";
        }

        private void lnkUpToDate_Click(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)((CommandItemControl)sender).Tag;
            WindowNameControls.UpdateWindowTitleCombobox(cmb);
        }

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