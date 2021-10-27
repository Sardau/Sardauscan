#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
#endregion
using Sardauscan.Gui.Controls;
namespace Sardauscan.Gui
{
    partial class ToolBoxView
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolBoxView));
			this.TaskListBox = new Sardauscan.Gui.Controls.DragDropTaskList();
			this.SuspendLayout();
			// 
			// TaskListBox
			// 
			this.TaskListBox.AllowDrop = true;
			this.TaskListBox.BackColor = System.Drawing.Color.White;
			this.TaskListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TaskListBox.In = Sardauscan.Core.ProcessingTask.eTaskItem.None;
			this.TaskListBox.Location = new System.Drawing.Point(0, 0);
			this.TaskListBox.Name = "TaskListBox";
			this.TaskListBox.Size = new System.Drawing.Size(272, 224);
			this.TaskListBox.TabIndex = 18;
			// 
			// ToolBoxView
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.Controls.Add(this.TaskListBox);
			this.Name = "ToolBoxView";
			this.Size = new System.Drawing.Size(272, 224);
			this.Load += new System.EventHandler(this.ToolBoxView_Load);
			this.ResumeLayout(false);

        }

        #endregion

        private DragDropTaskList TaskListBox;
    }
}
