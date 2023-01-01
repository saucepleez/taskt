using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary Commands")]
    [Attributes.ClassAttributes.SubGruop("Dictionary Item")]
    [Attributes.ClassAttributes.Description("This command allows you to get value in Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get value in Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetDictionaryValueCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please input The Dictionary Variable")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[SampleUsage("**myDictionary** or **{{{vMyDic}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Dictionary)]
        //[PropertyValidationRule("Dictionary", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Dictionary")]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        public string v_InputData { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please indicate the key for the Dictionary")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[SampleUsage("**key1** or **{{{vKeyName}}}**")]
        //[Remarks("If it is empty, it will be the value of Current Position, which can be used for Loop List command.")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.TextBox)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyIsOptional(true, "Current Position")]
        //[PropertyDisplayText(true, "Key")]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_Key))]
        public string v_Key { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please indicate the variable to apply result")]
        //[InputSpecification("Enter a unique dataset name that will be used later to traverse over the data")]
        //[SampleUsage("**vMyData** or **{{{myData}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        //[PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Result")]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_OutputVariable { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Select If Key does not Exists")]
        //[InputSpecification("")]
        //[SampleUsage("")]
        //[Remarks("")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyUISelectionOption("Error")]
        //[PropertyUISelectionOption("Set Empty")]
        //[PropertyIsOptional(true, "Error")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_WhenKeyDoesNotExists))]
        [PropertyUISelectionOption("Set Empty")]
        [PropertyDetailSampleUsage("**Set Empty**", "Result is Empty Value")]
        public string v_IfKeyDoesNotExists { get; set; }

        public GetDictionaryValueCommand()
        {
            this.CommandName = "GetDictionaryValueCommand";
            this.SelectionName = "Get Dictionary Item";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (var dic, var vKey) = this.GetDictionaryVariableAndKey(nameof(v_InputData), nameof(v_Key), engine);

            if (dic.ContainsKey(vKey))
            {
                dic[vKey].StoreInUserVariable(engine, v_OutputVariable);
            }
            else
            {
                string ifNotExists = this.GetUISelectionValue(nameof(v_IfKeyDoesNotExists), "Key Not Exists", engine);
                switch (ifNotExists)
                {
                    case "error":
                        throw new Exception("Key " + v_Key + " does not exists in the Dictionary");

                    case "set empty":
                        "".StoreInUserVariable(engine, v_OutputVariable);
                        break;
                }
            }
        }
    }
}