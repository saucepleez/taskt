using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable")]
    [Attributes.ClassAttributes.SubGruop("Convert Row")]
    [Attributes.ClassAttributes.CommandSettings("Convert DataTable Row To Dictionary")]
    [Attributes.ClassAttributes.Description("This command allows you to convert DataTable Row to Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert DataTable Row to Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ConvertDataTableRowToDictionaryCommand : ADataTableGetFromDataTableRowCommands, IDictionaryResultProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        //public string v_DataTable { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_RowIndex))]
        //public string v_RowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_OutputDictionaryName))]
        public override string v_Result { get; set; }

        public ConvertDataTableRowToDictionaryCommand()
        {
            //this.CommandName = "ConvertDataTableRowToDictionaryCommand";
            //this.SelectionName = "Convert DataTable Row To Dictionary";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;         
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //(var srcDT, var index) = this.ExpandUserVariablesAsDataTableAndRowIndex(nameof(v_DataTable), nameof(v_RowIndex), engine);
            (var srcDT, var index) = this.ExpandValueOrUserVariableAsDataTableAndRow(engine);

            var myDic = new Dictionary<string, string>();

            int cols = srcDT.Columns.Count;
            for (int i = 0; i < cols; i++)
            {
                myDic.Add(srcDT.Columns[i].ColumnName, srcDT.Rows[index][i]?.ToString() ?? "");
            }

            //myDic.StoreInUserVariable(engine, v_Result);
            this.StoreDictionaryInUserVariable(myDic, engine);
        }
    }
}