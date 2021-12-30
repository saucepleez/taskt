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
    [Attributes.ClassAttributes.SubGruop("Dictionary Action")]
    [Attributes.ClassAttributes.Description("This command allows you to copy a Dictionary.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to copy a Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class CopyDictionaryCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please input The Dictionary Variable")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a string of comma seperated values.")]
        [Attributes.PropertyAttributes.SampleUsage("**myDictionary1** or **{{{vMyDic1}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Dictionary)]
        public string v_InputData { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the new Dictionary")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**newDic** or **{{{newDic}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        [Attributes.PropertyAttributes.PropertyParameterDirection(Attributes.PropertyAttributes.PropertyParameterDirection.ParameterDirection.Output)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Dictionary)]
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
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            //Script.ScriptVariable varA = v_InputData.GetRawVariable(engine);

            //if (varA == null)
            //{
            //    throw new Exception("Dictionary 1 " + v_InputData + " does not exists.");
            //}
            //else if (!(varA.VariableValue is Dictionary<string, string>))
            //{
            //    throw new Exception("Dictionary 1 " + v_InputData + " is not supported Dictionary.");
            //}
            //Dictionary<string, string> srcDic = (Dictionary<string, string>)varA.VariableValue;
            Dictionary<string, string> srcDic = v_InputData.GetDictionaryVariable(engine);

            Dictionary<string, string> newDic = new Dictionary<string, string>(srcDic);

            //foreach(var v in srcDic)
            //{
            //    newDic.Add(v.Key, v.Value);
            //}

            newDic.StoreInUserVariable(engine, v_OutputName);
        }
        
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Dictionary: '" + v_InputData + "', Result: '" + v_OutputName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(v_InputData))
            {
                this.IsValid = false;
                this.validationResult += "Dictionary Variable Name is empty.\n";
            }
            if (String.IsNullOrEmpty(v_OutputName))
            {
                this.IsValid = false;
                this.validationResult += "New Dictionary is empty.\n";
            }

            return this.IsValid;
        }
    }
}