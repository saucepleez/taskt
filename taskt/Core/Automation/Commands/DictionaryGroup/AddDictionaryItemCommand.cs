using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary")]
    [Attributes.ClassAttributes.SubGruop("Dictionary Item")]
    [Attributes.ClassAttributes.CommandSettings("Add Dictionary Item")]
    [Attributes.ClassAttributes.Description("This command Adds a key and value to a existing Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add to a dictionary")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_dictionary))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class AddDictionaryItemCommand : ADictionaryAddCreateCommands, IHaveDataTableElements
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_BothDictionaryName))]
        public override string v_Dictionary { get; set; }

        //[XmlElement]
        //[PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_KeyAndValue))]
        //[PropertyParameterOrder(6000)]
        //public DataTable v_ColumnNameDataTable { get; set; }

        public AddDictionaryItemCommand()
        {
            //this.CommandName = "AddDictionaryItemCommand";
            //this.SelectionName = "Add Dictionary Item";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var outputDictionary = v_Dictionary.ExpandUserVariableAsDictinary(engine);
            var outputDictionary = this.ExpandUserVariableAsDictionary(engine);

            //outputDictionary.AddDataAndValueFromDataTable(v_ColumnNameDataTable, engine);
            AddDataAndValueFromDataTable(outputDictionary, engine);
        }

        //public override void BeforeValidate()
        //{
        //    base.BeforeValidate();
        //    DataTableControls.BeforeValidate((DataGridView)ControlsList[nameof(v_ColumnNameDataTable)], v_ColumnNameDataTable);
        //}
    }
}
