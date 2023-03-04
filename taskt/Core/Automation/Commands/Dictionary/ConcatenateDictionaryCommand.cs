using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary Commands")]
    [Attributes.ClassAttributes.SubGruop("Dictionary Action")]
    [Attributes.ClassAttributes.CommandSettings("Concatenate Dictionary")]
    [Attributes.ClassAttributes.Description("This command allows you to concatenate two Dictionaries.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to concatenate two Dictionaries.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ConcatenateDictionaryCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        [PropertyDescription("Dictionary1 Variable Name")]
        [PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        [PropertyDetailSampleUsage("**vDictionary1**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vDictionary1}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyValidationRule("Dictionary1", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Dictionary1")]
        public string v_InputDataA { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        [PropertyDescription("Dictionary2 Variable Name")]
        [PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        [PropertyDetailSampleUsage("**vDictionary2**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vDictionary2}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyValidationRule("Dictionary2", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Dictionary2")]
        public string v_InputDataB { get; set; }

        [XmlAttribute]
        [PropertyDescription("When Key already Exists")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("", true)]
        [PropertyDetailSampleUsage("**Ignore**", "Priority on Dictionary 1")]
        [PropertyDetailSampleUsage("**Overwrite**", "Priority on Dictionary 2")]
        [PropertyDetailSampleUsage("**Error**", "Rise a Error")]
        [Remarks("")]
        [PropertyIsOptional(true, "Ignore")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Overwrite")]
        [PropertyUISelectionOption("Error")]
        public string v_KeyExists { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_NewOutputDictionaryName))]
        [Remarks("Concatenate Dictionary 1, Dictionary 2 in that order")]
        public string v_OutputName { get; set; }

        public ConcatenateDictionaryCommand()
        {
            //this.CommandName = "ConcatenateDictionaryCommand";
            //this.SelectionName = "Concatenate Dictionary";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var dicA = v_InputDataA.GetDictionaryVariable(engine);

            var dicB = v_InputDataB.GetDictionaryVariable(engine);

            var myDic = new Dictionary<string, string>(dicA);

            string keyExists = this.GetUISelectionValue(nameof(v_KeyExists), "When Key Exists", engine);

            switch (keyExists)
            {
                case "ignore":
                    foreach(var v in dicB)
                    {
                        if (!myDic.ContainsKey(v.Key))
                        {
                            myDic.Add(v.Key, v.Value);
                        }
                    }
                    break;
                case "overwrite":
                    foreach(var v in dicB)
                    {
                        if (!myDic.ContainsKey(v.Key))
                        {
                            myDic.Add(v.Key, v.Value);
                        }
                        else
                        {
                            myDic[v.Key] = v.Value;
                        }
                    }
                    break;
                case "error":
                    foreach(var v in dicB)
                    {
                        if (!myDic.ContainsKey(v.Key))
                        {
                            myDic.Add(v.Key, v.Value);
                        }
                        else
                        {
                            throw new Exception("Key " + v.Key + " is already exists.");
                        }
                    }
                    break;
            }
            myDic.StoreInUserVariable(engine, v_OutputName);
        }
    }
}