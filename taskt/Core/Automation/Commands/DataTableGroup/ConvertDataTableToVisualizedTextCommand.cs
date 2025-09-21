using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable")]
    [Attributes.ClassAttributes.SubGruop("Convert")]
    [Attributes.ClassAttributes.CommandSettings("Convert DataTable To Visualized Text")]
    [Attributes.ClassAttributes.Description("This command allows you to Convert DataTable to Visualized Text.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Convert DataTable to Visualized Text.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ConvertDataTableToVisualizedTextCommand : ADataTableGetFromDataTableCommands
    {
        //[XmlAttribute]
        //public string v_DataTable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public override string v_Result { get; set; }

        public ConvertDataTableToVisualizedTextCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var dt = this.ExpandUserVariableAsDataTable(engine);
            var columnSize = dt.Columns.Count;
            var rows = dt.Rows.Count;

            var columnMaxLength = new List<int>();
            for (int i = 0; i < columnSize; i++)
            {
                columnMaxLength.Add(0);
            }

            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < columnSize; i++)
                {
                    var s = (row[i]?.ToString() ?? "").Length;
                    if (columnMaxLength[i] < s)
                    {
                        columnMaxLength[i] = s;
                    }
                }
            }
            for (int i = 0; i < columnSize; i++)
            {
                var s = dt.Columns[i].ColumnName.Length;
                if (columnMaxLength[i] < s)
                {
                    columnMaxLength[i] = s;
                }
            }

            var columnFormats = new List<string>();
            foreach(var s in columnMaxLength)
            {
                columnFormats.Add($"{{0, -{s}}}");
            }

            int rowLength = (int)Math.Log10(rows) + 1;
            if (rowLength < 3)
            {
                rowLength = 3; // Row
            }
            var rowFormat = $"{{0, {rowLength}}}";

            // first header
            string txt = $"{string.Format(rowFormat, "")} ";
            int cnt = 0;
            foreach(var s in columnFormats)
            {
                txt += $"| {string.Format(s, cnt)} ";
                cnt++;
            }
            txt = txt.TrimEnd() + "\r\n";

            // second header
            txt += $"{string.Format(rowFormat, "Row")} ";
            cnt = 0;
            foreach(var s in columnFormats)
            {
                txt += $"| {string.Format(s, dt.Columns[cnt].ColumnName)} ";
                cnt++;
            }
            txt = txt.TrimEnd() + "\r\n";

            // split lines
            for (int i = 0; i < rowLength; i++)
            {
                txt += "-";
            }
            txt += "-|-";

            for (int i = 0; i < columnSize - 1; i++)
            {
                for (int j = 0; j < columnMaxLength[i]; j++)
                {
                    txt += "-";
                }
                txt += "-|-";
            }
            for (int i = 0; i < columnMaxLength[columnSize - 1]; i++)
            {
                txt += "-";
            }
            txt += "\r\n";

            for (int i = 0; i < rows; i++)
            {
                txt += $"{string.Format(rowFormat, i)} ";
                for (int j = 0; j < columnSize; j++)
                {
                    txt += $"| {string.Format(columnFormats[j], dt.Rows[i][j]?.ToString() ?? "")} ";
                }
                txt = txt.TrimEnd() + "\r\n";
            }

            txt.StoreInUserVariable(engine, v_Result);
        }
    }
}