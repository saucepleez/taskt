﻿//Copyright (c) 2019 Jason Bayldon
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace taskt.UI.Forms.ScriptBuilder.Supplemental
{
    public partial class frmAbout : ThemedForm
    {
        public frmAbout()
        {
            InitializeComponent();
        }

        #region form events
        private void frmAbout_Load(object sender, EventArgs e)
        {
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var info = FileVersionInfo.GetVersionInfo(location);
            lblProjectName.Text = info.ProductName;
            //lblAppVersion.Text = "version: " + new Version(System.Windows.Forms.Application.ProductVersion);
            lblAppVersion.Text = "version: " + info.ProductVersion;
            //lblBuildDate.Text = "build date: " + System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString("MM.dd.yy hh.mm.ss");
            lblBuildDate.Text = "build date: " + System.IO.File.GetLastWriteTime(location).ToString("MM.dd.yy hh.mm.ss");
        }
        private void frmAbout_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
        #endregion

        //private void lblOneNote_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    Process.Start("https://github.com/ignatandrei/OneNoteOCR");
        //}

        //private void lblSelenium_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    Process.Start("https://github.com/SeleniumHQ/selenium");          
        //}

        //private void lblTaskScheduler_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    Process.Start("https://github.com/dahall/TaskScheduler");
        //}

        //private void lblLog4Net_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    Process.Start("https://github.com/dahall/TaskScheduler");
        //}

        //private void lblHTMLAgilityPack_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    Process.Start("https://www.nuget.org/packages/HtmlAgilityPack/");
        //}

        //private void lblIMAPX_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    Process.Start("https://github.com/azanov/imapx");
        //}

        //private void lblJetBrainsAnnotations_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    Process.Start("https://www.nuget.org/packages/JetBrains.Annotations/");
        //}

        //private void lblNewtonSoft_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    Process.Start("https://www.nuget.org/packages/newtonsoft.json/");
        //}

        //private void lblSuperSocketClientEngine_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    Process.Start("https://github.com/kerryjiang/SuperSocket.ClientEngine");
        //}

        //private void lblWebSocket4Net_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    Process.Start("https://github.com/kerryjiang/WebSocket4Net");
        //}


    }
}