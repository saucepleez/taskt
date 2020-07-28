using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using taskt.Core.Script;
using taskt.UI.CustomControls.CustomUIControls;

namespace taskt.UI.Forms.ScriptBuilder_Forms
{
    public partial class frmScriptBuilder : Form
    {
        #region Script Tab Control
        private void uiScriptTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (uiScriptTabControl.TabCount > 0)
            {
                ScriptFilePath = uiScriptTabControl.SelectedTab.ToolTipText.ToString();
                _selectedTabScriptActions = (UIListView)uiScriptTabControl.SelectedTab.Controls[0];
                ScriptObject scriptObject = (ScriptObject)uiScriptTabControl.SelectedTab.Tag;
                if (scriptObject != null)
                {
                    _scriptVariables = scriptObject.ScriptVariables;
                    _scriptElements = scriptObject.ScriptElements;
                }               
            }
        }

        //TODO Finish close button rendering
        private void uiScriptTabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tabControl = (TabControl)sender;
            Point imageLocation = new Point(15, 5);

            try
            {
                Image closeImage = new Bitmap(imgListTabControl.Images[0]);
                Rectangle tabRect = tabControl.GetTabRect(e.Index);
                tabRect.Offset(2, 2);
                string title = tabControl.TabPages[e.Index].Text + "  ";
                Font font = uiPaneTabs.Font;
                Brush titleBrush = new SolidBrush(Color.Black);
                e.Graphics.DrawString(title, font, titleBrush, new PointF(tabRect.X, tabRect.Y));
                if (tabControl.SelectedIndex >= 1)
                    e.Graphics.DrawImage(closeImage, new Point(tabRect.X + (tabRect.Width - imageLocation.X), imageLocation.Y));
            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message);
            }
        }

        private void uiScriptTabControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _lastClickPosition = Cursor.Position;
                cmsScriptTabActions.Show(Cursor.Position);
            }

        //TODO Finish close button click event
        //    else
        //    {
        //        TabControl tabControl = (TabControl)sender;
        //        Point imageHitArea = new Point(13, 2);
        //        Point point = e.Location;
        //        Rectangle tabRect = tabControl.GetTabRect(tabControl.SelectedIndex);
        //        int tabWidth = tabRect.Width - imageHitArea.X;
        //        tabRect.Offset(tabWidth, imageHitArea.Y);
        //        tabRect.Width = 16;
        //        tabRect.Height = 16;
        //        if (tabControl.SelectedIndex >= 1 && tabRect.Contains(point))
        //            tabControl.TabPages.Remove(tabControl.TabPages[tabControl.SelectedIndex]);
        //    }
        }

        private void uiScriptTabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (IsScriptRunning)
                e.Cancel = true;
        }

        private void tsmiCloseTab_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < uiScriptTabControl.TabCount; i++)
            {
                Rectangle tabRect = uiScriptTabControl.GetTabRect(i);
                if (tabRect.Contains(uiScriptTabControl.PointToClient(_lastClickPosition))
                    && uiScriptTabControl.TabCount > 1)
                {

                    TabPage tab = uiScriptTabControl.TabPages[i];
                    DialogResult result = CheckForUnsavedScript(tab);
                    if (result == DialogResult.Cancel)
                        return;
                    uiScriptTabControl.TabPages.RemoveAt(i);
                }
            }
        }

        private void tsmiCloseAllButThis_Click(object sender, EventArgs e)
        {
            //iterate through each tab, and check if it's the selected tab.
            //If it is, store and continue. If it isn't, check for unsaved changes.
            TabPage keepTab = new TabPage();
            for (int i = 0; i < uiScriptTabControl.TabCount; i++)
            {
                Rectangle tabRect = uiScriptTabControl.GetTabRect(i);
                if (!tabRect.Contains(uiScriptTabControl.PointToClient(_lastClickPosition)))
                {
                    TabPage tab = uiScriptTabControl.TabPages[i];
                    DialogResult result = CheckForUnsavedScript(tab);
                    if (result == DialogResult.Cancel)
                        return;
                }
                else if (tabRect.Contains(uiScriptTabControl.PointToClient(_lastClickPosition)))
                    keepTab = uiScriptTabControl.TabPages[i];
            }
            foreach (TabPage tab in uiScriptTabControl.TabPages)
            {
                if (tab.ToolTipText != keepTab.ToolTipText)
                    uiScriptTabControl.TabPages.Remove(tab);
            }
        }

        private void UpdateTabPage(TabPage tab, string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            tab.Text = fileName;
            tab.Name = fileName;
            tab.ToolTipText = filePath;
            tab.Controls[0].Name = fileName;
        }

        private DialogResult CheckForUnsavedScripts()
        {
            DialogResult result = new DialogResult();
            if (uiScriptTabControl.TabCount > 0)
            {
                foreach (TabPage tab in uiScriptTabControl.TabPages)
                {
                    result = CheckForUnsavedScript(tab);
                }
            }
            return result;
        }

        private DialogResult CheckForUnsavedScript(TabPage tab)
        {
            DialogResult result = new DialogResult();
            if (tab.Text.Contains(" *"))
            {
                result = MessageBox.Show($"Would you like to save {tab.Name}.json before closing this tab?",
                                         $"Save {tab.Name}.json", MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Yes)
                {
                    ClearSelectedListViewItems();
                    uiScriptTabControl.SelectedTab = tab;
                    SaveToFile(false);
                }
                else if (result == DialogResult.Cancel)
                    return result;
            }
            return result;
        }
        #endregion
    }
}
