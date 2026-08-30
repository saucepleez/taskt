using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    public abstract class ASeleniumSearchWebElementFromSomethingCommands : ScriptCommand, ISeleniumSearchWebElementParametersProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_SearchMethod))]
        [PropertySelectionChangeEvent(nameof(cmbSearchType_SelectionChangeCommited))]
        [PropertyParameterOrder(6000)]
        public string v_SearchMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_SearchParameter))]
        [PropertyParameterOrder(6100)]
        public string v_SearchParameter { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_SelectionMethod))]
        [PropertyParameterOrder(6200)]
        public string v_SelectionMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_WebElementIndex))]
        [PropertyParameterOrder(6300)]
        public string v_WebElementIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_WaitTimeForWebElement))]
        [PropertyParameterOrder(10000)]
        public string v_WaitTimeForWebElement { get; set; }

        protected void cmbSearchType_SelectionChangeCommited(object sender, EventArgs e)
        {
            var searchType = ((ComboBox)sender).SelectedItem?.ToString().ToLower() ?? "";
            FormUIControls.SetVisibleParameterControlGroup(ControlsList, nameof(v_WebElementIndex), !searchType.StartsWith("find element "));
        }
    }
}
