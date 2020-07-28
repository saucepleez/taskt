using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
using Group = taskt.Core.Attributes.ClassAttributes.Group;

namespace taskt.Commands
{
    [Serializable]
    [Group("Regex Commands")]
    [Description("This command allows you to replace all the matches in Text based on RegEx")]
    [UsesDescription("Use this command when you want to replace all matches in text based on Regex Pattern")]
    [ImplementationDescription("This command implements Replace Action of Regex for given input Text and Regex Pattern and returns a text after replacement")]
    public class RegexReplaceCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please input the data you want to perform regex on")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter Variable or Text to apply Regex on")]
        [SampleUsage("**Hello** or **vSomeVariable**")]
        [Remarks("")]
        public string v_InputTextData { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please enter regex pattern")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a Regex Pattern to apply to replace matches with given text")]
        [SampleUsage(@"**^([\w\-]+)** or **vSomeVariable**")]
        [Remarks("")]
        public string v_RegEx { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please input the data (text) to replace all the matches")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter Variable or Text to replace the matches")]
        [SampleUsage("**Hello** or **vSomeVariable**")]
        [Remarks("")]
        public string v_ReplaceTextData { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select variable to get regex result")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("")]
        public string v_OutputVariableName { get; set; }

        public RegexReplaceCommand()
        {
            CommandName = "RegexReplaceCommand";
            SelectionName = "Regex Replace";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vInputData = v_InputTextData.ConvertToUserVariable(engine);
            string vRegex = v_RegEx.ConvertToUserVariable(engine);
            string vReplaceData = v_ReplaceTextData.ConvertToUserVariable(engine);
            string resultData = Regex.Replace(vInputData, vRegex, vReplaceData);

            resultData.StoreInUserVariable(engine, v_OutputVariableName);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Replace All Matches with '" + v_ReplaceTextData + "', Get Result in: '" + v_OutputVariableName + "']";
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_RegEx", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputTextData", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ReplaceTextData", this, editor));
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_OutputVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_OutputVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_OutputVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);

            return RenderedControls;
        }
    }
}
