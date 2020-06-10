using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using taskt.Core;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands;
using taskt.Core.Settings;
using taskt.UI.Forms;
using taskt.UI.Forms.Supplement_Forms;

namespace taskt.UI.CustomControls
{
    public static class CommandControls
    {
        public static frmCommandEditor CurrentEditor { get; set; }

        public static List<Control> CreateDefaultInputGroupFor(string parameterName, ScriptCommand parent, frmCommandEditor editor)
        {
            //Todo: Test
            var controlList = new List<Control>();
            var label = CreateDefaultLabelFor(parameterName, parent);
            var input = CreateDefaultInputFor(parameterName, parent);
            var helpers = CreateUIHelpersFor(parameterName, parent, new Control[] { input }, editor);

            controlList.Add(label);
            controlList.AddRange(helpers);
            controlList.Add(input);

            return controlList;
        }

        public static List<Control> CreateDefaultOutputGroupFor(string parameterName, ScriptCommand parent, frmCommandEditor editor)
        {
            var controlList = new List<Control>();
            var label = CreateDefaultLabelFor(parameterName, parent);
            var variableNameControl = CreateStandardComboboxFor(parameterName, parent).AddVariableNames(editor);
            var helpers = CreateUIHelpersFor(parameterName, parent, new Control[] { variableNameControl }, editor);

            controlList.Add(label);
            controlList.AddRange(helpers);
            controlList.Add(variableNameControl);
            return controlList;
        }

        public static List<Control> CreateDefaultDropdownGroupFor(string parameterName, ScriptCommand parent, frmCommandEditor editor)
        {
            //Todo: Test
            var controlList = new List<Control>();
            var label = CreateDefaultLabelFor(parameterName, parent);
            var input = CreateDropdownFor(parameterName, parent);
            var helpers = CreateUIHelpersFor(parameterName, parent, new Control[] { input }, editor);

            controlList.Add(label);
            controlList.AddRange(helpers);
            controlList.Add(input);

            return controlList;
        }

        public static List<Control> CreateDataGridViewGroupFor(string parameterName, ScriptCommand parent, frmCommandEditor editor)
        {
            var controlList = new List<Control>();
            var label = CreateDefaultLabelFor(parameterName, parent);
            var gridview = CreateDataGridView(parent, parameterName);
            var helpers = CreateUIHelpersFor(parameterName, parent, new Control[] { gridview }, editor);

            controlList.Add(label);
            controlList.AddRange(helpers);
            controlList.Add(gridview);

            return controlList;
        }

        public static Control CreateDefaultLabelFor(string parameterName, ScriptCommand parent)
        {
            var variableProperties = parent.GetType().GetProperties().Where(f => f.Name == parameterName).FirstOrDefault();

            var propertyAttributesAssigned = variableProperties.GetCustomAttributes(typeof(PropertyDescription), true);

            Label inputLabel = new Label();
            if (propertyAttributesAssigned.Length > 0)
            {
                var attribute = (PropertyDescription)propertyAttributesAssigned[0];
                inputLabel.Text = attribute.Description;
            }
            else
            {
                inputLabel.Text = parameterName;
            }

            inputLabel.AutoSize = true;
            inputLabel.Font = new Font("Segoe UI Light", 12);
            inputLabel.ForeColor = Color.White;
            inputLabel.Name = "lbl_" + parameterName;
            CreateDefaultToolTipFor(parameterName, parent, inputLabel);
            return inputLabel;
        }

