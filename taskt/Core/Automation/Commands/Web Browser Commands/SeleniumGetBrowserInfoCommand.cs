using Newtonsoft.Json;
using OpenQA.Selenium;
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
    [Group("Web Browser Commands")]
    [Description("This command retrieves information from a Selenium web browser session.")]

    public class SeleniumGetBrowserInfoCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Browser Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Browser** command.")]
        [SampleUsage("MyBrowserInstance || {vBrowserInstance}")]
        [Remarks("Failure to enter the correct instance name or failure to first call the **Create Browser** command will cause an error.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Info Property")]
        [PropertyUISelectionOption("Window Title")]
        [PropertyUISelectionOption("Window URL")]
        [PropertyUISelectionOption("Current Handle ID")]
        [PropertyUISelectionOption("HTML Page Source")]
        [PropertyUISelectionOption("Handle ID List")]
        [InputSpecification("Indicate which info property to retrieve.")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InfoType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Info Variable")]
        [InputSpecification("Select or provide a variable from the variable list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required" +
                 " to pre-define your variables; however, it is highly recommended.")]
        public string v_OutputUserVariableName { get; set; }

        public SeleniumGetBrowserInfoCommand()
        {
            CommandName = "SeleniumGetBrowserInfoCommand";
            SelectionName = "Get Browser Info";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultBrowser";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var browserObject = engine.GetAppInstance(vInstance);
            var seleniumInstance = (IWebDriver)browserObject;
            var requestedInfo = v_InfoType.ConvertToUserVariable(sender);
            string info;

            switch (requestedInfo)
            {
                case "Window Title":
                    info = seleniumInstance.Title;
                    break;
                case "Window URL":
                    info = seleniumInstance.Url;
                    break;
                case "Current Handle ID":
                    info = seleniumInstance.CurrentWindowHandle;
                    break;
                case "HTML Page Source":
                    info = seleniumInstance.PageSource;
                    break;
                case "Handle ID List":
                    info = JsonConvert.SerializeObject(seleniumInstance.WindowHandles);
                    break;
                default:
                    throw new NotImplementedException($"{requestedInfo} is not implemented for lookup.");
            }
            //store data
            info.StoreInUserVariable(sender, v_OutputUserVariableName);
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_InfoType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Get {v_InfoType} - Store Info in '{v_OutputUserVariableName}' - Instance Name '{v_InstanceName}']";
        }
    }
}