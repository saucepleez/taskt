using Newtonsoft.Json;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using taskt.Core.Infrastructure;
using taskt.Core.Script;

namespace taskt.Core.Command
{
    public abstract class ScriptCommand
    {
        public string CommandID { get; set; }
        public string CommandName { get; set; }
        public bool IsCommented { get; set; }
        public string SelectionName { get; set; }
        public int DefaultPause { get; set; }
        public int LineNumber { get; set; }
        public bool PauseBeforeExecution { get; set; }
        public bool CommandEnabled { get; set; }

        [Attributes.PropertyAttributes.PropertyDescription("Private (Optional)")]
        [Attributes.PropertyAttributes.InputSpecification("Optional field to mark the command as private (data sensitive) in order to avoid its logging.")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public bool v_IsPrivate { get; set; }

        [Attributes.PropertyAttributes.PropertyDescription("Comment Field (Optional)")]
        [Attributes.PropertyAttributes.InputSpecification("Optional field to enter a custom comment which could potentially describe this command or the need for this command, if required.")]
        [Attributes.PropertyAttributes.SampleUsage("I am using this command to ...")]
        [Attributes.PropertyAttributes.Remarks("Optional")]
        public string v_Comment { get; set; }

        [JsonIgnore]
        public bool CustomRendering { get; set; }

        [JsonIgnore]
        public Color DisplayForeColor { get; set; }

        [JsonIgnore]
        public List<Control> RenderedControls;
        
        [JsonIgnore]
        public Dictionary<string, object> PropertyValues;

        [JsonIgnore]
        public bool IsSteppedInto { get; set; }

        [JsonIgnore]
        public IfrmScriptBuilder CurrentScriptBuilder { get; set; }

        [JsonIgnore]
        public LogEventLevel LogLevel { get; set; } = LogEventLevel.Information;

        public ScriptCommand()
        {
            DisplayForeColor = Color.SteelBlue;
            CommandEnabled = false;
            DefaultPause = 0;
            IsCommented = false;
            CustomRendering = false;
            GenerateID();
        }

        public void GenerateID()
        {
            var id = Guid.NewGuid();
            CommandID = id.ToString();
        }

        public virtual void RunCommand(object sender)
        {
            System.Threading.Thread.Sleep(DefaultPause);
        }

        public virtual void RunCommand(object sender, ScriptAction command)
        {
            System.Threading.Thread.Sleep(DefaultPause);
        }

        public virtual string GetDisplayValue()
        {
            if (String.IsNullOrEmpty(v_Comment))
            {
                return SelectionName;
            }
            else
            {
                return SelectionName + " [" + v_Comment + "]";
            }
        }

        public virtual List<Control> Render(IfrmCommandEditor editor)
        {
            RenderedControls = new List<Control>();
            return RenderedControls;
        }

        public virtual List<Control> Render()
        {
            RenderedControls = new List<Control>();
            return RenderedControls;
        }

        public virtual void Refresh(IfrmCommandEditor editor = null)
        {

        }
    }
}