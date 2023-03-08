using System;
using System.Xml.Serialization;
using System.Data;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Automation;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Variable Commands")]
    [Attributes.ClassAttributes.CommandSettings("Get Variable Type")]
    [Attributes.ClassAttributes.Description("This command allows you to get variable type.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get variable type.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetVariableTypeCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Variable Name")]
        [InputSpecification("Variable Name", true)]
        [PropertyDetailSampleUsage("**vSomeVariable**", PropertyDetailSampleUsage.ValueType.Value, "Variable Name")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Variable")]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_Result { get; set; }

        public GetVariableTypeCommand()
        {
            //this.CommandName = "GetVariableTypeCommand";
            //this.SelectionName = "Get Variable Type";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            var rawValue = v_userVariableName.GetRawVariable(engine).VariableValue;
            string result;
            if (rawValue is List<string>)
            {
                result = "LIST";
            }
            else if (rawValue is Dictionary<string, string>)
            {
                result = "DICTIONARY";
            }
            else if (rawValue is DataTable)
            {
                result = "DATATABLE";
            }
            else if (rawValue is Color)
            {
                result = "COLOR";
            }
            else if (rawValue is DateTime)
            {
                result = "DATETIME";
            }
            else if (rawValue is MimeKit.MimeMessage)
            {
                result = "EMAIL";
            }
            else if (rawValue is List<MimeKit.MimeMessage>)
            {
                result = "EMAILLIST";
            }
            else if (rawValue is AutomationElement)
            {
                result = "AUTOMATIONELEMENT";
            }
            else if (rawValue is string)
            {
                result = "BASIC";
            }
            else
            {
                result = "UNKNOWN";
            }
            result.StoreInUserVariable(engine, v_Result);
        }
    }
}