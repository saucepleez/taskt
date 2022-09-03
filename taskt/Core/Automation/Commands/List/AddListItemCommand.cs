using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.SubGruop("List Item")]
    [Attributes.ClassAttributes.Description("This command allows you to add list item.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add list item.  You can even use variables to modify other variables.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class AddListItemCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select a List Variable Name to modify")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vList** or **{{{vList}}}**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyValidationRule("List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "List Variable")]
        public string v_ListName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please define the input to be added to the variable")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter the input that the variable's value should be set to.")]
        [SampleUsage("**Hello** or **{{{vValue}}}**")]
        [Remarks("You can use variables in input if you encase them within brackets {{{vName}}}.  You can also perform basic math operations.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "Value")]
        public string v_Input { get; set; }
        public AddListItemCommand()
        {
            this.CommandName = "AddListItemCommand";
            this.SelectionName = "Add List Item";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            List<string> lst = v_ListName.GetListVariable(engine);

            var variableInput = v_Input.ConvertToUserVariable(sender);
            lst.Add(variableInput);
        }

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Add List Item List: '" + v_Input + "', Item: '" + v_ListName + "']";
        //}

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    //custom rendering
        //    base.Render(editor);

        //    //create control for variable name
        //    //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_userVariableName", this));
        //    //var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_userVariableName", this).AddVariableNames(editor);
        //    //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_userVariableName", this, new Control[] { VariableNameControl }, editor));
        //    //RenderedControls.Add(VariableNameControl);

        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Input", this, editor));

        //    var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctrls);

        //    return RenderedControls;
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_ListName))
        //    {
        //        this.validationResult += "List variable is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}