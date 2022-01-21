using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.User32;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using System.Linq;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.SubGruop("Window State")]
    [Attributes.ClassAttributes.Description("This command returns window position.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want window position.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'SetForegroundWindow', 'ShowWindow' from user32.dll to achieve automation.")]
    public class GetWindowPositionCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter or select the window position that you want to.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Input or Type the name of the window name that you want to.")]
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
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Starts with")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Ends with")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Exact match")]
        [Attributes.PropertyAttributes.SampleUsage("**Contains** or **Starts with** or **Ends with** or **Exact match**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        public string v_SearchMethod { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Specify the variable to recieve the window position X")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        public string v_VariablePositionX { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Specify the variable to recieve the window position Y")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        public string v_VariablePositionY { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Base position (Default is Top Left)")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Top Left")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Bottom Right")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Top Right")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Bottom Left")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Center")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        public string v_PositionBase { get; set; }

        [XmlIgnore]
        [NonSerialized]
        public ComboBox WindowNameControl;

        public GetWindowPositionCommand()
        {
            this.CommandName = "GetWindowPositionCommand";
            this.SelectionName = "Get Window Position";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Automation.Engine.AutomationEngineInstance)sender;

            string windowName = v_WindowName.ConvertToUserVariable(sender);
            string searchMethod = v_SearchMethod.ConvertToUserVariable(sender);
            if (String.IsNullOrEmpty(searchMethod))
            {
                searchMethod = "Contains";
            }

            bool targetIsCurrentWindow = windowName == engine.engineSettings.CurrentWindowKeyword;
            var targetWindows = User32Functions.FindTargetWindows(windowName, targetIsCurrentWindow, true);

            User32Functions.RECT pos = new User32Functions.RECT();
            bool isWindowsFound = false;

            //loop each window
            if (searchMethod == "Contains" || targetIsCurrentWindow)
            {
                foreach (var targetedWindow in targetWindows)
                {
                    pos = User32Functions.GetWindowPosition(targetedWindow);
                    isWindowsFound = true;
                }
            }
            else
            {
                Func<string, bool> searchFunc;
                switch(searchMethod)
                {
                    case "Starts with":
                        searchFunc = (s) => s.StartsWith(windowName);
                        break;

                    case "Ends with":
                        searchFunc = (s) => s.EndsWith(windowName);
                        break;

                    case "Exact match":
                        searchFunc = (s) => (s == windowName);
                        break;

                    default:
                        throw new Exception("Search method " + searchMethod + " is not support.");
                        break;
                }

                foreach (var targetedWindow in targetWindows)
                {
                    if (searchFunc(User32Functions.GetWindowTitle(targetedWindow)))
                    {
                        pos = User32Functions.GetWindowPosition(targetedWindow);
                        isWindowsFound = true;
                    }
                }
            }

            if (!isWindowsFound)
            {
                throw new Exception("Window name " + windowName + " is not found.");
                return;
            }

            var basePosition = v_PositionBase.ConvertToUserVariable(sender);
            if (String.IsNullOrEmpty(basePosition))
            {
                basePosition = "Top Left";
            }
            int x, y;
            switch (basePosition)
            {
                case "Top Left":
                    x = pos.left;
                    y = pos.top;
                    break;
                case "Bottom Right":
                    x = pos.right;
                    y = pos.bottom;
                    break;
                case "Top Right":
                    x = pos.right;
                    y = pos.top;
                    break;
                case "Bottom Left":
                    x = pos.left;
                    y = pos.bottom;
                    break;
                case "Center":
                    x = (pos.right + pos.left) / 2;
                    y = (pos.top + pos.bottom) / 2;
                    break;
                default:
                    throw new Exception("Base Position " + basePosition + " is not supported.");
                    break;
            }
            
            if (!String.IsNullOrEmpty(v_VariablePositionX))
            {
                x.ToString().StoreInUserVariable(sender, v_VariablePositionX);
            }
            if (!String.IsNullOrEmpty(v_VariablePositionY))
            {
                y.ToString().StoreInUserVariable(sender, v_VariablePositionY);
            }
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            return RenderedControls;

        }
        public override void Refresh(frmCommandEditor editor)
        {
            base.Refresh();
            WindowNameControl.AddWindowNames();
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: " + v_WindowName + "', Result(X) In: '" + v_VariablePositionX + "', Result(Y) In: '" + v_VariablePositionY + "']";
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

        public override void convertToIntermediate(EngineSettings settings, List<Core.Script.ScriptVariable> variables)
        {
            var cnv = new Dictionary<string, string>();
            cnv.Add("v_WindowName", "convertToIntermediateWindowName");
            convertToIntermediate(settings, cnv, variables);
        }

        public override void convertToRaw(EngineSettings settings)
        {
            var cnv = new Dictionary<string, string>();
            cnv.Add("v_WindowName", "convertToRawWindowName");
            convertToRaw(settings, cnv);
        }
    }
}