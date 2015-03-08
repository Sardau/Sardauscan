#region COPYRIGHT
/****************************************************************************
 *  Copyright (c) 2015 Fabio Ferretti <https://plus.google.com/+FabioFerretti3D>                 *
 *  This file is part of Sardauscan.                                        *
 *                                                                          *
 *  Sardauscan is free software: you can redistribute it and/or modify      *
 *  it under the terms of the GNU General Public License as published by    *
 *  the Free Software Foundation, either version 3 of the License, or       *
 *  (at your option) any later version.                                     *
 *                                                                          *
 *  Sardauscan is distributed in the hope that it will be useful,           *
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of          *
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the           *
 *  GNU General Public License for more details.                            *
 *                                                                          *
 *  You are not allowed to Sell in any form this code                       * 
 *  or any compiled version. This code is free and for free purpose only    *
 *                                                                          *
 *  You should have received a copy of the GNU General Public License       *
 *  along with Sardaukar.  If not, see <http://www.gnu.org/licenses/>       *
 ****************************************************************************
*/
#endregion
using Sardauscan.Gui.Controls;
namespace Sardauscan.Gui.Controls
{
	/// <summary>
	/// Drag and Drop control to construct/load/Save Process
	/// </summary>
    partial class DragDropTaskList
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.TaskBackgroundWorker = new System.ComponentModel.BackgroundWorker();
			this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.FileDialog = new System.Windows.Forms.OpenFileDialog();
			this.MessageLabel = new System.Windows.Forms.Label();
			this.MessagePanel = new System.Windows.Forms.Panel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.ListDragSource = new Sardauscan.Gui.Controls.TaskListBox();
			this.LoadButton = new Sardauscan.Gui.Controls.ImageButton();
			this.RunButton = new Sardauscan.Gui.Controls.ImageButton();
			this.SaveButton = new Sardauscan.Gui.Controls.ImageButton();
			this.DeleteBox = new Sardauscan.Gui.Controls.ImageButton();
			this.ListDragTarget = new Sardauscan.Gui.Controls.TaskListBox();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.MessagePanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.IsSplitterFixed = true;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.BackColor = System.Drawing.Color.White;
			this.splitContainer1.Panel1.Controls.Add(this.ListDragSource);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.BackColor = System.Drawing.Color.White;
			this.splitContainer1.Panel2.Controls.Add(this.MessagePanel);
			this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
			this.splitContainer1.Panel2.Controls.Add(this.ListDragTarget);
			this.splitContainer1.Size = new System.Drawing.Size(485, 278);
			this.splitContainer1.SplitterDistance = 242;
			this.splitContainer1.SplitterWidth = 1;
			this.splitContainer1.TabIndex = 3;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tableLayoutPanel1.Controls.Add(this.LoadButton, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.RunButton, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.SaveButton, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.DeleteBox, 3, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 241);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(240, 35);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// TaskBackgroundWorker
			// 
			this.TaskBackgroundWorker.WorkerReportsProgress = true;
			this.TaskBackgroundWorker.WorkerSupportsCancellation = true;
			this.TaskBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.TaskBackgroundWorker_DoWork);
			this.TaskBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.TaskBackgroundWorker_ProgressChanged);
			this.TaskBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.TaskBackgroundWorker_RunWorkerCompleted);
			// 
			// ToolTip
			// 
			this.ToolTip.UseAnimation = false;
			// 
			// FileDialog
			// 
			this.FileDialog.FileName = "FileDialog";
			// 
			// MessageLabel
			// 
			this.MessageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.MessageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MessageLabel.ForeColor = System.Drawing.Color.Gray;
			this.MessageLabel.Location = new System.Drawing.Point(17, -6);
			this.MessageLabel.Name = "MessageLabel";
			this.MessageLabel.Size = new System.Drawing.Size(207, 21);
			this.MessageLabel.TabIndex = 2;
			this.MessageLabel.Text = "Drag task  here";
			this.MessageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// MessagePanel
			// 
			this.MessagePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.MessagePanel.Controls.Add(this.pictureBox2);
			this.MessagePanel.Controls.Add(this.pictureBox1);
			this.MessagePanel.Controls.Add(this.MessageLabel);
			this.MessagePanel.Location = new System.Drawing.Point(0, 3);
			this.MessagePanel.Margin = new System.Windows.Forms.Padding(0);
			this.MessagePanel.Name = "MessagePanel";
			this.MessagePanel.Size = new System.Drawing.Size(241, 12);
			this.MessagePanel.TabIndex = 3;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::Sardauscan.Properties.Resources.Down;
			this.pictureBox1.Location = new System.Drawing.Point(-14, -19);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(45, 41);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 3;
			this.pictureBox1.TabStop = false;
			// 
			// ListDragSource
			// 
			this.ListDragSource.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.ListDragSource.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ListDragSource.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.ListDragSource.FormattingEnabled = true;
			this.ListDragSource.ItemHeight = 24;
			this.ListDragSource.Location = new System.Drawing.Point(0, 0);
			this.ListDragSource.Lock = false;
			this.ListDragSource.Name = "ListDragSource";
			this.ListDragSource.Size = new System.Drawing.Size(240, 276);
			this.ListDragSource.TabIndex = 0;
			this.ListDragSource.MouseClick += new System.Windows.Forms.MouseEventHandler(this.List_Click);
			this.ListDragSource.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ListDragSource_DrawItem);
			this.ListDragSource.DragOver += new System.Windows.Forms.DragEventHandler(this.ListDragTarget_DragOver);
			this.ListDragSource.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.ListDragSource_GiveFeedback);
			this.ListDragSource.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.ListDragSource_QueryContinueDrag);
			this.ListDragSource.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ListDragSource_MouseDown);
			this.ListDragSource.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ListDragSource_MouseMove);
			this.ListDragSource.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ListDragSource_MouseUp);
			// 
			// LoadButton
			// 
			this.LoadButton.AllowDrop = true;
			this.LoadButton.BackColor = System.Drawing.Color.White;
			this.LoadButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.LoadButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(55)))), ((int)(((byte)(85)))));
			this.LoadButton.Image = global::Sardauscan.Properties.Resources.Load;
			this.LoadButton.Location = new System.Drawing.Point(138, 3);
			this.LoadButton.Name = "LoadButton";
			this.LoadButton.Size = new System.Drawing.Size(29, 29);
			this.LoadButton.TabIndex = 22;
			this.LoadButton.TabStop = false;
			this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
			// 
			// RunButton
			// 
			this.RunButton.BackColor = System.Drawing.Color.Transparent;
			this.RunButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.RunButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.RunButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(55)))), ((int)(((byte)(85)))));
			this.RunButton.Image = global::Sardauscan.Properties.Resources.Play;
			this.RunButton.Location = new System.Drawing.Point(3, 3);
			this.RunButton.Name = "RunButton";
			this.RunButton.Size = new System.Drawing.Size(32, 29);
			this.RunButton.TabIndex = 20;
			this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
			// 
			// SaveButton
			// 
			this.SaveButton.AllowDrop = true;
			this.SaveButton.BackColor = System.Drawing.Color.White;
			this.SaveButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.SaveButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(55)))), ((int)(((byte)(85)))));
			this.SaveButton.Image = global::Sardauscan.Properties.Resources.Save;
			this.SaveButton.Location = new System.Drawing.Point(173, 3);
			this.SaveButton.Name = "SaveButton";
			this.SaveButton.Size = new System.Drawing.Size(29, 29);
			this.SaveButton.TabIndex = 21;
			this.SaveButton.TabStop = false;
			this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
			// 
			// DeleteBox
			// 
			this.DeleteBox.AllowDrop = true;
			this.DeleteBox.BackColor = System.Drawing.Color.White;
			this.DeleteBox.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DeleteBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(55)))), ((int)(((byte)(85)))));
			this.DeleteBox.Image = global::Sardauscan.Properties.Resources.Trash;
			this.DeleteBox.Location = new System.Drawing.Point(208, 3);
			this.DeleteBox.Name = "DeleteBox";
			this.DeleteBox.Size = new System.Drawing.Size(29, 29);
			this.DeleteBox.TabIndex = 0;
			this.DeleteBox.TabStop = false;
			this.DeleteBox.Click += new System.EventHandler(this.DeleteBox_Click);
			this.DeleteBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.DeleteBox_DragDrop);
			this.DeleteBox.DragOver += new System.Windows.Forms.DragEventHandler(this.ListDragTarget_DragOver);
			this.DeleteBox.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.ListDragSource_GiveFeedback);
			// 
			// ListDragTarget
			// 
			this.ListDragTarget.AllowDrop = true;
			this.ListDragTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ListDragTarget.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.ListDragTarget.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.ListDragTarget.FormattingEnabled = true;
			this.ListDragTarget.ItemHeight = 24;
			this.ListDragTarget.Location = new System.Drawing.Point(1, 17);
			this.ListDragTarget.Lock = false;
			this.ListDragTarget.Name = "ListDragTarget";
			this.ListDragTarget.Size = new System.Drawing.Size(238, 216);
			this.ListDragTarget.TabIndex = 0;
			this.ListDragTarget.MouseClick += new System.Windows.Forms.MouseEventHandler(this.List_Click);
			this.ListDragTarget.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ListDragSource_DrawItem);
			this.ListDragTarget.DragDrop += new System.Windows.Forms.DragEventHandler(this.ListDragTarget_DragDrop);
			this.ListDragTarget.DragOver += new System.Windows.Forms.DragEventHandler(this.ListDragTarget_DragOver);
			this.ListDragTarget.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.ListDragSource_GiveFeedback);
			this.ListDragTarget.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ListDragSource_MouseDown);
			this.ListDragTarget.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ListDragSource_MouseMove);
			this.ListDragTarget.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ListDragSource_MouseUp);
			// 
			// pictureBox2
			// 
			this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox2.Image = global::Sardauscan.Properties.Resources.Down;
			this.pictureBox2.Location = new System.Drawing.Point(210, -19);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(45, 41);
			this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox2.TabIndex = 4;
			this.pictureBox2.TabStop = false;
			// 
			// DragDropTaskList
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.Controls.Add(this.splitContainer1);
			this.DoubleBuffered = true;
			this.Name = "DragDropTaskList";
			this.Size = new System.Drawing.Size(485, 278);
			this.Load += new System.EventHandler(this.DragDropTaskList_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.MessagePanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private TaskListBox ListDragSource;
        private TaskListBox ListDragTarget;
        private ImageButton DeleteBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ImageButton RunButton;
        private System.ComponentModel.BackgroundWorker TaskBackgroundWorker;
				private System.Windows.Forms.ToolTip ToolTip;
				private ImageButton LoadButton;
				private ImageButton SaveButton;
				private System.Windows.Forms.OpenFileDialog FileDialog;
				private System.Windows.Forms.Label MessageLabel;
				private System.Windows.Forms.Panel MessagePanel;
				private System.Windows.Forms.PictureBox pictureBox1;
				private System.Windows.Forms.PictureBox pictureBox2;
    }
}
