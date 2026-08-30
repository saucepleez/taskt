using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.ExcelGroup;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Shape")]
    [Attributes.ClassAttributes.CommandSettings("Copy Shape By Name")]
    [Attributes.ClassAttributes.Description("This command allows you to copy Shape by Name")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to copy Shape by Name")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelCopyShapeByNameCommand : AExcelShapeActionCommands, IResultProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ChartName))]
        [PropertyDescription("New Shape Name")]
        [PropertyValidationRule("New Shape Name", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "New Shape Name")]
        [PropertyParameterOrder(8000)]
        public string v_NewName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store New Shape Name")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Store New Shape Name", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "Store New Shape Name")]
        [PropertyParameterOrder(8001)]
        public string v_Result { get; set; }

        public ExcelCopyShapeByNameCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.ExcelShapeAction(engine, new Action<Microsoft.Office.Interop.Excel.Shape>(shape =>
            {
                using(var bef = new InnerScriptVariable(engine))
                {
                    var getShapes = new ExcelGetShapeNamesAsListCommand()
                    {
                        v_InstanceName = this.v_InstanceName,
                    };

                    getShapes.v_Result = bef.VariableName;
                    getShapes.RunCommand(engine);
                    shape.Duplicate();

                    string tempNewShapeName;
                    using (var aft = new InnerScriptVariable(engine)) 
                    {
                        getShapes.v_Result = aft.VariableName;
                        getShapes.RunCommand(engine);

                        using (var uncommon = new InnerScriptVariable(engine))
                        {
                            var getUncommon = new GetCommonValuesOfListsCommand()
                            {
                                v_ListA = bef.VariableName,
                                v_ListB = aft.VariableName,
                                v_ListBOnly = uncommon.VariableName,
                            };
                            getUncommon.RunCommand(engine);

                            var lst = (List<string>)uncommon.VariableValue;
                            if (lst.Count == 0)
                            {
                                throw new Exception("New shape does not exists");
                            }
                            tempNewShapeName = lst[0];
                        }
                    }

                    if (string.IsNullOrEmpty(v_NewName))
                    {
                        StoreNewShapeName(tempNewShapeName, engine);
                    }
                    else
                    {
                        // rename new shape
                        var newShape = this.ExpandValueOrUserVariable(nameof(v_NewName), "New Name", engine);
                        var renameShape = new ExcelRenameShapeByNameCommand()
                        {
                            v_InstanceName = this.v_InstanceName,
                            v_ShapeName = tempNewShapeName,
                            v_NewName = newShape,
                        };
                        renameShape.RunCommand(engine);

                        StoreNewShapeName(newShape, engine);
                    }
                }
                
            }));
        }

        /// <summary>
        /// store new shape name to v_Result
        /// </summary>
        /// <param name="newShapeName"></param>
        /// <param name="engine"></param>
        private void StoreNewShapeName(string newShapeName, Engine.AutomationEngineInstance engine)
        {
            if (!string.IsNullOrEmpty(v_Result))
            {
                newShapeName.StoreInUserVariable(engine, v_Result);
            }
        }
    }
}