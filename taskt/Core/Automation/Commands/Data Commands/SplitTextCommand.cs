using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;
using taskt.Core.Script;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("Data Commands")]
    [Description("This command splits a string by a delimiter and saves the result in a list.")]
    public class SplitTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Text Data")]
        [InputSpecification("Provide a variable or text value.")]
        [SampleUsage("Sample text, to be splitted by comma delimiter || {vTextData}")]
        [Remarks("Providing data of a type other than a 'String' will result in an error.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InputText { get; set; }

        [XmlAttribute]
        [PropertyDescription("Text Delimiter")]
        [InputSpecification("Specify the character that will be used to split the text.")]
        [SampleUsage("[crLF] || [chars] || , || {vDelimiter}")]
        [Remarks("[crLF] can be used for line breaks and [chars] can be used to split each character.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SplitCharacter { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output List Variable")]
        [InputSpecification("Select or provide a variable from the variable list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required" +
                  " to pre-define your variables; however, it is highly recommended.")]
        public string v_OutputUserVariableName { get; set; }

        public SplitTextCommand()
        {
            CommandName = "SplitTextCommand";
            SelectionName = "Split Text";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var stringVariable = v_InputText.ConvertToUserVariable(sender);
            var splitCharacter = v_SplitCharacter;

            if(v_SplitCharacter != "[crLF]" && v_SplitCharacter != "[chars]")
            {
                splitCharacter = v_SplitCharacter.ConvertToUserVariable(sender);
            }
            List<string> splitString;
            if (splitCharacter == "[crLF]")
            {
                splitString = stringVariable.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            }
            else if (splitCharacter == "[chars]")
            {
                splitString = new List<string>();
                var chars = stringVariable.ToCharArray();
                foreach (var c in chars)
                {
                    splitString.Add(c.ToString());
                }
            }
            else
            {
                splitString = stringVariable.Split(new string[] { splitCharacter }, StringSplitOptions.None).ToList();
            }

            var engine = (AutomationEngineInstance)sender;
            
            var v_receivingVariable = v_OutputUserVariableName
                .Replace(engine.EngineSettings.VariableStartMarker, "")
                .Replace(engine.EngineSettings.VariableEndMarker, "");

            //get complex variable from engine and assign
            var requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == v_receivingVariable).FirstOrDefault();

            if (requiredComplexVariable == null)
            {
                engine.VariableList.Add(new ScriptVariable() { VariableName = v_receivingVariable, CurrentPosition = 0 });
                requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == v_receivingVariable).FirstOrDefault();
            }

            requiredComplexVariable.VariableValue = splitString;
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputText", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SplitCharacter", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Split '{v_InputText}' by '{v_SplitCharacter}' - Store List in '{v_OutputUserVariableName}']";
        }
    }
}