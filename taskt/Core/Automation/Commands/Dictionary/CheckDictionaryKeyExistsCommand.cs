using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary Commands")]
    [Attributes.ClassAttributes.SubGruop("Dictionary Key")]
    [Attributes.ClassAttributes.Description("This command allows you to check key existance in Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to check key existance in Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CheckDictionaryKeyExistsCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please input The Dictionary Variable")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Enter a string of comma seperated values.")]
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
        //[InputSpecification("Enter a string of comma seperated values.")]
        //[SampleUsage("**key1** or **{{{vKeyName}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.TextBox)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyValidationRule("Key", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Key")]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_Key))]
        [PropertyDescription("Name of the Dictionary Key to Check")]
        public string v_Key { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please specify the variable to apply Result")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[SampleUsage("**vResult** or **{{{vResult}}}**")]
        //[Remarks("")]
        //[PropertyIsVariablesList(true)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Boolean, true)]
        //[PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Result")]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [Remarks("If Item Exists, the Result is **True**")]
        public string v_applyToVariable { get; set; }

        public CheckDictionaryKeyExistsCommand()
        {
            this.CommandName = "CheckDictionaryKeyExistsCommand";
            this.SelectionName = "Check Dictionary Key Exists";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
            var vKey = v_Key.ConvertToUserVariable(sender);

            var dic = v_InputData.GetDictionaryVariable(engine);
            dic.ContainsKey(vKey).StoreInUserVariable(engine, v_applyToVariable);
        }
    }
}