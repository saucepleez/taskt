using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Script;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Secure Data Commands")]
    [Description("This command adds text as a SecureString into a variable.")]
    public class NewSecureStringCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("New SecureString Name")]
        [InputSpecification("Indicate a unique reference name for later use.")]
        [SampleUsage("vSecureString")]
        [Remarks("")]
        public string v_SecureString { get; set; }

        [XmlAttribute]
        [PropertyDescription("Input Text")]
        [InputSpecification("Enter the text for the variable.")]
        [SampleUsage("Some Text || {vText}")]
        [Remarks("You can use variables in input if you encase them within braces {vText}. You can also perform basic math operations.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Input { get; set; }

        public NewSecureStringCommand()
        {
            CommandName = "NewSecureStringCommand";
            SelectionName = "New SecureString";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var requiredVariable = VariableMethods.LookupVariable(engine, v_SecureString);

            //If Variable already exists
            if (requiredVariable != null)
                requiredVariable.VariableValue = v_Input.ConvertToUserVariable(engine).GetSecureString();
            else
            {
                engine.VariableList.Add(new ScriptVariable()
                {
                    VariableName = v_SecureString.ConvertToUserVariable(engine),
                    VariableValue = v_Input.ConvertToUserVariable(engine).GetSecureString()
                });
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            //custom rendering
            base.Render(editor);

            //create control for variable name
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SecureString", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Input", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Assign '{v_Input}' to New SecureString Variable '{v_SecureString}']";
        }
    }
}
