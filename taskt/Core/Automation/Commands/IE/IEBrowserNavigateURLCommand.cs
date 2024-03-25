﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Linq;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("IE Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to navigate a IE web browser session to a given URL or resource.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to navigate an existing IE instance to a known URL or web resource")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements the 'InternetExplorer' application object from SHDocVw.dll to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    public class IEBrowserNavigateURLCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create IE Browser** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **IEInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create IE Browser** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.IE)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the URL to navigate to")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the destination URL that you want the IE instance to navigate to")]
        [Attributes.PropertyAttributes.SampleUsage("https://mycompany.com/orders")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_URL { get; set; }

        public IEBrowserNavigateURLCommand()
        {
            this.CommandName = "IEBrowserNavigateURLCommand";
            this.SelectionName = "Navigate to URL";
            this.v_InstanceName = "";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            object browserObject = null;

            var vInstance = v_InstanceName.ExpandValueOrUserVariable(engine);

            browserObject = engine.GetAppInstance(vInstance);

            var browserInstance = (SHDocVw.InternetExplorer)browserObject;

            browserInstance.Navigate(v_URL.ExpandValueOrUserVariable(engine));

            IEBrowserCreateCommand.WaitForReadyState(browserInstance);

        }
        public override List<Control> Render(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            base.Render(editor);

            var instanceCtrls = CommandControls.CreateDefaultDropdownGroupFor("v_InstanceName", this, editor);
            UI.CustomControls.CommandControls.AddInstanceNames((ComboBox)instanceCtrls.Where(t => t.Name == "v_InstanceName").FirstOrDefault(), editor, Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.IE);
            RenderedControls.AddRange(instanceCtrls);
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_URL", this, editor));

            if (editor.creationMode == UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor.CreationMode.Add)
            {
                this.v_InstanceName = editor.appSettings.ClientSettings.DefaultBrowserInstanceName;
            }

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [URL: '" + v_URL + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
}