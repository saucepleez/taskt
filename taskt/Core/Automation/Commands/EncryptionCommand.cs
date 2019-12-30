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
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.Description("This command handles text encryption")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to store some data encrypted")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class EncryptionCommand : ScriptCommand
    {
        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Select Encryption Action")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Encrypt")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Decrypt")]
        [Attributes.PropertyAttributes.InputSpecification("Select an action to take")]
        [Attributes.PropertyAttributes.SampleUsage("Select from **Encrypt**, **Decrypt**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_EncryptionType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Supply the data or variable (ex. {someVariable})")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable or json array value")]
        [Attributes.PropertyAttributes.SampleUsage("**Test** or **{var}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Provide a Pass Phrase")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable or json array value")]
        [Attributes.PropertyAttributes.SampleUsage("**Test** or **{var}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_PassPhrase { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the encrypted data")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_applyToVariableName { get; set; }

        public EncryptionCommand()
        {
            this.CommandName = "EncryptionCommand";
            this.SelectionName = "Encryption Command";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_EncryptionType = "Encrypt";
            this.v_PassPhrase = "TASKT";


        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            //get variablized input
            var variableInput = v_InputValue.ConvertToUserVariable(sender);
            var passphrase = v_PassPhrase.ConvertToUserVariable(sender);

            string resultData = "";
            if (v_EncryptionType.ConvertToUserVariable(sender) == "Encrypt")
            {
                //encrypt data
                resultData = Core.EncryptionServices.EncryptString(variableInput, passphrase);
            }
            else if (v_EncryptionType.ConvertToUserVariable(sender) == "Decrypt")
            {
                //encrypt data
                resultData = Core.EncryptionServices.DecryptString(variableInput, passphrase);
            }
            else
            {
                throw new NotImplementedException($"Encryption Service Requested '{v_EncryptionType.ConvertToUserVariable(sender)}' has not been implemented");
            }

            //get variable
            var requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == v_applyToVariableName).FirstOrDefault();

            //create if var does not exist
            if (requiredComplexVariable == null)
            {
                engine.VariableList.Add(new Script.ScriptVariable() { VariableName = v_applyToVariableName, CurrentPosition = 0 });
                requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == v_applyToVariableName).FirstOrDefault();
            }

            //assign value to variable
            requiredComplexVariable.VariableValue = resultData;

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_EncryptionType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputValue", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_PassPhrase", this, editor));

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);

            return RenderedControls;

        }



        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [{v_EncryptionType} Data, apply to '{v_applyToVariableName}']";
        }
    }
}