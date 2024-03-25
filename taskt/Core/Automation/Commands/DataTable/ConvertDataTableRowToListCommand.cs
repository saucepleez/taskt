﻿using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Convert Row")]
    [Attributes.ClassAttributes.CommandSettings("Convert DataTable Row To List")]
    [Attributes.ClassAttributes.Description("This command allows you to convert DataTable Row to List")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert DataTable Row to List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ConvertDataTableRowToListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        public string v_DataTable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_RowIndex))]
        public string v_DataRowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public string v_Result { get; set; }

        public ConvertDataTableRowToListCommand()
        {
            //this.CommandName = "ConvertDataTableRowToListCommand";
            //this.SelectionName = "Convert DataTable Row To List";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;         
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            (var srcDT, var index) = this.ExpandUserVariablesAsDataTableAndRowIndex(nameof(v_DataTable), nameof(v_DataRowIndex), engine);

            List<string> myList = new List<string>();

            int cols = srcDT.Columns.Count;
            for (int i = 0; i < cols; i++)
            {
                myList.Add(srcDT.Rows[index][i]?.ToString() ?? "");
            }

            myList.StoreInUserVariable(engine, v_Result);
        }
    }
}