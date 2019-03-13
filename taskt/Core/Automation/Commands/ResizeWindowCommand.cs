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
    [Attributes.ClassAttributes.Description("This command resizes a window to a specified size.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to reize a window by name to a specific size on screen.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'SetWindowPos' from user32.dll to achieve automation.")]
    public class ResizeWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter or select the window that you want to resize.")]
        [Attributes.PropertyAttributes.InputSpecification("Input or Type the name of the window that you want to resize.")]
        [Attributes.PropertyAttributes.SampleUsage("**Untitled - Notepad**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the new required width (pixel) of the window.")]
        [Attributes.PropertyAttributes.InputSpecification("Input the new width size of the window")]
        [Attributes.PropertyAttributes.SampleUsage("0")]
        [Attributes.PropertyAttributes.Remarks("This number is limited by your resolution. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid width range could be 0-1920")]
        public string v_XWindowSize { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the new required height (pixel) of the window.")]
        [Attributes.PropertyAttributes.InputSpecification("Input the new height size of the window")]
        [Attributes.PropertyAttributes.SampleUsage("0")]
        [Attributes.PropertyAttributes.Remarks("This number is limited by your resolution. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid height range could be 0-1080")]
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

            var targetWindows = User32Functions.FindTargetWindows(windowName);

            //loop each window and set the window state
            foreach (var targetedWindow in targetWindows)
            {
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

                User32Functions.SetWindowSize(targetedWindow, xPos, yPos);
            }

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create window name helper control
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_WindowName", this));
            WindowNameControl = CommandControls.CreateStandardComboboxFor("v_WindowName", this).AddWindowNames();
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_WindowName", this, new Control[] { WindowNameControl }, editor));
            RenderedControls.Add(WindowNameControl);

            //create standard group controls
            var xGroup = CommandControls.CreateDefaultInputGroupFor("v_XWindowSize", this, editor);
            var yGroup = CommandControls.CreateDefaultInputGroupFor("v_YWindowSize", this, editor);
            RenderedControls.AddRange(xGroup);
            RenderedControls.AddRange(yGroup);
      
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
    }
}