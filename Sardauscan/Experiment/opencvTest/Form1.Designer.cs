namespace opencvTest
{
	partial class Form1
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
			this.DiffButton = new System.Windows.Forms.Button();
			this.SnapBox = new System.Windows.Forms.PictureBox();
			this.DiffBox = new System.Windows.Forms.PictureBox();
			this.cameraSelctionControl1 = new opencvTest.CameraSelectionControl();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.SnapBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.DiffBox)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// DiffButton
			// 
			this.DiffButton.Location = new System.Drawing.Point(0, 3);
			this.DiffButton.Name = "DiffButton";
			this.DiffButton.Size = new System.Drawing.Size(75, 23);
			this.DiffButton.TabIndex = 1;
			this.DiffButton.Text = "Diff";
			this.DiffButton.UseVisualStyleBackColor = true;
			this.DiffButton.Click += new System.EventHandler(this.DiffButton_Click);
			// 
			// SnapBox
			// 
			this.SnapBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.SnapBox.Location = new System.Drawing.Point(84, 3);
			this.SnapBox.Name = "SnapBox";
			this.SnapBox.Size = new System.Drawing.Size(184, 219);
			this.SnapBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.SnapBox.TabIndex = 2;
			this.SnapBox.TabStop = false;
			// 
			// DiffBox
			// 
			this.DiffBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DiffBox.Location = new System.Drawing.Point(274, 3);
			this.DiffBox.Name = "DiffBox";
			this.DiffBox.Size = new System.Drawing.Size(366, 216);
			this.DiffBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.DiffBox.TabIndex = 2;
			this.DiffBox.TabStop = false;
			// 
			// cameraSelctionControl1
			// 
			this.cameraSelctionControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cameraSelctionControl1.Location = new System.Drawing.Point(3, 3);
			this.cameraSelctionControl1.Name = "cameraSelctionControl1";
			this.cameraSelctionControl1.Size = new System.Drawing.Size(643, 219);
			this.cameraSelctionControl1.TabIndex = 0;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.cameraSelctionControl1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(649, 450);
			this.tableLayoutPanel1.TabIndex = 3;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.DiffButton);
			this.panel1.Controls.Add(this.DiffBox);
			this.panel1.Controls.Add(this.SnapBox);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 228);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(643, 219);
			this.panel1.TabIndex = 1;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(649, 450);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.SnapBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.DiffBox)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private CameraSelectionControl cameraSelctionControl1;
		private System.Windows.Forms.Button DiffButton;
		private System.Windows.Forms.PictureBox SnapBox;
		private System.Windows.Forms.PictureBox DiffBox;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel panel1;

	}
}

