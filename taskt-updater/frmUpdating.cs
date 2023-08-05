using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
using System.IO;

namespace taskt_updater
{
    public partial class frmUpdating : Form
    {
        private const string TASKT_WORK_DIR_NAME = "temp";

        string topLevelFolder = Application.StartupPath;
        string myPath = Application.ExecutablePath;

        private class Folders
        {
            public string Source { get; private set; }
            public string Target { get; private set; }

            public Folders(string source, string target)
            {
                Source = source;
                Target = target;
            }
        }

        private class ParamSet
        {
            public string name { get; private set; }
            public string value { get; private set; }

            public ParamSet(string name, string value)
            {
                this.name = name;
                this.value = value;
            }
        }

        #region form events
        public frmUpdating(string paramName, string paramValue)
        {
            InitializeComponent();
            bgwUpdate.RunWorkerAsync(new ParamSet(paramName, paramValue));   
        }

        private void frmUpdating_Load(object sender, EventArgs e)
        {

        }
        #endregion

        #region BackGroundWorker Events
        private void bgwUpdate_DoWork(object sender, DoWorkEventArgs e)
        {
            //get package
            bgwUpdate.ReportProgress(0, "Setting Up...");

            ParamSet param = (ParamSet)e.Argument;

            switch (param.name)
            {
                case "/d":
                    DownloadExtractNewRelease(param.value);
                    bgwUpdate.ReportProgress(0, "New Release Downloaded!!");
                    lblUpdate.Text = "New Release Extracted!!";
                    System.Threading.Thread.Sleep(1000);    // Wait a bit
                    break;
                case "/c":
                    CopyNewRelease(param.value);
                    bgwUpdate.ReportProgress(0, "New Release Copied!!");
                    lblUpdate.Text = "New Release Copied!!";
                    System.Threading.Thread.Sleep(1000);    // Wait a bit
                    break;
                case "/r":
                    RemoveDownloadRelease();
                    bgwUpdate.ReportProgress(0, "All Done!");
                    lblUpdate.Text = "All Done!";
                    System.Threading.Thread.Sleep(1000);    // Wait a bit
                    break;
                default:
                    break;
            }

            e.Result = param;
        }

        private void bgwUpdate_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblUpdate.Text = e.UserState.ToString();
        }

