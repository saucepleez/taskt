using System;
using System.Collections.Generic;
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
    [Description("This command returns a substring from a specified string.")]
    public class SubstringCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Text Data")]
        [InputSpecification("Provide a variable or text value.")]
        [SampleUsage("Sample text to extract substring from || {vTextData}")]
        [Remarks("Providing data of a type other than a 'String' will result in an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InputText { get; set; }

        [XmlAttribute]
        [PropertyDescription("Starting Index")]
        [InputSpecification("Indicate the starting position within the text.")]
        [SampleUsage("0 || 1 || {vStartingIndex}")]
        [Remarks("0 for beginning, 1 for first character, n for nth character")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_StartIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Substring Length (Optional)")]
        [InputSpecification("Indicate number of characters to extract.")]
        [SampleUsage("-1 || 1 || {vSubstringLength}")]
        [Remarks("-1 to keep remainder, 1 for 1 position after start index, etc.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
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
            var engine = (AutomationEngineInstance)sender;
            var inputText = v_InputText.ConvertToUserVariable(engine);
            var startIndex = int.Parse(v_StartIndex.ConvertToUserVariable(engine));
            var stringLength = int.Parse(v_StringLength.ConvertToUserVariable(engine));

            //apply substring
            if (stringLength >= 0)
            {
                inputText = inputText.Substring(startIndex, stringLength);
            }
            else
            {
                inputText = inputText.Substring(startIndex);
            }

            inputText.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
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