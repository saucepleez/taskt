using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.Description("This command convert a List to Dictionary.")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class ConvertListToDictionaryCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Supply the List to convert")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**vList** or **{{{vList}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.List)]
        public string v_InputList { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Supply the Dictionary Keys")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**vKeys** or **{{{vKeys}}}**")]
        [Attributes.PropertyAttributes.Remarks("If keys is empty, Dictionary key is item0, item1, ...")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.List)]
        public string v_Keys { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("When the number of items in the List is greater than the number of Keys (Default is Ignore)")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Ignore")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Error")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Try Create Keys")]
        public string v_KeysNotEnough { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("When the number of Keys is greater than the number of items in the List (Default is Ignore)")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Ignore")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Error")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Insert Empty Value")]
        public string v_ListItemNotEnough { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the Dictionary")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        [Attributes.PropertyAttributes.PropertyParameterDirection(Attributes.PropertyAttributes.PropertyParameterDirection.ParameterDirection.Output)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Dictionary)]
        public string v_applyToVariableName { get; set; }

        public ConvertListToDictionaryCommand()
        {
            this.CommandName = "ConvertListToDictionaryCommand";
            this.SelectionName = "Convert List To Dictionary";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            //get variable by regular name
            Script.ScriptVariable listVariable = v_InputList.GetRawVariable(engine);

            if (listVariable == null)
            {
                throw new Exception(v_InputList + " does not exists.");
            }
            else if (!(listVariable.VariableValue is List<string>))
            {
                throw new Exception(v_InputList + " is not supported List");
            }

            List<string> targetList = (List<string>)listVariable.VariableValue;

            Dictionary<string, string> myDic = new Dictionary<string, string>();
            if (String.IsNullOrEmpty(v_Keys))
            {
                for (int i = 0; i < targetList.Count; i++)
                {
                    myDic.Add("item" + i.ToString(), targetList[i]);
                }
            }
            else
            {
                Script.ScriptVariable keysVariable = v_Keys.GetRawVariable(engine);
                if (keysVariable == null)
                {
                    throw new Exception(v_Keys + " does not exists.");
                }
                else if (!(keysVariable.VariableValue is List<string>))
                {
                    throw new Exception(v_Keys + " is not supported List.");
                }

                List<string> targetKeys = (List<string>)keysVariable.VariableValue;

                if (String.IsNullOrEmpty(v_KeysNotEnough))
                {
                    v_KeysNotEnough = "Ignore";
                }
                if (String.IsNullOrEmpty(v_ListItemNotEnough))
                {
                    v_ListItemNotEnough = "Ignore";
                }

                if ((v_KeysNotEnough.ToLower() == "error") && (targetList.Count > targetKeys.Count))
                {
                    throw new Exception("The number of keys in " + v_Keys + " is not enough");
                }
                if ((v_ListItemNotEnough.ToLower() == "error") && (targetKeys.Count > targetList.Count))
                {
                    throw new Exception("The number of List items in " + v_InputList + " is not enough");
                }

                if (targetList.Count == targetKeys.Count)
                {
                    for (int i = 0; i < targetList.Count; i++)
                    {
                        myDic.Add(targetKeys[i], targetList[i]);
                    }
                }
                else if (targetList.Count > targetKeys.Count)
                {
                    switch (v_KeysNotEnough.ToLower())
                    {
                        case "ignore":
                            for (int i = 0; i < targetKeys.Count; i++)
                            {
                                myDic.Add(targetKeys[i], targetList[i]);
                            }
                            break;

                        case "try create keys":
                            for (int i = 0; i < targetKeys.Count; i++)
                            {
                                myDic.Add(targetKeys[i], targetList[i]);
                            }
                            for (int i = targetKeys.Count; i < targetList.Count; i++)
                            {
                                myDic.Add("item" + i.ToString(), targetList[i]);
                            }
                            break;

                        default:
                            throw new Exception("Strange value " + v_KeysNotEnough);
                            break;
                    }
                }
                else
                {
                    switch (v_ListItemNotEnough.ToLower())
                    {
                        case "ignore":
                            for (int i = 0; i < targetList.Count; i++)
                            {
                                myDic.Add(targetKeys[i], targetList[i]);
                            }
                            break;

                        case "insert empty value":
                            for (int i = 0; i < targetList.Count; i++)
                            {
                                myDic.Add(targetKeys[i], targetList[i]);
                            }
                            for (int i = targetList.Count; i < targetKeys.Count; i++)
                            {
                                myDic.Add(targetKeys[i], "");
                            }
                            break;

                        default:
                            throw new Exception("Strange value " + v_KeysNotEnough);
                            break;
                    }
                }
            }
            myDic.StoreInUserVariable(engine, v_applyToVariableName);
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Convert List '" + this.v_InputList + "' To Dictionary '" + this.v_applyToVariableName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_InputList))
            {
                this.validationResult += "List is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_applyToVariableName))
            {
                this.validationResult += "Variable to recieve the Dictionary is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}