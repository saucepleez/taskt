using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dialog/Message")]
    [Attributes.ClassAttributes.CommandSettings("Show HTML Input Dialog")]
    [Attributes.ClassAttributes.Description("Allows the entry of data into a web-enabled form")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want a fancy data collection.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'WebBrowser Control' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_input))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ShowHTMLInputDialogCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_MultiLinesTextBox))]
        [PropertyDescription("HTML for the Dialog")]
        [InputSpecification("HTML", true)]
        [PropertyCustomUIHelper("Launch HTML Builder", nameof(ShowHTMLBuilder))]
        [PropertyValidationRule("HTML", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(false, "HTML")]
        [PropertyFirstValue(
@"<!DOCTYPE html>
<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"">
<head>
  <meta charset=""utf-8"" />
  <title>Please Provide Information</title>
</head>
<body>
  <nav>
      <h1>taskt</h1>
      <small>free and open-source process automation</small>
  </nav>
  <br />
  <div>
<h1>Directions</h1>
<p>This a sample data collection form that can be presented to a user.  You can add and implement as many fields as you need or choose standard form inputs. Note, each field will require a <b>data-variable</b> or <b>v_applyToVariable</b> attribute specifying which variable should contain the respective value for the input field.<br>
<b>DO NOT USE</b> form tags to enclose the element. The variable information will not be retrieved correctly.</p>
<p>The <b>OK</b> button should call <b>chrome.webview.hostObjects.fm.OK();</b> with onclick attribute, etc.<br>
Similarly, The <b>Cancel</b> button should call <b>chrome.webview.hostObjects.fm.Cancel();</b> with onclick attribute, etc.</p> 
    <hr />
      <div>
        <div>
          <label for=""inputEmail4"">Email</label>
          <input type=""email"" id=""inputEmail4"" data-variable=""vInput"" placeholder=""Email"">
        </div>
        <div>
          <label for=""inputPassword4"">Password</label>
          <input type=""password"" id=""inputPassword4"" data-variable=""vPass"" placeholder=""Password"">
        </div>
      </div>
      <div>
        <label for=""inputAddress"">Address</label>
        <input type=""text"" id=""inputAddress"" data-variable=""vAddress"" placeholder=""1234 Main St"">
      </div>
      <div>
        <label for=""inputAddress2"">Address 2</label>
        <input type=""text"" id=""inputAddress2"" data-variable=""vAddress2"" placeholder=""Apartment, studio, or floor"">
      </div>
      <div>
        <div>
          <label for=""inputCity"">City</label>
          <input type=""text"" id=""inputCity"" data-variable=""vCity"">
        </div>
        <div>
          <label for=""inputState"">State</label>
          <input type=""text"" id=""inputState"" data-variable=""vState"">
        </div>
        <div>
          <label for=""inputZip"">Zip</label>
          <input type=""text"" id=""inputZip"" data-variable=""vZip"">
        </div>
      </div>
      <div>
        <div>
          <input type=""checkbox"" id=""gridCheck"" data-variable=""vCheck"">
          <label for=""gridCheck"">
              Check me out
          </label>
        </div>
      </div>
      <div>
        <label for=""exampleFormControlSelect1"">Example select</label>
        <select id=""exampleFormControlSelect1"" data-variable=""vSelected"">
          <option>1</option>
          <option>2</option>
          <option>3</option>
          <option>4</option>
          <option>5</option>
        </select>
      </div>
      <div>
        <p>Free input area</p>
        <textarea id=""freeInput"" data-variable=""vFree""></textarea>
      </div>
      <p><button onclick=""chrome.webview.hostObjects.fm.OK();"">Ok</button><br />
      <button onclick=""chrome.webview.hostObjects.fm.Cancel();"">Close</button></p>
  </div>
</body>
</html>")]
        public string v_InputHTML { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("When an Error should Occur on any Result other than 'OK'")]
        [PropertyUISelectionOption("Error On Close")]
        [PropertyUISelectionOption("Do Not Error On Close")]
        [PropertyIsOptional(true, "Error On Close")]
        [PropertyDisplayText(false, "Error")]
        public string v_ErrorOnClose { get; set; }

        public ShowHTMLInputDialogCommand()
        {
            //this.CommandName = "HTMLInputCommand";
            //this.SelectionName = "Prompt for HTML Input";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            if (engine.tasktEngineUI == null)
            {
                engine.ReportProgress("HTML UserInput Supported With UI Only");
                MessageBox.Show("HTML UserInput Supported With UI Only", "UserInput Command", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //// invoke ui for data collection
            //var result = engine.tasktEngineUI.Invoke(new Action(() =>
            //{
            //    // sample for temp testing
            //    var htmlInput = v_InputHTML.ExpandValueOrUserVariable(engine);

            //    var errorOnClose = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ErrorOnClose), engine);

            //    var variables = engine.tasktEngineUI.ShowHTMLInput(htmlInput);

            //    // if user selected Ok then process variables
            //    // null result means user cancelled/closed
            //    if (variables != null)
            //    {
            //        ////store each one into context
            //        //foreach (var variable in variables)
            //        //{
            //        //    variable.VariableValue.ToString().StoreInUserVariable(engine, variable.VariableName);
            //        //}

            //        Action<ScriptVariable> newVariableAction;
            //        if (engine.engineSettings.CreateMissingVariablesDuringExecution)
            //        {
            //            newVariableAction = new Action<ScriptVariable>((v) =>
            //            {
            //                engine.VariableList.Add(v);
            //            });
            //        }
            //        else
            //        {
            //            newVariableAction = new Action<ScriptVariable>((v) => {
            //                // nothing
            //            });
            //        }

            //        foreach(var v in variables)
            //        {
            //            var existsVar = engine.VariableList.FirstOrDefault(t => v.VariableName == t.VariableName);
            //            if (existsVar != null)
            //            {
            //                existsVar.VariableValue = v.VariableValue;
            //            }
            //            else
            //            {
            //                newVariableAction(v);
            //            }
            //        }
            //    }
            //    else if (errorOnClose == "Error On Close")
            //    {
            //        throw new Exception("Input Form was closed by the user");
            //    }
            //}));

            engine.tasktEngineUI.Invoke(new Action(() =>
            {
                // sample for temp testing
                var htmlInput = v_InputHTML.ExpandValueOrUserVariable(engine);

                var errorOnClose = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ErrorOnClose), engine);

                List<ScriptVariable> variables = null;
                using (var fm = new UI.Forms.ScriptEngine.Supplemental.frmHTMLDisplayForm())
                {
                    fm.TemplateHTML = htmlInput;

                    var dialogResult = fm.ShowDialog();

                    if (fm.Result == DialogResult.OK)
                    {
                        variables = fm.VariablesList;
                    }
                }

                // if user selected Ok then process variables
                // null result means user cancelled/closed
                if (variables != null)
                {
                    Action<ScriptVariable> newVariableAction;
                    if (engine.engineSettings.CreateMissingVariablesDuringExecution)
                    {
                        newVariableAction = new Action<ScriptVariable>((v) =>
                        {
                            engine.VariableList.Add(v);
                        });
                    }
                    else
                    {
                        newVariableAction = new Action<ScriptVariable>((v) => {
                            // nothing
                        });
                    }

                    foreach (var v in variables)
                    {
                        var existsVar = engine.VariableList.FirstOrDefault(t => v.VariableName == t.VariableName);
                        if (existsVar != null)
                        {
                            existsVar.VariableValue = v.VariableValue;
                        }
                        else
                        {
                            newVariableAction(v);
                        }
                    }
                }
                else if (errorOnClose == "Error On Close")
                {
                    throw new Exception("Input Form was closed by the user");
                }
            }));
        }

        private void ShowHTMLBuilder(object sender, EventArgs e)
        {
            using (var htmlForm = new UI.Forms.ScriptBuilder.CommandEditor.Supplemental.frmHTMLBuilder())
            {
                var htmlInput = (TextBox)ControlsList[nameof(v_InputHTML)];
                htmlForm.rtbHTML.Text = htmlInput.Text;

                if (htmlForm.ShowDialog(((Control)sender).FindForm()) == DialogResult.OK)
                {
                    htmlInput.Text = htmlForm.rtbHTML.Text;
                }
            }
        }
    }
}