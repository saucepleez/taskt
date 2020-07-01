using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("Error Handling Commands")]
    [Description("This command throws an exception during script execution.")]
    public class ThrowCommand : ScriptCommand
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
        [InputSpecification("Select the appropriate exception type to throw.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_ExceptionType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Exception Message")]
        [InputSpecification("Enter a custom exception message.")]
        [SampleUsage("A Custom Message || {vExceptionMessage}")]
        [Remarks("The selected exception with this custom message will be thrown.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ExceptionMessage { get; set; }

        public ThrowCommand()
        {
            CommandName = "ThrowCommand";
            SelectionName = "Throw";
            CommandEnabled = true;
            CustomRendering = true;
            v_ExceptionType = "Exception";
        }

        public override void RunCommand(object sender)
        {
            var exceptionMessage = v_ExceptionMessage.ConvertToUserVariable(sender);

            Exception ex;
            switch(v_ExceptionType)
            {
                case "AccessViolationException":
                    ex = new AccessViolationException(exceptionMessage);
                    break;
                case "ArgumentException":
                    ex = new ArgumentException(exceptionMessage);
                    break;
                case "ArgumentNullException":
                    ex = new ArgumentNullException(exceptionMessage);
                    break;
                case "ArgumentOutOfRangeException":
                    ex = new ArgumentOutOfRangeException(exceptionMessage);
                    break;
                case "DivideByZeroException":
                    ex = new DivideByZeroException(exceptionMessage);
                    break;
                case "Exception":
                    ex = new Exception(exceptionMessage);
                    break;
                case "FileNotFoundException":
                    ex = new FileNotFoundException(exceptionMessage);
                    break;
                case "FormatException":
                    ex = new FormatException(exceptionMessage);
                    break;
                case "IndexOutOfRangeException":
                    ex = new IndexOutOfRangeException(exceptionMessage);
                    break;
                case "InvalidDataException":
                    ex = new InvalidDataException(exceptionMessage);
                    break;
                case "InvalidOperationException":
                    ex = new InvalidOperationException(exceptionMessage);
                    break;
                case "KeyNotFoundException":
                    ex = new KeyNotFoundException(exceptionMessage);
                    break;
                case "NotSupportedException":
                    ex = new NotSupportedException(exceptionMessage);
                    break;
                case "NullReferenceException":
                    ex = new NullReferenceException(exceptionMessage);
                    break;
                case "OverflowException":
                    ex = new OverflowException(exceptionMessage);
                    break;
                case "TimeoutException":
                    ex = new TimeoutException(exceptionMessage);
                    break;
                default:
                    throw new NotImplementedException("Selected exception type " + v_ExceptionType + " is not implemented.");
            }
            throw ex;
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_ExceptionType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ExceptionMessage", this, editor));
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Exception '{v_ExceptionType}' - Message '{v_ExceptionMessage}']";
        }
    }
}