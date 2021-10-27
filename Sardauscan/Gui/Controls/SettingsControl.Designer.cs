#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
#endregion
using Sardauscan.Gui.Controls;
namespace Sardauscan.Gui.Controls
{
	/// <summary>
	/// Control to view/edit ApplicationSettings
	/// </summary>
    partial class SettingsControl
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
					this.Grid = new System.Windows.Forms.PropertyGrid();
			this.SuspendLayout();
			// 
			// propertyGrid1
			// 
			this.Grid.CommandsDisabledLinkColor = System.Drawing.SystemColors.InactiveCaption;
			this.Grid.CommandsLinkColor = System.Drawing.SystemColors.HotTrack;
			this.Grid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Grid.Location = new System.Drawing.Point(0, 0);
			this.Grid.Name = "propertyGrid1";
			this.Grid.Size = new System.Drawing.Size(454, 349);
			this.Grid.TabIndex = 2;
			this.Grid.ToolbarVisible = false;
			// 
			// SettingsView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.Grid);
			this.Name = "SettingsView";
			this.Size = new System.Drawing.Size(454, 349);
			this.VisibleChanged += new System.EventHandler(this.SettingsView_VisibleChanged);
			this.ResumeLayout(false);

        }

        #endregion

				private System.Windows.Forms.PropertyGrid Grid;
    }
}
