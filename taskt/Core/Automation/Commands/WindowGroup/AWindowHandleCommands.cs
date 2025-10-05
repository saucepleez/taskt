using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Abstract class for Window Handle command
    /// </summary>
    [Serializable]
    public abstract class AWindowHandleCommands : ScriptCommand, IWindowHandleProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_InputWindowHandle))]
        [PropertyParameterOrder(5000)]
        public virtual string v_WindowHandle { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WaitTime))]
        [PropertyParameterOrder(6000)]
        public  virtual string v_WaitTimeForWindow { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Window Name")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Window Name", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "Window Name")]
        [PropertyParameterOrder(10000)]
        public virtual string v_WindowNameResult { get; set; }

        //public AWindowHandleCommands()
        //{
        //}
    }
}