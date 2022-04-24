//using System;
//using System.Linq;
//using System.Xml.Serialization;
//using System.Data;
//using System.Windows.Forms;
//using System.Collections.Generic;
//using taskt.UI.Forms;
//using taskt.UI.CustomControls;

//namespace taskt.Core.Automation.Commands
//{
//    [Serializable]
//    [Attributes.ClassAttributes.Group("DataTable Commands")]
//    [Attributes.ClassAttributes.Description("This command allows you to add a columns to a DataTable by a DataTable")]
//    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add a columns to a DataTable by a DataTable.")]
//    [Attributes.ClassAttributes.ImplementationDescription("")]
//    public class AddDataTableColumnsAndFillValuesByDataTableCommand : ScriptCommand
//    {
//        [XmlAttribute]
//        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the DataTable Variable Name")]
//        [Attributes.PropertyAttributes.InputSpecification("Enter a existing DataTable to add rows to.")]
//        [Attributes.PropertyAttributes.SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
//        [Attributes.PropertyAttributes.Remarks("")]
//        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
//        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
//        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable)]
//        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
//        public string v_DataTableName { get; set; }

//        [XmlAttribute]
//        [Attributes.PropertyAttributes.PropertyDescription("Please specify the DataTable to set new Column values")]
//        [Attributes.PropertyAttributes.InputSpecification("")]
//        [Attributes.PropertyAttributes.SampleUsage("**vDataTable** or **{{{vDataTable}}}**")]
//        [Attributes.PropertyAttributes.Remarks("")]
//        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
//        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
//        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
//        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable)]
//        public string v_AddDataTableName { get; set; }

//        [XmlAttribute]
//        [Attributes.PropertyAttributes.PropertyDescription("If Colmun Name is already exists (Default is Ignore)")]
//        [Attributes.PropertyAttributes.InputSpecification("")]
//        [Attributes.PropertyAttributes.SampleUsage("**Ignore** or **Overwrite** or **Error**")]
//        [Attributes.PropertyAttributes.Remarks("")]
//        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
//        [Attributes.PropertyAttributes.PropertyUISelectionOption("Ignore")]
//        [Attributes.PropertyAttributes.PropertyUISelectionOption("Overwrite")]
//        [Attributes.PropertyAttributes.PropertyUISelectionOption("Error")]
//        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
//        public string v_IfColumnExists { set; get; }

//        [XmlAttribute]
//        [Attributes.PropertyAttributes.PropertyDescription("If the number of rows is less than the DataTable to set new Column (Default is Ignore)")]
//        [Attributes.PropertyAttributes.InputSpecification("")]
//        [Attributes.PropertyAttributes.SampleUsage("**Ignore** or **Add Rows** or **Error**")]
//        [Attributes.PropertyAttributes.Remarks("")]
//        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
//        [Attributes.PropertyAttributes.PropertyUISelectionOption("Ignore")]
//        [Attributes.PropertyAttributes.PropertyUISelectionOption("Add Rows")]
//        [Attributes.PropertyAttributes.PropertyUISelectionOption("Error")]
//        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
//        public string v_IfRowNotEnough { set; get; }

//        [XmlAttribute]
//        [Attributes.PropertyAttributes.PropertyDescription("If the number of DataTable to set new Column is less than the rows (Default is Ignore)")]
//        [Attributes.PropertyAttributes.InputSpecification("")]
//        [Attributes.PropertyAttributes.SampleUsage("**Ignore** or **Error**")]
//        [Attributes.PropertyAttributes.Remarks("")]
//        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
//        [Attributes.PropertyAttributes.PropertyUISelectionOption("Ignore")]
//        [Attributes.PropertyAttributes.PropertyUISelectionOption("Error")]
//        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
//        public string v_IfDTNotEnough { set; get; }

//        public AddDataTableColumnsAndFillValuesByDataTableCommand()
//        {
//            this.CommandName = "AddDataTableColumnsAndFillValuesByDataTableCommand";
//            this.SelectionName = "Add DataTable Columns And Fill Values By DataTable";
//            this.CommandEnabled = false;
//            this.CustomRendering = true;
//        }

//        public override void RunCommand(object sender)
//        {
//            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
//            DataTable myDT = v_DataTableName.GetDataTableVariable(engine);

//            DataTable srcDT = v_AddDataTableName.GetDataTableVariable(engine);

