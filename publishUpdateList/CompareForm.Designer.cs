namespace NDiffDiff
{
	partial class CompareForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompareForm));
            this.lblSpace = new System.Windows.Forms.Label();
            this.lblLines = new System.Windows.Forms.Label();
            this.lblA = new System.Windows.Forms.Label();
            this.lblB = new System.Windows.Forms.Label();
            this.lblOutput = new System.Windows.Forms.Label();
            this.lblDiff = new System.Windows.Forms.Label();
            this.lblRead = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblJit = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this._Display = new NDiffDiff.Display();
            this.SuspendLayout();
            // 
            // lblSpace
            // 
            this.lblSpace.AutoSize = true;
            this.lblSpace.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpace.Location = new System.Drawing.Point(362, 49);
            this.lblSpace.Margin = new System.Windows.Forms.Padding(3);
            this.lblSpace.Name = "lblSpace";
            this.lblSpace.Size = new System.Drawing.Size(139, 16);
            this.lblSpace.TabIndex = 5;
            this.lblSpace.Text = "Space: 000,000 bytes";
            // 
            // lblLines
            // 
            this.lblLines.AutoSize = true;
            this.lblLines.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLines.Location = new System.Drawing.Point(12, 49);
            this.lblLines.Margin = new System.Windows.Forms.Padding(3);
            this.lblLines.Name = "lblLines";
            this.lblLines.Size = new System.Drawing.Size(92, 16);
            this.lblLines.TabIndex = 2;
            this.lblLines.Text = "Diff: 000 lines";
            // 
            // lblA
            // 
            this.lblA.AutoSize = true;
            this.lblA.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblA.Location = new System.Drawing.Point(12, 8);
            this.lblA.Name = "lblA";
            this.lblA.Size = new System.Drawing.Size(119, 16);
            this.lblA.TabIndex = 0;
            this.lblA.Text = "File A: 0,000 lines";
            // 
            // lblB
            // 
            this.lblB.AutoSize = true;
            this.lblB.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblB.Location = new System.Drawing.Point(12, 29);
            this.lblB.Name = "lblB";
            this.lblB.Size = new System.Drawing.Size(119, 16);
            this.lblB.TabIndex = 1;
            this.lblB.Text = "File B: 0,000 lines";
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutput.Location = new System.Drawing.Point(202, 49);
            this.lblOutput.Margin = new System.Windows.Forms.Padding(3);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(105, 16);
            this.lblOutput.TabIndex = 3;
            this.lblOutput.Text = "Output: 0.00 ms";
            // 
            // lblDiff
            // 
            this.lblDiff.AutoSize = true;
            this.lblDiff.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiff.Location = new System.Drawing.Point(202, 29);
            this.lblDiff.Margin = new System.Windows.Forms.Padding(3);
            this.lblDiff.Name = "lblDiff";
            this.lblDiff.Size = new System.Drawing.Size(84, 16);
            this.lblDiff.TabIndex = 3;
            this.lblDiff.Text = "Diff: 0.00 ms";
            // 
            // lblRead
            // 
            this.lblRead.AutoSize = true;
            this.lblRead.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRead.Location = new System.Drawing.Point(202, 8);
            this.lblRead.Margin = new System.Windows.Forms.Padding(3);
            this.lblRead.Name = "lblRead";
            this.lblRead.Size = new System.Drawing.Size(96, 16);
            this.lblRead.TabIndex = 3;
            this.lblRead.Text = "Read: 0.00 ms";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(362, 8);
            this.lblTotal.Margin = new System.Windows.Forms.Padding(3);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(95, 16);
            this.lblTotal.TabIndex = 3;
            this.lblTotal.Text = "Time: 0.00 ms";
            // 
            // lblJit
            // 
            this.lblJit.AutoSize = true;
            this.lblJit.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblJit.Location = new System.Drawing.Point(362, 29);
            this.lblJit.Margin = new System.Windows.Forms.Padding(3);
            this.lblJit.Name = "lblJit";
            this.lblJit.Size = new System.Drawing.Size(78, 16);
            this.lblJit.TabIndex = 3;
            this.lblJit.Text = "Jit: 00.0 ms";
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(580, 6);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(188, 59);
            this.btnSubmit.TabIndex = 8;
            this.btnSubmit.Text = "提交(&S)";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // _Display
            // 
            this._Display.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._Display.AutoScroll = true;
            this._Display.BackColor = System.Drawing.Color.Black;
            this._Display.Location = new System.Drawing.Point(0, 72);
            this._Display.Name = "_Display";
            this._Display.Result = null;
            this._Display.Size = new System.Drawing.Size(768, 467);
            this._Display.TabIndex = 7;
            // 
            // CompareForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 539);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this._Display);
            this.Controls.Add(this.lblB);
            this.Controls.Add(this.lblA);
            this.Controls.Add(this.lblLines);
            this.Controls.Add(this.lblSpace);
            this.Controls.Add(this.lblJit);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.lblRead);
            this.Controls.Add(this.lblDiff);
            this.Controls.Add(this.lblOutput);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CompareForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NDiffDiff";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblSpace;
		private System.Windows.Forms.Label lblLines;
		private System.Windows.Forms.Label lblA;
		private System.Windows.Forms.Label lblB;
		private System.Windows.Forms.Label lblOutput;
		private System.Windows.Forms.Label lblDiff;
		private System.Windows.Forms.Label lblRead;
		private System.Windows.Forms.Label lblTotal;
		private System.Windows.Forms.Label lblJit;
		private Display _Display;
        private System.Windows.Forms.Button btnSubmit;

	}
}

