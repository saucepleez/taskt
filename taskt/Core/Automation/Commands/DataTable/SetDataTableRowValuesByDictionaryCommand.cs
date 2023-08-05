using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Row Action")]
    [Attributes.ClassAttributes.CommandSettings("Set DataTable Row Values By Dictionary")]
    [Attributes.ClassAttributes.Description("This command allows you to set a DataTable Row values to a DataTable by a Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a DataTable Row values to a DataTable by a Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SetDataTableRowValuesByDictionaryCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_BothDataTableName))]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_RowIndex))]
        public string v_RowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        public string v_RowValues { get; set; }

        [XmlAttribute]
        [PropertyDescription("When Dictionary Key does not Exists")]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**Ignore**", "Do not Set a Value")]
        [PropertyDetailSampleUsage("**Error**", "Rise a Error")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyIsOptional(true, "Ignore")]
        public string v_NotExistsKey { get; set; }

        public SetDataTableRowValuesByDictionaryCommand()
        {
            //this.CommandName = "SetDataTableRowValuesByDictionaryCommand";
            //this.SelectionName = "Set DataTable Row Values By Dictionary";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (var myDT, var rowIndex) = this.GetDataTableVariableAndRowIndex(nameof(v_DataTableName), nameof(v_RowIndex), engine);

            var myDic = v_RowValues.GetDictionaryVariable(engine);

            string ifKeyNotExists = this.GetUISelectionValue(nameof(v_NotExistsKey), "Key Not Exists", engine);

            // get columns list
            new GetDataTableColumnListCommand
            {
                v_DataTableName = this.v_DataTableName,
                v_OutputList = VariableNameControls.GetInnerVariableName(0, engine)
            }.RunCommand(engine);
            var columns = (List<string>)VariableNameControls.GetInnerVariable(0, engine).VariableValue;

            if (ifKeyNotExists == "error")
            {
                // check key and throw exception
                foreach(var item in myDic)
                {
                    if (!columns.Contains(item.Key))
                    {
                        throw new Exception("Column name " + item.Key + " does not exists");
                    }
                }
            }

            foreach(var item in myDic)
            {
                if (columns.Contains(item.Key))
                {
                    myDT.Rows[rowIndex][item.Key] = item.Value;
                }
            }
        }
    }
}