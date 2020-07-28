using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using taskt.Commands;
using taskt.Core.Script;
using taskt.UI.CustomControls.CustomUIControls;
using taskt.UI.Forms.Supplement_Forms;
using taskt.Utilities;
using VBFileSystem = Microsoft.VisualBasic.FileIO.FileSystem;

namespace taskt.UI.Forms.ScriptBuilder_Forms
{
    public partial class frmScriptBuilder : Form
    {

        #region Project Tool Strip, Buttons and Pane
        private void addProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddProject();
        }

        private void uiBtnProject_Click(object sender, EventArgs e)
        {
            AddProject();
        }

        public void AddProject()
        {
            tvProject.Nodes.Clear();
            var projectBuilder = new frmProjectBuilder();
            projectBuilder.ShowDialog();

            //Close taskt if add project form is closed at startup
            if (projectBuilder.DialogResult == DialogResult.Cancel && _scriptProject == null)
            {
                Application.Exit();
                return;
            }

            //Create new taskt project
            else if (projectBuilder.CreateProject == true)
            {
                DialogResult result = CheckForUnsavedScripts();
                if (result == DialogResult.Cancel)
                    return;
              
                uiScriptTabControl.TabPages.Clear();
                _scriptProjectPath = projectBuilder.NewProjectPath;

                string mainScriptPath = Path.Combine(_scriptProjectPath, "Main.json");
                string mainScriptName = Path.GetFileNameWithoutExtension(mainScriptPath);
                UIListView mainScriptActions = NewLstScriptActions(mainScriptName);
                List<ScriptVariable> mainScriptVariables = new List<ScriptVariable>();
                List<ScriptElement> mainScriptElements = new List<ScriptElement>();
                ShowMessageCommand helloWorldCommand = new ShowMessageCommand();

                helloWorldCommand.v_Message = "Hello World";
                mainScriptActions.Items.Insert(0, CreateScriptCommandListViewItem(helloWorldCommand));

                //Begin saving as main.xml
                ClearSelectedListViewItems();

                try
                {
                    //Serialize main script
                    var mainScript = Script.SerializeScript(mainScriptActions.Items, mainScriptVariables, mainScriptElements,
                                                            mainScriptPath, projectBuilder.NewProjectName);                  
                    //Create new project
                    Project proj = new Project(projectBuilder.NewProjectName);
                    _mainFileName = proj.Main;
                    //Save new project
                    proj.SaveProject(mainScriptPath, mainScript, _mainFileName);
                    //Open new project
                    _scriptProject = Project.OpenProject(mainScriptPath);
                    //Open main script
                    OpenFile(mainScriptPath);
                    ScriptFilePath = mainScriptPath;
                    //Show success dialog
                    Notify("Project has been created successfully!");
                }
                catch (Exception ex)
                {
                    Notify("An Error Occured: " + ex.Message);
                }
            }

            //Open existing taskt project
            else if (projectBuilder.OpenProject == true)
            {
                DialogResult result = CheckForUnsavedScripts();
                if (result == DialogResult.Cancel)
                    return;

                try
                {
                    //Open project
                    _scriptProject = Project.OpenProject(projectBuilder.ExistingMainPath);
                    _mainFileName = _scriptProject.Main;

                    if (Path.GetFileName(projectBuilder.ExistingMainPath) != _mainFileName)
                        throw new Exception("Attempted to open project from a script that isn't Main");

                    _scriptProjectPath = Path.GetDirectoryName(projectBuilder.ExistingMainPath);
                    uiScriptTabControl.TabPages.Clear();
                    //Open Main
                    OpenFile(projectBuilder.ExistingMainPath);
                    //show success dialog
                    Notify("Project has been opened successfully!");
                }
                catch (Exception ex)
                {
                    //show fail dialog
                    Notify("An Error Occured: " + ex.Message);
                    //Try adding project again
                    AddProject();
                    return;
                }
            }

            DirectoryInfo projectDirectoryInfo = new DirectoryInfo(_scriptProjectPath);
            TreeNode projectNode = new TreeNode(projectDirectoryInfo.Name);
            projectNode.Text = projectDirectoryInfo.Name;
            projectNode.Tag = projectDirectoryInfo.FullName;
            projectNode.Nodes.Add("Empty");
            projectNode.ContextMenuStrip = cmsProjectMainFolderActions;          
            tvProject.Nodes.Add(projectNode);
            projectNode.Expand();
        }

