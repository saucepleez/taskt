using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// 字典Set操作
    /// </summary>
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to loop through an Excel Dataset")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to iterate over a series of Excel cells.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command attempts to loop through a known Excel DataSet")]
    public class SetDictionaryValueCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select Dictionary Name")]
        [Attributes.PropertyAttributes.InputSpecification("Indicate a unique reference name for later use")]
        [Attributes.PropertyAttributes.SampleUsage("vMyDictionary")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DictionaryName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the key")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a string of comma seperated values.")]
        [Attributes.PropertyAttributes.SampleUsage("key1")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_Key { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the Value")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a string of comma seperated values.")]
        [Attributes.PropertyAttributes.SampleUsage("value1")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SetDictionaryValueCommand()
        {
            this.CommandName = "SetDictionaryValueCommand";
            this.SelectionName = "Set Dictionary Item";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vDicName = v_DictionaryName.ConvertToUserVariable(sender);
            var vKey = v_Key.ConvertToUserVariable(sender);
            var vValue = v_Value.ConvertToUserVariable(sender);
            var dataSetVariable = LookupVariable(engine);
            //Overwrites variable if it already exists
            if (engine.VariableList.Exists(x => x.VariableName == v_DictionaryName))
            {
                Script.ScriptVariable temp = engine.VariableList.Where(x => x.VariableName == v_DictionaryName).FirstOrDefault();
                engine.VariableList.Remove(temp);
            }
            //Declare local dictionary and assign output
            Dictionary<string, string> dict = (Dictionary<string, string>)dataSetVariable.VariableValue;
            dict[vKey] = vValue;
            Script.ScriptVariable _dics = new Script.ScriptVariable
            {
                VariableName = v_DictionaryName,
                VariableValue = dict
            };
            //Add to variable list
            engine.VariableList.Add(_dics);
        }

        private Script.ScriptVariable LookupVariable(Core.Automation.Engine.AutomationEngineInstance sendingInstance)
        {
            var requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == v_DictionaryName).FirstOrDefault();
            if ((requiredVariable == null) &&
                (v_DictionaryName.StartsWith(sendingInstance.engineSettings.VariableStartMarker)) &&
                (v_DictionaryName.EndsWith(sendingInstance.engineSettings.VariableEndMarker)))
            {
                //reformat and attempt
                var reformattedVariable = v_DictionaryName.Replace(sendingInstance.engineSettings.VariableStartMarker, "").Replace(sendingInstance.engineSettings.VariableEndMarker, "");
                requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == reformattedVariable).FirstOrDefault();
            }
            return requiredVariable;
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DictionaryName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Key", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Value", this, editor));
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [From: {v_DictionaryName}, Set: {v_Key}, Store In: {v_Value}]";
        }
    }
}
