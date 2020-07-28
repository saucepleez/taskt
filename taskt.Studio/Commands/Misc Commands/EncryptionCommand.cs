using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Script;
using taskt.Core.Utilities.CommandUtilities;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Misc Commands")]
    [Description("This command handles text encryption")]
    [UsesDescription("Use this command when you want to store some data encrypted")]
    [ImplementationDescription("")]
    public class EncryptionCommand : ScriptCommand
    {
        [XmlElement]
        [PropertyDescription("Select Encryption Action")]
        [PropertyUISelectionOption("Encrypt")]
        [PropertyUISelectionOption("Decrypt")]
        [InputSpecification("Select an action to take")]
        [SampleUsage("Select from **Encrypt**, **Decrypt**")]
        [Remarks("")]
        public string v_EncryptionType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Supply the data or variable (ex. {someVariable})")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Select or provide a variable or json array value")]
        [SampleUsage("**Test** or **{var}**")]
        [Remarks("")]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [PropertyDescription("Provide a Pass Phrase")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Select or provide a variable or json array value")]
        [SampleUsage("**Test** or **{var}**")]
        [Remarks("")]
        public string v_PassPhrase { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the variable to receive the encrypted data")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_applyToVariableName { get; set; }

        public EncryptionCommand()
        {
            CommandName = "EncryptionCommand";
            SelectionName = "Encryption Command";
            CommandEnabled = true;
            CustomRendering = true;
            v_EncryptionType = "Encrypt";
            v_PassPhrase = "TASKT";


        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            //get variablized input
            var variableInput = v_InputValue.ConvertToUserVariable(engine);
            var passphrase = v_PassPhrase.ConvertToUserVariable(engine);

            string resultData = "";
            if (v_EncryptionType.ConvertToUserVariable(engine) == "Encrypt")
            {
                //encrypt data
                resultData = EncryptionServices.EncryptString(variableInput, passphrase);
            }
            else if (v_EncryptionType.ConvertToUserVariable(engine) == "Decrypt")
            {
                //encrypt data
                resultData = EncryptionServices.DecryptString(variableInput, passphrase);
            }
            else
            {
                throw new NotImplementedException($"Encryption Service Requested '{v_EncryptionType.ConvertToUserVariable(engine)}' has not been implemented");
            }

            //get variable
            var requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == v_applyToVariableName).FirstOrDefault();

            //create if var does not exist
            if (requiredComplexVariable == null)
            {
                engine.VariableList.Add(new ScriptVariable() { VariableName = v_applyToVariableName, CurrentPosition = 0 });
                requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == v_applyToVariableName).FirstOrDefault();
            }

            //assign value to variable
            requiredComplexVariable.VariableValue = resultData;

        }
        public override List<Control> Render(IfrmCommandEditor editor)
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