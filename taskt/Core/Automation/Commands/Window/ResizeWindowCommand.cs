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
    [Attributes.ClassAttributes.Description("This command resizes a window to a specified size.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to reize a window by name to a specific size on screen.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'SetWindowPos' from user32.dll to achieve automation.")]
    public class ResizeWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter or select the window that you want to resize.")]
        [Attributes.PropertyAttributes.InputSpecification("Input or Type the name of the window that you want to resize.")]
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
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the new required width (pixel) of the window.")]
        [Attributes.PropertyAttributes.InputSpecification("Input the new width size of the window")]
        [Attributes.PropertyAttributes.SampleUsage("**640** or **{{{vWidth}}}**")]
        [Attributes.PropertyAttributes.Remarks("This number is limited by your resolution. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid width range could be 0-1920")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_XWindowSize { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the new required height (pixel) of the window.")]
        [Attributes.PropertyAttributes.InputSpecification("Input the new height size of the window")]
        [Attributes.PropertyAttributes.SampleUsage("**480** or **{{{vHeight}}}**")]
        [Attributes.PropertyAttributes.Remarks("This number is limited by your resolution. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid height range could be 0-1080")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_YWindowSize { get; set; }

        [XmlIgnore]
        [NonSerialized]
        public ComboBox WindowNameControl;
        public ResizeWindowCommand()
        {
            this.CommandName = "ResizeWindowCommand";
            this.SelectionName = "Resize Window";
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
            var variableXSize = v_XWindowSize.ConvertToUserVariable(sender);
            var variableYSize = v_YWindowSize.ConvertToUserVariable(sender);

            if (!int.TryParse(variableXSize, out int xPos))
            {
                throw new Exception("X Position Invalid - " + v_XWindowSize);
            }
            if (!int.TryParse(variableYSize, out int yPos))
            {
                throw new Exception("X Position Invalid - " + v_YWindowSize);
            }

            if (searchMethod == "Contains" || targetIsCurrentWindow)
            {
                //loop each window and set the window state
                foreach (var targetedWindow in targetWindows)
                {
                    User32Functions.SetWindowSize(targetedWindow, xPos, yPos);
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

                bool isResize = false;
                foreach (var targetedWindow in targetWindows)
                {
                    if (searchFunc(User32Functions.GetWindowTitle(targetedWindow)))
                    {
                        User32Functions.SetWindowSize(targetedWindow, xPos, yPos);
                        isResize = true;
                    }
                }
                if (!isResize)
                {
                    throw new Exception("Window name '" + windowName + "' is not found.Search method " + searchMethod + ".");
                }
            }

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create window name helper control
            //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_WindowName", this));
            //WindowNameControl = CommandControls.CreateStandardComboboxFor("v_WindowName", this).AddWindowNames(editor);
            //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_WindowName", this, new Control[] { WindowNameControl }, editor));
            //RenderedControls.Add(WindowNameControl);

            //RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_SearchMethod", this, editor));

            ////create standard group controls
            //var xGroup = CommandControls.CreateDefaultInputGroupFor("v_XWindowSize", this, editor);
            //var yGroup = CommandControls.CreateDefaultInputGroupFor("v_YWindowSize", this, editor);
            //RenderedControls.AddRange(xGroup);
            //RenderedControls.AddRange(yGroup);

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
            return base.GetDisplayValue() + " [Target Window: " + v_WindowName + ", Target Size (" + v_XWindowSize + "," + v_YWindowSize + ")]";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_WindowName))
            {
                this.validationResult += "Windows is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_XWindowSize))
            {
                this.validationResult += "Width is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_YWindowSize))
            {
                this.validationResult += "Height is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}