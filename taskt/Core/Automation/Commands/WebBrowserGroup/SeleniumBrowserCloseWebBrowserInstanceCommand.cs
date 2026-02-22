using System;
using System.IO;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Instance")]
    [Attributes.ClassAttributes.CommandSettings("Close Web Browser Instance")]
    [Attributes.ClassAttributes.Description("This command allows you to close a Selenium web browser session.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to close and end a web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserCloseWebBrowserInstanceCommand : ASeleniumWebDriverActionCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Delete Profile Folder when Specified")]
        [PropertyUISelectionOption("Recycle Bin")]
        [PropertyIsOptional(true, "Yes")]
        [PropertyValidationRule("Delete Profile", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Delete Profile")]
        [PropertyParameterOrder(7000)]
        public string v_DeleteProfileFolder { get; set; }

        public SeleniumBrowserCloseWebBrowserInstanceCommand()
        {
            //this.CommandName = "SeleniumBrowserCloseCommand";
            //this.SelectionName = "Close Browser";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var vInstance = v_InstanceName.ExpandValueOrUserVariable(engine);
            //var seleniumInstance = v_InstanceName.ExpandValueOrUserVariableAsSeleniumBrowserInstance(engine);

            //seleniumInstance.Quit();
            //seleniumInstance.Dispose();

            //engine.RemoveAppInstance(vInstance);

            this.WebDriverAction(new Action<OpenQA.Selenium.IWebDriver, string>((seleniumInstance, profilePath) =>
            {
                seleniumInstance.Quit();
                seleniumInstance.Dispose();

                var instanceName = this.GetInstanceNameFromWebBrowserInstance(seleniumInstance, engine);
                engine.RemoveAppInstance(instanceName);

                if (!string.IsNullOrEmpty(profilePath))
                {
                    if (Directory.Exists(profilePath))
                    {
                        var isRemove = false;
                        var isRecycle = false;
                        switch(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_DeleteProfileFolder), engine))
                        {
                            case "yes":
                                isRemove = true;
                                break;
                            case "recycle bin":
                                isRemove = true;
                                isRecycle = true;
                                break;
                        }
                        if (isRemove)
                        {
                            var removeFolder = new DeleteFolderCommand()
                            {
                                v_TargetFolderPath = profilePath,
                                v_MoveToRecycleBin = (isRecycle) ? "yes" : "no",
                            };
                            removeFolder.RunCommand(engine);
                        }
                    }
                }
            }), engine);
        }
    }
}