        public static void CreateDefaultToolTipFor(string parameterName, ScriptCommand parent, Control label)
        {
            var variableProperties = parent.GetType().GetProperties().Where(f => f.Name == parameterName).FirstOrDefault();
            var inputSpecificationAttributesAssigned = variableProperties.GetCustomAttributes(typeof(InputSpecification), true);
            var sampleUsageAttributesAssigned = variableProperties.GetCustomAttributes(typeof(SampleUsage), true);
            var remarksAttributesAssigned = variableProperties.GetCustomAttributes(typeof(Remarks), true);

            string toolTipText = "";
            if (inputSpecificationAttributesAssigned.Length > 0)
            {
                var attribute = (InputSpecification)inputSpecificationAttributesAssigned[0];
                toolTipText = attribute.Specification;
            }
            if (sampleUsageAttributesAssigned.Length > 0)
            {
                var attribute = (SampleUsage)sampleUsageAttributesAssigned[0];
                if (attribute.Usage.Length > 0)
                    toolTipText += "\nSample: " + attribute.Usage;
            }
            if (remarksAttributesAssigned.Length > 0)
            {
                var attribute = (Remarks)remarksAttributesAssigned[0];
                if (attribute.Remark.Length > 0)
                    toolTipText += "\n" + attribute.Remark;
            }

            ToolTip inputToolTip = new ToolTip();
            inputToolTip.ToolTipIcon = ToolTipIcon.Info;
            inputToolTip.IsBalloon = true;
            inputToolTip.ShowAlways = true;
            inputToolTip.ToolTipTitle = label.Text;
            inputToolTip.SetToolTip(label, toolTipText);
        }

        public static Control CreateDefaultInputFor(string parameterName, ScriptCommand parent, int height = 30, int width = 300)
        {
            var inputBox = new TextBox();
            inputBox.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            inputBox.DataBindings.Add("Text", parent, parameterName, false, DataSourceUpdateMode.OnPropertyChanged);
            inputBox.Height = height;
            inputBox.Width = width;

            if (inputBox.Height != 30)
            {
                inputBox.Multiline = true;
            }

            inputBox.Name = parameterName;
            return inputBox;
        }

        public static Control CreateDropdownFor(string parameterName, ScriptCommand parent)
        {
            var inputBox = new ComboBox();
            inputBox.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            inputBox.DataBindings.Add("Text", parent, parameterName, false, DataSourceUpdateMode.OnPropertyChanged);
            inputBox.Height = 30;
            inputBox.Width = 300;
            inputBox.Name = parameterName;

            var variableProperties = parent.GetType().GetProperties().Where(f => f.Name == parameterName).FirstOrDefault();
            var propertyAttributesAssigned = variableProperties.GetCustomAttributes(typeof(PropertyUISelectionOption), true);

            foreach (PropertyUISelectionOption option in propertyAttributesAssigned)
            {
                inputBox.Items.Add(option.UIOption);
            }

            return inputBox;
        }

        public static ComboBox CreateStandardComboboxFor(string parameterName, ScriptCommand parent)
        {
            var inputBox = new ComboBox();
            inputBox.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            inputBox.DataBindings.Add("Text", parent, parameterName, false, DataSourceUpdateMode.OnPropertyChanged);
            inputBox.Height = 30;
            inputBox.Width = 300;
            inputBox.Name = parameterName;

            return inputBox;
        }

