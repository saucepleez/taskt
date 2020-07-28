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
    [Description("This command waits for a window to exist.")]
    [UsesDescription("Use this command when you want to explicitly wait for a window to exist before continuing script execution.")]
    [ImplementationDescription("This command implements 'FindWindowNative', 'ShowWindow' from user32.dll to achieve automation.")]
    public class WaitForWindowToExistCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please enter or select the window name that you are waiting for to exist.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Input or Type the name of the window that you want to wait to exist.")]
        [SampleUsage("**Untitled - Notepad**")]
        [Remarks("")]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [PropertyDescription("Indicate how many seconds to wait before an error should be raised.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Specify how many seconds to wait before an error should be invoked")]
        [SampleUsage("**5**")]
        [Remarks("")]
        public string v_LengthToWait { get; set; }

        [XmlIgnore]
        [NonSerialized]
        public ComboBox WindowNameControl;

        public WaitForWindowToExistCommand()
        {
            CommandName = "WaitForWindowToExistCommand";
            SelectionName = "Wait For Window To Exist";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var lengthToWait = v_LengthToWait.ConvertToUserVariable(engine);
            var waitUntil = int.Parse(lengthToWait);
            var endDateTime = DateTime.Now.AddSeconds(waitUntil);

            IntPtr hWnd = IntPtr.Zero;

            while (DateTime.Now < endDateTime)
            {
                string windowName = v_WindowName.ConvertToUserVariable(engine);
                hWnd = User32Functions.FindWindow(windowName);

                if (hWnd != IntPtr.Zero) //If found
                    break;

                System.Threading.Thread.Sleep(1000);

            }

            if (hWnd == IntPtr.Zero)
            {
                throw new Exception("Window was not found in the allowed time!");
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

            //create standard group controls
            var lengthToWaitControlSet = CommandControls.CreateDefaultInputGroupFor("v_LengthToWait", this, editor);
            RenderedControls.AddRange(lengthToWaitControlSet);


            return RenderedControls;

        }
        public override void Refresh(IfrmCommandEditor editor)
        {
            base.Refresh();
            WindowNameControl.AddWindowNames();
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: '" + v_WindowName + "', Wait Up To " + v_LengthToWait + " seconds]";
        }

    }
}