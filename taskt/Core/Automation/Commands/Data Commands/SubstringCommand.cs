using System;
using System.Collections.Generic;
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
    [Group("Data Commands")]
    [Description("This command returns a substring from a specified string.")]
    public class SubstringCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Text Data")]
        [InputSpecification("Provide a variable or text value.")]
        [SampleUsage("Sample text to extract substring from || {vTextData}")]
        [Remarks("Providing data of a type other than a 'String' will result in an error.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InputText { get; set; }

        [XmlAttribute]
        [PropertyDescription("Starting Index")]
        [InputSpecification("Indicate the starting position within the text.")]
        [SampleUsage("0 || 1 || {vStartingIndex}")]
        [Remarks("0 for beginning, 1 for first character, n for nth character")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_StartIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Substring Length (Optional)")]
        [InputSpecification("Indicate number of characters to extract.")]
        [SampleUsage("-1 || 1 || {vSubstringLength}")]
        [Remarks("-1 to keep remainder, 1 for 1 position after start index, etc.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_StringLength { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Substring Variable")]
        [InputSpecification("Select or provide a variable from the variable list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required" +
                  " to pre-define your variables; however, it is highly recommended.")]
        public string v_OutputUserVariableName { get; set; }

        public SubstringCommand()
        {
            CommandName = "SubstringCommand";
            SelectionName = "Substring";
            CommandEnabled = true;
            CustomRendering = true;
            v_StringLength = "-1";
        }

        public override void RunCommand(object sender)
        {
            var inputText = v_InputText.ConvertToUserVariable(sender);
            var startIndex = int.Parse(v_StartIndex.ConvertToUserVariable(sender));
            var stringLength = int.Parse(v_StringLength.ConvertToUserVariable(sender));

            //apply substring
            if (stringLength >= 0)
            {
                inputText = inputText.Substring(startIndex, stringLength);
            }
            else
            {
                inputText = inputText.Substring(startIndex);
            }

            inputText.StoreInUserVariable(sender, v_OutputUserVariableName);
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputText", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_StartIndex", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_StringLength", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Get Substring From '{v_InputText}' - " +
                $"Store Substring in '{v_OutputUserVariableName}']";
        }
    }
}