using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.SubGruop("Convert")]
    [Attributes.ClassAttributes.Description("This command convert a List to a DataTable.")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class ConvertListToDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Supply the List to convert")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**vList** or **{{{vList}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.List)]
        public string v_InputList { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Supply the DataTable Columns")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**vColumns** or **{{{vColumnss}}}**")]
        [Attributes.PropertyAttributes.Remarks("If Columns is empty, DataTable column is column0, column1, ...")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.List)]
        public string v_Columns { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("When the number of items in the List is greater than the number of Columns")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyIsOptional(true, "Ignore")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Ignore")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Error")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Try Create Columns")]
        public string v_ColumnsNotEnough { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("When the number of Columns is greater than the number of items in the List")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyIsOptional(true, "Ignore")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Ignore")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Error")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Insert Empty Value")]
        public string v_ListItemNotEnough { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the DataTable")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        [Attributes.PropertyAttributes.PropertyParameterDirection(Attributes.PropertyAttributes.PropertyParameterDirection.ParameterDirection.Output)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable)]
        public string v_applyToVariableName { get; set; }

        public ConvertListToDataTableCommand()
        {
            this.CommandName = "ConvertListToDataTableCommand";
            this.SelectionName = "Convert List To DataTable";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            //get variable by regular name
            //Script.ScriptVariable listVariable = v_InputList.GetRawVariable(engine);
            //if (listVariable == null)
            //{
            //    throw new Exception(v_InputList + " does not exists.");
            //}
            //else if (!(listVariable.VariableValue is List<string>))
            //{
            //    throw new Exception(v_InputList + " is not supported List");
            //}
            //List<string> targetList = (List<string>)listVariable.VariableValue;
            List<string> targetList = v_InputList.GetListVariable(engine);

            DataTable myDT = new DataTable();
            if (String.IsNullOrEmpty(v_Columns))
            {
                myDT.Rows.Add();
                for (int i = 0; i < targetList.Count; i++)
                {
                    myDT.Columns.Add("column" + i.ToString());
                    myDT.Rows[0][i] = targetList[i];
                }
            }
            else
            {
                //Script.ScriptVariable keysVariable = v_Columns.GetRawVariable(engine);
                //if (keysVariable == null)
                //{
                //    throw new Exception(v_Columns + " does not exists.");
                //}
                //else if (!(keysVariable.VariableValue is List<string>))
                //{
                //    throw new Exception(v_Columns + " is not supported List.");
                //}
                //List<string> targetColumns = (List<string>)keysVariable.VariableValue;
                List<string> targetColumns = v_Columns.GetListVariable(engine);

                //if (String.IsNullOrEmpty(v_ColumnsNotEnough))
                //{
                //    v_ColumnsNotEnough = "Ignore";
                //}
                //if (String.IsNullOrEmpty(v_ListItemNotEnough))
                //{
                //    v_ListItemNotEnough = "Ignore";
                //}
                string columnsNotEnough = v_ColumnsNotEnough.GetUISelectionValue("v_ColumnsNotEnough", this, engine);
                string listItemNotEnough = v_ListItemNotEnough.GetUISelectionValue("v_ListItemNotEnough", this, engine);

                if ((columnsNotEnough == "error") && (targetList.Count > targetColumns.Count))
                {
                    throw new Exception("The number of keys in " + v_Columns + " is not enough");
                }
                if ((listItemNotEnough == "error") && (targetColumns.Count > targetList.Count))
                {
                    throw new Exception("The number of List items in " + v_InputList + " is not enough");
                }

                if (targetList.Count == targetColumns.Count)
                {
                    myDT.Rows.Add();
                    for (int i = 0; i < targetList.Count; i++)
                    {
                        myDT.Columns.Add(targetColumns[i]);
                        myDT.Rows[0][i] = targetList[i];
                    }
                }
                else if (targetList.Count > targetColumns.Count)
                {
                    switch (columnsNotEnough)
                    {
                        case "ignore":
                            myDT.Rows.Add();
                            for (int i = 0; i < targetList.Count; i++)
                            {
                                myDT.Columns.Add(targetColumns[i]);
                                myDT.Rows[0][i] = targetList[i];
                            }
                            break;

                        case "try create columns":
                            myDT.Rows.Add();
                            for (int i = 0; i < targetColumns.Count; i++)
                            {
                                myDT.Columns.Add(targetColumns[i]);
                                myDT.Rows[0][i] = targetList[i];
                            }
                            for (int i = targetColumns.Count; i < targetList.Count; i++)
                            {
                                myDT.Columns.Add("column" + i.ToString());
                                myDT.Rows[0][i] = targetList[i];
                            }
                            break;
                    }
                }
                else
                {
                    switch (listItemNotEnough)
                    {
                        case "ignore":
                            myDT.Rows.Add();
                            for (int i = 0; i < targetList.Count; i++)
                            {
                                myDT.Columns.Add(targetColumns[i]);
                                myDT.Rows[0][i] = targetList[i];
                            }
                            break;

                        case "insert empty value":
                            myDT.Rows.Add();
                            for (int i = 0; i < targetList.Count; i++)
                            {
                                myDT.Columns.Add(targetColumns[i]);
                                myDT.Rows[0][i] = targetList[i];
                            }
                            for (int i = targetList.Count; i < targetColumns.Count; i++)
                            {
                                myDT.Columns.Add(targetColumns[i]);
                                myDT.Rows[0][i] = "";
                            }
                            break;
                    }
                }
            }
            myDT.StoreInUserVariable(engine, v_applyToVariableName);
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Convert List '" + this.v_InputList + "' To DataTable '" + this.v_applyToVariableName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_InputList))
            {
                this.validationResult += "List is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_applyToVariableName))
            {
                this.validationResult += "Variable to recieve the DataTable is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}