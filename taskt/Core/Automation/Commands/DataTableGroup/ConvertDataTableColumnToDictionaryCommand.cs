using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable")]
    [Attributes.ClassAttributes.SubGruop("Convert Column")]
    [Attributes.ClassAttributes.CommandSettings("Convert DataTable Column To Dictionary")]
    [Attributes.ClassAttributes.Description("This command allows you to convert DataTable Column to Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert DataTable Column to Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ConvertDataTableColumnToDictionaryCommand : ADataTableGetFromDataTableColumnCommands, IDictionaryResultProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        //public string v_DataTable { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnType))]
        //public string v_ColumnType { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnNameIndex))]
        //public string v_ColumnIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Dictionary Key prefix")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**row**", PropertyDetailSampleUsage.ValueType.Value, "prefix")]
        [PropertyDetailSampleUsage("**{{{vPrefix}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "prefix")]
        [Remarks("When Specified **row** for Prefix, the Dictionary key is row0, row1, ...")]
        [PropertyIsOptional(true, "row")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyParameterOrder(8000)]
        public string v_KeyPrefix { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_OutputDictionaryName))]
        public override string v_Result { get; set; }

        public ConvertDataTableColumnToDictionaryCommand()
        {
            //this.CommandName = "ConvertDataTableColumnToDictionaryCommand";
            //this.SelectionName = "Convert DataTable Column To Dictionary";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;         
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            // TODO: key is other column value, List, etc

            string prefix;
            if (string.IsNullOrEmpty(v_KeyPrefix))
            {
                prefix = "row";
            }
            else
            {
                prefix = v_KeyPrefix.ExpandValueOrUserVariable(engine);
            }

            //(var srcDT, var colIndex) = this.ExpandUserVariablesAsDataTableAndColumnIndex(nameof(v_DataTable), nameof(v_ColumnType), nameof(v_ColumnIndex), engine);
            (var srcDT, var colIndex, _) = this.ExpandValueOrUserVariableAsDataTableAndColumn(engine);

            var myDic = new Dictionary<string, string>();
            for (int i = 0; i < srcDT.Rows.Count; i++)
            {
                myDic.Add(prefix + i.ToString(), srcDT.Rows[i][colIndex]?.ToString() ?? "");
            }

            //myDic.StoreInUserVariable(engine, v_Result);
            //this.StoreDictionaryInUserVariable(myDic, nameof(v_Result), engine);
            this.StoreDictionaryInUserVariable(myDic, engine);
        }
    }
}