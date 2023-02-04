using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("Instance")]
    [Attributes.ClassAttributes.Description("This command returns existance of browser instance.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to close an open instance of Excel.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserCheckBrowserInstanceExistsCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please Enter the instance name (ex. myInstance, {{{vInstance}}})")]
        //[InputSpecification("Enter the unique instance name that was specified in the **Create Browser** command")]
        //[SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        //[Remarks("Failure to enter the correct instance name or failure to first call **Create Broser** command will cause an error")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.WebBrowser)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please select the variable to receive the result")]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("Result is **TRUE** or **FALSE**.")]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Boolean, true)]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [Remarks("When WebBrowser Instance Exists, Result is **True**")]
        public string v_applyToVariableName { get; set; }

        public SeleniumBrowserCheckBrowserInstanceExistsCommand()
        {
            this.CommandName = "SeleniumBrowserCheckBrowserInstanceExistsCommand";
            this.SelectionName = "Check Browser Instance Exists";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            //this.v_InstanceName = "";
        }
        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            //try
            //{
            //    var browserObject = engine.GetAppInstance(vInstance);
            //    var seleniumInstance = (OpenQA.Selenium.IWebDriver)browserObject;
            //    "TRUE".StoreInUserVariable(sender, v_applyToVariableName);
            //}
            //catch
            //{
            //    "FALSE".StoreInUserVariable(sender, v_applyToVariableName);
            //}
            try
            {
                var _ = v_InstanceName.GetSeleniumBrowserInstance(engine);
                true.StoreInUserVariable(engine, v_applyToVariableName);
            }
            catch
            {
                false.StoreInUserVariable(engine, v_applyToVariableName);
            }
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var instanceCtrls = CommandControls.CreateDefaultDropdownGroupFor("v_InstanceName", this, editor);
        //    CommandControls.AddInstanceNames((ComboBox)instanceCtrls.Where(t => (t.Name == "v_InstanceName")).FirstOrDefault(), editor, PropertyInstanceType.InstanceType.WebBrowser);
        //    RenderedControls.AddRange(instanceCtrls);

        //    //create standard group controls
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));

        //    RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
        //    var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
        //    RenderedControls.AddRange(CommandControls.CreateDefaultUIHelpersFor("v_applyToVariableName", this, VariableNameControl, editor));
        //    RenderedControls.Add(VariableNameControl);

        //    if (editor.creationMode == frmCommandEditor.CreationMode.Add)
        //    {
        //        this.v_InstanceName = editor.appSettings.ClientSettings.DefaultBrowserInstanceName;
        //    }

        //    return RenderedControls;
        //}
        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Check Instance Name: '" + v_InstanceName + "', Result In: '" + v_applyToVariableName + "']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_InstanceName))
        //    {
        //        this.validationResult += "Instance is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_applyToVariableName))
        //    {
        //        this.validationResult += "Variable is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}