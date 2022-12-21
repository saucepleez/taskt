using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.User32;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.SubGruop("Window Actions")]
    [Attributes.ClassAttributes.Description("This command closes an open window.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to close an existing window by name.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class CloseWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please enter or select the window that you want to close. (ex. Notepad, %kwd_current_window%, {{{vWindow}}})")]
        [InputSpecification("Input or Type the name of the window that you want to close.")]
        [SampleUsage("**Untitled - Notepad** or **%kwd_current_window%** or **{{{vWindow}}}**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyCustomUIHelper("Up-to-date", "lnkUpToDate_Click")]
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

        [XmlIgnore]
        [NonSerialized]
        public ComboBox WindowNameControl;

        public CloseWindowCommand()
        {
            this.CommandName = "CloseWindowCommand";
            this.SelectionName = "Close Window";
            this.CommandEnabled = true;
            this.CustomRendering = true;

        }

        public override void RunCommand(object sender)
        {
            //string windowName = v_WindowName.ConvertToUserVariable(sender);
            //string searchMethod = v_SearchMethod.ConvertToUserVariable(sender);
            //if (String.IsNullOrEmpty(searchMethod))
            //{
            //    searchMethod = "Contains";
            //}

            //bool targetIsCurrentWindow = ((Automation.Engine.AutomationEngineInstance)sender).engineSettings.CurrentWindowKeyword == windowName;

            //var targetWindows = User32Functions.FindTargetWindows(windowName, targetIsCurrentWindow, (searchMethod != "Contains"));

            //if (searchMethod == "Contains" || targetIsCurrentWindow)
            //{
            //    //loop each window
            //    foreach (var targetedWindow in targetWindows)
            //    {
            //        User32Functions.CloseWindow(targetedWindow);
            //    }
            //}
            //else
            //{
            //    Func<string, bool> searchFunc;
            //    switch (searchMethod)
            //    {
            //        case "Starts with":
            //            searchFunc = (s) => s.StartsWith(windowName);
            //            break;

            //        case "Ends with":
            //            searchFunc = (s) => s.EndsWith(windowName);
            //            break;

            //        case "Exact match":
            //            searchFunc = (s) => (s == windowName);
            //            break;

            //        default:
            //            throw new Exception("Search method " + searchMethod + " is not support.");
            //            break;
            //    }

            //    bool isCloseWindow = false;

            //    //loop each window
            //    foreach (var targetedWindow in targetWindows)
            //    {
            //        if (searchFunc(User32Functions.GetWindowTitle(targetedWindow)))
            //        {
            //            User32Functions.CloseWindow(targetedWindow);
            //            isCloseWindow = true;
            //        }
            //    }

            //    if (!isCloseWindow)
            //    {
            //        throw new Exception("Window name " + windowName + " is not found. Search method is " + searchMethod + ".");
            //    }
            //}

            var engine = (Automation.Engine.AutomationEngineInstance)sender;

            string windowName = v_WindowName.ConvertToUserVariable(sender);
            string searchMethod = v_SearchMethod.GetUISelectionValue("v_SearchMethod", this, engine);

            var handles = WindowNameControls.FindWindows(windowName, searchMethod, engine);
            foreach (var handle in handles)
            {
                User32Functions.CloseWindow(handle);
            }
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create window name helper control
            //RenderedControls.Add(UI.CustomControls.CommandControls.CreateDefaultLabelFor("v_WindowName", this));
            //WindowNameControl = UI.CustomControls.CommandControls.CreateStandardComboboxFor("v_WindowName", this).AddWindowNames(editor);
            //RenderedControls.AddRange(UI.CustomControls.CommandControls.CreateUIHelpersFor("v_WindowName", this, new Control[] { WindowNameControl }, editor));
            //RenderedControls.Add(WindowNameControl);

            //RenderedControls.AddRange(UI.CustomControls.CommandControls.CreateDefaultDropdownGroupFor("v_SearchMethod", this, editor));

            // DBG
            //User32Functions.GetWindowNames();


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
            return base.GetDisplayValue() + " [Target Window: " + v_WindowName + "]";
        }

        private void lnkUpToDate_Click(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)((CommandItemControl)sender).Tag;
            WindowNameControls.UpdateWindowTitleCombobox(cmb);
        }

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