        public static List<Control> CreateUIHelpersFor(string parameterName, ScriptCommand parent, Control[] targetControls,
            frmCommandEditor editor)
        {
            var variableProperties = parent.GetType().GetProperties().Where(f => f.Name == parameterName).FirstOrDefault();
            var propertyUIHelpers = variableProperties.GetCustomAttributes(typeof(PropertyUIHelper), true);
            var controlList = new List<Control>();

            if (propertyUIHelpers.Count() == 0)
            {
                return controlList;
            }

            foreach (PropertyUIHelper attrib in propertyUIHelpers)
            {
                CommandItemControl helperControl = new CommandItemControl();
                helperControl.Padding = new Padding(10, 0, 0, 0);
                helperControl.ForeColor = Color.AliceBlue;
                helperControl.Font = new Font("Segoe UI Semilight", 10);
                helperControl.Name = parameterName + "_helper";
                helperControl.Tag = targetControls.FirstOrDefault();
                helperControl.HelperType = attrib.AdditionalHelper;

                switch (attrib.AdditionalHelper)
                {
                    case PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper:
                        //show variable selector
                        helperControl.CommandImage = Images.GetUIImage("VariableCommand");
                        helperControl.CommandDisplay = "Insert Variable";
                        helperControl.Click += (sender, e) => ShowVariableSelector(sender, e);
                        break;

                    case PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper:
                        //show file selector
                        helperControl.CommandImage = Images.GetUIImage("ClipboardGetTextCommand");
                        helperControl.CommandDisplay = "Select a File";
                        helperControl.Click += (sender, e) => ShowFileSelector(sender, e, editor);
                        break;

                    case PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper:
                        //show file selector
                        helperControl.CommandImage = Images.GetUIImage("ClipboardGetTextCommand");
                        helperControl.CommandDisplay = "Select a Folder";
                        helperControl.Click += (sender, e) => ShowFolderSelector(sender, e, editor);
                        break;

                    case PropertyUIHelper.UIAdditionalHelperType.ShowImageRecogitionHelper:
                        //show file selector
                        helperControl.CommandImage = Images.GetUIImage("OCRCommand");
                        helperControl.CommandDisplay = "Capture Reference Image";
                        helperControl.Click += (sender, e) => ShowImageCapture(sender, e);

                        CommandItemControl testRun = new CommandItemControl();
                        testRun.Padding = new Padding(10, 0, 0, 0);
                        testRun.ForeColor = Color.AliceBlue;

                        testRun.CommandImage = Images.GetUIImage("OCRCommand");
                        testRun.CommandDisplay = "Run Image Recognition Test";
                        testRun.ForeColor = Color.AliceBlue;
                        testRun.Tag = targetControls.FirstOrDefault();
                        testRun.Click += (sender, e) => RunImageCapture(sender, e);
                        controlList.Add(testRun);
                        break;

                    case PropertyUIHelper.UIAdditionalHelperType.ShowCodeBuilder:
                        //show variable selector
                        helperControl.CommandImage = Images.GetUIImage("RunScriptCommand");
                        helperControl.CommandDisplay = "Code Builder";
                        helperControl.Click += (sender, e) => ShowCodeBuilder(sender, e, editor);
                        break;

                    case PropertyUIHelper.UIAdditionalHelperType.ShowMouseCaptureHelper:
                        helperControl.CommandImage = Images.GetUIImage("SendMouseMoveCommand");
                        helperControl.CommandDisplay = "Capture Mouse Position";
                        helperControl.ForeColor = Color.AliceBlue;
                        helperControl.Click += (sender, e) => ShowMouseCaptureForm(sender, e);
                        break;
                    case PropertyUIHelper.UIAdditionalHelperType.ShowElementRecorder:
                        //show variable selector
                        helperControl.CommandImage = Images.GetUIImage("ClipboardGetTextCommand");
                        helperControl.CommandDisplay = "Element Recorder";
                        helperControl.Click += (sender, e) => ShowElementRecorder(sender, e, editor);
                        break;
                    case PropertyUIHelper.UIAdditionalHelperType.GenerateDLLParameters:
                        //show variable selector
                        helperControl.CommandImage = Images.GetUIImage("ExecuteDLLCommand");
                        helperControl.CommandDisplay = "Generate Parameters";
                        helperControl.Click += (sender, e) => GenerateDLLParameters(sender, e);
                        break;
                    case PropertyUIHelper.UIAdditionalHelperType.ShowDLLExplorer:
                        //show variable selector
                        helperControl.CommandImage = Images.GetUIImage("ExecuteDLLCommand");
                        helperControl.CommandDisplay = "Launch DLL Explorer";
                        helperControl.Click += (sender, e) => ShowDLLExplorer(sender, e);
                        break;
                    case PropertyUIHelper.UIAdditionalHelperType.AddInputParameter:
                        //show variable selector
                        helperControl.CommandImage = Images.GetUIImage("ExecuteDLLCommand");
                        helperControl.CommandDisplay = "Add Input Parameter";
                        helperControl.Click += (sender, e) => AddInputParameter(sender, e, editor);
                        break;
                    case PropertyUIHelper.UIAdditionalHelperType.ShowHTMLBuilder:
                        helperControl.CommandImage = Images.GetUIImage("ExecuteDLLCommand");
                        helperControl.CommandDisplay = "Launch HTML Builder";
                        helperControl.Click += (sender, e) => ShowHTMLBuilder(sender, e, editor);
                        break;
                    case PropertyUIHelper.UIAdditionalHelperType.ShowIfBuilder:
                        //show variable selector
                        helperControl.CommandImage = Images.GetUIImage("VariableCommand");
                        helperControl.CommandDisplay = "Add New If Statement";
                        break;
                    case PropertyUIHelper.UIAdditionalHelperType.ShowLoopBuilder:
                        //show variable selector
                        helperControl.CommandImage = Images.GetUIImage("VariableCommand");
                        helperControl.CommandDisplay = "Add New Loop Statement";
                        break;

                        //default:
                        //    MessageBox.Show("Command Helper does not exist for: " + attrib.additionalHelper.ToString());
                        //    break;
                }

                controlList.Add(helperControl);
            }

            return controlList;
        }

