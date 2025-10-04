using System;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary")]
    [Attributes.ClassAttributes.SubGruop("Convert")]
    [Attributes.ClassAttributes.CommandSettings("Convert Dictionary To DataTable")]
    [Attributes.ClassAttributes.Description("This command allows you to convert Dictionary to DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert Dictionary to DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_dictionary))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ConvertDictionaryToDataTableCommand : ADictionaryGetFromDictionaryCommands, IDataTableResultProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        //public string v_Dictionary { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_OutputDataTableName))]
        public override string v_Result { get; set; }

        public ConvertDictionaryToDataTableCommand()
        {
            //this.CommandName = "ConvertDictionaryTDataTableCommand";
            //this.SelectionName = "Convert Dictionary To DataTable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var dic = v_Dictionary.ExpandUserVariableAsDictinary(engine);
            var dic = this.ExpandUserVariableAsDictionary(engine);

            DataTable myDT = new DataTable();
            myDT.Rows.Add();
            foreach(var item in dic)
            {
                myDT.Columns.Add(item.Key);
                myDT.Rows[0][item.Key] = item.Value;
            }

            //DT.StoreInUserVariable(engine, v_Result);
            this.StoreDataTableInUserVariable(myDT, engine);
        }
    }
}