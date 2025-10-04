using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary")]
    [Attributes.ClassAttributes.SubGruop("Dictionary Action")]
    [Attributes.ClassAttributes.CommandSettings("Create Dictionary")]
    [Attributes.ClassAttributes.Description("This command created a DataTable with the column names provided")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create a new Dictionary")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_dictionary))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class CreateDictionaryCommand : ADictionaryAddCreateCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_OutputDictionaryName))]
        public override string v_Dictionary { get; set; }

        //[XmlElement]
        //[PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_KeyAndValue))]
        //[PropertyParameterOrder(6000)]
        //public DataTable v_ColumnNameDataTable { get; set; }

        public CreateDictionaryCommand()
        {
            //this.CommandName = "CreateDictionaryCommand";
            //this.SelectionName = "Create Dictionary";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var outputDictionary = new Dictionary<string, string>();

            //outputDictionary.AddDataAndValueFromDataTable(v_ColumnNameDataTable, engine);
            AddDataAndValueFromDataTable(outputDictionary, engine);

            //outputDictionary.StoreInUserVariable(engine, v_Dictionary);

            this.StoreDictionaryInUserVariable(outputDictionary, nameof(v_Dictionary), engine);
        }

        //public override void BeforeValidate()
        //{
        //    base.BeforeValidate();
        //    DataTableControls.BeforeValidate((DataGridView)ControlsList[nameof(v_ColumnNameDataTable)], v_ColumnNameDataTable);
        //}
    }
}