        public static DataGridView CreateDataGridView(object sourceCommand, string dataSourceName)
        {
            var gridView = new DataGridView();
            gridView.AllowUserToAddRows = true;
            gridView.AllowUserToDeleteRows = true;
            gridView.Size = new Size(400, 250);
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridView.DataBindings.Add("DataSource", sourceCommand, dataSourceName, false, DataSourceUpdateMode.OnPropertyChanged);
            gridView.AllowUserToResizeRows = false;
            return gridView;
        }

        private static void ShowCodeBuilder(object sender, EventArgs e, frmCommandEditor editor)
        {
            //get textbox text
            CommandItemControl commandItem = (CommandItemControl)sender;
            TextBox targetTextbox = (TextBox)commandItem.Tag;

            frmCodeBuilder codeBuilder = new frmCodeBuilder(targetTextbox.Text);

            if (codeBuilder.ShowDialog() == DialogResult.OK)
            {
                targetTextbox.Text = codeBuilder.rtbCode.Text;
            }
        }

        private static void ShowMouseCaptureForm(object sender, EventArgs e)
        {
            frmShowCursorPosition frmShowCursorPos = new frmShowCursorPosition();

            //if user made a successful selection
            if (frmShowCursorPos.ShowDialog() == DialogResult.OK)
            {
                //Todo - ideally one function to add to textbox which adds to class

                //add selected variables to associated control text
                CurrentEditor.flw_InputVariables.Controls["v_XMousePosition"].Text = frmShowCursorPos.XPosition.ToString();
                CurrentEditor.flw_InputVariables.Controls["v_YMousePosition"].Text = frmShowCursorPos.YPosition.ToString();

                //find current command and add to underlying class
                SendMouseMoveCommand cmd = (SendMouseMoveCommand)CurrentEditor.SelectedCommand;
                cmd.v_XMousePosition = frmShowCursorPos.XPosition.ToString();
                cmd.v_YMousePosition = frmShowCursorPos.YPosition.ToString();
            }
        }

