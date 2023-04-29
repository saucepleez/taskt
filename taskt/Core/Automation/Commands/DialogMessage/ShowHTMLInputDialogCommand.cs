using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dialog/Message Commands")]
    [Attributes.ClassAttributes.CommandSettings("Show HTML Input Dialog")]
    [Attributes.ClassAttributes.Description("Allows the entry of data into a web-enabled form")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want a fancy data collection.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'WebBrowser Control' to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ShowHTMLInputDialogCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_MultiLinesTextBox))]
        [PropertyDescription("HTML for the Dialog")]
        [InputSpecification("HTML", true)]
        [PropertyCustomUIHelper("Launch HTML Builder", nameof(ShowHTMLBuilder))]
        [PropertyValidationRule("HTML", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(false, "")]
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
    <h5><b>Directions:</b> This a sample data collection form that can be presented to a user.  You can add and implement as many fields as you need or choose standard form inputs. Note, each field will require a <b>v_applyToVariable</b> attribute specifying which variable should contain the respective value for the input field.</h5>

    <hr />
    <form>
      <div>
        <div>
          <label for=""inputEmail4"">Email</label>
          <input type=""email"" id=""inputEmail4"" v_applyToVariable=""vInput"" placeholder=""Email"">
        </div>
        <div>
          <label for=""inputPassword4"">Password</label>
          <input type=""password"" id=""inputPassword4"" v_applyToVariable=""vPass"" placeholder=""Password"">
        </div>
      </div>
      <div>
        <label for=""inputAddress"">Address</label>
        <input type=""text"" id=""inputAddress"" v_applyToVariable=""vAddress"" placeholder=""1234 Main St"">
      </div>
      <div>
        <label for=""inputAddress2"">Address 2</label>
        <input type=""text"" id=""inputAddress2"" v_applyToVariable=""vAddress2"" placeholder=""Apartment, studio, or floor"">
      </div>
      <div>
        <div>
          <label for=""inputCity"">City</label>
          <input type=""text"" id=""inputCity"" v_applyToVariable=""vCity"">
        </div>
        <div>
          <label for=""inputState"">State</label>
          <input type=""text"" id=""inputState"" v_applyToVariable=""vState"">
        </div>
        <div>
          <label for=""inputZip"">Zip</label>
          <input type=""text"" id=""inputZip"" v_applyToVariable=""vZip"">
        </div>
      </div>
      <div>
        <div>
          <input type=""checkbox"" id=""gridCheck"" v_applyToVariable=""vCheck"">
          <label for=""gridCheck"">
              Check me out
          </label>
        </div>
      </div>
      <div>
        <label for=""exampleFormControlSelect1"">Example select</label>
        <select id=""exampleFormControlSelect1"" v_applyToVariable=""vSelected"">
          <option>1</option>
          <option>2</option>
          <option>3</option>
          <option>4</option>
          <option>5</option>
        </select>
      </div>
      <p><button onclick=""window.external.Ok();"">Ok</button><br />
      <button onclick=""window.external.Cancel();"">Close</button></p>
    </form>
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
        public string v_ErrorOnClose { get; set; }

        public ShowHTMLInputDialogCommand()
        {
            //this.CommandName = "HTMLInputCommand";
            //this.SelectionName = "Prompt for HTML Input";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            if (engine.tasktEngineUI == null)
            {
                engine.ReportProgress("HTML UserInput Supported With UI Only");
                MessageBox.Show("HTML UserInput Supported With UI Only", "UserInput Command", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //invoke ui for data collection
            var result = engine.tasktEngineUI.Invoke(new Action(() =>
                {
                    //sample for temp testing
                    var htmlInput = v_InputHTML.ConvertToUserVariable(sender);

                    var errorOnClose = this.GetUISelectionValue(nameof(v_ErrorOnClose), engine);

                    var variables = engine.tasktEngineUI.ShowHTMLInput(htmlInput);

                    //if user selected Ok then process variables
                    //null result means user cancelled/closed
                    if (variables != null)
                    {
                        //store each one into context
                        foreach (var variable in variables)
                        {
                            variable.VariableValue.ToString().StoreInUserVariable(sender, variable.VariableName);
                        }
                    }
                    else if (errorOnClose == "Error On Close")
                    {
                        throw new Exception("Input Form was closed by the user");
                    }
                }
            ));
        }

        private void ShowHTMLBuilder(object sender, EventArgs e)
        {
            using (var htmlForm = new UI.Forms.Supplemental.frmHTMLBuilder())
            {
                var htmlInput = (TextBox)ControlsList[nameof(v_InputHTML)];
                htmlForm.rtbHTML.Text = htmlInput.Text;

                if (htmlForm.ShowDialog() == DialogResult.OK)
                {
                    htmlInput.Text = htmlForm.rtbHTML.Text;
                }
            }
        }
    }
}