using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List")]
    [Attributes.ClassAttributes.SubGruop("Convert")]
    [Attributes.ClassAttributes.CommandSettings("Convert List To Visualized Text")]
    [Attributes.ClassAttributes.Description("This command allows you to Convert List to Visualized Text.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Convert List to Visualized Text.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ConvertListToVisualizedTextCommand : AListGetFromListCommands
    {
        //[XmlAttribute]
        //public string v_List { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public override string v_Result { get; set; }

        public ConvertListToVisualizedTextCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var list = this.ExpandUserVariableAsList(engine);

            var maxValueLength = 0;
            foreach (var item in list)
            {
                var s = item.Length;
                if (maxValueLength < s)
                {
                    maxValueLength = s;
                }
            }
            if (maxValueLength < 5)
            {
                maxValueLength = 5;  // Value
            }

            var size = list.Count;
            int indexDigits = (int)Math.Log10(size) + 1;
            if (indexDigits < 5)
            {
                indexDigits = 5; // Index
            }

            string indexFormat = $"{{0,{indexDigits}}}";
            string valueFormat = $"{{0,-{maxValueLength}}}";

            string txt = $"{string.Format(indexFormat, "Index")} | {string.Format(valueFormat, "Value")}\r\n";
            for (int i = 0; i < indexDigits; i++)
            {
                txt += "-";
            }
            txt += "-|-";
            for (int i = 0; i < maxValueLength; i++)
            {
                txt += "-";
            }
            txt += "\r\n";

            for (int i = 0; i < size; i++)
            {
                txt += $"{string.Format(indexFormat, i)} | {string.Format(valueFormat, list[i])}\r\n";
            }

            txt.StoreInUserVariable(engine, v_Result);
        }
    }
}