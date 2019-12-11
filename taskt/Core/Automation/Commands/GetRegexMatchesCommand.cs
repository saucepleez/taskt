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
    [Attributes.ClassAttributes.Description("This command allows you to loop through an Excel Dataset")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to iterate over a series of Excel cells.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command attempts to loop through a known Excel DataSet")]
    public class GetRegexMatchesCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Enter Regex Expressions.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a string of comma seperated values.")]
        [Attributes.PropertyAttributes.SampleUsage("name1,name2,name3,name4")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_Regex { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please input the data you want to perform regex on.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a string of comma seperated values.")]
        [Attributes.PropertyAttributes.SampleUsage("name1,name2,name3,name4")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_InputData { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please input a variable name.")]
        [Attributes.PropertyAttributes.InputSpecification("Enter a string of comma seperated values.")]
        [Attributes.PropertyAttributes.SampleUsage("name1,name2,name3,name4")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_OutputVariable { get; set; }

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
            Match[] matches = Regex.Matches(vInputData, vRegex)
                       .Cast<Match>()
                       .ToArray();
            var arr = string.Join(",", matches.AsEnumerable());

            Script.ScriptVariable outputMatches = new Script.ScriptVariable
            {
                VariableName = v_OutputVariable,
                VariableValue = arr
            };

            engine.VariableList.Add(outputMatches);


        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Regex", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputData", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_OutputVariable", this, editor));



            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue();
        }
    }
}