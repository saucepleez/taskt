using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Data Commands")]
    [Description("This command performs a specified operation on a string to modify it.")]
    public class ModifyStringCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Text Data")]
        [InputSpecification("Provide a variable or text value.")]
        [SampleUsage("A sample text || {vStringVariable}")]
        [Remarks("Providing data of a type other than a 'String' will result in an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InputText { get; set; }

        [XmlAttribute]
        [PropertyDescription("String Function")]
        [PropertyUISelectionOption("To Upper Case")]
        [PropertyUISelectionOption("To Lower Case")]
        [PropertyUISelectionOption("To Base64 String")]
        [PropertyUISelectionOption("From Base64 String")]
        [InputSpecification("Select a string function to apply to the input text or variable.")]
        [SampleUsage("")]
        [Remarks("Each function, when applied to text data, converts it to a specific format.")]
        public string v_TextOperation { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Text Variable")]
        [InputSpecification("Select or provide a variable from the variable list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required" +
                  " to pre-define your variables; however, it is highly recommended.")]
        public string v_OutputUserVariableName { get; set; }

        public ModifyStringCommand()
        {
            CommandName = "ModifyStringCommand";
            SelectionName = "Modify String";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            var stringValue = v_InputText.ConvertToUserVariable(engine);

            switch (v_TextOperation)
            {
                case "To Upper Case":
                    stringValue = stringValue.ToUpper();
                    break;
                case "To Lower Case":
                    stringValue = stringValue.ToLower();
                    break;
                case "To Base64 String":
                    byte[] textAsBytes = Encoding.ASCII.GetBytes(stringValue);
                    stringValue = Convert.ToBase64String(textAsBytes);
                    break;
                case "From Base64 String":
                    byte[] encodedDataAsBytes = Convert.FromBase64String(stringValue);
                    stringValue = ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
                    break;
                default:
                    throw new NotImplementedException("Conversion Type '" + v_TextOperation + "' not implemented!");
            }

            stringValue.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputText", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_TextOperation", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Convert '{v_InputText}' {v_TextOperation} - Store Text in '{v_OutputUserVariableName}']";
        }
    }
}