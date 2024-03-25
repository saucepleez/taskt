﻿//using System;
//using System.Xml.Serialization;
//using System.Windows.Forms;
//using System.Collections.Generic;
//using taskt.UI.CustomControls;
//using taskt.Core.Automation.Attributes.PropertyAttributes;

//namespace taskt.Core.Automation.Commands
//{
//    [Serializable]
//    [Attributes.ClassAttributes.Group("DataTable Commands")]
//    [Attributes.ClassAttributes.SubGruop("Other")]
//    [Attributes.ClassAttributes.Description("This command allows you to get a DataRow from a DataTable")]
//    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add a datarow to a DataTable.")]
//    [Attributes.ClassAttributes.ImplementationDescription("")]
//    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
//    public class GetDataRowCommand : ScriptCommand
//    {
//        [XmlAttribute]
//        [PropertyDescription("Please indicate the DataTable Variable Name")]
//        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
//        [InputSpecification("Enter a existing DataTable to fet rows from.")]
//        [SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
//        [Remarks("")]
//        [PropertyShowSampleUsageInDescription(true)]
//        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
//        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
//        [PropertyIsVariablesList(true)]
//        public string v_DataTableName { get; set; }

//        [XmlAttribute]
//        [PropertyDescription("Please enter the index of the DataRow")]
//        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
//        [InputSpecification("Enter a valid DataRow index value")]
//        [SampleUsage("**0** or **{{{vRowIndex}}}**")]
//        [Remarks("")]
//        [PropertyShowSampleUsageInDescription(true)]
//        [PropertyTextBoxSetting(1, false)]
//        public string v_DataRowIndex { get; set; }

//        [XmlAttribute]
//        [PropertyDescription("Please Specify the Variable Name To Assign The DataRow")]
//        [InputSpecification("Select or provide a variable from the variable list")]
//        [SampleUsage("**vSomeVariable**")]
//        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
//        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
//        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
//        [PropertyIsVariablesList(true)]
//        public string v_UserVariableName { get; set; }

//        public GetDataRowCommand()
//        {
//            this.CommandName = "GetDataRowCommand";
//            this.SelectionName = "Get DataRow";
//            this.CommandEnabled = true;
//            this.CustomRendering = true;         

//        }

//        public override void RunCommand(Engine.AutomationEngineInstance engine)
//        {
//            //DataTable dataTable = (DataTable)v_DataTableName.GetRawVariable(engine).VariableValue;

//            //int index = int.Parse(v_DataRowIndex.ExpandValueOrUserVariable(engine));

//            //DataRow row = dataTable.Rows[index];

//            //row.StoreInUserVariable(engine, v_UserVariableName);
//            var convertToDic = new ConvertDataTableRowToDictionaryCommand()
//            {
//                v_DataTableName = this.v_DataTableName,
//                v_DataRowIndex = this.v_DataRowIndex,
//                v_OutputVariableName = this.v_UserVariableName,
//            };
//            convertToDic.RunCommand(engine);
//        }
       
//        public override List<Control> Render(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
//        {
//            base.Render(editor);

//            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
//            RenderedControls.AddRange(ctrls);

//            return RenderedControls;
//        }

//        public override string GetDisplayValue()
//        {
//            return base.GetDisplayValue() + $" [Get DataRow '{v_DataRowIndex}' from '{v_DataTableName}', Store In: '{v_UserVariableName}']";
//        }
//    }
//}