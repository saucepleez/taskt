using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using System.Text.RegularExpressions;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Regex Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to Split Text based on RegEx Match")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Split Text based on Regex Pattern")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Split Action of Regex for given input Text and Regex Pattern and returns an array of strings")]
    public class RegexSplitCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please input the data you want to perform regex on")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter Variable or Text to apply Regex on")]
        [Attributes.PropertyAttributes.SampleUsage("**Hello** or **vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_InputTextData { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter regex pattern")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a Regex Pattern to apply to split text based on RegEx")]
        [Attributes.PropertyAttributes.SampleUsage(@"**^([\w\-]+)** or **vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_RegEx { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select variable to get regex result")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_OutputVariableName { get; set; }

        public RegexSplitCommand()
        {
            this.CommandName = "RegexSplitCommand";
            this.SelectionName = "Regex Split";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vInputData = v_InputTextData.ConvertToUserVariable(engine);
            string vRegex = v_RegEx.ConvertToUserVariable(engine);
            var vResultData = Regex.Split(vInputData, vRegex).ToList();

            engine.AddVariable(v_OutputVariableName, vResultData);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Apply Regex to '" + v_InputTextData + "', Get Result in: '" + v_OutputVariableName + "']";
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_RegEx", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputTextData", this, editor));
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_OutputVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_OutputVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_OutputVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);

            return RenderedControls;
        }
    }
}
