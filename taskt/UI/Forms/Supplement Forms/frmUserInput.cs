using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taskt.UI.Forms.Supplemental
{
    public partial class frmUserInput : UIForm
    {
        public Core.AutomationCommands.UserInputCommand InputCommand { get; set; }
        public List<Control> InputControls;
        public frmUserInput()
        {
            InitializeComponent();
        }

        private void frmUserInput_Load(object sender, EventArgs e)
        {

            InputControls = new List<Control>();


            //get presentation data from command
            this.lblHeader.Text = InputCommand.v_InputHeader;
            this.lblDirections.Text = InputCommand.v_InputDirections;

            //get input table
            var inputTable = InputCommand.v_UserInputConfig;

            //loop each data collection point
            foreach (DataRow rw in inputTable.Rows)
            {
                //get properties to render controls with
                var fieldType = rw["Type"] as string;
                var fieldLabel = rw["Label"] as string;
                var fieldSize = rw["Size"] as string;

                //attempt to parse custom width/height
                int fieldWidth, fieldHeight;
                try
                {
                    //format should be X,Y
                    var fieldSizeData = fieldSize.Split(',');
                    fieldWidth = int.Parse(fieldSizeData[0].Trim());
                    fieldHeight = int.Parse(fieldSizeData[1].Trim());
                }
                catch (Exception)
                {
                    //if something goes wrong just use defaults
                    fieldWidth = 500;
                    fieldHeight = 100;
                }

                //get default value
                var defaultFieldValue = rw["DefaultValue"] as string;

                var label = new Label();
                label.AutoSize = true;
                label.ForeColor = Color.SteelBlue;
                label.Font = lblDirections.Font;
                label.Text = fieldLabel;
                flwInputControls.Controls.Add(label);

                switch (fieldType)
                {
                    //add more cases here
                    default:
                        var textBox = new TextBox();
                        textBox.Multiline = true;
                        textBox.Width = fieldWidth;
                        textBox.Height = fieldHeight;
                        textBox.Margin = new Padding(10,5,0,0);
                        textBox.Text = defaultFieldValue;
                        textBox.Font = label.Font;
                        textBox.ForeColor = Color.SteelBlue;
                        InputControls.Add(textBox);
                        flwInputControls.Controls.Add(textBox);
                        break;
                }




            }



        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }

    public class UserInput
    {
        public Control RenderedControl { get; set; }
    }
}
