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
    [Attributes.ClassAttributes.Description("This command allows you to concatenate two Dictionaries.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to concatenate two Dictionaries.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class ConcatenateDictionaryCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please input The Dictionary Variable 1")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a string of comma seperated values.")]
        [Attributes.PropertyAttributes.SampleUsage("**myDictionary1** or **{{{vMyDic1}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Dictionary)]
        public string v_InputDataA { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please input The Dictionary Variable 2")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a string of comma seperated values.")]
        [Attributes.PropertyAttributes.SampleUsage("**myDictionary2** or **{{{vMyDic2}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Dictionary)]
        public string v_InputDataB { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("If Key already exists")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**Ignore** or **Overwrite** or **Error**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyIsOptional(true, "Ignore")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Ignore")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Overwrite")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Error")]
        public string v_KeyExists { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the result Dictionary")]
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

        public ConcatenateDictionaryCommand()
        {
            this.CommandName = "ConcatenateDictionaryCommand";
            this.SelectionName = "Concatenate Dictionary";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            //Script.ScriptVariable varA = v_InputDataA.GetRawVariable(engine);

            //if (varA == null)
            //{
            //    throw new Exception("Dictionary 1 " + v_InputDataA + " does not exists.");
            //}
            //else if (!(varA.VariableValue is Dictionary<string, string>))
            //{
            //    throw new Exception("Dictionary 1 " + v_InputDataA + " is not supported Dictionary.");
            //}
            //Dictionary<string, string> dicA = (Dictionary<string, string>)varA.VariableValue;
            Dictionary<string, string> dicA = v_InputDataA.GetDictionaryVariable(engine);

            //Script.ScriptVariable varB = v_InputDataB.GetRawVariable(engine);
            //if (varB == null)
            //{
            //    throw new Exception("Dictionary 2 " + v_InputDataB + " does not exists.");
            //}
            //else if (!(varB.VariableValue is Dictionary<string, string>))
            //{
            //    throw new Exception("Dictionary 2 " + v_InputDataB + " is not supported Dictionary.");
            //}
            //Dictionary<string, string> dicB = (Dictionary<string, string>)varB.VariableValue;
            Dictionary<string, string> dicB = v_InputDataB.GetDictionaryVariable(engine);

            Dictionary<string, string> myDic = new Dictionary<string, string>();
            foreach(var v in dicA)
            {
                myDic.Add(v.Key, v.Value);
            }

            //string keyExists;
            //if (String.IsNullOrEmpty(v_KeyExists))
            //{
            //     keyExists = "Ignore";
            //}
            //else
            //{
            //     keyExists = v_KeyExists.ConvertToUserVariable(engine);
            //}
            string keyExists = v_KeyExists.GetUISelectionValue("v_KeyExists", this, engine);

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
        
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Dictionary1: '" + v_InputDataA + "', Dictionary2: '" + v_InputDataB + "', Result: '" + v_OutputName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            if (String.IsNullOrEmpty(v_InputDataA))
            {
                this.IsValid = false;
                this.validationResult += "Dictionary Variable Name 1 is empty.\n";
            }
            if (String.IsNullOrEmpty(v_InputDataB))
            {
                this.IsValid = false;
                this.validationResult += "Dictionary Variable Name 2 is empty.\n";
            }
            if (String.IsNullOrEmpty(v_OutputName))
            {
                this.IsValid = false;
                this.validationResult += "Result Dictionary is empty.\n";
            }

            return this.IsValid;
        }
    }
}