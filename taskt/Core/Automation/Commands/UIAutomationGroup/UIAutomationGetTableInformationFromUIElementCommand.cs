using System;
using System.Windows.Automation;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.UIAutomationGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation")]
    [Attributes.ClassAttributes.SubGruop("Get From UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Get Table Information From UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to get Table Information from UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Table Information from UIElement.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationGetTableInformationFromUIElementCommand : AGetFromUIElementCommands, IResultProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Information Type")]
        [PropertyUISelectionOption("Column Count")]
        [PropertyUISelectionOption("Row Count")]
        [PropertyValidationRule("Information Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Information")]
        [PropertyParameterOrder(6000)]
        public string v_InformationType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(7000)]
        public string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Contains Header Row, Column")]
        [PropertyIsOptional(true, "No")]
        [PropertyValidationRule("Contains Header", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "Contains Header")]
        [PropertyParameterOrder(8000)]
        public string v_ContainsHeader { get; set; }

        public UIAutomationGetTableInformationFromUIElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var containsHeader = this.ExpandValueOrUserVariableAsYesNo(nameof(v_ContainsHeader), engine);
            var infoType = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_InformationType), engine);

            this.UIElementAction(engine,
                new Action<AutomationElement>((targetElement) =>
                {
                    if (targetElement.TryGetCurrentPattern(GridPattern.Pattern, out object gridObj))
                    {
                        var grid = (GridPattern)gridObj;
                        if (containsHeader)
                        {
                            var customRows = EM_UIElementTableUIElement.GetRowsFromDataGridView(targetElement);
                            if (customRows.Count > 0)
                            {
                                // DataGridView(.net)
                                switch (infoType)
                                {
                                    case "column count":
                                        var cols = EM_UIElementTableUIElement.GetColumnsFromDataGridView(customRows[0]);
                                        cols.Count.StoreInUserVariable(engine, v_Result);
                                        break;
                                    case "row count":
                                        customRows.Count.StoreInUserVariable(engine, v_Result);
                                        break;
                                }
                            }
                            else
                            {
                                // GridPattern
                                var rows = EM_UIElementTableUIElement.GetRowsFromGridPattern(targetElement);
                                switch (infoType)
                                {
                                    case "column count":
                                        var cols = EM_UIElementTableUIElement.GetColumnsFromGridPattern(rows[0]);
                                        cols.Count.StoreInUserVariable(engine, v_Result);
                                        break;
                                    case "row count":
                                        rows.Count.StoreInUserVariable(engine, v_Result);
                                        break;
                                }
                            }
                        }
                        else
                        {
                            switch (infoType)
                            {
                                case "column count":
                                    grid.Current.ColumnCount.StoreInUserVariable(engine, v_Result);
                                    break;
                                case "row count":
                                    grid.Current.RowCount.StoreInUserVariable(engine, v_Result);
                                    break;
                            }
                        }
                    }
                    else if (targetElement.TryGetCurrentPattern(SelectionPattern.Pattern, out object selPattern))
                    {
                        var selPtn = (SelectionPattern)selPattern;
                        var rows = EM_UIElementTableUIElement.GetRowsFromSelectionTable(targetElement);
                        switch(infoType)
                        {
                            case "column count":
                                var cols = EM_UIElementTableUIElement.GetColumnsFromSelectionTable(rows[0]);
                                cols.Count.StoreInUserVariable(engine, v_Result);
                                break;
                            case "row count":
                                rows.Count.StoreInUserVariable(engine, v_Result);
                                break;
                        }
                    }
                    else
                    {
                        this.ValueCanNotRetrievedProcess("Table Information", new Action(() =>
                        {
                            "".StoreInUserVariable(engine, v_Result);
                        }), engine);
                        return;
                    }
                })
            );
        }
    }
}