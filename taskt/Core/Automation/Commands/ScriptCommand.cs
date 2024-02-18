using System;
using System.Xml.Serialization;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    public abstract class ScriptCommand : ILParameterUI
    {
        [XmlAttribute]
        public string CommandID { get; set; }
        [XmlAttribute]
        public string CommandName { get; set; }
        [XmlAttribute]
        public bool IsCommented { get; set; }
        [XmlAttribute]
        public string SelectionName { get; set; }
        [XmlAttribute]
        public int DefaultPause { get; set; }
        [XmlAttribute]
        public int LineNumber { get; set; }
        [XmlAttribute]
        public bool PauseBeforeExeucution { get; set; }
        [XmlIgnore]
        public Color DisplayForeColor { get; set; }

        [XmlAttribute]
        [PropertyDescription("Comment Field")]
        [InputSpecification("Optional field to enter a custom comment which could potentially describe this command or the need for this command, if required")]
        [SampleUsage("I am using this command to ...")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)]
        [PropertyTextBoxSetting(3, true)]
        [PropertyIsOptional(true)]
        [PropertyParameterOrder(int.MaxValue)]
        public string v_Comment { get; set; }
        
        [XmlAttribute]
        public bool CommandEnabled { get; set; }

        [XmlIgnore]
        public bool IsValid { get; protected set; }

        [XmlIgnore]
        public bool IsWarning { get; protected set; }

        [XmlIgnore]
        public string validationResult { get; protected set; }

        [XmlIgnore]
        public bool IsMatched { get; protected set; }

        [XmlIgnore]
        public bool IsDontSavedCommand { get; set; }

        [XmlIgnore]
        public bool IsNewInsertedCommand { get; set; }

        [XmlIgnore]
        public bool CustomRendering { get; set; }

        [XmlIgnore]
        [NonSerialized]
        public List<Control> RenderedControls;

        [XmlIgnore]
        [NonSerialized]
        protected Dictionary<string, Control> _ControlsList;

        [XmlIgnore]
        public Dictionary<string, Control> ControlsList 
        {
            get
            {
                return this._ControlsList;
            }
        }

        public ScriptCommand()
        {
            this.DisplayForeColor = Color.SteelBlue;
            this.CommandEnabled = false;
            this.DefaultPause = 0;
            this.IsCommented = false;
            this.CustomRendering = false;
            this.GenerateID();
            this.IsValid = true;
            this.IsWarning = false;
            this.validationResult = "";
            this.IsMatched = false;
            this.IsDontSavedCommand = false;
            this.IsNewInsertedCommand = false;

            var tp = this.GetType();
            this.CommandName = tp.Name;
            var commandSettings = tp.GetCustomAttribute<CommandSettings>() ?? new CommandSettings();
            this.SelectionName = commandSettings.selectionName;
            this.CommandEnabled = commandSettings.commandEnable;
            this.CustomRendering = commandSettings.customeRender;
        }

        public void GenerateID()
        {
            var id = Guid.NewGuid();
            this.CommandID = id.ToString();
        }

        #region RunCommand

        public virtual void RunCommand(Engine.AutomationEngineInstance engine)
        {
            System.Threading.Thread.Sleep(DefaultPause);
        }
        public virtual void RunCommand(Engine.AutomationEngineInstance engine, Script.ScriptAction command)
        {
            System.Threading.Thread.Sleep(DefaultPause);
        }
        #endregion

        #region GetDisplayValue
        public virtual string GetDisplayValue()
        {
            return DisplayTextControls.GetDisplayText(this);
        }
        #endregion

        #region Render, Refresh, etc

        public virtual List<Control> Render(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            RenderedControls = new List<Control>();

            var attrAutoRender = this.GetType().GetCustomAttribute<EnableAutomateRender>();
            if (attrAutoRender?.enableAutomateRender ?? false)
            {
                RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor, attrAutoRender.forceRenderComment));

                this._ControlsList = new Dictionary<string, Control>();
                foreach (Control control in RenderedControls)
                {
                    this._ControlsList.Add(control.Name, control);
                    if (control is FlowLayoutPanel flp)
                    {
                        foreach (Control c in flp.Controls)
                        {
                            this._ControlsList.Add(c.Name, c);
                        }
                    }
                }

                return RenderedControls;
            }
            else
            {
                return RenderedControls;
            }
        }

        public virtual List<Control> Render()
        {
            RenderedControls = new List<Control>();
            return RenderedControls;
        }
        public virtual void AfterShown(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {

        }

        public virtual void Refresh(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {

        }
        #endregion

        #region Validate
        public virtual void BeforeValidate()
        {
        }

        public virtual bool IsValidate(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            (this.IsValid, this.IsWarning, this.validationResult) = ValidationControls.CheckValidation(this);

            return this.IsValid;
        }
        #endregion

        #region intermediate
        /// <summary>
        /// convert to Intermediate script. this method use default convert method.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="variables"></param>
        public virtual void ConvertToIntermediate(EngineSettings settings, List<Script.ScriptVariable> variables)
        {
            //IntermediateControls.ConvertToIntermediate(this, settings, variables);
            var convertMethods = new Dictionary<string, string>();
            var props = this.GetParameterProperties(true);
            foreach(var prop in props)
            {
                var virtualProp = prop.GetVirtualProperty();
                var methods = PropertyControls.GetCustomAttributeWithVirtual<PropertyIntermediateConvert>(prop, virtualProp) ?? new PropertyIntermediateConvert();

                if (methods.intermediateMethod.Length > 0)
                {
                    convertMethods.Add(prop.Name, methods.intermediateMethod);
                }
            }

            if (convertMethods.Count > 0)
            {
                IntermediateControls.ConvertToIntermediate(this, settings, convertMethods, variables);
            }
            else
            {
                IntermediateControls.ConvertToIntermediate(this, settings, variables);
            }
        }

        /// <summary>
        /// convert to intermediate script. this method enable to specify convert methods.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="convertMethods"></param>
        /// <param name="variables"></param>
        public void ConvertToIntermediate(EngineSettings settings, Dictionary<string, string> convertMethods, List<Script.ScriptVariable> variables)
        {
            IntermediateControls.ConvertToIntermediate(this, settings, convertMethods, variables);
        }

        /// <summary>
        /// convert to raw script. this method use default convert method.
        /// </summary>
        /// <param name="settings"></param>
        public virtual void ConvertToRaw(EngineSettings settings)
        {
            //IntermediateControls.ConvertToRaw(this, settings);
            var convertMethods = new Dictionary<string, string>();
            var props = this.GetParameterProperties(true);
            foreach (var prop in props)
            {
                var virtualProp = prop.GetVirtualProperty();
                var methods = PropertyControls.GetCustomAttributeWithVirtual<PropertyIntermediateConvert>(prop, virtualProp) ?? new PropertyIntermediateConvert();
                
                if (methods.rawMethod.Length > 0)
                {
                    convertMethods.Add(prop.Name, methods.rawMethod);
                }
            }

            if (convertMethods.Count > 0)
            {
                IntermediateControls.ConvertToRaw(this, settings, convertMethods);
            }
            else
            {
                IntermediateControls.ConvertToRaw(this, settings);
            }
        }

        /// <summary>
        /// convert to raw script. this method enable to specify convert methods.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="convertMethods"></param>
        public void ConvertToRaw(EngineSettings settings, Dictionary<string, string> convertMethods)
        {
            IntermediateControls.ConvertToRaw(this, settings, convertMethods);
        }
        #endregion

        public ScriptCommand Clone()
        {
            var newCommand = (ScriptCommand)MemberwiseClone();
            var currentProps = this.GetParameterProperties();

            var newProps = newCommand.GetParameterProperties();

            for (int i = currentProps.Count - 1; i >=0 ; i--)
            {
                var v = currentProps[i].GetValue(this);
                if (v is System.Data.DataTable table)
                {
                    newProps[i].SetValue(newCommand, table.Copy());
                }
            }

            //return (ScriptCommand)MemberwiseClone();
            return newCommand;
        }

        #region Math Replace
        /// <summary>
        /// check parameters value matches
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="caseSensitive"></param>
        /// <param name="checkParameters"></param>
        /// <param name="checkCommandName"></param>
        /// <param name="checkComment"></param>
        /// <param name="checkDisplayText"></param>
        /// <param name="checkInstanceType"></param>
        /// <param name="instanceType"></param>
        /// <returns></returns>
        public bool CheckMatched(string keyword, bool caseSensitive, bool checkParameters, bool checkCommandName, bool checkComment, bool checkDisplayText, bool checkInstanceType, string instanceType)
        {
            this.IsMatched = SearchReplaceControls.CheckMatched(this, keyword, caseSensitive, checkParameters, checkCommandName, checkComment, checkDisplayText, checkInstanceType, instanceType);
            return this.IsMatched;
        }

        /// <summary>
        /// replace parameters value
        /// </summary>
        /// <param name="trg"></param>
        /// <param name="keyword"></param>
        /// <param name="replacedText"></param>
        /// <param name="caseSensitive"></param>
        /// <param name="instanceType"></param>
        /// <returns></returns>
        public bool Replace(SearchReplaceControls.ReplaceTarget trg, string keyword, string replacedText, bool caseSensitive, string instanceType = "")
        {
            this.IsDontSavedCommand =  SearchReplaceControls.Replace(this, trg, keyword, replacedText, caseSensitive, instanceType);
            return this.IsDontSavedCommand;
        }
        #endregion

        #region instance counter

        /// <summary>
        /// general method to Add Instance
        /// </summary>
        /// <param name="counter"></param>
        public virtual void AddInstance(InstanceCounter counter)
        {
            InstanceCounterControls.AddInstance(this, counter);
        }

        /// <summary>
        /// general method to Remove Instance
        /// </summary>
        /// <param name="counter"></param>
        public virtual void RemoveInstance(InstanceCounter counter)
        {
            InstanceCounterControls.RemoveInstance(this, counter);
        }
        #endregion
    }
}