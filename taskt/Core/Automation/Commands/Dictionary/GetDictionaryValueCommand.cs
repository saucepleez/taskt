using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using taskt.UI.Forms;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary Commands")]
    [Attributes.ClassAttributes.SubGruop("Dictionary Item")]
    [Attributes.ClassAttributes.Description("This command allows you to get value in Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get value in Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class GetDictionaryValueCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please input The Dictionary Variable")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**myDictionary** or **{{{vMyDic}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Dictionary)]
        public string v_InputData { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the key for the Dictionary")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**key1** or **{{{vKeyName}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.TextBox)]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        public string v_Key { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the variable to apply result")]
        [Attributes.PropertyAttributes.InputSpecification("Enter a unique dataset name that will be used later to traverse over the data")]
        [Attributes.PropertyAttributes.SampleUsage("**vMyData** or **{{{myData}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        public string v_OutputVariable { get; set; }

        public GetDictionaryValueCommand()
        {
            this.CommandName = "GetDictionaryValueCommand";
            this.SelectionName = "Get Dictionary Item";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //Retrieve Dictionary by name
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vKey = v_Key.ConvertToUserVariable(sender);
            //var dataSetVariable = LookupVariable(engine);

            ////Declare local dictionary and assign output
            //Dictionary<string,string> dict = (Dictionary<string,string>)dataSetVariable.VariableValue;
            //Script.ScriptVariable Output = new Script.ScriptVariable
            //{
            //    VariableName = v_OutputVariable,
            //    VariableValue = dict[vKey]
            //};

            ////Overwrites variable if it already exists
            //if (engine.VariableList.Exists(x => x.VariableName == Output.VariableName))
            //{
            //    Script.ScriptVariable temp = engine.VariableList.Where(x => x.VariableName == Output.VariableName).FirstOrDefault();
            //    engine.VariableList.Remove(temp);
            //}
            ////Add to variable list
            //engine.VariableList.Add(Output);

            //Dictionary<string, string> dic = (Dictionary<string, string>)v_InputData.GetRawVariable(engine).VariableValue;
            Dictionary<string, string> dic = v_InputData.GetDictionaryVariable(engine);

            if (dic.ContainsKey(vKey))
            {
                dic[vKey].StoreInUserVariable(engine, v_OutputVariable);
            }
            else
            {
                throw new Exception("Key " + v_Key + " does not exists in the Dictionary");
            }
        }
        //private Script.ScriptVariable LookupVariable(Core.Automation.Engine.AutomationEngineInstance sendingInstance)
        //{
        //    //search for the variable
        //    var requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == v_InputData).FirstOrDefault();

        //    //if variable was not found but it starts with variable naming pattern
        //    if ((requiredVariable == null) && (v_InputData.StartsWith(sendingInstance.engineSettings.VariableStartMarker)) && (v_InputData.EndsWith(sendingInstance.engineSettings.VariableEndMarker)))
        //    {
        //        //reformat and attempt
        //        var reformattedVariable = v_InputData.Replace(sendingInstance.engineSettings.VariableStartMarker, "").Replace(sendingInstance.engineSettings.VariableEndMarker, "");
        //        requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == reformattedVariable).FirstOrDefault();
        //    }

        //    return requiredVariable;
        //}
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_OutputVariable", this, editor));
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputData", this, editor));
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Key", this, editor));

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [From: {v_InputData}, Get: {v_Key}, Store In: {v_OutputVariable}]";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(v_OutputVariable))
            {
                this.IsValid = false;
                this.validationResult += "Variable is empty.\n";
            }
            if (String.IsNullOrEmpty(v_InputData))
            {
                this.IsValid = false;
                this.validationResult += "Dictionary Variable Name is empty.\n";
            }
            if (String.IsNullOrEmpty(v_Key))
            {
                this.IsValid = false;
                this.validationResult += "Key is empty.\n";
            }

            return this.IsValid;
        }
    }
}