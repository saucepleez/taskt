using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Worksheet")]
    [Attributes.ClassAttributes.CommandSettings("Get Worksheets")]
    [Attributes.ClassAttributes.Description("This command allows you to get a specific worksheet names")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to switch to a specific worksheet")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelGetWorksheetsCommand : AExcelSheetCommand
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_SheetName))]
        //public string v_SheetName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Search Method")]
        [InputSpecification("", true)]
        [SampleUsage("**Contains** or **Starts with** or **Ends with**")]
        [Remarks("")]
        [PropertyUISelectionOption("Contains")]
        [PropertyUISelectionOption("Starts with")]
        [PropertyUISelectionOption("Ends with")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsOptional(true, "Contains")]
        [PropertyParameterOrder(7000)]
        public string v_CompareMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        [PropertyParameterOrder(7001)]
        public string v_Result { get; set; }

        public ExcelGetWorksheetsCommand()
        {
            //this.CommandName = "ExcelGetWorksheetsCommand";
            //this.SelectionName = "Get Worksheets";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var excelInstance = v_InstanceName.ExpandValueOrUserVariableAsExcelInstance(engine);

            List<string> sheetNames = new List<string>();

            var targetSheetName = v_SheetName.ExpandValueOrUserVariable(engine);
            
            if (string.IsNullOrEmpty(targetSheetName))
            {
                foreach (Microsoft.Office.Interop.Excel.Worksheet sh in excelInstance.Worksheets)
                {
                    sheetNames.Add(sh.Name);
                }
            }
            else
            {
                Func<string, string, bool> func = null;

                var searchMethod = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_CompareMethod), "Search Method", engine);

                switch (searchMethod)
                {
                    case "contains":
                        func = (sht, search) => { return sht.Contains(search); };
                        break;
                    case "starts with":
                        func = (sht, search) => { return sht.StartsWith(search); };
                        break;
                    case "ends with":
                        func = (sht, search) => { return sht.EndsWith(search); };
                        break;
                }

                foreach (Microsoft.Office.Interop.Excel.Worksheet sh in excelInstance.Worksheets)
                {
                    if (func(sh.Name, targetSheetName))
                    {
                        sheetNames.Add(sh.Name);
                    }
                }
            }

            sheetNames.StoreInUserVariable(engine, v_Result);
        }
    }
}