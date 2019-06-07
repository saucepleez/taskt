using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{


    [Serializable]
    [Attributes.ClassAttributes.Group("Variable Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to explicitly add a variable if you are not using **Set Variable* with the setting **Create Missing Variables** at runtime.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to modify the value of variables.  You can even use variables to modify other variables.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    public class AddVariableCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please define the name of the new variable")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If the variable exists, the value of the old variable will be replaced with the new one")]
        public string v_userVariableName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please define the input to be set to above variable")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the input that the variable's value should be set to.")]
        [Attributes.PropertyAttributes.SampleUsage("Hello or [vNum]+1")]
        [Attributes.PropertyAttributes.Remarks("You can use variables in input if you encase them within brackets [vName].  You can also perform basic math operations.")]
        public string v_Input { get; set; }
        [Attributes.PropertyAttributes.PropertyDescription("Define the action to take if the variable already exists")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Do Nothing If Variable Exists")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Error If Variable Exists")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Replace If Variable Exists")]
        [Attributes.PropertyAttributes.InputSpecification("Select the appropriate handler from the list")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_IfExists { get; set; }
        public AddVariableCommand()
        {
            this.CommandName = "AddVariableCommand";
            this.SelectionName = "New Variable";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;


            if (!engine.VariableList.Any(f => f.VariableName == v_userVariableName))
            {
                //variable does not exist so add to the list
                try
                {

                    var variableValue = v_Input.ConvertToUserVariable(engine);

                    engine.VariableList.Add(new Script.ScriptVariable
                    {
                        VariableName = v_userVariableName,
                        VariableValue = variableValue
                    });
                }
                catch (Exception ex)
                {
                    throw new Exception("Encountered an error when adding variable '" + v_userVariableName + "': " + ex.ToString());
                }
            }
            else
            {
                //variable exists so decide what to do
                switch (v_IfExists)
                {
                    case "Replace If Variable Exists":
                        v_Input.ConvertToUserVariable(sender).StoreInUserVariable(engine, v_userVariableName);
                        break;
                    case "Error If Variable Exists":
                        throw new Exception("Attempted to create a variable that already exists! Use 'Set Variable' instead or change the Exception Setting in the 'Add Variable' Command.");
                    default:
                        break;
                }
               
            }

         
         

        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_userVariableName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Input", this, editor));


            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_IfExists", this));
            var dropdown = CommandControls.CreateDropdownFor("v_IfExists", this);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_IfExists", this, new Control[] { dropdown }, editor));
            RenderedControls.Add(dropdown);

            return RenderedControls;
        }



        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Assign '" + v_Input + "' to New Variable '" + v_userVariableName + "']";
        }
    }
}