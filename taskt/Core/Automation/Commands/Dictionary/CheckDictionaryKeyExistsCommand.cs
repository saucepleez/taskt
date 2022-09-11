using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
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
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.TextBox)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Key", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Key")]
        public string v_Key { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the variable to apply Result")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**vResult** or **{{{vResult}}}**")]
        [Remarks("")]
        [PropertyIsVariablesList(true)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Boolean, true)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Result")]
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

            //Dictionary<string, string> dic = (Dictionary<string, string>)v_InputData.GetRawVariable(engine).VariableValue;
            Dictionary<string, string> dic = v_InputData.GetDictionaryVariable(engine);
            dic.ContainsKey(vKey).StoreInUserVariable(engine, v_applyToVariable);
        }
        
        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctrls);

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + $" [From: {v_InputData}, Key: {v_Key}, Result: {v_applyToVariable}]";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(v_InputData))
        //    {
        //        this.IsValid = false;
        //        this.validationResult += "Dictionary Variable Name is empty.\n";
        //    }
        //    if (String.IsNullOrEmpty(v_Key))
        //    {
        //        this.IsValid = false;
        //        this.validationResult += "Key is empty.\n";
        //    }
        //    if (String.IsNullOrEmpty(v_applyToVariable))
        //    {
        //        this.IsValid = false;
        //        this.validationResult += "Variable is empty.\n";
        //    }

        //    return this.IsValid;
        //}
    }
}