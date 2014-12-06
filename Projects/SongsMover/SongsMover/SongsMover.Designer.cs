namespace SongsMover
{
    partial class SongsMover
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tvTree = new System.Windows.Forms.TreeView();
            this.btnSource = new System.Windows.Forms.Button();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.txtTarget = new System.Windows.Forms.TextBox();
            this.btnTarget = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.fbdSource = new System.Windows.Forms.FolderBrowserDialog();
            this.fbdTraget = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSize = new System.Windows.Forms.Label();
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.bwWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // tvTree
            // 
            this.tvTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvTree.CheckBoxes = true;
            this.tvTree.Location = new System.Drawing.Point(12, 65);
            this.tvTree.Name = "tvTree";
            this.tvTree.Size = new System.Drawing.Size(656, 364);
            this.tvTree.TabIndex = 0;
            this.tvTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.TvTreeAfterCheck);
            // 
            // btnSource
            // 
            this.btnSource.Location = new System.Drawing.Point(530, 10);
            this.btnSource.Name = "btnSource";
            this.btnSource.Size = new System.Drawing.Size(138, 23);
            this.btnSource.TabIndex = 1;
            this.btnSource.Text = "Select Source";
            this.btnSource.UseVisualStyleBackColor = true;
            this.btnSource.Click += new System.EventHandler(this.BtnSourceClick);
            // 
            // txtSource
            // 
            this.txtSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSource.Location = new System.Drawing.Point(12, 13);
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(512, 20);
            this.txtSource.TabIndex = 2;
            // 
            // txtTarget
            // 
            this.txtTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTarget.Location = new System.Drawing.Point(12, 39);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.Size = new System.Drawing.Size(512, 20);
            this.txtTarget.TabIndex = 4;
            // 
            // btnTarget
            // 
            this.btnTarget.Location = new System.Drawing.Point(530, 36);
            this.btnTarget.Name = "btnTarget";
            this.btnTarget.Size = new System.Drawing.Size(138, 23);
            this.btnTarget.TabIndex = 3;
            this.btnTarget.Text = "SelectTarget";
            this.btnTarget.UseVisualStyleBackColor = true;
            this.btnTarget.Click += new System.EventHandler(this.BtnTargetClick);
            // 
            // btnGo
            // 
            this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGo.Location = new System.Drawing.Point(12, 489);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(656, 36);
            this.btnGo.TabIndex = 5;
            this.btnGo.Text = "GO!!";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.BtnGoClick);
            // 
            // fbdTraget
            // 
            this.fbdTraget.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(9, 432);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "FilesSize(MB):";
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblSize.Location = new System.Drawing.Point(88, 432);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(15, 16);
            this.lblSize.TabIndex = 7;
            this.lblSize.Text = "0";
            // 
            // pbProgress
            // 
            this.pbProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pbProgress.Location = new System.Drawing.Point(12, 451);
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(653, 32);
            this.pbProgress.TabIndex = 8;
            // 
            // bwWorker
            // 
            this.bwWorker.WorkerReportsProgress = true;
            this.bwWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BwWorkerDoWork);
            this.bwWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BwWorkerProgressChanged);
            this.bwWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BwWorkerRunWorkerCompleted);
            // 
            // SongsMover
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 529);
            this.Controls.Add(this.pbProgress);
            this.Controls.Add(this.lblSize);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.txtTarget);
            this.Controls.Add(this.btnTarget);
            this.Controls.Add(this.txtSource);
            this.Controls.Add(this.btnSource);
            this.Controls.Add(this.tvTree);
            this.Name = "SongsMover";
            this.Text = "SongsMover";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tvTree;
        private System.Windows.Forms.Button btnSource;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.TextBox txtTarget;
        private System.Windows.Forms.Button btnTarget;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.FolderBrowserDialog fbdSource;
        private System.Windows.Forms.FolderBrowserDialog fbdTraget;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.ProgressBar pbProgress;
        private System.ComponentModel.BackgroundWorker bwWorker;
    }
}