        public static void ShowVariableSelector(object sender, EventArgs e)
        {
            //create variable selector form
            frmVariableSelector newVariableSelector = new frmVariableSelector();

            //get copy of user variables and append system variables, then load to combobox
            var variableList = CurrentEditor.ScriptVariables.Select(f => f.VariableName).ToList();
            variableList.AddRange(Common.GenerateSystemVariables().Select(f => f.VariableName));
            newVariableSelector.lstVariables.Items.AddRange(variableList.ToArray());

            //if user pressed "OK"
            if (newVariableSelector.ShowDialog() == DialogResult.OK)
            {
                //ensure that a variable was actually selected
                if (newVariableSelector.lstVariables.SelectedItem == null)
                {
                    //return out as nothing was selected
                    MessageBox.Show("There were no variables selected!");
                    return;
                }

                //grab the referenced input assigned to the 'insert variable' button instance
                CommandItemControl inputBox = (CommandItemControl)sender;
                //currently variable insertion is only available for simply textboxes

                //load settings
                var settings = new ApplicationSettings().GetOrCreateApplicationSettings();

                if (inputBox.Tag is TextBox)
                {
                    TextBox targetTextbox = (TextBox)inputBox.Tag;
                    //concat variable name with brackets [vVariable] as engine searches for the same
                    targetTextbox.Text = targetTextbox.Text + string.Concat(settings.EngineSettings.VariableStartMarker,
                        newVariableSelector.lstVariables.SelectedItem.ToString(), settings.EngineSettings.VariableEndMarker);
                }
                else if (inputBox.Tag is ComboBox)
                {
                    ComboBox targetCombobox = (ComboBox)inputBox.Tag;
                    //concat variable name with brackets [vVariable] as engine searches for the same
                    targetCombobox.Text = targetCombobox.Text + string.Concat(settings.EngineSettings.VariableStartMarker,
                        newVariableSelector.lstVariables.SelectedItem.ToString(), settings.EngineSettings.VariableEndMarker);
                }
                else if (inputBox.Tag is DataGridView)
                {
                    DataGridView targetDGV = (DataGridView)inputBox.Tag;

                    if (targetDGV.SelectedCells.Count == 0)
                    {
                        MessageBox.Show("Please make sure you have selected an action and selected a cell before attempting" +
                            " to insert a variable!", "No Cell Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (targetDGV.SelectedCells[0].ColumnIndex == 0)
                    {
                        MessageBox.Show("Invalid Cell Selected!", "Invalid Cell Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    targetDGV.SelectedCells[0].Value = targetDGV.SelectedCells[0].Value +
                        string.Concat(settings.EngineSettings.VariableStartMarker, newVariableSelector.lstVariables.SelectedItem.ToString(),
                        settings.EngineSettings.VariableEndMarker);
                }


            }
        }

        private static void ShowFileSelector(object sender, EventArgs e, frmCommandEditor editor)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                CommandItemControl inputBox = (CommandItemControl)sender;
                //currently variable insertion is only available for simply textboxes
                TextBox targetTextbox = (TextBox)inputBox.Tag;
                //concat variable name with brackets [vVariable] as engine searches for the same
                targetTextbox.Text = ofd.FileName;
            }
        }

        private static void ShowFolderSelector(object sender, EventArgs e, frmCommandEditor editor)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if(fbd.ShowDialog() == DialogResult.OK)
            {
                CommandItemControl inputBox = (CommandItemControl)sender;
                TextBox targetTextBox = (TextBox)inputBox.Tag;
                targetTextBox.Text = fbd.SelectedPath;
            }
        }

        private static void ShowImageCapture(object sender, EventArgs e)
        {
            ApplicationSettings settings = new ApplicationSettings().GetOrCreateApplicationSettings();
            var minimizePreference = settings.ClientSettings.MinimizeToTray;

            if (minimizePreference)
            {
                settings.ClientSettings.MinimizeToTray = false;
                settings.Save(settings);
            }

            HideAllForms();

            var userAcceptance = MessageBox.Show("The image capture process will now begin and display a screenshot of the" +
                " current desktop in a custom full-screen window.  You may stop the capture process at any time by pressing" +
                " the 'ESC' key, or selecting 'Close' at the top left. Simply create the image by clicking once to start" +
                " the rectangle and clicking again to finish. The image will be cropped to the boundary within the red rectangle." +
                " Shall we proceed?", "Image Capture", MessageBoxButtons.YesNo);

            if (userAcceptance == DialogResult.Yes)
            {
                frmImageCapture imageCaptureForm = new frmImageCapture();

                if (imageCaptureForm.ShowDialog() == DialogResult.OK)
                {
                    CommandItemControl inputBox = (CommandItemControl)sender;
                    UIPictureBox targetPictureBox = (UIPictureBox)inputBox.Tag;
                    targetPictureBox.Image = imageCaptureForm.UserSelectedBitmap;
                    var convertedImage = Common.ImageToBase64(imageCaptureForm.UserSelectedBitmap);
                    var convertedLength = convertedImage.Length;
                    targetPictureBox.EncodedImage = convertedImage;
                    imageCaptureForm.Show();
                }
            }

            ShowAllForms();

            if (minimizePreference)
            {
                settings.ClientSettings.MinimizeToTray = true;
                settings.Save(settings);
            }
        }

        private static void RunImageCapture(object sender, EventArgs e)
        {
            //get input control
            CommandItemControl inputBox = (CommandItemControl)sender;
            UIPictureBox targetPictureBox = (UIPictureBox)inputBox.Tag;
            string imageSource = targetPictureBox.EncodedImage;

            if (string.IsNullOrEmpty(imageSource))
            {
                MessageBox.Show("Please capture an image before attempting to test!");
                return;
            }

            //hide all
            HideAllForms();

            try
            {
                //run image recognition
                ImageRecognitionCommand imageRecognitionCommand = new ImageRecognitionCommand();
                imageRecognitionCommand.v_ImageCapture = imageSource;
                imageRecognitionCommand.TestMode = true;
                imageRecognitionCommand.RunCommand(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
            //show all forms
            ShowAllForms();
        }

        private static void ShowElementRecorder(object sender, EventArgs e, frmCommandEditor editor)
        {
            //get command reference
            UIAutomationCommand cmd = (UIAutomationCommand)editor.SelectedCommand;

            //create recorder
            frmThickAppElementRecorder newElementRecorder = new frmThickAppElementRecorder();
            newElementRecorder.SearchParameters = cmd.v_UIASearchParameters;

            //show form
            newElementRecorder.ShowDialog();

            ComboBox txtWindowName = (ComboBox)editor.flw_InputVariables.Controls["v_WindowName"];
            txtWindowName.Text = newElementRecorder.cboWindowTitle.Text;

            editor.WindowState = FormWindowState.Normal;
            editor.BringToFront();
        }

        private static void GenerateDLLParameters(object sender, EventArgs e)
        {
            ExecuteDLLCommand cmd = (ExecuteDLLCommand)CurrentEditor.SelectedCommand;

            var filePath = CurrentEditor.flw_InputVariables.Controls["v_FilePath"].Text;
            var className = CurrentEditor.flw_InputVariables.Controls["v_ClassName"].Text;
            var methodName = CurrentEditor.flw_InputVariables.Controls["v_MethodName"].Text;
            DataGridView parameterBox = (DataGridView)CurrentEditor.flw_InputVariables.Controls["v_MethodParameters"];

            //clear all rows
            cmd.v_MethodParameters.Rows.Clear();

            //Load Assembly
            try
            {
                Assembly requiredAssembly = Assembly.LoadFrom(filePath);

                //get type
                Type t = requiredAssembly.GetType(className);

                //verify type was found
                if (t == null)
                {
                    MessageBox.Show("The class '" + className + "' was not found in assembly loaded at '" + filePath + "'",
                        "Class Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                //get method
                MethodInfo m = t.GetMethod(methodName);

                //verify method found
                if (m == null)
                {
                    MessageBox.Show("The method '" + methodName + "' was not found in assembly loaded at '" + filePath + "'",
                        "Method Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                //get parameters
                var reqdParams = m.GetParameters();

                if (reqdParams.Length > 0)
                {
                    cmd.v_MethodParameters.Rows.Clear();
                    foreach (var param in reqdParams)
                    {
                        cmd.v_MethodParameters.Rows.Add(param.Name, "");
                    }
                }
                else
                {
                    MessageBox.Show("There are no parameters required for this method!", "No Parameters Required",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error generating the parameters: " + ex.ToString());
            }
        }

        private static void ShowDLLExplorer(object sender, EventArgs e)
        {
            //create form
            frmDLLExplorer dllExplorer = new frmDLLExplorer();

            //show dialog
            if (dllExplorer.ShowDialog() == DialogResult.OK)
            {
                //user accepted the selections
                //declare command
                ExecuteDLLCommand cmd = (ExecuteDLLCommand)CurrentEditor.SelectedCommand;

                //add file name
                if (!string.IsNullOrEmpty(dllExplorer.FileName))
                {
                    CurrentEditor.flw_InputVariables.Controls["v_FilePath"].Text = dllExplorer.FileName;
                }

                //add class name
                if (dllExplorer.lstClasses.SelectedItem != null)
                {
                    CurrentEditor.flw_InputVariables.Controls["v_ClassName"].Text = dllExplorer.lstClasses.SelectedItem.ToString();
                }

                //add method name
                if (dllExplorer.lstMethods.SelectedItem != null)
                {
                    CurrentEditor.flw_InputVariables.Controls["v_MethodName"].Text = dllExplorer.lstMethods.SelectedItem.ToString();
                }

                cmd.v_MethodParameters.Rows.Clear();

                //add parameters
                if ((dllExplorer.lstParameters.Items.Count > 0) &&
                    (dllExplorer.lstParameters.Items[0].ToString() != "This method requires no parameters!"))
                {
                    foreach (var param in dllExplorer.SelectedParameters)
                    {
                        cmd.v_MethodParameters.Rows.Add(param, "");
                    }
                }
            }
        }

        private static void AddInputParameter(object sender, EventArgs e, frmCommandEditor editor)
        {
            DataGridView inputControl = (DataGridView)CurrentEditor.flw_InputVariables.Controls["v_UserInputConfig"];
            var inputTable = (DataTable)inputControl.DataSource;
            var newRow = inputTable.NewRow();
            newRow["Size"] = "500,100";
            inputTable.Rows.Add(newRow);
        }

        private static void ShowHTMLBuilder(object sender, EventArgs e, frmCommandEditor editor)
        {
            var htmlForm = new frmHTMLBuilder();

            RichTextBox inputControl = (RichTextBox)editor.flw_InputVariables.Controls["v_InputHTML"];
            htmlForm.rtbHTML.Text = inputControl.Text;

            if (htmlForm.ShowDialog() == DialogResult.OK)
            {
                inputControl.Text = htmlForm.rtbHTML.Text;
            }
        }

        public static void ShowAllForms()
        {
            foreach (Form frm in Application.OpenForms)
            {
                frm.WindowState = FormWindowState.Normal;
            }
        }

        public static void HideAllForms()
        {
            foreach (Form frm in Application.OpenForms)
            {
                frm.WindowState = FormWindowState.Minimized;
            }
        }

        public static List<AutomationCommand> GenerateCommandsandControls()
        {
            var commandList = new List<AutomationCommand>();

            var commandClasses = Assembly.GetExecutingAssembly().GetTypes()
                                 .Where(t => t.Namespace == "taskt.Core.Automation.Commands")
                                 .Where(t => t.Name != "ScriptCommand")
                                 .Where(t => t.IsAbstract == false)
                                 .Where(t => t.BaseType.Name == "ScriptCommand")
                                 .ToList();

            var userPrefs = new ApplicationSettings().GetOrCreateApplicationSettings();

            //Loop through each class
            foreach (var commandClass in commandClasses)
            {
                var groupingAttribute = commandClass.GetCustomAttributes(typeof(Group), true);
                string groupAttribute = "";
                if (groupingAttribute.Length > 0)
                {
                    var attributeFound = (Group)groupingAttribute[0];
                    groupAttribute = attributeFound.Name;
                }

                //Instantiate Class
                ScriptCommand newCommand = (ScriptCommand)Activator.CreateInstance(commandClass);

                //If command is enabled, pull for display and configuration
                if (newCommand.CommandEnabled)
                {
                    var newAutomationCommand = new AutomationCommand();
                    newAutomationCommand.CommandClass = commandClass;
                    newAutomationCommand.Command = newCommand;
                    newAutomationCommand.DisplayGroup = groupAttribute;
                    newAutomationCommand.FullName = string.Join(" - ", groupAttribute, newCommand.SelectionName);
                    newAutomationCommand.ShortName = newCommand.SelectionName;

                    if (userPrefs.ClientSettings.PreloadBuilderCommands)
                    {
                        //newAutomationCommand.RenderUIComponents();
                    }

                    //call RenderUIComponents to render UI controls
                    commandList.Add(newAutomationCommand);
                }
            }

            return commandList;
        }

        public static ComboBox AddWindowNames(this ComboBox cbo)
        {
            if (cbo == null)
                return null;

            cbo.Items.Clear();
            cbo.Items.Add("Current Window");

            Process[] processlist = Process.GetProcesses();

            //pull the main window title for each
            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    //add to the control list of available windows
                    cbo.Items.Add(process.MainWindowTitle);
                }
            }

            return cbo;
        }

        public static ComboBox AddVariableNames(this ComboBox cbo, frmCommandEditor editor)
        {
            if (cbo == null)
                return null;

            if (editor != null)
            {
                cbo.Items.Clear();

                foreach (var variable in editor.ScriptVariables)
                {
                    cbo.Items.Add(variable.VariableName);
                }
            }

            return cbo;
        }
    }
}
