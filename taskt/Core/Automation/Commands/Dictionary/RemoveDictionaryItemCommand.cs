using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary Commands")]
    [Attributes.ClassAttributes.SubGruop("Dictionary Item")]
    [Attributes.ClassAttributes.CommandSettings("Remove Dictionary Item")]
    [Attributes.ClassAttributes.Description("This command allows you to remove item in Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to remove item in Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class RemoveDictionaryItemCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_BothDictionaryName))]
        public string v_InputData { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_Key))]
        public string v_Key { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_WhenKeyDoesNotExists))]
        [PropertyUISelectionOption("Ignore")]
        [PropertyDetailSampleUsage("**Ignore**", "Don't Remove the Dictionary Item")]
        public string v_IfKeyDoesNotExists { get; set; }

        public RemoveDictionaryItemCommand()
        {
            //this.CommandName = "RemoveDictionaryItemCommand";
            //this.SelectionName = "Remove Dictionary Item";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (var dic, var vKey) = this.GetDictionaryVariableAndKey(nameof(v_InputData), nameof(v_Key), engine);

            if (!dic.Remove(vKey))
            {
                string ifNotExists = this.GetUISelectionValue(nameof(v_IfKeyDoesNotExists), "Key Not Exists", engine);
                switch (ifNotExists)
                {
                    case "error":
                        throw new Exception("Dictionary does not has key name " + vKey);
                }
            }
        }
    }
}