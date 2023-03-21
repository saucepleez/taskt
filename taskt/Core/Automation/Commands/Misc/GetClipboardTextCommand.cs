using System;
using System.Xml.Serialization;
using taskt.Core.Automation.User32;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.SubGruop("Clipboard")]
    [Attributes.ClassAttributes.CommandSettings("Get Clipboard Text")]
    [Attributes.ClassAttributes.Description("This command allows you to get text from the clipboard.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to copy the data from the clipboard and apply it to a variable.  You can then use the variable to extract the value.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against the VariableList from the scripting engine using System.Windows.Forms.Clipboard.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetClipboardTextCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please select a variable to get clipboard contents")]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Clipboard Contents")]
        public string v_userVariableName { get; set; }

        //[XmlIgnore]
        //[NonSerialized]
        //public ComboBox VariableNameControl;

        public GetClipboardTextCommand()
        {
            //this.CommandName = "ClipboardCommand";
            //this.SelectionName = "Get Clipboard Text";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            User32Functions.GetClipboardText().StoreInUserVariable(sender, v_userVariableName);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //create window name helper control
        //    //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_userVariableName", this));
        //    //VariableNameControl = CommandControls.CreateStandardComboboxFor("v_userVariableName", this).AddVariableNames(editor);
        //    //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_userVariableName", this, new Control[] { VariableNameControl }, editor));
        //    //RenderedControls.Add(VariableNameControl);
        //    RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_userVariableName", this, editor));

        //    return RenderedControls;

        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Get Text From Clipboard and Apply to Variable: " + v_userVariableName + "]";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_userVariableName))
        //    {
        //        this.validationResult += "Variable is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}
