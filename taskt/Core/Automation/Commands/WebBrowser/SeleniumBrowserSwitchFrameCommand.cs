using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("Actions")]
    [Attributes.ClassAttributes.Description("This command allows you to create a new Selenium web browser session which enables automation for websites.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create a browser that will eventually perform web automation such as checking an internal company intranet site to retrieve data")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserSwitchFrameCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name (ex. myInstance , {{{vInstance}}})")]
        [Attributes.PropertyAttributes.InputSpecification("Signifies a unique name that will represemt the application instance.  This unique name allows you to refer to the instance by name in future commands, ensuring that the commands you specify run against the correct application.")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Browser** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate Frame Selection Type")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Index")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Name or ID")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Parent Frame")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Default Content")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Alert")]
        [Attributes.PropertyAttributes.InputSpecification("Select an option which best fits to the specification you would like to make.")]
        [Attributes.PropertyAttributes.SampleUsage("Select one of the provided options.")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SelectionType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Frame Search Parameter (If Selection Type is 'Index' or 'Name of ID', please enter)")]
        [Attributes.PropertyAttributes.SampleUsage("Index: **0** or **{{{vIndex}}}**, Name/ID: **top** or **{{{vName}}}**")]
        [Attributes.PropertyAttributes.Remarks("If selection type is 'Index', default index is 0.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_FrameParameter { get; set; }

        public SeleniumBrowserSwitchFrameCommand()
        {
            this.CommandName = "SeleniumBrowserSwitchFrameCommand";
            this.SelectionName = "Switch Browser Frame";
            this.v_InstanceName = "";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_SelectionType = "Index";
            this.v_FrameParameter = "0";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            var vInstance = v_InstanceName.ConvertToUserVariable(sender);

            var browserObject = engine.GetAppInstance(vInstance);
            var seleniumInstance = (OpenQA.Selenium.IWebDriver)browserObject;
            var frameIndex = v_FrameParameter.ConvertToUserVariable(sender);
            var selectionType = v_SelectionType.ConvertToUserVariable(sender);

            switch (selectionType)
            {
                case "Index":
                    int intFrameIndex;
                    if (!int.TryParse(frameIndex, out intFrameIndex))
                    {
                        intFrameIndex = 0;
                    }
                    seleniumInstance.SwitchTo().Frame(intFrameIndex);
                    break;
                case "Name or ID":
                    seleniumInstance.SwitchTo().Frame(frameIndex);
                    break;
                case "Parent Frame":
                    seleniumInstance.SwitchTo().ParentFrame();
                    break;
                case "Default Content":
                    seleniumInstance.SwitchTo().DefaultContent();
                    break;
                case "Alert":
                    seleniumInstance.SwitchTo().Alert();
                    break;
                default:
                    throw new NotImplementedException($"Logic to Select Frame '{selectionType}' Not Implemented");
            }
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);


            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_SelectionType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FrameParameter", this, editor));

            if (editor.creationMode == frmCommandEditor.CreationMode.Add)
            {
                this.v_InstanceName = editor.appSettings.ClientSettings.DefaultBrowserInstanceName;
            }

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return $"{base.GetDisplayValue()} - [Find {v_SelectionType}, Instance Name: '{v_InstanceName}']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_InstanceName))
            {
                this.validationResult += "Instance name is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_SelectionType))
            {
                this.validationResult += "Selection Type is empty.\n";
                this.IsValid = false;
            }
            else
            {
                switch (this.v_SelectionType)
                {
                    case "Index":
                        break;

                    case "Name or ID":
                        NameOrIDValidate();
                        break;

                    default:
                        break;
                }
            }

            return this.IsValid;
        }

        private void NameOrIDValidate()
        {
            if (!String.IsNullOrEmpty(this.v_FrameParameter))
            {
                this.validationResult += "Frame Search Parameter is empty.\n";
                this.IsValid = false;
            }
        }
    }
}