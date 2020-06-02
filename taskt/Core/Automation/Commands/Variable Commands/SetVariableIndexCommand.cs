using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using taskt.UI.CustomControls;
namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Variable Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to modify variables.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to modify the value of variables.  You can even use variables to modify other variables.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    public class SetVariableIndexCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select a variable to modify")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_userVariableName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please set the current index of the variable")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the input that the variable's index should be set to.")]
        [Attributes.PropertyAttributes.SampleUsage("1, 2, etc.")]
        [Attributes.PropertyAttributes.Remarks("You can use variables in input if you encase them within brackets {vName}.  You can also perform basic math operations.")]
        public string v_Index { get; set; }
        public SetVariableIndexCommand()
        {
            this.CommandName = "SetVariableIndexCommand";
            this.SelectionName = "Set Variable Index";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            var requiredVariable = LookupVariable(engine);

            //if still not found and user has elected option, create variable at runtime
            if ((requiredVariable == null) && (engine.engineSettings.CreateMissingVariablesDuringExecution))
            {
                engine.VariableList.Add(new Script.ScriptVariable() { VariableName = v_userVariableName });
                requiredVariable = LookupVariable(engine);
            }

            if (requiredVariable != null)
            {

                var index = int.Parse(v_Index.ConvertToUserVariable(sender));

                requiredVariable.CurrentPosition = index;
          
            }
            else
            {
                throw new Exception("Attempted to update variable index, but variable was not found. Enclose variables within brackets, ex. {vVariable}");
            }
        }

        private Script.ScriptVariable LookupVariable(Core.Automation.Engine.AutomationEngineInstance sendingInstance)
        {
            //search for the variable
            var requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == v_userVariableName).FirstOrDefault();

            //if variable was not found but it starts with variable naming pattern
            if ((requiredVariable == null) && (v_userVariableName.StartsWith(sendingInstance.engineSettings.VariableStartMarker)) && (v_userVariableName.EndsWith(sendingInstance.engineSettings.VariableEndMarker)))
            {
                //reformat and attempt
                var reformattedVariable = v_userVariableName.Replace(sendingInstance.engineSettings.VariableStartMarker, "").Replace(sendingInstance.engineSettings.VariableEndMarker, "");
                requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == reformattedVariable).FirstOrDefault();
            }

            return requiredVariable;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Update Variable '" + v_userVariableName + "' index to '" + v_Index + "']";
        }

        public override List<Control> Render(UI.Forms.frmCommandEditor editor)
        {
            //custom rendering
            base.Render(editor);


            //create control for variable name
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_userVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_userVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_userVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Index", this, editor));

        

            return RenderedControls;


        }
    }
}