        private void LoadChildren(TreeNode parentNode, string directory)
        {
            DirectoryInfo parentDirectoryInfo = new DirectoryInfo(directory);
            try
            {
                foreach (DirectoryInfo childDirectoryInfo in parentDirectoryInfo.GetDirectories())
                {
                    if (childDirectoryInfo.Attributes != FileAttributes.Hidden)
                        NewNode(parentNode, childDirectoryInfo.FullName, "folder");
                }
                foreach (FileInfo fileInfo in parentDirectoryInfo.GetFiles())
                {
                    if (fileInfo.Attributes != FileAttributes.Hidden)
                        NewNode(parentNode, fileInfo.FullName, "file");
                }
            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message);
            }
        }

        private void NewNode(TreeNode parentNode, string childPath, string type)
        {
            if (type == "folder")
            {
                DirectoryInfo childDirectoryInfo = new DirectoryInfo(childPath);

                TreeNode innerFolderNode = new TreeNode(childDirectoryInfo.Name);
                innerFolderNode.Name = childDirectoryInfo.Name;
                innerFolderNode.Text = childDirectoryInfo.Name;
                innerFolderNode.Tag = childDirectoryInfo.FullName;
                innerFolderNode.Nodes.Add("Empty");
                innerFolderNode.ContextMenuStrip = cmsProjectFolderActions;
                innerFolderNode.ImageIndex = 0; //folder icon
                innerFolderNode.SelectedImageIndex = 0;
                parentNode.Nodes.Add(innerFolderNode);
            }
            else if (type == "file")
            {
                FileInfo childFileInfo = new FileInfo(childPath);

                TreeNode fileNode = new TreeNode(childFileInfo.Name);
                fileNode.Name = childFileInfo.Name;
                fileNode.Text = childFileInfo.Name;
                fileNode.Tag = childFileInfo.FullName;
                
                if (fileNode.Name != _mainFileName && fileNode.Name != "project.config")
                    fileNode.ContextMenuStrip = cmsProjectFileActions;

                if (fileNode.Tag.ToString().ToLower().Contains(".json"))
                {
                    fileNode.ImageIndex = 1; //script file icon
                    fileNode.SelectedImageIndex = 1;
                }
                else if (fileNode.Tag.ToString().ToLower().Contains(".xlsx") ||
                         fileNode.Tag.ToString().ToLower().Contains(".csv"))
                {
                    fileNode.ImageIndex = 3; //excel file icon
                    fileNode.SelectedImageIndex = 3;
                }
                else if (fileNode.Tag.ToString().ToLower().Contains(".docx"))
                {
                    fileNode.ImageIndex = 4; //word file icon
                    fileNode.SelectedImageIndex = 4;
                }
                else if (fileNode.Tag.ToString().ToLower().Contains(".pdf"))
                {
                    fileNode.ImageIndex = 5; //pdf file icon
                    fileNode.SelectedImageIndex = 5;
                }
                else
                {
                    fileNode.ImageIndex = 2; //default file icon
                    fileNode.SelectedImageIndex = 2;
                }

                parentNode.Nodes.Add(fileNode);
            }
        }
        #endregion

        #region Project TreeView Events
        private void tvProject_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (Directory.Exists(e.Node.Tag.ToString()))
            {
                e.Node.Nodes.Clear();
                LoadChildren(e.Node, e.Node.Tag.ToString());
            }
            else
                e.Cancel = true;
        }

        private void tvProject_DoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (IsScriptRunning)
                return;

            if (e == null || e.Button == MouseButtons.Left)
            {
                try
                {
                    string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
                    string currentOpenScriptFilePath = _scriptFilePath;

                    if (File.Exists(selectedNodePath) && selectedNodePath.ToLower().Contains(".json"))
                        OpenFile(selectedNodePath);
                    else if (File.Exists(selectedNodePath))
                        Process.Start(selectedNodePath);
                }
                catch (Exception ex)
                {
                    Notify("An Error Occured: " + ex.Message);
                }
            }
        }

        private void tvProject_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            tvProject.SelectedNode = e.Node;
        }

        private void tvProject_KeyDown(object sender, KeyEventArgs e)
        {
            string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
            bool isFolder;
            if (Directory.Exists(selectedNodePath))
                isFolder = true;
            else
                isFolder = false;
            if (e.KeyCode == Keys.Delete && isFolder)
                tsmiDeleteFolder_Click(sender, e);
            else if (e.KeyCode == Keys.Delete && !isFolder)
                tsmiDeleteFile_Click(sender, e);
            else if (e.KeyCode == Keys.Enter && !isFolder)
                tvProject_DoubleClick(sender, null);
            else if (e.Control)
            {
                if (e.KeyCode == Keys.C)
                    tsmiCopyFolder_Click(sender, e);
                if (e.KeyCode == Keys.V)
                    tsmiPasteFolder_Click(sender, e);
            }
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
        #endregion

        #region Project Folder Context Menu Strip
        private void tsmiCopyFolder_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
                Clipboard.SetData(DataFormats.Text, selectedNodePath);
            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message);
            }
        }

        private void tsmiDeleteFolder_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
                string selectedNodeName = tvProject.SelectedNode.Text.ToString();
                if (selectedNodeName != _scriptProject.ProjectName)
                {
                    DialogResult result = MessageBox.Show($"Are you sure you would like to delete {selectedNodeName}?",
                                                 $"Delete {selectedNodeName}", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        if (Directory.Exists(selectedNodePath))
                        {
                            Directory.Delete(selectedNodePath, true);
                            tvProject.Nodes.Remove(tvProject.SelectedNode);
                        }
                        else
                            throw new FileNotFoundException();
                    }
                }
                else
                {
                    throw new Exception($"Cannot delete {selectedNodeName}");
                }
            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message);
            }
        }

        private void tsmiNewFolder_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
                string newFolderPath = Path.Combine(selectedNodePath, "New folder");

                if (!Directory.Exists(newFolderPath))
                {
                    Directory.CreateDirectory(newFolderPath);
                    DirectoryInfo newDirectoryInfo = new DirectoryInfo(newFolderPath);
                    NewNode(tvProject.SelectedNode, newFolderPath, "folder");
                }
                else
                {
                    int count = 1;
                    string newerFolderPath = newFolderPath;
                    while (Directory.Exists(newerFolderPath))
                    {
                        newerFolderPath = $"{newFolderPath} ({count})";
                        count += 1;
                    }
                    Directory.CreateDirectory(newerFolderPath);
                    DirectoryInfo newDirectoryInfo = new DirectoryInfo(newerFolderPath);

                    NewNode(tvProject.SelectedNode, newerFolderPath, "folder");
                }
            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message);
            }
        }

        private void tsmiPasteFolder_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
                string copiedNodePath = Clipboard.GetData(DataFormats.Text).ToString();

                if (Directory.Exists(copiedNodePath))
                {
                    DirectoryInfo copiedNodeDirectoryInfo = new DirectoryInfo(copiedNodePath);

                    if (Directory.Exists(Path.Combine(selectedNodePath, copiedNodeDirectoryInfo.Name)))
                        throw new Exception("A directory with this name already exists in this location");

                    else if (copiedNodePath == _scriptProjectPath)
                        throw new Exception("The project directory cannot be copied or moved");

                    else
                    {
                        VBFileSystem.CopyDirectory(copiedNodePath, Path.Combine(selectedNodePath, copiedNodeDirectoryInfo.Name));
                        NewNode(tvProject.SelectedNode, copiedNodePath, "folder");
                    }
                }
                else if (File.Exists(copiedNodePath))
                {
                    FileInfo copiedNodeFileInfo = new FileInfo(copiedNodePath);

                    if (File.Exists(Path.Combine(selectedNodePath, copiedNodeFileInfo.Name)))
                        throw new Exception("A file with this name already exists in this location");

                    else if (copiedNodeFileInfo.Name == _mainFileName || copiedNodeFileInfo.Name == "project.config")
                        throw new Exception("This file cannot be copied or moved");

                    else
                    {
                        File.Copy(copiedNodePath, Path.Combine(selectedNodePath, copiedNodeFileInfo.Name));
                        NewNode(tvProject.SelectedNode, copiedNodePath, "file");
                    }
                }
                else
                    throw new Exception("Attempted to paste something that isn't a file or folder");

            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message);
            }
        }

        private void tsmiRenameFolder_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
                if (selectedNodePath != _scriptProjectPath)
                {
                    DirectoryInfo selectedNodeDirectoryInfo = new DirectoryInfo(selectedNodePath);

                    string newName = "";
                    var newNameForm = new frmInputBox("Enter the new name of the folder", "Rename Folder");
                    newNameForm.ShowDialog();

                    if (newNameForm.DialogResult == DialogResult.OK)
                        newName = newNameForm.txtInput.Text;
                    else if (newNameForm.DialogResult == DialogResult.Cancel)
                        return;

                    string newPath = Path.Combine(selectedNodeDirectoryInfo.Parent.FullName, newName);
                    bool isInvalidProjectName = new[] { @"/", @"\" }.Any(c => newName.Contains(c));

                    if (isInvalidProjectName)
                        throw new Exception("Illegal characters in path");

                    if (Directory.Exists(newPath))
                        throw new Exception("A folder with this name already exists");

                    FileSystem.Rename(selectedNodePath, newPath);
                    tvProject.SelectedNode.Name = newName;
                    tvProject.SelectedNode.Text = newName;
                    tvProject.SelectedNode.Tag = newPath;
                }
            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message);
            }

        }
        private void tsmiNewScriptFile_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
                string newFilePath = Path.Combine(selectedNodePath, "New Script.json");
                UIListView newScriptActions = NewLstScriptActions();
                List<ScriptVariable> newScripVariables = new List<ScriptVariable>();
                List<ScriptElement> newScriptElements = new List<ScriptElement>();
                var helloWorldCommand = new ShowMessageCommand();
                helloWorldCommand.v_Message = "Hello World";
                newScriptActions.Items.Insert(0, CreateScriptCommandListViewItem(helloWorldCommand));

                if (!File.Exists(newFilePath))
                {
                    Script.SerializeScript(newScriptActions.Items, newScripVariables, newScriptElements, newFilePath, _scriptProject.ProjectName);
                    NewNode(tvProject.SelectedNode, newFilePath, "file");
                    OpenFile(newFilePath);
                }
                else
                {
                    int count = 1;
                    string newerFilePath = newFilePath;
                    while (File.Exists(newerFilePath))
                    {
                        string newDirectoryPath = Path.GetDirectoryName(newFilePath);
                        string newFileNameWithoutExtension = Path.GetFileNameWithoutExtension(newFilePath);
                        newerFilePath = Path.Combine(newDirectoryPath, $"{newFileNameWithoutExtension} ({count}).json");
                        count += 1;
                    }
                    Script.SerializeScript(newScriptActions.Items, newScripVariables, newScriptElements, newerFilePath, _scriptProject.ProjectName);
                    NewNode(tvProject.SelectedNode, newerFilePath, "file");
                    OpenFile(newerFilePath);
                }

            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message);
            }
        }
        #endregion

        #region Project File Context Menu Strip
        private void tsmiCopyFile_Click(object sender, EventArgs e)
        {
            tsmiCopyFolder_Click(sender, e);
        }

        private void tsmiDeleteFile_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
                string selectedNodeName = tvProject.SelectedNode.Text.ToString();
                if (selectedNodeName != _mainFileName && selectedNodeName != "project.config")
                {
                    var result = MessageBox.Show($"Are you sure you would like to delete {selectedNodeName}?",
                                             $"Delete {selectedNodeName}", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        if (File.Exists(selectedNodePath))
                        {
                            string selectedFileName = Path.GetFileNameWithoutExtension(selectedNodePath);
                            File.Delete(selectedNodePath);
                            tvProject.Nodes.Remove(tvProject.SelectedNode);
                            var foundTab = uiScriptTabControl.TabPages.Cast<TabPage>()
                                                                      .Where(t => t.ToolTipText == selectedNodePath)
                                                                      .FirstOrDefault();
                            if (foundTab != null)
                                uiScriptTabControl.TabPages.Remove(foundTab);
                        }
                        else
                            throw new FileNotFoundException();
                    }
                }
                else
                    throw new Exception($"Cannot delete {selectedNodeName}");
            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message);
            }
        }

        private void tsmiRenameFile_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
                string selectedNodeName = tvProject.SelectedNode.Text.ToString();
                string selectedNodeNameWithoutExtension = Path.GetFileNameWithoutExtension(selectedNodeName);
                string selectedNodeFileExtension = Path.GetExtension(selectedNodePath);

                if (selectedNodeName != _mainFileName && selectedNodeName != "project.config")
                {
                    FileInfo selectedNodeDirectoryInfo = new FileInfo(selectedNodePath);

                    string newNameWithoutExtension = "";
                    var newNameForm = new frmInputBox("Enter the new name of the file without extension", "Rename File");
                    newNameForm.ShowDialog();

                    if (newNameForm.DialogResult == DialogResult.OK)
                        newNameWithoutExtension = newNameForm.txtInput.Text;
                    else if (newNameForm.DialogResult == DialogResult.Cancel)
                        return;

                    string newName = newNameWithoutExtension + selectedNodeFileExtension;
                    string newPath = Path.Combine(selectedNodeDirectoryInfo.DirectoryName, newName);

                    bool isInvalidProjectName = new[] { @"/", @"\" }.Any(c => newNameWithoutExtension.Contains(c));
                    if (isInvalidProjectName)
                        throw new Exception("Illegal characters in path");

                    if (File.Exists(newPath))
                        throw new Exception("A file with this name already exists");

                    var foundTab = uiScriptTabControl.TabPages.Cast<TabPage>().Where(t => t.ToolTipText == selectedNodePath)
                                                                          .FirstOrDefault();

                    if (foundTab != null)
                    {
                        DialogResult result = CheckForUnsavedScript(foundTab);
                        if (result == DialogResult.Cancel)
                            return;

                        uiScriptTabControl.TabPages.Remove(foundTab);
                    }

                    FileSystem.Rename(selectedNodePath, newPath);
                    tvProject.SelectedNode.Name = newName;
                    tvProject.SelectedNode.Text = newName;
                    tvProject.SelectedNode.Tag = newPath;
                }
            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message);
            }
        }
        #endregion

        #region Project Pane Buttons
        private void uiBtnRefresh_Click(object sender, EventArgs e)
        {
            tvProject.Refresh();
        }

        private void uiBtnExpand_Click(object sender, EventArgs e)
        {
            tvProject.ExpandAll();
        }

        private void uiBtnCollapse_Click(object sender, EventArgs e)
        {
            tvProject.CollapseAll();
        }

        private void uiBtnOpenDirectory_Click(object sender, EventArgs e)
        {
            Process.Start(_scriptProjectPath);
        }
        #endregion
    }
}
