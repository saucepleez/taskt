﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Xml.Serialization;
//using System.Data;
//using System.Windows.Forms;
//using taskt.UI.CustomControls;
//using taskt.Core.Automation.Attributes.PropertyAttributes;

//namespace taskt.Core.Automation.Commands
//{
//    [Serializable]
//    [Attributes.ClassAttributes.Group("DataTable Commands")]
//    [Attributes.ClassAttributes.SubGruop("Other")]
//    [Attributes.ClassAttributes.Description("This command allows you to parse a dataset row column into a variable.")]
//    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to extract data from a dataset variable")]
//    [Attributes.ClassAttributes.ImplementationDescription("")]
//    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
//    public class ParseDatasetRowCommand : ScriptCommand
//    {
//        [XmlAttribute]
//        [PropertyDescription("Supply the name of the variable containing the datasource")]
//        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
//        [InputSpecification("Select or provide a variable")]
//        [SampleUsage("**vSomeVariable**")]
//        [Remarks("")]
//        public string v_DatasetName { get; set; }

//        [XmlAttribute]
//        [PropertyDescription("Please Select Column Parse Type")]
//        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
//        [PropertyUISelectionOption("By Column Name")]
//        [PropertyUISelectionOption("By Column Index")]
//        [InputSpecification("")]
//        [SampleUsage("")]
//        [Remarks("")]
//        public string v_ColumnParseType { get; set; }

//        [XmlAttribute]
//        [PropertyDescription("Specify Column Name or Index")]
//        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
//        [InputSpecification("")]
//        [SampleUsage("")]
//        [Remarks("")]
//        public string v_ColumnParameter { get; set; }

//        [XmlAttribute]
//        [PropertyDescription("Please select the variable to receive the extracted column data")]
//        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
//        [InputSpecification("Select or provide a variable from the variable list")]
//        [SampleUsage("**vSomeVariable**")]
//        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
//        public string v_applyToVariableName { get; set; }

//        [XmlAttribute]
//        [PropertyDescription("Optional - Specify Alternate Row Number")]
//        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
//        [InputSpecification("If not executing within a loop, select the applicable index of the row required")]
//        [SampleUsage("**0** or **vRowNumber**")]
//        [Remarks("")]
//        public string v_SpecifiedRow { get; set; }

//        public ParseDatasetRowCommand()
//        {
//            this.CommandName = "ParseRowCommand";
//            this.SelectionName = "Parse Dataset Row";
//            this.CommandEnabled = true;
//            this.CustomRendering = true;
//            this.v_SpecifiedRow = "N/A";
//            this.v_ColumnParseType = "By Column Name";   
//        }

//        public override void RunCommand(Engine.AutomationEngineInstance engine)
//        {
//            ////try to find dataset based on variable name
//            //var dataSourceVariable = engine.VariableList.FirstOrDefault(f => engine.engineSettings.VariableStartMarker + f.VariableName + engine.engineSettings.VariableEndMarker == v_DatasetName);

//            //if (dataSourceVariable == null)
//            //{
//            //    //see if match is found without encasing
//            //    dataSourceVariable = engine.VariableList.FirstOrDefault(f => f.VariableName == v_DatasetName);

//            //    //no match was found
//            //    if (dataSourceVariable == null)
//            //    {
//            //        throw new Exception($"Data Source {v_DatasetName} Not Found! Did you input the correct name?");
//            //    }
//            //}

//            //var columnName = v_ColumnParameter.ExpandValueOrUserVariable(engine);
//            //var parseStrat = v_ColumnParseType.ExpandValueOrUserVariable(engine);
//            ////get datatable
//            //var dataTable = (DataTable)dataSourceVariable.VariableValue;

//            //int requiredRowNumber;
//            //if (!int.TryParse(this.v_SpecifiedRow.ExpandValueOrUserVariable(engine), out requiredRowNumber))
//            //{
//            //    requiredRowNumber = dataSourceVariable.CurrentPosition;
//            //}

//            ////get required row
//            //var requiredRow = dataTable.Rows[requiredRowNumber];

//            ////parse column name based on requirement
//            //object requiredColumn;
//            //if (parseStrat == "By Column Index")
//            //{
//            //    requiredColumn = requiredRow[int.Parse(columnName)];
//            //}
//            //else
//            //{
//            //    requiredColumn = requiredRow[columnName];
//            //}

//            ////store value in variable
//            //requiredColumn.ToString().StoreInUserVariable(engine, v_applyToVariableName);

//            string colType = "";
//            switch(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ColumnParseType), engine))
//            {
//                case "by column name":
//                    colType = "Column Name";
//                    break;
//                case "by column index":
//                    colType = "Index";
//                    break;
//            }

//            var getTableValue = new GetDataTableValueCommand()
//            {
//                v_DataTableName = this.v_DatasetName,
//                v_ColumnType = colType,
//                v_ColumnIndex = this.v_ColumnParseType,
//                v_RowIndex = this.v_SpecifiedRow,
//                v_UserVariableName = this.v_applyToVariableName,
//            };
//            getTableValue.RunCommand(engine);
//        }

//        public override List<Control> Render(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
//        {
//            base.Render(editor);

//            //create standard group controls
//            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DatasetName", this, editor));

//            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_ColumnParseType", this, editor));
//            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ColumnParameter", this, editor));

//            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
//            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
//            RenderedControls.AddRange(CommandControls.CreateDefaultUIHelpersFor("v_applyToVariableName", this, VariableNameControl, editor));
//            RenderedControls.Add(VariableNameControl);

//            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SpecifiedRow", this, editor));

//            return RenderedControls;

//        }

//        public override string GetDisplayValue()
//        {
//            return $"{base.GetDisplayValue()} - [Select '{v_ColumnParameter}' {v_ColumnParseType} from '{v_DatasetName}', Apply Result(s) To Variable: {v_applyToVariableName}]";
//        }
//    }
//}