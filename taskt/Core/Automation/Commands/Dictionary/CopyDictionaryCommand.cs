﻿using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary Commands")]
    [Attributes.ClassAttributes.SubGruop("Dictionary Action")]
    [Attributes.ClassAttributes.Description("This command allows you to copy a Dictionary.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to copy a Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CopyDictionaryCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please input The Dictionary Variable to Copy")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Enter a string of comma seperated values.")]
        //[SampleUsage("**myDictionary1** or **{{{vMyDic1}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Dictionary)]
        //[PropertyValidationRule("Dictionary to Copy", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Dictionary to Copy")]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        [PropertyDescription("Dictionary Variable Name to Copy")]
        [PropertyValidationRule("Dictionary to Copy", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Dictionary to Copy")]
        public string v_InputData { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please indicate the new Dictionary")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[SampleUsage("**newDic** or **{{{newDic}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Dictionary)]
        //[PropertyValidationRule("New Dictionary", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "New Dictionary")]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_NewOutputDictionaryName))]
        //[PropertyDescription("New Dictionary Variable")]
        //[PropertyValidationRule("New Dictionary", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "New Dictionary")]
        //[PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        //[PropertyDetailSampleUsage("**vNewDictionary**", PropertyDetailSampleUsage.ValueType.VariableName)]
        //[PropertyDetailSampleUsage("**{{{vNewDictionary}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        public string v_OutputName { get; set; }

        public CopyDictionaryCommand()
        {
            this.CommandName = "CopyDictionaryCommand";
            this.SelectionName = "Copy Dictionary";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            Dictionary<string, string> srcDic = v_InputData.GetDictionaryVariable(engine);

            Dictionary<string, string> newDic = new Dictionary<string, string>(srcDic);

            newDic.StoreInUserVariable(engine, v_OutputName);
        }
    }
}