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

        public AWindowHandleCommands()
        {
        }
    }
}