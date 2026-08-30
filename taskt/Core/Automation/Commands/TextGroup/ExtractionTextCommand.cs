using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.CommandSettings("Extraction Text")]
    [Attributes.ClassAttributes.Description("This command allows you to perform advanced string extraction.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to extract a piece of text from a larger text or variable")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExtractionTextCommand : ScriptCommand, IHaveDataTableElements
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(TextControls), nameof(TextControls.v_Text_MultiLine))]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Text Extraction Type")]
        [InputSpecification("", true)]
        [PropertyUISelectionOption("Extract All After Text")]
        [PropertyUISelectionOption("Extract All Before Text")]
        [PropertyUISelectionOption("Extract All Between Text")]
        [PropertySelectionChangeEvent(nameof(cmbTextExtraction_SelectionChangeCommitted))]
        public string v_TextExtractionType { get; set; }

        [XmlElement]
        [PropertyDescription("Extraction Parameters")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(false, false, true, 400, 135)]
        [PropertySecondaryLabel(true)]
        [PropertyAddtionalParameterInfo("Extract All After Text\tLeading Text", "The beginning of the text to be extracted", "**Hello** or **{{{vStart}}}**")]
        [PropertyAddtionalParameterInfo("Extract All After Text\tSkip Past Occurences", "", "")]
        [PropertyAddtionalParameterInfo("Extract All Before Text\tTrailing Text", "The end of text to be extracted", "**Hello** or **{{{vEnd}}}**")]
        [PropertyAddtionalParameterInfo("Extract All Before Text\tSkip Past Occurences", "")]
        [PropertyAddtionalParameterInfo("Extract All Between Text\tLeading Text", "The beginning of the text to be extracted", "**Hello** or **{{{vStart}}}**")]
        [PropertyAddtionalParameterInfo("Extract All Between Text\tTrailing Text", "The end of text to be extracted", "**Hello** or **{{{vEnd}}}**")]
        [PropertyAddtionalParameterInfo("Extract All Between Text\tSkip Past Occurences", "")]
        [PropertyDataGridViewColumnSettings("Parameter Name", "Parameter Name", true)]
        [PropertyDataGridViewColumnSettings("Parameter Value", "Parameter Value", false)]
        [PropertyDataGridViewCellEditEvent(nameof(DataTableControls) + "+" + nameof(DataTableControls.FirstColumnReadonlySubsequentEditableDataGridView_CellBeginEdit), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellBeginEdit)]
        [PropertyDataGridViewCellEditEvent(nameof(DataTableControls) + "+" + nameof(DataTableControls.FirstColumnReadonlySubsequentEditableDataGridView_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        public DataTable v_TextExtractionTable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_applyToVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("When Text Not Found")]
        [PropertyUISelectionOption("Rise A Error")]
        [PropertyUISelectionOption("Get Empty Text")]
        [PropertyDetailSampleUsage("Rise A Error", "Rise A Error")]
        [PropertyDetailSampleUsage("Get Empty Text", "Get Empty Text")]
        [PropertyIsOptional(true, "Get Empty Text")]
        [PropertyFirstValue("Get Empty Text")]
        [PropertyDisplayText(false, "")]
        public string v_WhenTextNotFound { get; set; }

        public ExtractionTextCommand()
        {
            //this.CommandName = "ExtractionTextCommand";
            //this.SelectionName = "Extraction Text";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;

            //define parameter table
            //this.v_TextExtractionTable = new System.Data.DataTable
            //{
            //    TableName = DateTime.Now.ToString("TextExtractorParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"))
            //};

            //this.v_TextExtractionTable.Columns.Add("Parameter Name");
            //this.v_TextExtractionTable.Columns.Add("Parameter Value");
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //get variablized input
            var targetText = v_InputValue.ExpandValueOrUserVariable(engine);

            var parms = DataTableControls.GetFieldValues(v_TextExtractionTable, "Parameter Name", "Parameter Value", engine);

            string extractedText = "";
            try
            {
                switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_TextExtractionType), engine))
                {
                    case "extract all after text":
                        extractedText = ExtractLeadingText(targetText, parms, engine);
                        break;

                    case "extract all before text":
                        extractedText = ExtractTrailingText(targetText, parms, engine);
                        break;

                    case "extract all between text":
                        extractedText = ExtractLeadingText(targetText, parms, engine);
                        parms["Skip Past Occurences"] = "0";    // force change parameter value
                        extractedText = ExtractTrailingText(extractedText, parms, engine);
                        break;
                }
            }
            catch (Exception ex) 
            {
                if (ex.Message.StartsWith("No value was found after skipping "))
                {
                    switch(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenTextNotFound), engine))
                    {
                        case "get empty text":
                            extractedText = "";
                            break;
                        case "rise a error":
                            throw ex;
                    }
                }
                else
                {
                    throw ex;
                }
            }


            //store variable
            extractedText.StoreInUserVariable(engine, v_applyToVariableName);
        }

        private void cmbTextExtraction_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var extractionAction = (ComboBox)sender;

            var ParametersGridViewHelper = (DataGridView)ControlsList[nameof(v_TextExtractionTable)];

            if (ParametersGridViewHelper.DataSource == null)
            {
                return;
            }
            var textParameters = (DataTable)ParametersGridViewHelper.DataSource;

            var Parameters2ndLabel = (Label)ControlsList["lbl2_" + nameof(v_TextExtractionTable)];
            Parameters2ndLabel.Text = "";
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

        /// <summary>
        /// after text
        /// </summary>
        /// <param name="targetText"></param>
        /// <param name="parms"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static string ExtractLeadingText(string targetText, Dictionary<string, string> parms, Engine.AutomationEngineInstance engine)
        {
            if (parms.Keys.Contains("Leading Text") && parms.Keys.Contains("Skip Past Occurences"))
            {
                var index = parms["Skip Past Occurences"].ExpandValueOrUserVariableAsInteger("Skip Past Occurences", engine);
                var searchText = parms["Leading Text"];

                var positions = SearchTextPositions(targetText, searchText);

                if (index < 0)
                {
                    index += positions.Count;
                }

                if (positions.Count <= index)
                {
                    throw new Exception("No value was found after skipping " + index + " instance(s).");
                }

                return targetText.Substring(positions[index] + searchText.Length);
            }
            else
            {
                // error
                throw new Exception("Extract Leading Text Parameters not Enough.");
            }
        }

        /// <summary>
        /// before text
        /// </summary>
        /// <param name="targetText"></param>
        /// <param name="parms"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static string ExtractTrailingText(string targetText, Dictionary<string, string> parms, Engine.AutomationEngineInstance engine)
        {
            if (parms.Keys.Contains("Trailing Text") && parms.Keys.Contains("Skip Past Occurences"))
            {
                var index = parms["Skip Past Occurences"].ExpandValueOrUserVariableAsInteger("Skip Past Occurences", engine);
                var searchText = parms["Trailing Text"];

                var positions = SearchTextPositions(targetText, searchText);

                if (index < 0)
                {
                    index += positions.Count;
                }

                if (positions.Count <= index)
                {
                    throw new Exception("No value was found after skipping " + index + " instance(s).");
                }

                return targetText.Substring(0, positions[index]);
            }
            else
            {
                // error
                throw new Exception("Extract Trailing Text Parameters not Enough.");
            }
        }

        private static List<int> SearchTextPositions(string targetText, string searchText)
        {
            var positions = new List<int>();
            var pos = targetText.IndexOf(searchText, 0);
            while (pos >= 0)
            {
                positions.Add(pos);
                pos = targetText.IndexOf(searchText, pos + 1);
            }
            return positions;
        }

        public override void BeforeValidate()
        {
            base.BeforeValidate();

            var dgv = FormUIControls.GetPropertyControl<DataGridView>(ControlsList, nameof(v_TextExtractionTable));
            DataTableControls.BeforeValidate_NoRowAdding(dgv, v_TextExtractionTable);
        }
    }
}