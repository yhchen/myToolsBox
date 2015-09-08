namespace publishUpdateList
{
    partial class MainForm
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
            this.tbContent = new System.Windows.Forms.TextBox();
            this.gbCtrlArea = new System.Windows.Forms.GroupBox();
            this.btn_OpenFile = new System.Windows.Forms.Button();
            this.tbSelecFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labURL = new System.Windows.Forms.Label();
            this.btn_Compare = new System.Windows.Forms.Button();
            this.tbURL = new System.Windows.Forms.TextBox();
            this.btnSelecFile = new System.Windows.Forms.Button();
            this.gbCtrlArea.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbContent
            // 
            this.tbContent.AcceptsTab = true;
            this.tbContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbContent.Location = new System.Drawing.Point(12, 12);
            this.tbContent.MaxLength = 524287;
            this.tbContent.Multiline = true;
            this.tbContent.Name = "tbContent";
            this.tbContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbContent.Size = new System.Drawing.Size(928, 495);
            this.tbContent.TabIndex = 0;
            this.tbContent.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbContent_KeyDown);
            // 
            // gbCtrlArea
            // 
            this.gbCtrlArea.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbCtrlArea.Controls.Add(this.btnSelecFile);
            this.gbCtrlArea.Controls.Add(this.btn_OpenFile);
            this.gbCtrlArea.Controls.Add(this.tbSelecFile);
            this.gbCtrlArea.Controls.Add(this.label1);
            this.gbCtrlArea.Controls.Add(this.labURL);
            this.gbCtrlArea.Controls.Add(this.btn_Compare);
            this.gbCtrlArea.Controls.Add(this.tbURL);
            this.gbCtrlArea.Location = new System.Drawing.Point(12, 513);
            this.gbCtrlArea.Name = "gbCtrlArea";
            this.gbCtrlArea.Size = new System.Drawing.Size(928, 93);
            this.gbCtrlArea.TabIndex = 1;
            this.gbCtrlArea.TabStop = false;
            this.gbCtrlArea.Text = "操作区";
            // 
            // btn_OpenFile
            // 
            this.btn_OpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_OpenFile.Location = new System.Drawing.Point(813, 18);
            this.btn_OpenFile.Name = "btn_OpenFile";
            this.btn_OpenFile.Size = new System.Drawing.Size(109, 23);
            this.btn_OpenFile.TabIndex = 5;
            this.btn_OpenFile.Text = "打开(&C)";
            this.btn_OpenFile.UseVisualStyleBackColor = true;
            this.btn_OpenFile.Click += new System.EventHandler(this.btn_OpenFile_Click);
            // 
            // tbSelecFile
            // 
            this.tbSelecFile.AllowDrop = true;
            this.tbSelecFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSelecFile.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbSelecFile.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.tbSelecFile.Location = new System.Drawing.Point(86, 20);
            this.tbSelecFile.Name = "tbSelecFile";
            this.tbSelecFile.Size = new System.Drawing.Size(711, 21);
            this.tbSelecFile.TabIndex = 4;
            this.tbSelecFile.TextChanged += new System.EventHandler(this.tbSelecFile_TextChanged);
            this.tbSelecFile.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbSelecFile_DragDrop);
            this.tbSelecFile.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbSelecFile_DragEnter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "打开文件 :";
            // 
            // labURL
            // 
            this.labURL.AutoSize = true;
            this.labURL.Location = new System.Drawing.Point(15, 63);
            this.labURL.Name = "labURL";
            this.labURL.Size = new System.Drawing.Size(65, 12);
            this.labURL.TabIndex = 2;
            this.labURL.Text = "上传 URL :";
            // 
            // btn_Compare
            // 
            this.btn_Compare.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Compare.Location = new System.Drawing.Point(813, 58);
            this.btn_Compare.Name = "btn_Compare";
            this.btn_Compare.Size = new System.Drawing.Size(109, 23);
            this.btn_Compare.TabIndex = 1;
            this.btn_Compare.Text = "比对(&C)";
            this.btn_Compare.UseVisualStyleBackColor = true;
            this.btn_Compare.Click += new System.EventHandler(this.btn_Compare_Click);
            // 
            // tbURL
            // 
            this.tbURL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbURL.Location = new System.Drawing.Point(86, 58);
            this.tbURL.Name = "tbURL";
            this.tbURL.Size = new System.Drawing.Size(711, 21);
            this.tbURL.TabIndex = 0;
            this.tbURL.TextChanged += new System.EventHandler(this.tbURL_TextChanged);
            // 
            // btnSelecFile
            // 
            this.btnSelecFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelecFile.Location = new System.Drawing.Point(764, 19);
            this.btnSelecFile.Name = "btnSelecFile";
            this.btnSelecFile.Size = new System.Drawing.Size(33, 23);
            this.btnSelecFile.TabIndex = 6;
            this.btnSelecFile.Text = "...";
            this.btnSelecFile.UseVisualStyleBackColor = true;
            this.btnSelecFile.Click += new System.EventHandler(this.btnSelecFile_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 613);
            this.Controls.Add(this.gbCtrlArea);
            this.Controls.Add(this.tbContent);
            this.Name = "MainForm";
            this.Text = "更新发布程序";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.gbCtrlArea.ResumeLayout(false);
            this.gbCtrlArea.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbContent;
        private System.Windows.Forms.GroupBox gbCtrlArea;
        private System.Windows.Forms.Button btn_Compare;
        private System.Windows.Forms.TextBox tbURL;
        private System.Windows.Forms.Label labURL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbSelecFile;
        private System.Windows.Forms.Button btn_OpenFile;
        private System.Windows.Forms.Button btnSelecFile;
    }
}

