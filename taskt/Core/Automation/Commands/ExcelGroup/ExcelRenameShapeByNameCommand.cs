using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.ExcelGroup;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Shape")]
    [Attributes.ClassAttributes.CommandSettings("Rename Shape By Name")]
    [Attributes.ClassAttributes.Description("This command allows you to rename Shape by Name")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to rename Shape by Name")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelRenameShapeByNameCommand : AExcelShapeActionCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ChartName))]
        [PropertyDescription("New Shape Name")]
        [PropertyValidationRule("New Shape Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "New Shape Name")]
        [PropertyParameterOrder(8000)]
        public string v_NewName { get; set; }

        public ExcelRenameShapeByNameCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.ExcelShapeAction(engine, new Action<Microsoft.Office.Interop.Excel.Shape>(shape =>
            {
                var newName = this.ExpandValueOrUserVariable(nameof(v_NewName), "New Name", engine);

                if (shape.Name != newName)
                {
                    using (var re = new InnerScriptVariable(engine))
                    {
                        var checkShape = new ExcelCheckShapeExistsByNameCommand()
                        {
                            v_InstanceName = this.v_InstanceName,
                            v_ShapeName = newName,
                            v_Result = re.VariableName,
                        };
                        checkShape.RunCommand(engine);

                        var isExists = bool.Parse(re.VariableValue.ToString());
                        if (!isExists)
                        {
                            shape.Name = newName;
                        }
                        else
                        {
                            throw new Exception($"Shape Name is already Used. Name: {v_NewName}, Expanded Value: '{newName}'");
                        }
                    }
                }
            }));
        }
    }
}