using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Convert Row")]
    [Attributes.ClassAttributes.CommandSettings("Convert DataTable Row To Dictionary")]
    [Attributes.ClassAttributes.Description("This command allows you to convert DataTable Row to Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert DataTable Row to Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ConvertDataTableRowToDictionaryCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_RowIndex))]
        public string v_DataRowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_OutputDictionaryName))]
        public string v_OutputVariableName { get; set; }

        public ConvertDataTableRowToDictionaryCommand()
        {
            //this.CommandName = "ConvertDataTableRowToDictionaryCommand";
            //this.SelectionName = "Convert DataTable Row To Dictionary";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;         
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (var srcDT, var index) = this.GetDataTableVariableAndRowIndex(nameof(v_DataTableName), nameof(v_DataRowIndex), engine);

            Dictionary<string, string> myDic = new Dictionary<string, string>();

            int cols = srcDT.Columns.Count;
            for (int i = 0; i < cols; i++)
            {
                myDic.Add(srcDT.Columns[i].ColumnName, srcDT.Rows[index][i]?.ToString() ?? "");
            }

            myDic.StoreInUserVariable(engine, v_OutputVariableName);
        }
    }
}