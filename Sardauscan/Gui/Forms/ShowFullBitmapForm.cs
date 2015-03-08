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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace Sardauscan.Gui.Forms
{
	/// <summary>
	/// Form to show a full size bitmap
	/// </summary>
    public partial class ShowFullBitmapForm : Form
    {
			/// <summary>
			/// Default Ctor
			/// </summary>
        public ShowFullBitmapForm(PictureBox picture)
        {
            InitializeComponent();
            this.m_PictureBox = picture;
            picture.Click += new EventHandler(picture_Click);
            picture.Cursor = Cursors.Hand;
        }

        void picture_Click(object sender, EventArgs e)
        {
           // if(Bitmap!=null)
                this.ShowDialog();
        }

        private PictureBox m_PictureBox;
        public Image Bitmap { get { return m_PictureBox.Image; }}

        private void AlignControl()
        {
            if(Bitmap!=null)
            {
                this.panel.Width=Bitmap.Width;
                this.panel.Height = Bitmap.Height;
            }
        }
        
        private void panel_Paint(object sender, PaintEventArgs e)
        {
            AlignControl();
            if(Bitmap!=null)
                e.Graphics.DrawImage(Bitmap,new Point(0,0));
        }

        private void SavePictureBox_Click(object sender, EventArgs e)
        {
            if (/*Bitmap!=null && */saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = saveFileDialog1.FileName;
                string ext = Path.GetExtension(filename).Trim(".".ToCharArray());
                ImageCodecInfo codec = GetCodec(ext);
                if (codec == null)
                {
                    MessageBox.Show("Unknow Format extention:\n no file saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                Bitmap.Save(filename, codec, null);

            }
        }

        ImageCodecInfo GetCodec(string extention)
        {
            string ext = "*." + extention.ToLower();
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                string supported =codec.FilenameExtension.ToLower();
                if(supported.Contains(ext))
                       return codec;
            }
            return null;
        }
 
        private void ShowFullBitmapForm_Load(object sender, EventArgs e)
        {
            this.saveFileDialog1.Filter = GetImageFilter();
        }
        /// <summary>
        /// Get the Filter string for all supported image types.
        /// This can be used directly to the FileDialog class Filter Property.
        /// </summary>
        /// <returns></returns>
        public string GetImageFilter()
        {
            StringBuilder allImageExtensions = new StringBuilder();
            string separator = "";
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            Dictionary<string, string> images = new Dictionary<string, string>();
            foreach (ImageCodecInfo codec in codecs)
            {
                allImageExtensions.Append(separator);
                allImageExtensions.Append(codec.FilenameExtension);
                separator = ";";
                images.Add(string.Format("{0} Files: ({1})", codec.FormatDescription, codec.FilenameExtension),
                           codec.FilenameExtension);
            }
            StringBuilder sb = new StringBuilder();
            if (allImageExtensions.Length > 0)
            {
                sb.AppendFormat("{0}|{1}", "All Images", allImageExtensions.ToString());
            }
            images.Add("All Files", "*.*");
            foreach (KeyValuePair<string, string> image in images)
            {
                sb.AppendFormat("|{0}|{1}", image.Key, image.Value);
            }
            return sb.ToString();
        }

    }
}
