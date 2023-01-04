using System;
using System.Xml.Serialization;
using System.Data;
using System.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Column Action")]
    [Attributes.ClassAttributes.Description("This command allows you to get the column name List of a DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get the column name List of a DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetDataTableColumnListCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please indicate the DataTable Variable Name")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Enter a existing DataTable.")]
        //[SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyValidationRule("DataTable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "DataTable")]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Specify the Variable Name To Assign the column name List")]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        //[PropertyValidationRule("List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "List")]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public string v_OutputList { get; set; }

        public GetDataTableColumnListCommand()
        {
            this.CommandName = "GetDataTableColumnListCommand";
            this.SelectionName = "Get DataTable Column List";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            DataTable myDT = v_DataTableName.GetDataTableVariable(engine);

            //List<string> colList = new List<string>();
            //foreach(DataColumn col in myDT.Columns)
            //{
            //    colList.Add(col.ColumnName);
            //}

            //colList.StoreInUserVariable(engine, v_OutputList);

            myDT.Columns.Cast<DataColumn>().Select(col => col.ColumnName).ToList().StoreInUserVariable(engine, v_OutputList);
        }
    }
}