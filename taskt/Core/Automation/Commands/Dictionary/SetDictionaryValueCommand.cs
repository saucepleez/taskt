using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary Commands")]
    [Attributes.ClassAttributes.SubGruop("Dictionary Item")]
    [Attributes.ClassAttributes.Description("This command allows you to set value in Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set value in Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SetDictionaryValueCommand : ScriptCommand
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
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_BothDictionaryName))]
        public string v_InputData { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please indicate the key for the Dictionary")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Enter a string of comma seperated values.")]
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
        [PropertyDescription("Please indicate the value for the Dictionary")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**value1** or **{{{vValue}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.TextBox)]
        [PropertyDisplayText(true, "Value")]
        public string v_Value { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Select If Key does not Exists")]
        //[InputSpecification("")]
        //[SampleUsage("")]
        //[Remarks("")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyUISelectionOption("Error")]
        //[PropertyUISelectionOption("Ignore")]
        //[PropertyUISelectionOption("Add")]
        //[PropertyIsOptional(true, "Error")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_WhenKeyDoesNotExists))]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Add")]
        [PropertyDetailSampleUsage("**Ignore**", "Don't Set the Dictionary Item")]
        [PropertyDetailSampleUsage("**Add**", "Add New Dictionary Item")]
        public string v_IfKeyDoesNotExists { get; set; }

        public SetDictionaryValueCommand()
        {
            this.CommandName = "SetDictionaryValueCommand";
            this.SelectionName = "Set Dictionary Value";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (var dic, var vKey) = this.GetDictionaryVariableAndKey(nameof(v_InputData), nameof(v_Key), engine);

            string valueToSet = v_Value.ConvertToUserVariable(engine);
            if (dic.ContainsKey(vKey))
            {
                dic[vKey] = valueToSet;
            }
            else
            {
                string ifNotExits = this.GetUISelectionValue(nameof(v_IfKeyDoesNotExists), "Key Not Exists", engine);
                switch (ifNotExits)
                {
                    case "error":
                        throw new Exception("Key " + v_Key + " does not exists in the Dictionary");

                    case "ignore":
                        break;
                    case "add":
                        dic.Add(vKey, valueToSet);
                        break;
                }
            }
        }
    }
}