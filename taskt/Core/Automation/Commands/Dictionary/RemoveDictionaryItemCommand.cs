using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary Commands")]
    [Attributes.ClassAttributes.SubGruop("Dictionary Item")]
    [Attributes.ClassAttributes.Description("This command allows you to remove item in Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to remove item in Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class RemoveDictionaryItemCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please input The Dictionary Variable")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**myDictionary** or **{{{vMyDic}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Dictionary)]
        [PropertyValidationRule("Dictionary", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Dictionary")]
        public string v_InputData { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please indicate the key for the Dictionary")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**key1** or **{{{vKeyName}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.TextBox)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyIsOptional(true, "Current Position")]
        [PropertyDisplayText(true, "Key")]
        public string v_Key { get; set; }

        public RemoveDictionaryItemCommand()
        {
            this.CommandName = "RemoveDictionaryItemCommand";
            this.SelectionName = "Remove Dictionary Item";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
            //var vKey = v_Key.ConvertToUserVariable(sender);

            //Dictionary<string, string> dic = v_InputData.GetDictionaryVariable(engine);

            (var dic, var vKey) = this.GetDictionaryVariableAndKey(nameof(v_InputData), nameof(v_Key), engine);

            if (!dic.Remove(vKey))
            {
                throw new Exception("Dictionary does not has key name " + vKey);
            }
        }
    }
}