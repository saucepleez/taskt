using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.SubGruop("Convert")]
    [Attributes.ClassAttributes.Description("This command convert a List to Dictionary.")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ConvertListToDictionaryCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Supply the List to convert")]
        [InputSpecification("")]
        [SampleUsage("**vList** or **{{{vList}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyValidationRule("List to Convert", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "List to Convert")]
        public string v_InputList { get; set; }

        [XmlAttribute]
        [PropertyDescription("Supply the Dictionary Keys Name List")]
        [InputSpecification("")]
        [SampleUsage("**vKeys** or **{{{vKeys}}}**")]
        [Remarks("If keys is empty, Dictionary key is item0, item1, ...")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyDisplayText(true, "Dictionary Keys Name List")]
        public string v_Keys { get; set; }

        [XmlAttribute]
        [PropertyDescription("When the number of items in the List is greater than the number of Keys")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyIsOptional(true, "Ignore")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyUISelectionOption("Try Create Keys")]
        public string v_KeysNotEnough { get; set; }

        [XmlAttribute]
        [PropertyDescription("When the number of Keys is greater than the number of items in the List")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyIsOptional(true, "Ignore")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyUISelectionOption("Insert Empty Value")]
        public string v_ListItemNotEnough { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the variable to receive the Dictionary Variable")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Dictionary)]
        [PropertyValidationRule("Dictionary", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Dictionary")]
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
            var engine = (Engine.AutomationEngineInstance)sender;

            List<string> targetList = v_InputList.GetListVariable(engine);

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
                List<string> targetKeys = v_Keys.GetListVariable(engine);

                string keysNotEnough = v_KeysNotEnough.GetUISelectionValue("v_KeysNotEnough", this, engine);
                string listItemNotEnough = v_ListItemNotEnough.GetUISelectionValue("v_ListItemNotEnough", this, engine);

                if ((keysNotEnough == "error") && (targetList.Count > targetKeys.Count))
                {
                    throw new Exception("The number of keys in " + v_Keys + " is not enough");
                }
                if ((listItemNotEnough == "error") && (targetKeys.Count > targetList.Count))
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
                    switch (keysNotEnough)
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
                    }
                }
                else
                {
                    switch (listItemNotEnough)
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
                    }
                }
            }
            myDic.StoreInUserVariable(engine, v_applyToVariableName);
        }
    }
}