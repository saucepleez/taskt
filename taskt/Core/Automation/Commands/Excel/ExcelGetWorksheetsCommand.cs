using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Sheet")]
    [Attributes.ClassAttributes.CommandSettings("Get Worksheets")]
    [Attributes.ClassAttributes.Description("This command allows you to get a specific worksheet names")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to switch to a specific worksheet")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelGetWorksheetsCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_SheetName))]
        public string v_SheetName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Search Method")]
        [InputSpecification("", true)]
        [SampleUsage("**Contains** or **Start with** or **End with**")]
        [Remarks("")]
        [PropertyUISelectionOption("Contains")]
        [PropertyUISelectionOption("Start with")]
        [PropertyUISelectionOption("End with")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsOptional(true, "Contains")]
        public string v_SearchMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public string v_applyToVariable { get; set; }

        public ExcelGetWorksheetsCommand()
        {
            //this.CommandName = "ExcelGetWorksheetsCommand";
            //this.SelectionName = "Get Worksheets";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var excelInstance = v_InstanceName.GetExcelInstance(engine);

            List<string> sheetNames = new List<string>();

            var targetSheetName = v_SheetName.ConvertToUserVariable(sender);
            
            if (String.IsNullOrEmpty(targetSheetName))
            {
                foreach (Microsoft.Office.Interop.Excel.Worksheet sh in excelInstance.Worksheets)
                {
                    sheetNames.Add(sh.Name);
                }
            }
            else
            {
                Func<string, string, bool> func = null;

                var searchMethod = this.GetUISelectionValue(nameof(v_SearchMethod), "Search Method", engine);

                switch (searchMethod)
                {
                    case "contains":
                        func = (sht, search) => { return sht.Contains(search); };
                        break;
                    case "start with":
                        func = (sht, search) => { return sht.StartsWith(search); };
                        break;
                    case "end with":
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

            sheetNames.StoreInUserVariable(engine, v_applyToVariable);
        }
    }
}