using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.UIAutomationGroup;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation")]
    [Attributes.ClassAttributes.SubGruop("Get From UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Get Text From Table UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to get Text Value from Table UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Text Value from Table UIElement.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationGetTextFromTableUIElementCommand : AGetFromUIElementCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        //public string v_TargetElement { get; set; }

        // todo: create table row&column interface
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDetailSampleUsage("**0**", "Specify the First Row Index")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Row Index")]
        [PropertyDetailSampleUsage("**{{{vRow}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Row Index")]
        [PropertyDescription("Row Index")]
        [InputSpecification("Row Index", true)]
        [PropertyValidationRule("Row", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Row")]
        [PropertyParameterOrder(6000)]
        public string v_Row { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDetailSampleUsage("**0**", "Specify the First Column Index")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Column Index")]
        [PropertyDetailSampleUsage("**{{{vColumn}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Column Index")]
        [PropertyDescription("Column Index")]
        [InputSpecification("Column Index", true)]
        [PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Column")]
        [PropertyParameterOrder(6100)]
        public string v_Column { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(6200)]
        public string v_Result { get; set; }

        public UIAutomationGetTextFromTableUIElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var targetElement = v_TargetElement.ExpandUserVariableAsUIElement(engine);
            //int row = v_Row.ExpandValueOrUserVariableAsInteger("v_Row", engine);
            //int column = v_Column.ExpandValueOrUserVariableAsInteger("v_Column", engine);

            //AutomationElement cellElem = UIElementControls.GetTableUIElement(targetElement, row, column);

            //string res = UIElementControls.GetTextValue(cellElem);
            //res.StoreInUserVariable(engine, v_Result);

            using (var cellVar = new InnerScriptVariable(engine))
            {
                var getCell = new UIAutomationSearchUIElementFromTableUIElementCommand()
                {
                    v_TargetElement = this.v_TargetElement,
                    v_Row = this.v_Row,
                    v_Column = this.v_Column,
                    v_AutomationElementVariable = cellVar.VariableName,
                };
                getCell.RunCommand(engine);

                var getText = new UIAutomationGetTextFromUIElementCommand()
                {
                    v_TargetElement = cellVar.VariableName,
                    v_Result = this.v_Result,
                };
                getText.RunCommand(engine);
            }
        }
    }
}