using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Variable Commands")]
    [Description("This command adds a new variable or updates an existing variable.")]
    public class NewVariableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("New Variable Name")]
        [InputSpecification("Indicate a unique reference name for later use.")]
        [SampleUsage("vSomeVariable")]
        [Remarks("")]
        public string v_VariableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Input Value")]
        [InputSpecification("Enter the value for the variable.")]
        [SampleUsage("Hello || {vNum} || {vNum}+1")]
        [Remarks("You can use variables in input if you encase them within braces {vSomeValue}. You can also perform basic math operations.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Input { get; set; }

        [PropertyDescription("Additional Actions")]
        [PropertyUISelectionOption("Do Nothing If Variable Exists")]
        [PropertyUISelectionOption("Error If Variable Exists")]
        [PropertyUISelectionOption("Replace If Variable Exists")]
        [InputSpecification("Select an action to take if the variable already exists.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_IfExists { get; set; }

        public NewVariableCommand()
        {
            CommandName = "NewVariableCommand";
            SelectionName = "New Variable";
            CommandEnabled = true;
            CustomRendering = true;
            v_IfExists = "Do Nothing If Variable Exists";
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (AutomationEngineInstance)sender;

            if (!engine.VariableList.Any(f => f.VariableName == v_VariableName))
            {
                //variable does not exist so add to the list
                try
                {
                    v_Input.StoreInUserVariable(engine, v_VariableName);
                }
                catch (Exception ex)
                {
                    throw new Exception("Encountered an error when adding variable '" + v_VariableName + "': " + ex.ToString());
                }
            }
            else
            {
                //variable exists so decide what to do
                switch (v_IfExists)
                {
                    case "Replace If Variable Exists":
                        v_Input.ConvertToUserVariable(engine).StoreInUserVariable(engine, v_VariableName);
                        break;
                    case "Error If Variable Exists":
                        throw new Exception("Attempted to create a variable that already exists! Use 'Set Variable' instead or change the Exception Setting in the 'Add Variable' Command.");
                    default:
                        break;
                }
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_VariableName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Input", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_IfExists", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Assign '{v_Input}' to New Variable '{v_VariableName}']";
        }
    }
}