//            string ifColExists = "Ignore";
//            if (!String.IsNullOrEmpty(v_IfColumnExists))
//            {
//                ifColExists = v_IfColumnExists.ConvertToUserVariable(engine);
//            }
//            ifColExists = ifColExists.ToLower();
//            switch (ifColExists)
//            {
//                case "ignore":
//                case "overwrite":
//                case "error":
//                    break;
//                default:
//                    throw new Exception("Strange value if column already exists " + v_IfColumnExists);
//                    break;
//            }

//            string ifRowNotEnough = "Ignore";
//            if (!String.IsNullOrEmpty(v_IfRowNotEnough))
//            {
//                ifRowNotEnough = v_IfRowNotEnough.ConvertToUserVariable(engine);
//            }
//            ifRowNotEnough = ifRowNotEnough.ToLower();
//            switch (ifRowNotEnough)
//            {
//                case "ignore":
//                case "add rows":
//                case "error":
//                    break;
//                default:
//                    throw new Exception("Strange value If the number of rows is less than the DataTable to set new Column " + v_IfRowNotEnough);
//                    break;
//            }

//            // rows check
//            if (myDT.Rows.Count < srcDT.Rows.Count)
//            {
//                switch (ifRowNotEnough)
//                {
//                    case "ignore":
//                    case "add rows":
//                        break;
//                    case "error":
//                        throw new Exception("The number of rows is less than the DataTable to set new Column");
//                        break;
//                }
//            }

//            string ifListNotEnough = "Ignore";
//            if (!String.IsNullOrEmpty(v_IfDTNotEnough))
//            {
//                ifListNotEnough = v_IfDTNotEnough.ConvertToUserVariable(engine);
//            }
//            ifListNotEnough = ifListNotEnough.ToLower();
//            switch (ifListNotEnough)
//            {
//                case "ignore":
//                case "error":
//                    break;
//                default:
//                    throw new Exception("Strange value If the number of DataTable to set new Column is less than the rows " + v_IfDTNotEnough);
//                    break;
//            }
//            if ((myDT.Rows.Count > srcDT.Rows.Count) && (ifListNotEnough == "error"))
//            {
//                throw new Exception("The number of DataTable to set new Column is less than the rows");
//            }

//            // column name check
//            List<string> srcColList = new List<string>();
//            for (int i = 0; i < srcDT.Columns.Count; i++)
//            {
//                srcColList.Add(srcDT.Columns[i].ColumnName);
//            }
//            List<string> myColList = new List<string>();
//            for (int i = 0; i < myDT.Columns.Count; i++)
//            {
//                myColList.Add(myDT.Columns[i].ColumnName);
//            }
//            // check col only
//            foreach(var col in srcColList)
//            {
//                if (myColList.Contains(col))
//                {
//                    switch (ifColExists)
//                    {
//                        case "ignore":
//                        case "overwrite":
//                            break;
//                        case "error":
//                            throw new Exception("Column Name " + col + " is already exists");
//                            break;
//                    }
//                }
//            }
            
//            foreach (string col in srcColList)
//            {
//                if (!myColList.Contains(col))
//                {
//                    myDT.Columns.Add(col);
//                }
//                else if (ifColExists == "ignore")
//                {
//                    continue;
//                }
//                int maxRows = (myDT.Rows.Count > srcDT.Rows.Count) ? srcDT.Rows.Count : myDT.Rows.Count;
//                for (int i = 0; i < maxRows; i++)
//                {
//                    myDT.Rows[i][col] = srcDT.Rows[i][col];
//                }
//                if ((myDT.Rows.Count < srcDT.Rows.Count) && (ifRowNotEnough == "add rows"))
//                {
//                    for (int i = myDT.Rows.Count; i < srcDT.Rows.Count; i++)
//                    {
//                        myDT.Rows.Add();
//                        myDT.Rows[i][col] = srcDT.Rows[i][col];
//                    }
//                }
//            }
//        }
//        public override List<Control> Render(frmCommandEditor editor)
//        {
//            base.Render(editor);

//            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
//            RenderedControls.AddRange(ctrls);

//            return RenderedControls;
//        }

//        public override string GetDisplayValue()
//        {
//            return base.GetDisplayValue() + " [Add DataTable Columns from '" + v_AddDataTableName + "' to '" + v_DataTableName + "']";
//        }
//    }
//}