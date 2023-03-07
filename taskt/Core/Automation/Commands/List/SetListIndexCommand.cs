using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.SubGruop("Other")]
    [Attributes.ClassAttributes.CommandSettings("Set List Index")]
    [Attributes.ClassAttributes.Description("This command allows you to modify List Index.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to modify List Index.  You can even use variables to modify other variables.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SetListIndexCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select a List Variable Name to modify")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vList** or **{{{vList}}}**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyValidationRule("List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "List")]
        public string v_ListName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please set the current Index of the List")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter the input that the variable's index should be set to.")]
        [SampleUsage("**0** or **-1** or **{{{vIndex}}}**")]
        [Remarks("You can use variables in input if you encase them within brackets {{{vName}}}.  You can also perform basic math operations.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Index", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Index")]
        public string v_Index { get; set; }

        public SetListIndexCommand()
        {
            //this.CommandName = "SetListIndexCommand";
            //this.SelectionName = "Set List Index";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            var requiredVariable = v_ListName.GetRawVariable(engine);

            if (requiredVariable == null)
            {
                throw new Exception("Attempted to update variable index, but variable was not found. Enclose variables within brackets, ex. {vVariable}");
            }
            Type varType = requiredVariable.VariableValue.GetType();
            if (!varType.IsGenericType || (varType.GetGenericTypeDefinition() != typeof(List<>)))
            {
                throw new Exception(v_ListName + " is not List");
            }

            var index = int.Parse(v_Index.ConvertToUserVariable(sender));
            if (index >= 0)
            {
                requiredVariable.CurrentPosition = index;
            }
            else
            {
                throw new Exception("Index is not >= 0");
            }
        }
    }
}