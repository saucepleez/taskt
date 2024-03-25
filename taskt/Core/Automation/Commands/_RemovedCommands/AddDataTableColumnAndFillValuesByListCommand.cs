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
//    [Attributes.ClassAttributes.Description("This command allows you to add a column to a DataTable by a List")]
//    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add a column to a DataTable by a List.")]
//    [Attributes.ClassAttributes.ImplementationDescription("")]
//    public class AddDataTableColumnAndFillValuesByListCommand : ScriptCommand
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
//        [Attributes.PropertyAttributes.PropertyDescription("Please specify the Column Name to add")]
//        [Attributes.PropertyAttributes.InputSpecification("")]
//        [Attributes.PropertyAttributes.SampleUsage("**newColumn** or **{{{vNewColumn}}}**")]
//        [Attributes.PropertyAttributes.Remarks("")]
//        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
//        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
//        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
//        public string v_AddColumnName { get; set; }

//        [XmlAttribute]
//        [Attributes.PropertyAttributes.PropertyDescription("Please specify the List to set new Column values")]
//        [Attributes.PropertyAttributes.InputSpecification("")]
//        [Attributes.PropertyAttributes.SampleUsage("**vList** or **{{{vList}}}**")]
//        [Attributes.PropertyAttributes.Remarks("")]
//        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
//        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
//        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
//        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.List)]
//        public string v_AddListName { get; set; }

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
//        [Attributes.PropertyAttributes.PropertyDescription("If the number of rows is less than the List (Default is Ignore)")]
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
//        [Attributes.PropertyAttributes.PropertyDescription("If the number of List items is less than the rows (Default is Ignore)")]
//        [Attributes.PropertyAttributes.InputSpecification("")]
//        [Attributes.PropertyAttributes.SampleUsage("**Ignore** or **Error**")]
//        [Attributes.PropertyAttributes.Remarks("")]
//        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
//        [Attributes.PropertyAttributes.PropertyUISelectionOption("Ignore")]
//        [Attributes.PropertyAttributes.PropertyUISelectionOption("Error")]
//        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
//        public string v_IfListNotEnough { set; get; }

//        public AddDataTableColumnAndFillValuesByListCommand()
//        {
//            this.CommandName = "AddDataTableColumnFillValuesByListCommand";
//            this.SelectionName = "Add DataTable Column And Fill Values By List";
//            this.CommandEnabled = false;
//            this.CustomRendering = true;
//        }

//        public override void RunCommand(object sender)
//        {
//            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
//            DataTable myDT = v_DataTableName.GetDataTableVariable(engine);

//            string newColName = v_AddColumnName.ConvertToUserVariable(engine);

//            List<string> myList = v_AddListName.GetListVariable(engine);

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
//                    throw new Exception("Strange value If the number of rows is less than the List " + v_IfRowNotEnough);
//                    break;
//            }

//            // rows check
//            if (myDT.Rows.Count < myList.Count)
//            {
//                switch (ifRowNotEnough)
//                {
//                    case "ignore":
//                    case "add rows":
//                        break;
//                    case "error":
//                        throw new Exception("The number of rows is less than the List");
//                        break;
//                }
//            }

//            string ifListNotEnough = "Ignore";
//            if (!String.IsNullOrEmpty(v_IfListNotEnough))
//            {
//                ifListNotEnough = v_IfListNotEnough.ConvertToUserVariable(engine);
//            }
//            ifListNotEnough = ifListNotEnough.ToLower();
//            switch (ifListNotEnough)
//            {
//                case "ignore":
//                case "error":
//                    break;
//                default:
//                    throw new Exception("Strange value If the number of List items is less than the rows " + v_IfColumnExists);
//                    break;
//            }
//            if ((myDT.Rows.Count > myList.Count) && (ifListNotEnough == "error"))
//            {
//                throw new Exception("The number of List items is less than the rows");
//            }

//            // column name check
//            bool isExistsCol = false;
//            for (int i = 0; i < myDT.Columns.Count; i++)
//            {
//                if (newColName == myDT.Columns[i].ColumnName)
//                {
//                    switch (ifColExists)
//                    {
//                        case "ignore":
//                            return; // nothing to do
//                            break;
//                        case "overwrite":
//                            isExistsCol = true;
//                            break;
//                        case "error":
//                            throw new Exception("Column Name " + v_AddColumnName + " is already exists");
//                            break;
//                    }
//                }
//            }
//            if (!isExistsCol)
//            {
//                myDT.Columns.Add(newColName);
//            }

//            int maxRow = (myDT.Rows.Count > myList.Count) ? myList.Count : myDT.Rows.Count;
//            for (int i = 0; i < maxRow; i++)
//            {
//                myDT.Rows[i][newColName] = myList[i];
//            }
//            if ((myDT.Rows.Count < myList.Count) && (ifRowNotEnough == "add rows"))
//            {
//                for (int i = myDT.Rows.Count; i < myList.Count; i++)
//                {
//                    myDT.Rows.Add();
//                    myDT.Rows[i][newColName] = myList[i];
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
//            return base.GetDisplayValue() + " [Add DataTable '" + v_DataTableName + "' Column Name '" + v_AddColumnName + "' List '" + v_AddListName + "']";
//        }
//    }
//}