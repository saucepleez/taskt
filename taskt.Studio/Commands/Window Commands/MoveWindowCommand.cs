using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.User32;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Window Commands")]
    [Description("This command moves a window to a specified location on screen.")]
    [UsesDescription("Use this command when you want to move an existing window by name to a certain point on the screen.")]
    [ImplementationDescription("This command implements 'FindWindowNative', 'SetWindowPos' from user32.dll to achieve automation.")]
    public class MoveWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyDescription("Please enter or select the window that you want to move.")]
        [InputSpecification("Input or Type the name of the window that you want to move.")]
        [SampleUsage("**Untitled - Notepad**")]
        [Remarks("")]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyDescription("Please indicate the new X horizontal coordinate (pixel) for the window's location.  0 starts at the left of the screen.")]
        [InputSpecification("Input the new horizontal coordinate of the window, 0 starts at the left and goes to the right")]
        [SampleUsage("0")]
        [Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range could be 0-1920")]
        public string v_XWindowPosition { get; set; }
        [XmlAttribute]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyDescription("Please indicate the new Y vertical coordinate (pixel) for the window's location.  0 starts at the top of the screen.")]
        [InputSpecification("Input the new vertical coordinate of the window, 0 starts at the top and goes downwards")]
        [SampleUsage("0")]
        [Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range could be 0-1080")]
        public string v_YWindowPosition { get; set; }

        [XmlIgnore]
        [NonSerialized]
        public ComboBox WindowNameControl;


        public MoveWindowCommand()
        {
            CommandName = "MoveWindowCommand";
            SelectionName = "Move Window";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            string windowName = v_WindowName.ConvertToUserVariable(engine);

            var targetWindows = User32Functions.FindTargetWindows(windowName);

            //loop each window
            foreach (var targetedWindow in targetWindows)
            {
                var variableXPosition = v_XWindowPosition.ConvertToUserVariable(engine);
                var variableYPosition = v_YWindowPosition.ConvertToUserVariable(engine);

                if (!int.TryParse(variableXPosition, out int xPos))
                {
                    throw new Exception("X Position Invalid - " + v_XWindowPosition);
                }
                if (!int.TryParse(variableYPosition, out int yPos))
                {
                    throw new Exception("X Position Invalid - " + v_XWindowPosition);
                }


                User32Functions.SetWindowPosition(targetedWindow, xPos, yPos);
            }
        }
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            //create window name helper control
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_WindowName", this));
            WindowNameControl = CommandControls.CreateStandardComboboxFor("v_WindowName", this).AddWindowNames();
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_WindowName", this, new Control[] { WindowNameControl }, editor));
            RenderedControls.Add(WindowNameControl);

            var xGroup = CommandControls.CreateDefaultInputGroupFor("v_XWindowPosition", this, editor);
            var yGroup = CommandControls.CreateDefaultInputGroupFor("v_YWindowPosition", this, editor);
            RenderedControls.AddRange(xGroup);
            RenderedControls.AddRange(yGroup);

            //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_XWindowPosition", this));
            //var xPositionControl = CommandControls.CreateDefaultInputFor("v_XWindowPosition", this);
            //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_XWindowPosition", this, new Control[] { xPositionControl }, editor));
            //RenderedControls.Add(xPositionControl);

            //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_YWindowPosition", this));
            //var yPositionControl = CommandControls.CreateDefaultInputFor("v_YWindowPosition", this);
            //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_YWindowPosition", this, new Control[] { yPositionControl }, editor));
            //RenderedControls.Add(yPositionControl);


            return RenderedControls;

        }
        public override void Refresh(IfrmCommandEditor editor)
        {
            base.Refresh();
            WindowNameControl.AddWindowNames();
        }


        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: " + v_WindowName + ", Target Coordinates (" + v_XWindowPosition + "," + v_YWindowPosition + ")]";
        }
    }
}