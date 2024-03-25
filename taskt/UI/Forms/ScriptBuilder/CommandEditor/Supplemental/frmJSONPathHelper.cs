﻿using Newtonsoft.Json.Linq;
using System;
using System.Text;
using System.Windows.Forms;

/*
 * NOTE: This form is called primarily by frmCommandEditor, so the namespace looks like this
 */
namespace taskt.UI.Forms.ScriptBuilder.CommandEditor.Supplemental
{
    public partial class frmJSONPathHelper : ThemedForm
    {
        public frmJSONPathHelper()
        {
            InitializeComponent();
            this.FormClosed += SupplementFormsEvents.SupplementFormClosed;
        }

        private void frmJSONPathHelper_Load(object sender, EventArgs e)
        {
            SupplementFormsEvents.SupplementFormLoad(this);
            lblMessage.Text = "";
        }

        #region Open JSON
        private void picOpenFromFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                OpenFromFileProcess(openFileDialog1.FileName);
            }
        }
        private void picOpenFromURL_Click(object sender, EventArgs e)
        {
            using (var fm = new ScriptBuilder.CommandEditor.Supplemental.frmInputBox("Input URL", "Please specify JSON URL", "https://"))
            {
                if (fm.ShowDialog() == DialogResult.OK)
                {
                    OpenFromURLProcess(fm.InputValue);
                }
            }
        }
        private void picClear_Click(object sender, EventArgs e)
        {
            txtRawJSON.Text = "";
        }

        private void OpenFromFileProcess(string path)
        {
            try
            {
                using (var sr = new System.IO.StreamReader(path))
                {
                    txtRawJSON.Text = sr.ReadToEnd();
                    lblMessage.Text = "File Open.";
                }
                ParseJSONProcess();
            }
            catch
            {
                lblMessage.Text = "Fail Open File.";
            }
        }

        private void OpenFromURLProcess(string url)
        {
            try
            {
                var wc = new System.Net.WebClient();
                wc.Encoding = Encoding.UTF8;
                wc.Headers.Add("user-agent", "request");
                txtRawJSON.Text = wc.DownloadString(url);
                lblMessage.Text = "URL Open.";
                ParseJSONProcess();
            }
            catch
            {
                lblMessage.Text = "Fail Open From URL.";
            }
        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            ParseJSONProcess();
        }
        #endregion

        #region JSON Parse
        private void ParseJSONProcess()
        {
            tvJSON.BeginUpdate();
            tvJSON.Nodes.Clear();

            try
            {
                // root is object ?
                JObject json = JObject.Parse(txtRawJSON.Text);

                TreeNode node = new TreeNode("\"\" - Object")
                {
                    Tag = json.ToString()
                };

                createJSONTree(json, node);

                tvJSON.Nodes.Add(node);
                tvJSON.ExpandAll();

                tvJSON.Nodes[0].EnsureVisible();

                lblMessage.Text = "JSON parsed.";
            }
            catch
            {
                try
                {
                    JArray jary = JArray.Parse(txtRawJSON.Text);

                    tvJSON.Nodes.Clear();

                    TreeNode node = new TreeNode("\"\" - Array")
                    {
                        Tag = jary.ToString()
                    };

                    createJSONTree(jary, node);

                    tvJSON.Nodes.Add(node);
                    tvJSON.ExpandAll();

                    tvJSON.Nodes[0].EnsureVisible();

                    lblMessage.Text = "JSON parsed.";
                }
                catch
                {
                    lblMessage.Text = "Invalid JSON!";
                }
            }

            tvJSON.EndUpdate();
        }

        private void createJSONTree(JToken root, TreeNode tree)
        {
            var JArrayFunc = new Action<JArray, TreeNode>((ary, tnode) =>
            {
                tnode.Text = $"\"{parsePath(ary.Path)}\" - Array";
                tnode.Tag = ary.ToString();
                createJSONTree(ary, tnode);
            });
            var JObjectFunc = new Action<JObject, TreeNode>((obj, tnode) =>
            {
                tnode.Text = $"\"{parsePath(obj.Path)}\" - Object";
                tnode.Tag = obj.ToString();
                createJSONTree(obj, tnode);
            });
            var JValueFunc = new Action<JValue, TreeNode>((v, tnode) =>
            {
                tnode.Text = $"\"{parsePath(v.Path)}\" - Value";

                if (v.Value == null)
                {
                    tnode.Tag = "null";
                }
                else
                {
                    tnode.Tag = v.Value.ToString();
                }
            });

            JToken node = root.First;
            while (node != null)
            {
                TreeNode tnode = new TreeNode();

                object tryValue = node.Value<object>();
                
                // DBG
                //Console.WriteLine(tryValue.GetType().FullName);

                if (tryValue is JProperty prop)
                {
                    if (prop.HasValues)
                    {
                        JToken value = prop.Value;

                        if (value is JValue v)
                        {
                            //tnode.Text = $"\"{parsePath(v.Path)}\" - Value";

                            //if (v.Value == null)
                            //{
                            //    tnode.Tag = "null";
                            //}
                            //else
                            //{
                            //    tnode.Tag = ((JValue)value).Value.ToString();
                            //}
                            JValueFunc(v, tnode);
                        }
                        else if (value is JArray ary)
                        {
                            //tnode.Text = $"\"{parsePath(ary.Path)}\" - Array";
                            //tnode.Tag = ary.ToString();
                            //createJSONTree(ary, tnode);
                            JArrayFunc(ary, tnode);
                        }
                        else if (value is JObject obj)
                        {
                            //tnode.Text = $"\"{parsePath(obj.Path)}\" - Object";
                            //tnode.Tag = obj.ToString();
                            //createJSONTree(obj, tnode);
                            JObjectFunc(obj, tnode);
                        }
                    }
                }
                else if (tryValue is JObject obj)
                {
                    //tnode.Text = $"\"{parsePath(obj.Path)}\" - Object";
                    //tnode.Tag = obj.ToString();
                    //createJSONTree(obj, tnode);
                    JObjectFunc(obj, tnode);
                }
                else if (tryValue is JArray ary)
                {
                    //tnode.Text = $"\"{parsePath(ary.Path)}\" - Array";
                    //tnode.Tag = ary.ToString();
                    //createJSONTree(ary, tnode);
                    JArrayFunc(ary, tnode);
                }
                else if (tryValue is JValue v)
                {
                    //tnode.Text = $"\"{parsePath(jv.Path)}\" - Value";
                    //tnode.Tag = jv.ToString();
                    JValueFunc(v, tnode);
                }

                tree.Nodes.Add(tnode);

                node = node.Next;
            }
        }
        private static string parsePath(string path)
        {
            int idx = path.LastIndexOf(".");
            if (idx < 0)
            {
                return parseArrayPath(path);
            }
            else
            {
                string myPath = path.Substring(idx + 1);
                return parseArrayPath(myPath);
            }
        }

        private static string parseArrayPath(string path)
        {
            if (path.EndsWith("]"))
            {
                return path.Substring(path.LastIndexOf("["));
            }
            else
            {
                return path;
            }
        }
        #endregion

        #region tvJSON events
        private void tvJSON_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            txtJSONPathResult.Text = (string)e.Node.Tag;
            txtJSONPath.Text = getJSONPath(e.Node);
        }

        private string getJSONPath(TreeNode node)
        {
            string jsonPath = "";

            TreeNode p = node;
            while (p.Parent != null)
            {
                string nodeText = p.Text;
                int idx = nodeText.LastIndexOf("-");
                string nodePath = nodeText.Substring(0, idx).Trim();
                string nodeType = nodeText.Substring(idx + 1).Trim();

                if (nodePath.StartsWith("\"") && nodePath.EndsWith("\""))
                {
                    nodePath = nodePath.Substring(1, nodePath.Length - 2);
                }

                switch (nodeType)
                {
                    case "Value":
                    case "Object":
                        jsonPath = nodePath + "." + jsonPath;
                        break;
                    case "Array":
                        jsonPath = nodePath + jsonPath;
                        break;
                    default:
                        break;
                }

                p = p.Parent;
            }

            if ((jsonPath.Length > 1) && (jsonPath.EndsWith(".")))
            {
                // remove last dot
                jsonPath = jsonPath.Substring(0, jsonPath.Length - 1);
            }
            jsonPath = "$." + jsonPath; // set first $.

            return jsonPath;
        }

        #endregion

        #region txtJSONPath
        private void txtJSONPath_DoubleClick(object sender, EventArgs e)
        {
            txtJSONPath.SelectAll();
            if (!string.IsNullOrEmpty(txtJSONPath.Text))
            {
                Clipboard.SetText(txtJSONPath.Text);
                lblMessage.Text = "JSONPath Copied.";
            }
        }

        private void txtJSONPathResult_DoubleClick(object sender, EventArgs e)
        {
            txtJSONPathResult.SelectAll();
            if (!string.IsNullOrEmpty(txtJSONPathResult.Text))
            {
                Clipboard.SetText(txtJSONPathResult.Text);
                lblMessage.Text = "Result Copied.";
            }
        }
        #endregion

        #region txtRawJSON
        private void txtRawJSON_DoubleClick(object sender, EventArgs e)
        {
            ParseJSONProcess();
        }
        #endregion

        #region Footer buttons
        private void uiBtnAdd_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        #endregion

        #region Properties
        public string JSONPath
        {
            get
            {
                return this.txtJSONPath.Text;
            }
        }

        #endregion

        #region Drag&Drop
        private void txtRawJSON_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void txtRawJSON_DragDrop(object sender, DragEventArgs e)
        {
            string[] fileNames = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            OpenFromFileProcess(fileNames[0]);
        }
        #endregion
    }
}
