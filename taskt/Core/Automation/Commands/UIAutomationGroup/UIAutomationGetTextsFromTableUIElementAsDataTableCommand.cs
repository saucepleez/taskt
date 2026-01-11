using System;
using System.Data;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.UIAutomationGroup;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation")]
    [Attributes.ClassAttributes.SubGruop("Get From UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Get Texts From Table UIElement As DataTable")]
    [Attributes.ClassAttributes.Description("This command allows you to get Texts from Table UIElement as DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Texts from Table UIElement as DataTable.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationGetTextsFromTableUIElementAsDataTableCommand : AGetFromUIElementCommands, IDataTableResultProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_OutputDataTableName))]
        [PropertyParameterOrder(6000)]
        public string v_Result { get; set; }

        public UIAutomationGetTextsFromTableUIElementAsDataTableCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            int GetTableInfo(string elementVarName, string infoType)
            {
                using (var v = new InnerScriptVariable(engine))
                {
                    var getFromTable = new UIAutomationGetTableInformationFromUIElementCommand()
                    {
                        v_TargetElement = elementVarName,
                        v_InformationType = infoType,
                        v_ContainsHeader = "Yes",
                        v_Result = v.VariableName,
                    };
                    getFromTable.RunCommand(engine);
                    return int.Parse(v.VariableValue.ToString());
                }
            }

            string GetTableText(string elementVarName, int row, int col)
            { 
                using (var v = new InnerScriptVariable(engine))
                {
                    var getFromTable = new UIAutomationGetTextFromTableUIElementCommand()
                    {
                        v_TargetElement = elementVarName,
                        v_Row = row.ToString(),
                        v_Column = col.ToString(),
                        v_Result = v.VariableName,
                    };
                    getFromTable.RunCommand(engine);
                    return v.VariableValue.ToString();
                }
            }

            int rows = GetTableInfo(v_TargetElement, "Row Count");
            int cols = GetTableInfo(v_TargetElement, "Column Count");

            var dt = new DataTable();
            for (int i = 0; i < cols; i++)
            {
                dt.Columns.Add();
            }

            dt.BeginLoadData();
            for (int i = 0; i < rows; i++)
            {
                var row = dt.NewRow();
                for (int j = 0; j < cols; j++)
                {
                    row[j] = GetTableText(v_TargetElement, i, j);
                }
                dt.Rows.Add(row);
            }
            dt.EndLoadData();

            this.StoreDataTableInUserVariable(dt, engine);
        }
    }
}