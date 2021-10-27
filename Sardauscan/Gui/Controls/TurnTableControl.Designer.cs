#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
#endregion
namespace Sardauscan.Gui.Controls
{
    partial class TurnTableControl
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
            this.SuspendLayout();
            // 
            // TurnTableView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.Highlight;
            this.Name = "TurnTableView";
            this.Size = new System.Drawing.Size(321, 203);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TableFrom_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TableFrom_MouseDown);
            this.MouseEnter += new System.EventHandler(this.TableFrom_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.TableFrom_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TableFrom_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TableFrom_MouseUp);
            this.Resize += new System.EventHandler(this.TurnTableView_Resize);
            this.ResumeLayout(false);

        }

        #endregion

    }
}
