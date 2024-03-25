﻿//using System;
//using System.Xml.Serialization;
//using System.Data;
//using System.Windows.Forms;
//using System.Collections.Generic;
//using taskt.UI.CustomControls;
//using taskt.Core.Automation.Attributes.PropertyAttributes;

//namespace taskt.Core.Automation.Commands
//{
//    [Serializable]
//    [Attributes.ClassAttributes.Group("DataTable Commands")]
//    [Attributes.ClassAttributes.SubGruop("Other")]
//    [Attributes.ClassAttributes.Description("This command allows you to get the datarow count of a DataTable")]
//    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get the datarow count of a DataTable.")]
//    [Attributes.ClassAttributes.ImplementationDescription("")]
//    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
//    public class GetDataRowCountCommand : ScriptCommand
//    {
//        [XmlAttribute]
//        [PropertyDescription("Please indicate the DataTable Variable Name")]
//        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
//        [InputSpecification("Enter a existing DataTable.")]
//        [SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
//        [Remarks("")]
//        [PropertyShowSampleUsageInDescription(true)]
//        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
//        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
//        public string v_DataTableName { get; set; }

//        [XmlAttribute]
//        [PropertyDescription("Please Specify the Variable Name To Assign the Result")]
//        [InputSpecification("Select or provide a variable from the variable list")]
//        [SampleUsage("**vSomeVariable**")]
//        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
//        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
//        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
//        [PropertyIsVariablesList(true)]
//        public string v_UserVariableName { get; set; }

//        public GetDataRowCountCommand()
//        {
//            this.CommandName = "GetDataRowCountCommand";
//            this.SelectionName = "Get DataRow Count";
//            this.CommandEnabled = true;
//            this.CustomRendering = true;
//        }

//        public override void RunCommand(Engine.AutomationEngineInstance engine)
//        {
//            DataTable dataTable = (DataTable)v_DataTableName.GetRawVariable(engine).VariableValue;

//            var count = dataTable.Rows.Count.ToString();

//            count.StoreInUserVariable(engine, v_UserVariableName);
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
//            return base.GetDisplayValue() + $" [From '{v_DataTableName}', Store In: '{v_UserVariableName}']";
//        }
//    }
//}