using MSHTML;
using SHDocVw;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("IE Browser Commands")]
    [Description("This command finds and attaches to an existing IE Web Browser session.")]
    public class IEFindBrowserCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("IE Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Browser** command.")]
        [SampleUsage("IEBrowser || {vIEBrowser}")]
        [Remarks("Failure to enter the correct instance or failure to first call **Create Browser** command will cause an error.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Browser Name (Title)")]
        [InputSpecification("Select the Name (Title) of the IE Browser Instance to get attached to.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_IEBrowserName { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private ComboBox _ieBrowerNameDropdown;

        public IEFindBrowserCommand()
        {
            CommandName = "IEFindBrowserCommand";
            SelectionName = "Find Browser";
            v_InstanceName = "default";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            var instanceName = v_InstanceName.ConvertToUserVariable(sender);

            bool browserFound = false;
            var shellWindows = new ShellWindows();
            foreach (IWebBrowser2 shellWindow in shellWindows)
            {
                if ((shellWindow.Document is HTMLDocument) && (v_IEBrowserName==null || shellWindow.Document.Title == v_IEBrowserName))
                {
                    engine.AddAppInstance(instanceName, shellWindow.Application);
                    browserFound = true;
                    break;
                }
            }

            //try partial match
            if (!browserFound)
            {
                foreach (IWebBrowser2 shellWindow in shellWindows)
                {
                    if ((shellWindow.Document is HTMLDocument) && 
                        ((shellWindow.Document.Title.Contains(v_IEBrowserName) || 
                        shellWindow.Document.Url.Contains(v_IEBrowserName))))
                    {
                        engine.AddAppInstance(instanceName, shellWindow.Application);
                        browserFound = true;
                        break;
                    }
                }
            }

            if (!browserFound)
            {
                throw new Exception("Browser was not found!");
            }
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));

            _ieBrowerNameDropdown = (ComboBox)CommandControls.CreateDropdownFor("v_IEBrowserName", this);
            var shellWindows = new ShellWindows();
            foreach (IWebBrowser2 shellWindow in shellWindows)
            {
                if (shellWindow.Document is HTMLDocument)
                    _ieBrowerNameDropdown.Items.Add(shellWindow.Document.Title);
            }
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_IEBrowserName", this));
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_IEBrowserName", this, new Control[] { _ieBrowerNameDropdown }, editor));
            RenderedControls.Add(_ieBrowerNameDropdown);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Having Title '{v_IEBrowserName}' - Instance Name '{v_InstanceName}']";
        }
    }

}