using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Script;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Dictionary Commands")]
    [Description("This command returns a dictionary value based on a specified key.")]
    public class GetDictionaryValueCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Dictionary")]
        [InputSpecification("Specify the dictionary variable to get a value from.")]
        [SampleUsage("{vDictionary}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InputDictionary { get; set; }

        [XmlAttribute]
        [PropertyDescription("Key")]
        [InputSpecification("Specify the key to get the value for.")]
        [SampleUsage("SomeKey || {vKey}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Key { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Value Variable")]
        [InputSpecification("Select or provide a variable from the variable list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required" +
                 " to pre-define your variables; however, it is highly recommended.")]
        public string v_OutputUserVariableName { get; set; }

        public GetDictionaryValueCommand()
        {
            CommandName = "GetDictionaryValueCommand";
            SelectionName = "Get Dictionary Value";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //Retrieve Dictionary by name
            var engine = (AutomationEngineInstance)sender;
            var vKey = v_Key.ConvertToUserVariable(engine);
            var dictionaryVariable = LookupVariable(engine);

            //Declare local dictionary and assign output
            Dictionary<string,string> dict = (Dictionary<string,string>)dictionaryVariable.VariableValue;
            var dictValue = dict[vKey].ConvertToUserVariable(engine);

            engine.AddVariable(v_OutputUserVariableName, dictValue);
        }
        
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputDictionary", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Key", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [From '{v_InputDictionary}' for Key '{v_Key}' - Store Value in '{v_OutputUserVariableName}']";
        }

        private ScriptVariable LookupVariable(AutomationEngineInstance sendingInstance)
        {
            //search for the variable
            var requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == v_InputDictionary).FirstOrDefault();

            //if variable was not found but it starts with variable naming pattern
            if ((requiredVariable == null) && 
                (v_InputDictionary.StartsWith(sendingInstance.EngineSettings.VariableStartMarker)) && 
                (v_InputDictionary.EndsWith(sendingInstance.EngineSettings.VariableEndMarker)))
            {
                //reformat and attempt
                var reformattedVariable = v_InputDictionary
                    .Replace(sendingInstance.EngineSettings.VariableStartMarker, "")
                    .Replace(sendingInstance.EngineSettings.VariableEndMarker, "");
                requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == reformattedVariable).FirstOrDefault();
            }

            return requiredVariable;
        }
    }
}