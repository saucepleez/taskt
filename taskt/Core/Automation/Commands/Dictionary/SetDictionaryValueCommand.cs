using System;
using System.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;
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
        [PropertyDescription("Please input The Dictionary Variable")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a string of comma seperated values.")]
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
        [InputSpecification("Enter a string of comma seperated values.")]
        [SampleUsage("**key1** or **{{{vKeyName}}}**")]
        [Remarks("If it is empty, it will be the value of Current Position, which can be used for Loop List command.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.TextBox)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyIsOptional(true, "Current Position")]
        [PropertyDisplayText(true, "Key")]
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

            Dictionary<string, string> dic = v_InputData.GetDictionaryVariable(engine);

            string vKey;
            if (String.IsNullOrEmpty(v_Key))
            {
                var variable = v_InputData.GetRawVariable(engine);
                int pos = variable.CurrentPosition;
                string[] keys = dic.Keys.ToArray();
                if ((pos >= 0) && (pos < keys.Length))
                {
                    vKey = keys[pos];
                }
                else
                {
                    throw new Exception("Strange Current Position in Dictionary " + pos);
                }
            }
            else
            {
                vKey = v_Key.ConvertToUserVariable(engine);
            }

            if (dic.ContainsKey(vKey))
            {
                dic[vKey] = v_Value.ConvertToUserVariable(sender);
            }
            else
            {
                throw new Exception("Key " + v_Key + " does not exists in the Dictionary");
            }
        }
    }
}