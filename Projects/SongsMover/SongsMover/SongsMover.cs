using System;
using System.IO;
using System.Windows.Forms;

namespace SongsMover
{
    public partial class SongsMover : Form
    {
        private long m_size;

        public SongsMover()
        {
            InitializeComponent();
        }

        private void BtnSourceClick(object sender, EventArgs e)
        {
            var showDialog = fbdSource.ShowDialog();

            if (showDialog != DialogResult.OK) return;

            txtSource.Text = fbdSource.SelectedPath;

            FillTree(txtSource.Text);

            m_size = 0;
        }

        private void BtnTargetClick(object sender, EventArgs e)
        {
            var showDialog = fbdTraget.ShowDialog();

            if (showDialog == DialogResult.OK)
            {
                txtTarget.Text = fbdTraget.SelectedPath;
            }
        }

        private void BtnGoClick(object sender, EventArgs e)
        {
            pbProgress.Maximum = (int)(m_size / 100);

            btnGo.Enabled = false;

            bwWorker.RunWorkerAsync();
        }

        private void CopyFilesRecursive(TreeNode treeNode, string newPath, string oldPath)
        {
            try
            {
                foreach (TreeNode node in treeNode.Nodes)
                {
                    if (node.Checked)
                    {
                        if (File.Exists(node.Name))
                        {
                            var fileInfo = new FileInfo(node.Name);
                            var length = fileInfo.Length;

                            var newDir = node.Parent.Name.Replace(oldPath, newPath);

                            if (!Directory.Exists(newDir))
                            {
                                Directory.CreateDirectory(newDir);
                            }

                            var destFileName = node.Name.Replace(oldPath, newPath);

                            if (!File.Exists(destFileName))
                            {
                                File.Copy(node.Name, destFileName);
                            }

                            bwWorker.ReportProgress(((int)length / 100));
                        }
                    }

                    CopyFilesRecursive(node, newPath, oldPath);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        private void FillTree(string path)
        {
            tvTree.BeginUpdate();

            tvTree.Nodes.Clear();

            var treeNode = tvTree.Nodes.Add("All Files");

            FillTreeRecursive(treeNode, path);

            tvTree.EndUpdate();
        }

        private static void FillTreeRecursive(TreeNode treeNode, string path)
        {
            var directories = Directory.GetDirectories(path);

            foreach (var directory in directories)
            {
                var node = treeNode.Nodes.Add(directory, Path.GetFileName(directory));

                FillTreeRecursive(node, directory);
            }

            var files = Directory.GetFiles(path, "*.mp3");

            foreach (var file in files)
            {
                treeNode.Nodes.Add(file, Path.GetFileNameWithoutExtension(file));
            }
        }

        private void TvTreeAfterCheck(object sender, TreeViewEventArgs e)
        {
            foreach (TreeNode node in e.Node.Nodes)
            {
                if (node.Checked != e.Node.Checked)
                {
                    node.Checked = e.Node.Checked;
                }
            }

            if (!File.Exists(e.Node.Name)) return;

            var file = new FileInfo(e.Node.Name);

            var length = (int)file.Length;
            m_size += e.Node.Checked ? length : -length;

            lblSize.Text = ((m_size / 1024f) / 1024f).ToString();
        }

        private void BwWorkerDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var treeNode = tvTree.Nodes[0];

            CopyFilesRecursive(treeNode, txtTarget.Text, txtSource.Text);
        }

        private void BwWorkerProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            pbProgress.Value += e.ProgressPercentage;
        }

        private void BwWorkerRunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            btnGo.Enabled = true;

            pbProgress.Value = 0;

            MessageBox.Show(@"All Da Songs Moved");
        }
    }
}
