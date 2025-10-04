using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON")]
    [Attributes.ClassAttributes.SubGruop("Get/Set")]
    [Attributes.ClassAttributes.CommandSettings("Get JSON Values As DataTable")]
    [Attributes.ClassAttributes.Description("This command allows you to Get JSON Values From JSON and Result Values is DataTable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get Values From JSON and Result Values is DataTable")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetJSONValuesAsDataTableCommand : AJSONGetFromJContainerCommands, IDataTableResultProperties
    {
        //[XmlAttribute]
        //public string v_Json { get; set; }

        //[XmlAttribute]
        //public string v_JsonExtractor { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_OutputDataTableName))]
        [Remarks("DataTable has JSONPath and Value columns")]
        public override string v_Result { get; set; }

        public GetJSONValuesAsDataTableCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            (_, var root, _) = this.ExpandValueOrUserVariableAsJSON(engine);
            var jsonPath = v_JsonExtractor.ExpandValueOrUserVariable(engine);

            var elems = root.SelectTokens(jsonPath);

            var tbl = this.CreateEmptyDataTable();
            tbl.Columns.Add("JSONPath");
            tbl.Columns.Add("Value");
            foreach(var e in elems)
            {
                tbl.Rows.Add(new string[] { e.Path, e.ToString() ?? ""});
            }
            this.StoreDataTableInUserVariable(tbl, engine);
        }
    }
}