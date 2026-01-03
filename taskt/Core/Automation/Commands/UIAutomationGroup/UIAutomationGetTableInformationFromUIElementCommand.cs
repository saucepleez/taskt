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

        public UIAutomationGetTableInformationFromUIElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.UIElementAction(engine,
                new Action<AutomationElement>((targetElement) =>
                {
                    if (targetElement.TryGetCurrentPattern(GridPattern.Pattern, out object gridObj))
                    {
                        var grid = (GridPattern)gridObj;
                        switch(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_InformationType), engine))
                        {
                            case "column count":
                                grid.Current.ColumnCount.StoreInUserVariable(engine, v_Result);
                                break;
                            case "row count":
                                grid.Current.RowCount.StoreInUserVariable(engine, v_Result);
                                break;
                        }
                    }
                    else
                    {
                        this.ValueCanNotRetrievedProcess("Table Value", new Action(() =>
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