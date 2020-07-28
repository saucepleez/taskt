using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
    [Group("Data Commands")]
    [Description("This command performs advanced text extraction.")]
    public class TextExtractionCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Text Data")]
        [InputSpecification("Provide a variable or text value.")]
        [SampleUsage("Sample text to perform text extraction on || {vTextData}")]
        [Remarks("Providing data of a type other than a 'String' will result in an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InputText { get; set; }

        [XmlAttribute]
        [PropertyDescription("Text Extraction Type")]
        [PropertyUISelectionOption("Extract All After Text")]
        [PropertyUISelectionOption("Extract All Before Text")]
        [PropertyUISelectionOption("Extract All Between Text")]
        [InputSpecification("Select the type of extraction.")]
        [SampleUsage("")]
        [Remarks("For trailing text, use 'After Text'. For leading text, use 'Before Text'. For text between two substrings, use 'Between Text'.")]
        public string v_TextExtractionType { get; set; }

        [XmlElement]
        [PropertyDescription("Extraction Parameters")]
        [InputSpecification("Define the required extraction parameters, which is dependent on the type of extraction.")]
        [SampleUsage("A substring from input text || {vSubstring}")]
        [Remarks("Set parameter values for each parameter name based on the extraction type.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public DataTable v_TextExtractionTable { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Text Variable")]
        [InputSpecification("Select or provide a variable from the variable list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required" +
                  " to pre-define your variables; however, it is highly recommended.")]
        public string v_OutputUserVariableName { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView _parametersGridViewHelper;

        public TextExtractionCommand()
        {
            CommandName = "TextExtractionCommand";
            SelectionName = "Text Extraction";
            CommandEnabled = true;
            CustomRendering = true;

            //define parameter table
            v_TextExtractionTable = new DataTable
            {
                TableName = DateTime.Now.ToString("TextExtractorParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"))
            };

            v_TextExtractionTable.Columns.Add("Parameter Name");
            v_TextExtractionTable.Columns.Add("Parameter Value");
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            //get variablized input
            var variableInput = v_InputText.ConvertToUserVariable(engine);

            string variableLeading, variableTrailing, skipOccurences, extractedText;

            //handle extraction cases
            switch (v_TextExtractionType)
            {
                case "Extract All After Text":
                    //extract trailing texts            
                    variableLeading = GetParameterValue("Leading Text").ConvertToUserVariable(engine);
                    skipOccurences = GetParameterValue("Skip Past Occurences").ConvertToUserVariable(engine);
                    extractedText = ExtractLeadingText(variableInput, variableLeading, skipOccurences);
                    break;
                case "Extract All Before Text":
                    //extract leading text
                    variableTrailing = GetParameterValue("Trailing Text").ConvertToUserVariable(engine);
                    skipOccurences = GetParameterValue("Skip Past Occurences").ConvertToUserVariable(engine);
                    extractedText = ExtractTrailingText(variableInput, variableTrailing, skipOccurences);
                    break;
                case "Extract All Between Text":
                    //extract leading and then trailing which gives the items between
                    variableLeading = GetParameterValue("Leading Text").ConvertToUserVariable(engine);
                    variableTrailing = GetParameterValue("Trailing Text").ConvertToUserVariable(engine);
                    skipOccurences = GetParameterValue("Skip Past Occurences").ConvertToUserVariable(engine);

                    //extract leading
                    extractedText = ExtractLeadingText(variableInput, variableLeading, skipOccurences);

                    //extract trailing -- assume we will take to the first item
                    extractedText = ExtractTrailingText(extractedText, variableTrailing, "0");

                    break;
                default:
                    throw new NotImplementedException("Extraction Type Not Implemented: " + v_TextExtractionType);
            }

            //store variable
            extractedText.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputText", this, editor));

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_TextExtractionType", this));
            var selectionControl = (ComboBox)CommandControls.CreateDropdownFor("v_TextExtractionType", this);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_TextExtractionType", this, new Control[] { selectionControl }, editor));
            selectionControl.SelectionChangeCommitted += TextExtraction_SelectionChangeCommitted;
            RenderedControls.Add(selectionControl);

            _parametersGridViewHelper = new DataGridView();
            _parametersGridViewHelper.AllowUserToAddRows = true;
            _parametersGridViewHelper.AllowUserToDeleteRows = true;
            _parametersGridViewHelper.Size = new Size(350, 125);
            _parametersGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _parametersGridViewHelper.DataBindings.Add("DataSource", this, "v_TextExtractionTable", false, DataSourceUpdateMode.OnPropertyChanged);
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_TextExtractionTable", this));
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_TextExtractionTable", this, new Control[] { _parametersGridViewHelper }, editor));
            RenderedControls.Add(_parametersGridViewHelper);

            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Extract Text From '{v_InputText}' - Store Text in '{v_OutputUserVariableName}']";
        }

        private void TextExtraction_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox extractionAction = (ComboBox)sender;

            if ((_parametersGridViewHelper == null) || 
                (extractionAction == null) || 
                (_parametersGridViewHelper.DataSource == null))
                return;

            var textParameters = (DataTable)_parametersGridViewHelper.DataSource;
            textParameters.Rows.Clear();

            switch (extractionAction.SelectedItem)
            {
                case "Extract All After Text":
                    textParameters.Rows.Add("Leading Text", "");
                    textParameters.Rows.Add("Skip Past Occurences", "0");
                    break;
                case "Extract All Before Text":
                    textParameters.Rows.Add("Trailing Text", "");
                    textParameters.Rows.Add("Skip Past Occurences", "0");
                    break;
                case "Extract All Between Text":
                    textParameters.Rows.Add("Leading Text", "");
                    textParameters.Rows.Add("Trailing Text", "");
                    textParameters.Rows.Add("Skip Past Occurences", "0");
                    break;
                default:
                    break;
            }
        }

        private string GetParameterValue(string parameterName)
        {
            return ((from rw in v_TextExtractionTable.AsEnumerable()
                     where rw.Field<string>("Parameter Name") == parameterName
                     select rw.Field<string>("Parameter Value")).FirstOrDefault());
        }

        private string ExtractLeadingText(string input, string substring, string occurences)
        {
            //verify the occurence index
            int leadingOccurenceIndex = 0;

            if (!int.TryParse(occurences, out leadingOccurenceIndex))
            {
                throw new Exception("Invalid Index For Extraction - " + occurences);
            }

            //find index matches
            var leadingOccurencesFound = Regex.Matches(input, substring).Cast<Match>().Select(m => m.Index).ToList();

            //handle if we are searching beyond what was found
            if (leadingOccurenceIndex >= leadingOccurencesFound.Count)
            {
                throw new Exception("No value was found after skipping " + leadingOccurenceIndex + " instance(s).  Only " + 
                    leadingOccurencesFound.Count + " instances exist.");
            }

            //declare start position
            var startPosition = leadingOccurencesFound[leadingOccurenceIndex] + substring.Length;

            //substring and apply to variable
            return input.Substring(startPosition);
        }

        private string ExtractTrailingText(string input, string substring, string occurences)
        {
            //verify the occurence index
            int leadingOccurenceIndex = 0;
            if (!int.TryParse(occurences, out leadingOccurenceIndex))
            {
                throw new Exception("Invalid Index For Extraction - " + occurences);
            }

            //find index matches
            var trailingOccurencesFound = Regex.Matches(input, substring).Cast<Match>().Select(m => m.Index).ToList();

            //handle if we are searching beyond what was found
            if (leadingOccurenceIndex >= trailingOccurencesFound.Count)
            {
                throw new Exception("No value was found after skipping " + leadingOccurenceIndex + " instance(s).  Only " + 
                    trailingOccurencesFound.Count + " instances exist.");
            }

            //declare start position
            var endPosition = trailingOccurencesFound[leadingOccurenceIndex];

            //substring
            return input.Substring(0, endPosition);
        }
    }
}