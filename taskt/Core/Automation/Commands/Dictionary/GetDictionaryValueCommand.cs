using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary Commands")]
    [Attributes.ClassAttributes.SubGruop("Dictionary Item")]
    [Attributes.ClassAttributes.CommandSettings("Get Dictionary Value")]
    [Attributes.ClassAttributes.Description("This command allows you to get value in Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get value in Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_dictionary))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetDictionaryValueCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        public string v_Dictionary { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_Key))]
        public string v_Key { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_OutputVariable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_WhenKeyDoesNotExists))]
        [PropertyUISelectionOption("Set Empty")]
        [PropertyDetailSampleUsage("**Set Empty**", "Result is Empty Value")]
        public string v_IfKeyDoesNotExists { get; set; }

        [XmlAttribute]
        [PropertyDescription("Key Type")]
        [InputSpecification("")]
        [Remarks("")]
        [PropertyUISelectionOption("Key")]
        [PropertyUISelectionOption("Index")]
        [PropertyDetailSampleUsage("**Key**", "Key Value is Dictionary Key Name")]
        [PropertyDetailSampleUsage("**Key**", "Key Value is Dictionary Key Index")]
        [PropertyIsOptional(true, "Key")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Key Type", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "Result")]
        public string v_KeyType { get; set; }

        public GetDictionaryValueCommand()
        {
            //this.CommandName = "GetDictionaryValueCommand";
            //this.SelectionName = "Get Dictionary Item";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            if (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_KeyType), engine) == "index")
            {
                var getKeys = new GetDictionaryKeysListCommand()
                {
                    v_Dictionary = this.v_Dictionary,
                    v_OutputVariable = VariableNameControls.GetInnerVariableName(0, engine),
                };
                getKeys.RunCommand(engine);
                var keys = (List<string>)VariableNameControls.GetInnerVariable(0, engine).VariableValue;
                var index = this.ExpandValueOrUserVariableAsInteger(nameof(v_Key), "Key", engine);
                if (index < 0)
                {
                    index += keys.Count;
                }
                if (index >= 0 && index < keys.Count)
                {
                    v_Key = keys[index];    // override Key name
                }
                else
                {
                    throw new Exception($"Index value is Out of Range. Value: '{v_Key}', Expand Value: '{index}'");
                }
            }

            (var dic, var vKey) = this.ExpandUserVariablesAsDictionaryAndKey(nameof(v_Dictionary), nameof(v_Key), engine);

            if (dic.ContainsKey(vKey))
            {
                dic[vKey].StoreInUserVariable(engine, v_OutputVariable);
            }
            else
            {
                string ifNotExists = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_IfKeyDoesNotExists), "Key Not Exists", engine);
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