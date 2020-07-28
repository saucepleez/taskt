using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Infrastructure;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Error Handling Commands")]
    [Description("This command defines a catch block whose commands will execute if an exception is thrown from the " +
                 "associated try.")]
    public class CatchCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Exception Type")]
        [PropertyUISelectionOption("AccessViolationException")]
        [PropertyUISelectionOption("ArgumentException")]
        [PropertyUISelectionOption("ArgumentNullException")]
        [PropertyUISelectionOption("ArgumentOutOfRangeException")]
        [PropertyUISelectionOption("DivideByZeroException")]
        [PropertyUISelectionOption("Exception")]
        [PropertyUISelectionOption("FileNotFoundException")]
        [PropertyUISelectionOption("FormatException")]
        [PropertyUISelectionOption("IndexOutOfRangeException")]
        [PropertyUISelectionOption("InvalidDataException")]
        [PropertyUISelectionOption("InvalidOperationException")]
        [PropertyUISelectionOption("KeyNotFoundException")]
        [PropertyUISelectionOption("NotSupportedException")]
        [PropertyUISelectionOption("NullReferenceException")]
        [PropertyUISelectionOption("OverflowException")]
        [PropertyUISelectionOption("TimeoutException")]
        [InputSpecification("Select the appropriate exception type.")]
        [SampleUsage("")]
        [Remarks("This command will be executed if the type of the exception that occurred in the try block matches the selected exception type.")]
        public string v_ExceptionType { get; set; }

        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }

        public CatchCommand()
        {
            CommandName = "CatchCommand";
            SelectionName = "Catch";
            CommandEnabled = true;
            CustomRendering = true;
            v_ExceptionType = "Exception";
        }

        public override void RunCommand(object sender)
        {
            //no execution required, used as a marker by the Automation Engine
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Comment", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_ExceptionType", this, editor));
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" ({v_ExceptionType})";
        }
    }
}
