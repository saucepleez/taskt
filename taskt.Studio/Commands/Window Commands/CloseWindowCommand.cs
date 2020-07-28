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
    [Description("This command closes an open window.")]
    [UsesDescription("Use this command when you want to close an existing window by name.")]
    [ImplementationDescription("This command implements 'FindWindowNative', 'SendMessage' from user32.dll to achieve automation.")]
    public class CloseWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please enter or select the window that you want to close.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Input or Type the name of the window that you want to close.")]
        [SampleUsage("**Untitled - Notepad**")]
        [Remarks("")]
        public string v_WindowName { get; set; }

        [XmlIgnore]
        [NonSerialized]
        public ComboBox WindowNameControl;

        public CloseWindowCommand()
        {
            CommandName = "CloseWindowCommand";
            SelectionName = "Close Window";
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
                User32Functions.CloseWindow(targetedWindow);
            }
        }
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            //create window name helper control
            RenderedControls.Add(UI.CustomControls.CommandControls.CreateDefaultLabelFor("v_WindowName", this));
            WindowNameControl = UI.CustomControls.CommandControls.CreateStandardComboboxFor("v_WindowName", this).AddWindowNames();
            RenderedControls.AddRange(UI.CustomControls.CommandControls.CreateUIHelpersFor("v_WindowName", this, new Control[] { WindowNameControl }, editor));
            RenderedControls.Add(WindowNameControl);

            return RenderedControls;

        }
        public override void Refresh(IfrmCommandEditor editor)
        {
            base.Refresh();
            WindowNameControl.AddWindowNames();
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: " + v_WindowName + "]";
        }
    }
}