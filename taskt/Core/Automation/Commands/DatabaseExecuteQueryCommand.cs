using System;
using System.Xml.Serialization;
using System.Data;
using System.Collections.Generic;
using taskt.UI.Forms;
using System.Windows.Forms;
using taskt.UI.CustomControls;
using System.Data.OleDb;
using System.Drawing;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Database Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to perform a database query and apply the result to a dataset")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to select data from a database.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'OLEDB' to achieve automation.")]
    public class DatabaseExecuteQueryCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Define Query Execution Type")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Return Dataset")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Execute NonQuery")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Execute Stored Procedure")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_QueryType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Define Query to Execute")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Query { get; set; }

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Define Query Parameters")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public DataTable v_QueryParameters { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView QueryParametersGridView;

        [XmlIgnore]
        [NonSerialized]
        private List<Control> QueryParametersControls;


        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Apply Result To Variable")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_DatasetName { get; set; }

        public DatabaseExecuteQueryCommand()
        {
            this.CommandName = "DatabaseExecuteQueryCommand";
            this.SelectionName = "Execute Database Query";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_InstanceName = "sqlDefault";

            this.v_QueryParameters = new System.Data.DataTable
            {
                TableName = "QueryParamTable" + DateTime.Now.ToString("MMddyy.hhmmss")
            };

            this.v_QueryParameters.Columns.Add("Parameter Name");
            this.v_QueryParameters.Columns.Add("Parameter Value");
            this.v_QueryParameters.Columns.Add("Parameter Type");

        }

        public override void RunCommand(object sender)
        {
            //create engine, instance, query
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var query = v_Query.ConvertToUserVariable(sender);

            //define connection
            var databaseConnection = (OleDbConnection)engine.GetAppInstance(vInstance);
            var queryExecutionType = v_QueryType.ConvertToUserVariable(sender);

            //define commad
            var oleCommand = new System.Data.OleDb.OleDbCommand(query, databaseConnection);

            //add parameters
            foreach (DataRow rw in this.v_QueryParameters.Rows)
            {
                var parameterName = rw.Field<string>("Parameter Name").ConvertToUserVariable(sender);
                var parameterValue = rw.Field<string>("Parameter Value").ConvertToUserVariable(sender);
                var parameterType = rw.Field<string>("Parameter Type").ConvertToUserVariable(sender);

                object convertedValue = null;
                //"STRING", "BOOLEAN", "DECIMAL", "INT16", "INT32", "INT64", "DATETIME", "DOUBLE", "SINGLE", "GUID", "BYTE", "BYTE[]"
                switch (parameterType)
                {
                    case "STRING":
                        convertedValue = parameterValue;
                        break;
                    case "BOOLEAN":
                        convertedValue = Convert.ToBoolean(parameterValue);
                        break;
                    case "DECIMAL":
                        convertedValue = Convert.ToDecimal(parameterValue);
                        break;
                    case "INT16":
                        convertedValue = Convert.ToInt16(parameterValue);
                        break;
                    case "INT32":
                        convertedValue = Convert.ToInt32(parameterValue);
                        break;
                    case "INT64":
                        convertedValue = Convert.ToInt64(parameterValue);
                        break;
                    case "DATETIME":
                        convertedValue = Convert.ToDateTime(parameterValue);
                        break;
                    case "DOUBLE":
                        convertedValue = Convert.ToDouble(parameterValue);
                        break;
                    case "SINGLE":
                        convertedValue = Convert.ToSingle(parameterValue);
                        break;
                    case "GUID":
                        convertedValue = Guid.Parse(parameterValue);
                        break;
                    case "BYTE":
                        convertedValue = Convert.ToByte(parameterValue);
                        break;
                    case "BYTE[]":
                        convertedValue = System.Text.Encoding.UTF8.GetBytes(parameterValue);
                        break;
                    default:
                        throw new NotImplementedException($"Parameter Type '{parameterType}' not implemented!");
                }

                oleCommand.Parameters.AddWithValue(parameterName, convertedValue);


            }

            if (queryExecutionType == "Return Dataset")
            {

                DataTable dataTable = new DataTable();
                OleDbDataAdapter adapter = new OleDbDataAdapter(oleCommand);
                adapter.SelectCommand = oleCommand;
                databaseConnection.Open();
                adapter.Fill(dataTable);
                databaseConnection.Close();

                
                dataTable.TableName = v_DatasetName;
                engine.DataTables.Add(dataTable);

                engine.AddVariable(v_DatasetName, dataTable);
           
            }
            else if (queryExecutionType == "Execute NonQuery")
            {
                databaseConnection.Open();
                var result = oleCommand.ExecuteNonQuery();
                databaseConnection.Close();

                engine.AddVariable(v_DatasetName, result.ToString());
            }
            else if(queryExecutionType == "Execute Stored Procedure")
            {
                oleCommand.CommandType = CommandType.StoredProcedure;
                databaseConnection.Open();
                var result = oleCommand.ExecuteNonQuery();
                databaseConnection.Close();
                engine.AddVariable(v_DatasetName, result.ToString());
            }
            else
            {
                throw new NotImplementedException($"Query Execution Type '{queryExecutionType}' not implemented.");
            }

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));


            var queryControls = CommandControls.CreateDefaultInputGroupFor("v_Query", this, editor);
            var queryBox = (TextBox)queryControls[2];
            queryBox.Multiline = true;
            queryBox.Height = 150;
            RenderedControls.AddRange(queryControls);

            //set up query parameter controls
            QueryParametersGridView = new DataGridView();
            QueryParametersGridView.AllowUserToAddRows = true;
            QueryParametersGridView.AllowUserToDeleteRows = true;
            QueryParametersGridView.Size = new Size(400, 250);
            QueryParametersGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            QueryParametersGridView.AutoGenerateColumns = false;
        

            var selectColumn = new DataGridViewComboBoxColumn();
            selectColumn.HeaderText = "Type";
            selectColumn.DataPropertyName = "Parameter Type";
            selectColumn.DataSource = new string[] { "STRING", "BOOLEAN", "DECIMAL", "INT16", "INT32", "INT64", "DATETIME", "DOUBLE", "SINGLE", "GUID", "BYTE", "BYTE[]" };
            QueryParametersGridView.Columns.Add(selectColumn);

            var paramNameColumn = new DataGridViewTextBoxColumn();
            paramNameColumn.HeaderText = "Name";
            paramNameColumn.DataPropertyName = "Parameter Name";
            QueryParametersGridView.Columns.Add(paramNameColumn);

            var paramValueColumn = new DataGridViewTextBoxColumn();
            paramValueColumn.HeaderText = "Value";
            paramValueColumn.DataPropertyName = "Parameter Value";
            QueryParametersGridView.Columns.Add(paramValueColumn);

            QueryParametersGridView.DataBindings.Add("DataSource", this, "v_QueryParameters", false, DataSourceUpdateMode.OnPropertyChanged);
         
            QueryParametersControls = new List<Control>();

            QueryParametersControls.Add(CommandControls.CreateDefaultLabelFor("v_QueryParameters", this));
            QueryParametersControls.AddRange(CommandControls.CreateUIHelpersFor("v_QueryParameters", this, new Control[] { QueryParametersGridView }, editor));

            CommandItemControl helperControl = new CommandItemControl();
            helperControl.Padding = new Padding(10, 0, 0, 0);
            helperControl.ForeColor = Color.AliceBlue;
            helperControl.Font = new Font("Segoe UI Semilight", 10);
            helperControl.Name = "add_param_helper";
            helperControl.CommandImage = UI.Images.GetUIImage("VariableCommand");
            helperControl.CommandDisplay = "Add Parameter";
            helperControl.Click += (sender, e) => AddParameter(sender, e);
            QueryParametersControls.Add(helperControl);
            QueryParametersControls.Add(QueryParametersGridView);
            RenderedControls.AddRange(QueryParametersControls);



            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_QueryType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DatasetName", this, editor));
            return RenderedControls;

        }

        private void AddParameter(object sender, EventArgs e)
        {
            this.v_QueryParameters.Rows.Add();
        }

        public override string GetDisplayValue()
        {
            return $"{base.GetDisplayValue()} - [{v_QueryType}, Apply Result to Variable '{v_DatasetName}', Instance Name: '{v_InstanceName}']";
        }
    }
}