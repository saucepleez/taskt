using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    /*
     * Abstract class for Window Handle command
     */
    public abstract class AWindowHandleCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_InputWindowHandle))]
        [PropertyParameterOrder(5000)]
        public string v_WindowHandle { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WaitTime))]
        [PropertyParameterOrder(6000)]
        public string v_WaitTime { get; set; }

        public AWindowHandleCommand()
        {
        }
    }
}