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
    [Attributes.ClassAttributes.Description("This command moves a window to a specified location on screen.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to move an existing window by name to a certain point on the screen.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'SetWindowPos' from user32.dll to achieve automation.")]
    public class MoveWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter or select the window that you want to move.")]
        [Attributes.PropertyAttributes.InputSpecification("Input or Type the name of the window that you want to move.")]
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
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the new X horizontal coordinate (pixel) for the window's location.  0 starts at the left of the screen.")]
        [Attributes.PropertyAttributes.InputSpecification("Input the new horizontal coordinate of the window, 0 starts at the left and goes to the right")]
        [Attributes.PropertyAttributes.SampleUsage("**0** or **{{{vXPos}}}**")]
        [Attributes.PropertyAttributes.Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range could be 0-1920")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_XWindowPosition { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the new Y vertical coordinate (pixel) for the window's location.  0 starts at the top of the screen.")]
        [Attributes.PropertyAttributes.InputSpecification("Input the new vertical coordinate of the window, 0 starts at the top and goes downwards")]
        [Attributes.PropertyAttributes.SampleUsage("**0** or **{{{vYPos}}}**")]
        [Attributes.PropertyAttributes.Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range could be 0-1080")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_YWindowPosition { get; set; }

        [XmlIgnore]
        [NonSerialized]
        public ComboBox WindowNameControl;


        public MoveWindowCommand()
        {
            this.CommandName = "MoveWindowCommand";
            this.SelectionName = "Move Window";
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

            var variableXPosition = v_XWindowPosition.ConvertToUserVariable(sender);
            var variableYPosition = v_YWindowPosition.ConvertToUserVariable(sender);

            if (!int.TryParse(variableXPosition, out int xPos))
            {
                throw new Exception("X Position Invalid - " + v_XWindowPosition);
            }
            if (!int.TryParse(variableYPosition, out int yPos))
            {
                throw new Exception("X Position Invalid - " + v_XWindowPosition);
            }

            if (searchMethod == "Contains" || targetIsCurrentWindow)
            {
                //loop each window
                foreach (var targetedWindow in targetWindows)
                {
                    User32Functions.SetWindowPosition(targetedWindow, xPos, yPos);
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

                bool isMoveWindow = false;
                //loop each window
                foreach (var targetedWindow in targetWindows)
                {
                    if (searchFunc(User32Functions.GetWindowTitle(targetedWindow)))
                    {
                        User32Functions.SetWindowPosition(targetedWindow, xPos, yPos);
                        isMoveWindow = true;
                    }
                }
                if (!isMoveWindow)
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

            //var xGroup = CommandControls.CreateDefaultInputGroupFor("v_XWindowPosition", this, editor);
            //var yGroup = CommandControls.CreateDefaultInputGroupFor("v_YWindowPosition", this, editor);
            //RenderedControls.AddRange(xGroup);
            //RenderedControls.AddRange(yGroup);

            //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_XWindowPosition", this));
            //var xPositionControl = CommandControls.CreateDefaultInputFor("v_XWindowPosition", this);
            //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_XWindowPosition", this, new Control[] { xPositionControl }, editor));
            //RenderedControls.Add(xPositionControl);

            //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_YWindowPosition", this));
            //var yPositionControl = CommandControls.CreateDefaultInputFor("v_YWindowPosition", this);
            //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_YWindowPosition", this, new Control[] { yPositionControl }, editor));
            //RenderedControls.Add(yPositionControl);

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
            return base.GetDisplayValue() + " [Target Window: " + v_WindowName + ", Target Coordinates (" + v_XWindowPosition + "," + v_YWindowPosition + ")]";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_WindowName))
            {
                this.validationResult += "Windows is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_XWindowPosition))
            {
                this.validationResult += "X is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_YWindowPosition))
            {
                this.validationResult += "Y is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}