        private void bgwUpdate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error is null)
            {
                ParamSet param = (ParamSet)e.Result;

                switch (param.name)
                {
                    case "/d":
                        var zipFileName = Path.GetFileName(param.value);
                        int dotPosition = zipFileName.LastIndexOf(".");
                        zipFileName = zipFileName.Substring(0, dotPosition);

                        string tasktFolder = Directory.GetParent(topLevelFolder).FullName;

                        var copyProcess = new System.Diagnostics.Process();
                        copyProcess.StartInfo.FileName = Path.Combine(tasktFolder, TASKT_WORK_DIR_NAME, zipFileName, "Resources", "taskt-updater.exe");
                        copyProcess.StartInfo.Arguments = "/c \"" + tasktFolder + "\"";
                        copyProcess.Start();
                        this.Close();
                        break;
                    case "/c":
                        string newTasktFolder = Directory.GetParent(topLevelFolder).Parent.Parent.FullName;
                        var removeProcess = new System.Diagnostics.Process();
                        removeProcess.StartInfo.FileName = Path.Combine(newTasktFolder, "Resources", "taskt-updater.exe");
                        removeProcess.StartInfo.Arguments = "/r \"" + newTasktFolder + "\"";
                        removeProcess.Start();
                        this.Close();
                        break;
                    case "/r":
                        this.Close();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                MessageBox.Show(e.Error.ToString());
            }
        }
        #endregion

        #region file folder

        private void DownloadExtractNewRelease(string packageURL)
        {
            //define update folder
            var tempUpdateFolder = Path.Combine(Directory.GetParent(topLevelFolder).FullName, TASKT_WORK_DIR_NAME);

            // DBG
            //MessageBox.Show("temp: " + tempUpdateFolder + "\r\nURL: " + packageURL);

            //delete existing
            //if (Directory.Exists(tempUpdateFolder))
            //{
            //    System.IO.Directory.Delete(tempUpdateFolder, true);
            //}

            //create folder
            System.IO.Directory.CreateDirectory(tempUpdateFolder);

            bgwUpdate.ReportProgress(0, "Downloading Update...");

            //create uri and download package
            Uri uri = new Uri(packageURL);
            string localPackagePath = System.IO.Path.Combine(tempUpdateFolder, System.IO.Path.GetFileName(uri.LocalPath));

            //if package exists for some reason then delete
            if (System.IO.File.Exists(localPackagePath))
            {
                System.IO.File.Delete(localPackagePath);
            }

            //create web client
            System.Net.WebClient newWebClient = new System.Net.WebClient();

            //download file
            newWebClient.DownloadFile(uri, localPackagePath);

            bgwUpdate.ReportProgress(0, "Extracting Update...");

            using (FileStream zipToOpen = new FileStream(localPackagePath, FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    ExtractZipToDirectory(archive, tempUpdateFolder, true);
                }
            }
        }

        private void CopyNewRelease(string oldTasktFolder)
        {
            //create deployment folder reference
            var deploymentFolder = Directory.GetParent(topLevelFolder).FullName;

            bgwUpdate.ReportProgress(0, "Deployed to " + deploymentFolder);

            bgwUpdate.ReportProgress(0, "Updating Files...");

            if (oldTasktFolder.StartsWith("\"") && oldTasktFolder.EndsWith("\""))
            {
                oldTasktFolder = oldTasktFolder.Substring(1, oldTasktFolder.Length - 2);
            }

            // DBG
            //MessageBox.Show("dep: " + deploymentFolder + "\r\nold: " + oldTasktFolder);

            CopyDirectory(deploymentFolder, oldTasktFolder);
        }

        private void RemoveDownloadRelease()
        {
            bgwUpdate.ReportProgress(0, "Remove Downloaded Files...");

            string tempFolder = Path.Combine(Directory.GetParent(topLevelFolder).FullName, TASKT_WORK_DIR_NAME);

            // DBG
            //MessageBox.Show("temp: " + tempFolder);

            DeleteDirectory(tempFolder);
        }

        private void ExtractZipToDirectory(ZipArchive archive, string destinationDirectoryName, bool overwrite)
        {
            if (!overwrite)
            {
                archive.ExtractToDirectory(destinationDirectoryName);
                return;
            }
            foreach (ZipArchiveEntry file in archive.Entries)
            {
                string completeFileName = Path.Combine(destinationDirectoryName, file.FullName);
                if (file.Name == "")
                {
                    // Assuming Empty for Directory
                    Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                    continue;
                }
                file.ExtractToFile(completeFileName, true);
            }
        }

        private void CopyDirectory(string source, string target)
        {
            var stack = new Stack<Folders>();
            stack.Push(new Folders(source, target));

            while (stack.Count > 0)
            {
                var folders = stack.Pop();

                Directory.CreateDirectory(folders.Target);
                foreach (var file in Directory.GetFiles(folders.Source, "*.*"))
                {
                    var destPath = Path.Combine(folders.Target, Path.GetFileName(file));
                    if (destPath != myPath)
                    {
                        try
                        {
                            File.Copy(file, destPath, true);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Path: " + destPath + "\r\n" + ex.Message);
                        }
                    }
                }

                foreach (var folder in Directory.GetDirectories(folders.Source))
                {
                    try
                    {
                        stack.Push(new Folders(folder, Path.Combine(folders.Target, Path.GetFileName(folder))));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void DeleteDirectory(string target)
        {
            var stack = new Stack<string>();
            stack.Push(target);

            while (stack.Count > 0)
            {
                var currentFolder = stack.Pop();

                var files = Directory.GetFiles(currentFolder, "*.*");
                foreach (var file in files)
                {
                    File.Delete(file);
                }

                var dirs = Directory.GetDirectories(currentFolder);
                if (dirs.Length > 0)
                {
                    stack.Push(currentFolder);
                    foreach (var folder in dirs)
                    {
                        stack.Push(Path.Combine(currentFolder, folder));
                    }
                }
                else
                {
                    Directory.Delete(currentFolder);
                }
            }
        }
        #endregion
    }
}
