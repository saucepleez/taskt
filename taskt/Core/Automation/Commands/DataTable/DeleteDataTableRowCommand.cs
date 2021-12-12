using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using taskt.UI.Forms;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to delete a DataTable Row")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to delete a DataTable Row.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class DeleteDataTableRowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the DataTable Variable Name to be delete a row")]
        [Attributes.PropertyAttributes.InputSpecification("Enter a existing DataTable Variable Name")]
        [Attributes.PropertyAttributes.SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please specify the Row Index to delete")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**0** or **{{{vRow}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_RowIndex { get; set; }

        public DeleteDataTableRowCommand()
        {
            this.CommandName = "DeleteDataTableRowCommand";
            this.SelectionName = "Delete DataTable Row";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            Script.ScriptVariable dtVar = v_DataTableName.GetRawVariable(engine);
            DataTable myDT;
            if (!(dtVar.VariableValue is DataTable))
            {
                throw new Exception(v_DataTableName + " is not DataTable");
            }
            else
            {
                myDT = (DataTable)dtVar.VariableValue;
            }

            var vIndex = v_RowIndex.ConvertToUserVariable(engine);
            int index = int.Parse(vIndex);
            
            if ((index < 0) || (index >= myDT.Rows.Count))
            {
                throw new Exception("Row index " + v_RowIndex + " does not exists");
            }

            myDT.Rows[index].Delete();
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Delete DataTable '" + v_DataTableName + "' Row Index '" + v_RowIndex + "']";
        }
    }
}