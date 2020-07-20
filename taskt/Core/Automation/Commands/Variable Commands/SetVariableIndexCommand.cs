using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;
using taskt.Core.Script;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("Variable Commands")]
    [Description("This command sets the current index of a variable.")]
    public class SetVariableIndexCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Variable Name")]
        [InputSpecification("Select or provide a variable from the variable list.")]
        [SampleUsage("vSomeVariable || {vSomeVariable}")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_VariableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Variable Index")]
        [InputSpecification("Enter the index of the variable.")]
        [SampleUsage("1 || 2 || {vIndex}")]
        [Remarks("You can use variables in input if you encase them within braces {vIndex}. You can also perform basic math operations.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Index { get; set; }

        public SetVariableIndexCommand()
        {
            CommandName = "SetVariableIndexCommand";
            SelectionName = "Set Variable Index";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (AutomationEngineInstance)sender;
            var requiredVariable = VariableMethods.LookupVariable(engine, v_VariableName);

            //if still not found and user has elected option, create variable at runtime
            if ((requiredVariable == null) && (engine.EngineSettings.CreateMissingVariablesDuringExecution))
            {
                engine.VariableList.Add(new ScriptVariable() { VariableName = v_VariableName });
                requiredVariable = VariableMethods.LookupVariable(engine, v_VariableName);
            }

            if (requiredVariable != null)
            {
                var index = int.Parse(v_Index.ConvertToUserVariable(sender));
                requiredVariable.CurrentPosition = index;
            }
            else
            {
                throw new Exception("Attempted to update variable index, but variable was not found. Enclose variables within braces, ex. {vVariable}");
            }
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            //custom rendering
            base.Render(editor);

            //create control for variable name
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_VariableName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Index", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Update Variable '{v_VariableName}' Index to '{v_Index}']";
        }
    }
}