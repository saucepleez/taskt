using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary Commands")]
    [Attributes.ClassAttributes.Description("This command Reads a Config file and stores it into a Dictionary.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to load a config file.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop and OLEDB to achieve automation.")]
    public class LoadDictionaryCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Dictionary Name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a name for a Dictionary.")]
        [Attributes.PropertyAttributes.SampleUsage("**myDictionary**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DictionaryName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the workbook file path")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the applicable file that should be loaded into the Dictionary.")]
        [Attributes.PropertyAttributes.SampleUsage(@"C:\temp\myfile.xlsx or [vFilePath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the Sheet Name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the sheet name of the workbook to be read.")]
        [Attributes.PropertyAttributes.SampleUsage("Sheet1")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SheetName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the Key column")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the key column name to create a Dictionary off of.")]
        [Attributes.PropertyAttributes.SampleUsage("keyColumn")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_KeyColumn { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the Value Column")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a value column name to create a Dictionary off of.")]
        [Attributes.PropertyAttributes.SampleUsage("valueColumn")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ValueColumn { get; set; }

        public LoadDictionaryCommand()
        {
            this.CommandName = "LoadDictionaryCommand";
            this.SelectionName = "Load Dictionary";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vInstance = DateTime.Now.ToString();
            var vFilePath = v_FilePath.ConvertToUserVariable(sender);
            var vDictionaryName = v_DictionaryName;
            var vDictionary = LookupVariable(engine); 
            if (vDictionary != null)
            {
                vDictionaryName = vDictionary.VariableName;
            }

            var newExcelSession = new Microsoft.Office.Interop.Excel.Application
            {
                Visible = false
            };

            engine.AddAppInstance(vInstance, newExcelSession);


            var excelObject = engine.GetAppInstance(vInstance);
            Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;

            //Query required from workbook using OLEDB
            DatasetCommands dataSetCommand = new DatasetCommands();
            DataTable requiredData = dataSetCommand.CreateDataTable(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + vFilePath + @";Extended Properties=""Excel 12.0;HDR=YES;IMEX=1""", "Select " + v_KeyColumn + "," + v_ValueColumn + " From [" + v_SheetName + "$]");

            var dictlist = requiredData.AsEnumerable().Select(x => new
            {
                keys = (string)x[v_KeyColumn],
                values = (string)x[v_ValueColumn]
            }).ToList();
            Dictionary<string, string> outputDictionary = new Dictionary<string, string>();
            foreach (var dict in dictlist)
            {
                outputDictionary.Add(dict.keys, dict.values);
            }


            Script.ScriptVariable newDictionary = new Script.ScriptVariable
            {
                VariableName = vDictionaryName,
                VariableValue = outputDictionary
            };
            //close excel
            excelInstance.Quit();

            //remove instance
            engine.RemoveAppInstance(vInstance);

            //Overwrites variable if it already exists
            if (engine.VariableList.Exists(x => x.VariableName == newDictionary.VariableName))
            {
                Script.ScriptVariable tempDictionary = engine.VariableList.Where(x => x.VariableName == newDictionary.VariableName).FirstOrDefault();
                engine.VariableList.Remove(tempDictionary);
            }
            engine.VariableList.Add(newDictionary);
        }

        private Script.ScriptVariable LookupVariable(Core.Automation.Engine.AutomationEngineInstance sendingInstance)
        {
            //search for the variable
            var requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == v_DictionaryName).FirstOrDefault();

            //if variable was not found but it starts with variable naming pattern
            if ((requiredVariable == null) && (v_DictionaryName.StartsWith(sendingInstance.engineSettings.VariableStartMarker)) && (v_DictionaryName.EndsWith(sendingInstance.engineSettings.VariableEndMarker)))
            {
                //reformat and attempt
                var reformattedVariable = v_DictionaryName.Replace(sendingInstance.engineSettings.VariableStartMarker, "").Replace(sendingInstance.engineSettings.VariableEndMarker, "");
                requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == reformattedVariable).FirstOrDefault();
            }

            return requiredVariable;
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DictionaryName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SheetName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_KeyColumn", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ValueColumn", this, editor));


            return RenderedControls;

        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Load Dictionary from '" + v_FilePath + "' and store in: '" +v_DictionaryName+ "']";
        }
    }
}