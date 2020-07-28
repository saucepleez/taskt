using System;
using System.Collections.Generic;
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
    [Group("Engine Commands")]
    [Description("This command allows you to set preferences for engine behavior.")]
    [UsesDescription("Use this command when you want to change the engine behavior.")]
    [ImplementationDescription("")]
    public class SetEnginePreferenceCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Select Parameter Type")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUISelectionOption("Enable Automatic Calculations")]
        [PropertyUISelectionOption("Disable Automatic Calculations")]
        [InputSpecification("")]
        [Remarks("")]
        public string v_PreferenceType { get; set; }

        public SetEnginePreferenceCommand()
        {
            CommandName = "SetEnginePreferenceCommand";
            SelectionName = "Set Engine Preference";
            CommandEnabled = true;
            CustomRendering = true;

        }
        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            var preference = v_PreferenceType.ConvertToUserVariable(engine);

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
        public override List<Control> Render(IfrmCommandEditor editor)
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