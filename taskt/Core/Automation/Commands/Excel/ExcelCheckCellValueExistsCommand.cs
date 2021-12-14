using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using System.Linq;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Cell")]
    [Attributes.ClassAttributes.Description("This command checks existance value from a specified Excel Cell.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a value from a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    public class ExcelCheckCellValueExistsCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Excel)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Cell Location")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the actual location of the cell.")]
        [Attributes.PropertyAttributes.SampleUsage("**A1** or **B10** or **{{{vAddress}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_ExcelCellAddress { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the result")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Value type (Default is Cell)")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**Cell** or **Formula** or **Format** or **Color** or **Comment**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Cell")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Formula")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Back Color")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        [Attributes.PropertyAttributes.PropertySecondaryLabel(true)]
        [Attributes.PropertyAttributes.PropertyAddtionalParameterInfo("Cell", "Check cell has value or not")]
        [Attributes.PropertyAttributes.PropertyAddtionalParameterInfo("Formula", "Check cell has formula or not")]
        [Attributes.PropertyAttributes.PropertyAddtionalParameterInfo("Back Color", "Check back color is not white")]
        public string v_ValueType { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private ComboBox cmbValueType;

        [XmlIgnore]
        [NonSerialized]
        private Label lbl2ndValueType;

        [XmlIgnore]
        [NonSerialized]
        private Label lblValueType;

        public ExcelCheckCellValueExistsCommand()
        {
            this.CommandName = "ExcelCheckCellValueExistsCommand";
            this.SelectionName = "Check Cell Value Exists";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            this.v_InstanceName = "";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var excelObject = engine.GetAppInstance(vInstance);

            var targetAddress = v_ExcelCellAddress.ConvertToUserVariable(sender);

            var valueType = v_ValueType.ConvertToUserVariable(sender);
            if (String.IsNullOrEmpty(valueType))
            {
                valueType = "Cell";
            }

            Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
            Microsoft.Office.Interop.Excel.Worksheet excelSheet = excelInstance.ActiveSheet;

            bool valueState;
            switch (valueType)
            {
                case "Cell":
                    valueState= !String.IsNullOrEmpty((string)excelSheet.Range[targetAddress].Text);
                    break;
                case "Formula":
                    valueState = ((string)excelInstance.Range[targetAddress].Formula).StartsWith("=");
                    break;
                case "Back Color":
                    valueState = ((long)excelInstance.Range[targetAddress].Interior.Color) != 16777215;
                    break;
                default:
                    throw new Exception("Value type " + valueType + " is not support.");
                    break;
            }
             
            (valueState ? "TRUE" : "FALSE").StoreInUserVariable(sender, v_userVariableName);            
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ctls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctls);

            cmbValueType = (ComboBox)ctls.Where(t => t.Name == "v_ValueType").FirstOrDefault();
            cmbValueType.SelectedIndexChanged += (sender, e) => cmbValueType_SelectedIndexChanged(sender, e);

            lbl2ndValueType = (Label)ctls.Where(t => t.Name == "lbl2_v_ValueType").FirstOrDefault();
            lblValueType = (Label)ctls.GetControlsByName("v_ValueType", CommandControls.CommandControlType.Label)[0];

            if (editor.creationMode == frmCommandEditor.CreationMode.Add)
            {
                this.v_InstanceName = editor.appSettings.ClientSettings.DefaultExcelInstanceName;
            }

            return RenderedControls;
        }

        private void cmbValueType_SelectedIndexChanged(object sender, EventArgs e) 
        {
            string searchedKey = cmbValueType.SelectedItem.ToString();
            Dictionary<string, string> dic = (Dictionary<string, string>)lblValueType.Tag;

            lbl2ndValueType.Text = dic.ContainsKey(searchedKey) ? dic[searchedKey] : "";
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Check " + v_ValueType + " Value Exists From '" + v_ExcelCellAddress + "' and apply to variable '" + v_userVariableName + "', Instance Name: '" + v_InstanceName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_InstanceName))
            {
                this.validationResult += "Instance is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_ExcelCellAddress))
            {
                this.validationResult += "Address is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_userVariableName))
            {
                this.validationResult += "Variable is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}