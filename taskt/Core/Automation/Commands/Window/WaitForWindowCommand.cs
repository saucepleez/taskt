﻿using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.SubGruop("Window Actions")]
    [Attributes.ClassAttributes.CommandSettings("Wait For Window To Exist")]
    [Attributes.ClassAttributes.Description("This command waits for a window to exist.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to explicitly wait for a window to exist before continuing script execution.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'ShowWindow' from user32.dll to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class WaitForWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WindowName))]
        public string v_WindowName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_CompareMethod))]
        public string v_SearchMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WaitTime))]
        public string v_LengthToWait { get; set; }

        [XmlIgnore]
        [NonSerialized]
        public ComboBox WindowNameControl;

        public WaitForWindowCommand()
        {
            //this.CommandName = "WaitForWindowCommand";
            //this.SelectionName = "Wait For Window To Exist";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            try
            {
                WindowNameControls.FindWindows(this, nameof(v_WindowName), nameof(v_SearchMethod), nameof(v_LengthToWait), engine);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void Refresh(frmCommandEditor editor)
        {
            base.Refresh();
            WindowNameControl.AddWindowNames();
        }
    }
}