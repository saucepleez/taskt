using System;
using System.Xml.Serialization;
using System.Data;

namespace taskt.Core.Automation.Commands
{
 

   
    [Serializable]
    [Attributes.ClassAttributes.Group("Database Commands")]
    [Attributes.ClassAttributes.Description("This command selects data from a database and applies it against a dataset")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to select data from a database.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'OLEDB' to achieve automation.")]
    public class DatabaseRunQueryCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please create a dataset variable name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter a custom name that references the dataset.")]
        [Attributes.PropertyAttributes.SampleUsage("**MyData**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DataSetName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the connection string")]
        [Attributes.PropertyAttributes.InputSpecification("Enter a valid connection string to be used by the database.")]
        [Attributes.PropertyAttributes.SampleUsage(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\myFolder\myAccessFile.accdb;Persist Security Info = False;")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ConnectionString { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please provide the query to run")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the query as text that should be executed.")]
        [Attributes.PropertyAttributes.SampleUsage("**Select * From [table]**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_UserQuery { get; set; }
        public DatabaseRunQueryCommand()
        {
            this.CommandName = "DatabaseRunQueryCommand";
            this.SelectionName = "Run Query";
            this.CommandEnabled = false;
        }

        public override void RunCommand(object sender)
        {

            DatasetCommands dataSetCommand = new DatasetCommands();
            DataTable requiredData = dataSetCommand.CreateDataTable(v_ConnectionString.ConvertToUserVariable(sender), v_UserQuery);

            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            Script.ScriptVariable newDataset = new Script.ScriptVariable
            {
                VariableName = v_DataSetName,
                VariableValue = requiredData
            };

            engine.VariableList.Add(newDataset);

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [" + v_UserQuery + "]";
        }
    }
}