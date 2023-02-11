using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("Actions")]
    [Attributes.ClassAttributes.Description("This command allows you to change browser window size.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to change browser window size.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserResizeBrowser : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please Enter the instance name (ex. myInstance, {{{vInstance}}})")]
        //[InputSpecification("Enter the unique instance name that was specified in the **Create Browser** command")]
        //[SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        //[Remarks("Failure to enter the correct instance name or failure to first call **Create Browser** command will cause an error")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.WebBrowser)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }


        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Browser Window Width")]
        [InputSpecification("Width", true)]
        [PropertyDetailSampleUsage("**640**", PropertyDetailSampleUsage.ValueType.Value, "Window Width")]
        [PropertyDetailSampleUsage("**{{{vWidth}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Window Width")]
        [Remarks("Empty means Current Width")]
        [PropertyIsOptional(true, "Empty and means Current Width")]
        [PropertyDisplayText(true, "Width")]
        public string v_BrowserWidth { get; set; }


        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Browser Window Height")]
        [InputSpecification("Height", true)]
        [PropertyDetailSampleUsage("**480**", PropertyDetailSampleUsage.ValueType.Value, "Window Height")]
        [PropertyDetailSampleUsage("**{{{vHeight}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Window Height")]
        [Remarks("Empty means Current Height")]
        [PropertyIsOptional(true, "Empty and means Current Height")]
        [PropertyDisplayText(true, "Height")]
        public string v_BrowserHeight { get; set; }

        public SeleniumBrowserResizeBrowser()
        {
            this.CommandName = "SeleniumBrowserResizeBrowserCommand";
            this.SelectionName = "Resize Browser";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            //var browserObject = engine.GetAppInstance(vInstance);
            //var seleniumInstance = (IWebDriver)browserObject;
            var seleniumInstance = v_InstanceName.GetSeleniumBrowserInstance(engine);

            var currentSize = seleniumInstance.Manage().Window.Size;

            int width;
            if (String.IsNullOrEmpty(v_BrowserWidth))
            {
                width = currentSize.Width;
            }
            else
            {
                //width = int.Parse(v_BrowserWidth.ConvertToUserVariable(sender));
                width = this.ConvertToUserVariableAsInteger(nameof(v_BrowserWidth), engine);
            }

            int height;
            if (String.IsNullOrEmpty(v_BrowserHeight))
            {
                height = currentSize.Height;
            }
            else
            {
                //height = int.Parse(v_BrowserHeight.ConvertToUserVariable(sender));
                height = this.ConvertToUserVariableAsInteger(nameof(v_BrowserHeight), engine);
            }
            seleniumInstance.Manage().Window.Size = new System.Drawing.Size(width, height);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctrls);

        //    if (editor.creationMode == frmCommandEditor.CreationMode.Add)
        //    {
        //        this.v_InstanceName = editor.appSettings.ClientSettings.DefaultBrowserInstanceName;
        //    }

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_InstanceName))
        //    {
        //        this.validationResult += "Instance name is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}