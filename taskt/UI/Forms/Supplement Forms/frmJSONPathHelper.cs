using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmJSONPathHelper : ThemedForm
    {
        public frmJSONPathHelper()
        {
            InitializeComponent();
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
        }

        private void OpenFromFileProcess(string path)
        {
            using (var sr = new System.IO.StreamReader(path))
            {
                txtRawJSON.Text = sr.ReadToEnd();
                lblMessage.Text = "File Open.";
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
            try
            {
                // root is object ?
                JObject json = JObject.Parse(txtRawJSON.Text);
                tvJSON.Nodes.Clear();

                TreeNode node = new TreeNode("\"\" - Object");
                node.Tag = json.ToString();

                createJSONTree(json, node);

                tvJSON.Nodes.Add(node);
                tvJSON.ExpandAll();

                lblMessage.Text = "JSON parsed.";
            }
            catch
            {
                try
                {
                    JArray jary = JArray.Parse(txtRawJSON.Text);

                    tvJSON.Nodes.Clear();

                    TreeNode node = new TreeNode("\"\" - Array");
                    node.Tag = jary.ToString();

                    createJSONTree(jary, node);

                    tvJSON.Nodes.Add(node);
                    tvJSON.ExpandAll();

                    lblMessage.Text = "JSON parsed.";
                }
                catch
                {
                    lblMessage.Text = "Invalid JSON!";
                }
            }
        }

        private void createJSONTree(JToken root, TreeNode tree)
        {
            JToken node = root.First;
            while (node != null)
            {
                TreeNode tnode = new TreeNode();

                object tryValue = node.Value<object>();
                Console.WriteLine(tryValue.GetType().FullName);

                if (tryValue is JProperty)
                {
                    JProperty prop = (JProperty)tryValue;

                    if (prop.HasValues)
                    {
                        JToken value = prop.Value;

                        if (value is JValue)
                        {
                            JValue v = (JValue)value;
                            tnode.Text = "\"" + parsePath(v.Path) + "\" - Value";

                            if (v.Value == null)
                            {
                                tnode.Tag = "null";
                            }
                            else
                            {
                                tnode.Tag = ((JValue)value).Value.ToString();
                            }
                        }
                        else if (value is JArray)
                        {
                            JArray ary = (JArray)value;
                            tnode.Text = "\"" + parsePath(ary.Path) + "\" - Array";
                            tnode.Tag = ary.ToString();

                            createJSONTree(ary, tnode);
                        }
                        else if (value is JObject)
                        {
                            JObject obj = (JObject)value;
                            tnode.Text = "\"" + parsePath(obj.Path) + "\" - Object";
                            tnode.Tag = obj.ToString();
                            createJSONTree(obj, tnode);
                        }
                    }
                }
                else if (tryValue is JObject)
                {
                    JObject obj = (JObject)tryValue;
                    tnode.Text = "\"" + parsePath(obj.Path) + "\" - Object";
                    tnode.Tag = obj.ToString();
                    createJSONTree(obj, tnode);
                }
                else if (tryValue is JValue)
                {
                    JValue jv = (JValue)tryValue;
                    tnode.Text = "\"" + parsePath(jv.Path) + "\" - Value";
                    tnode.Tag = jv.ToString();
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
        }
        #endregion

       
    }
}
