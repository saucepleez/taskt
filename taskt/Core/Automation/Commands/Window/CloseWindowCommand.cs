using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
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
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'SendMessage' from user32.dll to achieve automation.")]
    public class CloseWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter or select the window that you want to close. (ex. Notepad, %kwd_current_window%, {{{vWindow}}})")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Input or Type the name of the window that you want to close.")]
        [Attributes.PropertyAttributes.SampleUsage("**Untitled - Notepad** or **%kwd_current_window%** or **{{{vWindow}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsWindowNamesList(true)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_WindowName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Window title search method (Default is Contains)")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Contains")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Start with")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("End with")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Exact match")]
        [Attributes.PropertyAttributes.SampleUsage("**Contains** or **Start with** or **End with** or **Exact match**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
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
            string windowName = v_WindowName.ConvertToUserVariable(sender);
            string searchMethod = v_SearchMethod.ConvertToUserVariable(sender);
            if (String.IsNullOrEmpty(searchMethod))
            {
                searchMethod = "Contains";
            }

            bool targetIsCurrentWindow = ((Automation.Engine.AutomationEngineInstance)sender).engineSettings.CurrentWindowKeyword == windowName;

            var targetWindows = User32Functions.FindTargetWindows(windowName, targetIsCurrentWindow, (searchMethod != "Contains"));

            if (searchMethod == "Contains" || targetIsCurrentWindow)
            {
                //loop each window
                foreach (var targetedWindow in targetWindows)
                {
                    User32Functions.CloseWindow(targetedWindow);
                }
            }
            else
            {
                Func<string, bool> searchFunc;
                switch (searchMethod)
                {
                    case "Start with":
                        searchFunc = (s) => s.StartsWith(windowName);
                        break;

                    case "End with":
                        searchFunc = (s) => s.EndsWith(windowName);
                        break;

                    case "Exact match":
                        searchFunc = (s) => (s == windowName);
                        break;

                    default:
                        throw new Exception("Search method " + searchMethod + " is not support.");
                        break;
                }

                bool isCloseWindow = false;

                //loop each window
                foreach (var targetedWindow in targetWindows)
                {
                    if (searchFunc(User32Functions.GetWindowTitle(targetedWindow)))
                    {
                        User32Functions.CloseWindow(targetedWindow);
                        isCloseWindow = true;
                    }
                }

                if (!isCloseWindow)
                {
                    throw new Exception("Window name " + windowName + " is not found. Search method is " + searchMethod + ".");
                }
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



            RenderedControls.AddRange(UI.CustomControls.CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

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

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_WindowName))
            {
                this.validationResult += "Window is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}