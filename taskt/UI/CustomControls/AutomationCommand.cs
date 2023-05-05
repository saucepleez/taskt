using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands;
using taskt.UI.Forms;

namespace taskt.UI.CustomControls
{
    public class AutomationCommand
    {
        public Type CommandClass { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string DisplayGroup { get; set; }
        public string DisplaySubGroup { get; set; }
        public ScriptCommand Command { get; set; }
        public List<Control> UIControls { get; set; }

        public void RenderUIComponents(frmCommandEditor editorForm)
        {
            if (Command == null)
            {
                throw new InvalidOperationException("Command cannot be null!");
            }

            UIControls = new List<Control>();
            if (Command.CustomRendering)
            {
                var renderedControls = Command.Render(editorForm);

                if (renderedControls.Count == 0)
                {
                    var label = new Label();
                    var theme = editorForm.Theme.ErrorLabel;
                    //label.ForeColor = Color.Red;
                    //label.AutoSize = true;
                    //label.Font = new Font("Segoe UI", 18, FontStyle.Bold);
                    label.Font = new Font(theme.Font, theme.FontSize, theme.Style);
                    label.AutoSize = true;
                    label.ForeColor = theme.FontColor;
                    label.BackColor = theme.BackColor;
                    label.Text = "No Controls are defined for rendering!  If you intend to override with custom controls, you must handle the Render() method of this command!  If you do not wish to override with your own custom controls then set 'CustomRendering' to False.";
                    UIControls.Add(label);
                }
                else
                {
                    foreach (var ctrl in renderedControls)
                    {
                        UIControls.Add(ctrl);
                    }

                    //generate comment command if user did not generate it
                    //var commentControlExists = renderedControls.Any(f => f.Name == "v_Comment");

                    var commentControlExists = renderedControls.Any((ctrl) =>
                    {
                        if (ctrl is FlowLayoutPanel flp)
                        {
                            foreach (Control c in flp.Controls)
                            {
                                if (c.Name == "v_Comment")
                                {
                                    return true;
                                }
                            }
                            return false;
                        }
                        else
                        {
                            return (ctrl.Name == "v_Comment");
                        }
                    });

                    if (!commentControlExists)
                    {
                        UIControls.Add(CommandControls.CreateDefaultLabelFor("v_Comment", Command));
                        UIControls.Add(CommandControls.CreateDefaultInputFor("v_Comment", Command, 100, 300));
                    }
                }
            }
            else
            {
                var label = new Label();
                var theme = editorForm.Theme.ErrorLabel;
                //label.ForeColor = Color.Red;
                //label.AutoSize = true;
                //label.Font = new Font("Segoe UI", 18, FontStyle.Bold);
                label.Font = new Font(theme.Font, theme.FontSize, theme.Style);
                label.AutoSize = true;
                label.ForeColor = theme.FontColor;
                label.BackColor = theme.BackColor;
                label.Text = "Command not enabled for custom rendering!";
                UIControls.Add(label);
            }
        }

        public void Bind(frmCommandEditor editor)
        {
            //preference to preload is false
            //if (UIControls is null)
            //{
            this.RenderUIComponents(editor);
            //}

            foreach (var ctrl in UIControls)
            {

                if (ctrl.DataBindings.Count > 0)
                {
                    var newBindingList = new List<Binding>();
                    foreach (Binding binding in ctrl.DataBindings)
                    {
                        newBindingList.Add(new Binding(binding.PropertyName, Command, binding.BindingMemberInfo.BindingField, false, DataSourceUpdateMode.OnPropertyChanged));
                    }

                    ctrl.DataBindings.Clear();

                    foreach (var newBinding in newBindingList)
                    {
                        ctrl.DataBindings.Add(newBinding);
                    }
                }

                if (ctrl is CommandItemControl control)
                {
                    switch (control.HelperType)
                    {
                        case PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper:
                            control.DataSource = editor.scriptVariables;
                            break;
                        default:
                            break;
                    }
                }

                //if (ctrl is UIPictureBox)
                //{

                //    var typedControl = (UIPictureBox)InputControl;

                //}

                //Todo: helper for loading variables, move to attribute
                if ((ctrl.Name == "v_userVariableName") && (ctrl is ComboBox))
                {
                    var variableCbo = (ComboBox)ctrl;
                    variableCbo.Items.Clear();
                    foreach (var var in editor.scriptVariables)
                    {
                        variableCbo.Items.Add(var.VariableName);
                    }
                }
            }
        }
    }
}
