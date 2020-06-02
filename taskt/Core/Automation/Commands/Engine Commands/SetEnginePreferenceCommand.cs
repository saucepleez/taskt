using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Engine Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to set preferences for engine behavior.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to change the engine behavior.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class SetEnginePreferenceCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select Parameter Type")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Enable Automatic Calculations")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Disable Automatic Calculations")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_PreferenceType { get; set; }

        public SetEnginePreferenceCommand()
        {
            this.CommandName = "SetEnginePreferenceCommand";
            this.SelectionName = "Set Engine Preference";
            this.CommandEnabled = true;
            this.CustomRendering = true;

        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            var preference = v_PreferenceType.ConvertToUserVariable(sender);

            switch (preference)
            {
                case "Enable Automatic Calculations":
                    engine.AutoCalculateVariables = true;
                    break;
                case "Disable Automatic Calculations":
                    engine.AutoCalculateVariables = false;
                    break;
                default:
                    throw new NotImplementedException($"The preference '{preference}' is not implemented.");
            }


        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_PreferenceType", this, editor));

            return RenderedControls;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [{v_PreferenceType}]";
        }
    }
}