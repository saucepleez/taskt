using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using System.Text.RegularExpressions;
using taskt.Core.Utilities.CommonUtilities;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Regex Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to get all matches based on Regex Pattern")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get all matches from an input (text) based on Regex Pattern")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Matches' Action of Regex for given input Text and Regex Pattern and returns all found matches")]
    public class GetRegexMatchesCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter regex pattern")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a Regex Pattern to apply to Input Text to get all the matches")]
        [Attributes.PropertyAttributes.SampleUsage(@"**^([\w\-]+)** or **vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_Regex { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please input the data you want to perform regex on")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter Variable or Text to apply Regex on")]
        [Attributes.PropertyAttributes.SampleUsage("**Hello** or **vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_InputData { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select variable to get regex result")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_OutputVariableName { get; set; }

        public GetRegexMatchesCommand()
        {
            this.CommandName = "GetRegexMatchesCommand";
            this.SelectionName = "Get Regex Matches";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vInputData = v_InputData.ConvertToUserVariable(engine);
            string vRegex = v_Regex.ConvertToUserVariable(engine);

            var matches = (from match in Regex.Matches(vInputData, vRegex).Cast<Match>() select match.Groups[0].Value).ToList();

            engine.AddVariable(v_OutputVariableName,matches);
        }
        public override List<Control> Render(frmCommandEditor editor)
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