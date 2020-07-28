using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    [Description("This command allows you to get all matches based on Regex Pattern")]
    [UsesDescription("Use this command when you want to get all matches from an input (text) based on Regex Pattern")]
    [ImplementationDescription("This command implements 'Matches' Action of Regex for given input Text and Regex Pattern and returns all found matches")]
    public class GetRegexMatchesCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please enter regex pattern")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a Regex Pattern to apply to Input Text to get all the matches")]
        [SampleUsage(@"**^([\w\-]+)** or **vSomeVariable**")]
        [Remarks("")]
        public string v_Regex { get; set; }
        [XmlAttribute]
        [PropertyDescription("Please input the data you want to perform regex on")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter Variable or Text to apply Regex on")]
        [SampleUsage("**Hello** or **vSomeVariable**")]
        [Remarks("")]
        public string v_InputData { get; set; }
        [XmlAttribute]
        [PropertyDescription("Please select variable to get regex result")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("")]
        public string v_OutputVariableName { get; set; }

        public GetRegexMatchesCommand()
        {
            CommandName = "GetRegexMatchesCommand";
            SelectionName = "Get Regex Matches";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vInputData = v_InputData.ConvertToUserVariable(engine);
            string vRegex = v_Regex.ConvertToUserVariable(engine);

            var matches = (from match in Regex.Matches(vInputData, vRegex).Cast<Match>() select match.Groups[0].Value).ToList();

            engine.AddVariable(v_OutputVariableName,matches);
        }
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Regex", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputData", this, editor));
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_OutputVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_OutputVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_OutputVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Apply Regex to '" + v_InputData + "', Get All Matches in: '" + v_OutputVariableName + "']";
        }
    }
}