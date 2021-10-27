#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
#endregion
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Security.Permissions;
using Sardauscan.Core;
using System.Drawing.Imaging;

namespace Sardauscan.Gui.Controls
{
	/// <summary>
	/// Custom button state enum
	/// </summary>
    public enum CustomButtonState
    {
        Normal = 1,
        Hot,
        Pressed,
        Disabled,
        Focused
    }

	/// <summary>
	/// Image button
	/// </summary>
    public class ImageButton : Control, IButtonControl
    {
			/// <summary>
			/// Default ctor
			/// </summary>
			public ImageButton()
            : base()
        {
            this.SetStyle(ControlStyles.Selectable | ControlStyles.StandardClick | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
            this.Text = string.Empty;
            //Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ForeColor = SkinInfo.ForeColor;
						BackColor = Color.Transparent;
            Image = global::Sardauscan.Properties.Resources.Mark_Question;
            Cursor = Cursors.Hand;
        }


        #region Private Instance Variables

        private DialogResult m_DialogResult;
        private bool m_IsDefault;

        private CustomButtonState m_ButtonState = CustomButtonState.Normal;

        private ContentAlignment m_TextAlign = ContentAlignment.MiddleCenter;
        private Image m_Image;

        private bool keyPressed;
        private Rectangle contentRect;

        #endregion

        #region IButtonControl Implementation

        [Category("Behavior"), DefaultValue(typeof(DialogResult), "None")]
        [Description("The dialog result produced in a modal form by clicking the button.")]
        public DialogResult DialogResult
        {
            get { return m_DialogResult; }
            set
            {
                if (Enum.IsDefined(typeof(DialogResult), value))
                    m_DialogResult = value;
            }
        }


        public void NotifyDefault(bool value)
        {
            if (m_IsDefault != value)
                m_IsDefault = value;
            this.Invalidate();
        }


        public void PerformClick()
        {
            if (this.CanSelect)
                base.OnClick(EventArgs.Empty);
        }


        #endregion

        #region Properties

        //ButtonState
        [Browsable(false)]
        public CustomButtonState ButtonState
        {
            get { return m_ButtonState; }
        }


        //DefaultSize
        protected override System.Drawing.Size DefaultSize
        {
            get { return new Size(75, 23); }
        }


        //IsDefault
        [Browsable(false)]
        public bool IsDefault
        {
            get { return m_IsDefault; }
        }


        //ImageIndex
        [Category("Appearance"), DefaultValue(-1)]
        [Description("The index of the image in the image list to display in the face of the control.")]
        //[TypeConverter(typeof(ImageIndexConverter))]
        //[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design", typeof(System.Drawing.Design.UITypeEditor))]
        public virtual Image Image
        {
            get { return m_Image; }
            set
            {
                m_Image = value;
                this.Invalidate();
            }
        }



        //TextAlign
        [Category("Appearance"), DefaultValue(typeof(ContentAlignment), "MiddleCenter")]
        [Description("The alignment of the text that will be displayed in the face of the control.")]
        public ContentAlignment TextAlign
        {
            get { return m_TextAlign; }
            set
            {
                if (!Enum.IsDefined(typeof(ContentAlignment), value))
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(ContentAlignment));
                if (m_TextAlign == value)
                    return;
                m_TextAlign = value;
                this.Invalidate();
            }
        }


        #endregion

        #region Overriden Methods

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Space)
            {
                keyPressed = true;
                m_ButtonState = CustomButtonState.Pressed;
            }
            OnStateChange(EventArgs.Empty);
        }


        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.KeyCode == Keys.Space)
            {
                if (this.ButtonState == CustomButtonState.Pressed)
                    this.PerformClick();
                keyPressed = false;
                m_ButtonState = CustomButtonState.Focused;
            }
            OnStateChange(EventArgs.Empty);
        }


        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (!keyPressed)
                m_ButtonState = CustomButtonState.Hot;
            OnStateChange(EventArgs.Empty);
        }


        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (!keyPressed)
                if (this.IsDefault)
                    m_ButtonState = CustomButtonState.Focused;
                else
                    m_ButtonState = CustomButtonState.Normal;
            OnStateChange(EventArgs.Empty);
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                this.Focus();
                m_ButtonState = CustomButtonState.Pressed;
            }
            OnStateChange(EventArgs.Empty);
        }


        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            m_ButtonState = CustomButtonState.Focused;
            OnStateChange(EventArgs.Empty);
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (new Rectangle(Point.Empty, this.Size).Contains(e.X, e.Y) && e.Button == MouseButtons.Left)
                m_ButtonState = CustomButtonState.Pressed;
            else
            {
                if (keyPressed)
                    return;
                m_ButtonState = CustomButtonState.Hot;
            }
            OnStateChange(EventArgs.Empty);
        }


        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            m_ButtonState = CustomButtonState.Focused;
            this.NotifyDefault(true);
        }


        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (this.FindForm().Focused)
                this.NotifyDefault(false);
            m_ButtonState = CustomButtonState.Normal;
        }


        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            if (this.Enabled)
                m_ButtonState = CustomButtonState.Normal;
            else
                m_ButtonState = CustomButtonState.Disabled;
            OnStateChange(EventArgs.Empty);
        }


        protected override void OnClick(EventArgs e)
        {
            //Click gets fired before MouseUp which is handy
            if (this.ButtonState == CustomButtonState.Pressed)
            {
                this.Focus();
                this.PerformClick();
            }
        }


        protected override void OnDoubleClick(EventArgs e)
        {
            if (this.ButtonState == CustomButtonState.Pressed)
            {
                this.Focus();
                this.PerformClick();
            }
        }


        protected override bool ProcessMnemonic(char charCode)
        {
            if (IsMnemonic(charCode, this.Text))
            {
                base.OnClick(EventArgs.Empty);
                return true;
            }
            return base.ProcessMnemonic(charCode);
        }


        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //Simulate Transparency
            System.Drawing.Drawing2D.GraphicsContainer g = pevent.Graphics.BeginContainer();
            Rectangle translateRect = this.Bounds;
            pevent.Graphics.TranslateTransform(-this.Left, -this.Top);
            PaintEventArgs pe = new PaintEventArgs(pevent.Graphics, translateRect);
            this.InvokePaintBackground(this.Parent, pe);
            this.InvokePaint(this.Parent, pe);
            pevent.Graphics.ResetTransform();
            pevent.Graphics.EndContainer(g);
            pevent.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

						Color fillColor = SkinInfo.BackColor;

            Rectangle r = this.ClientRectangle;


						Color col = GetParentBackColor(this);
            if (currentState == CustomButtonState.Hot)
            {
                col = col.ModifyLuminosity(-0.01);
            }

            using (Brush bgBrush = new SolidBrush(col))
            {
                pevent.Graphics.FillRectangle(bgBrush, r);
            }
						
            contentRect = r;
        }

				protected Color GetParentBackColor( Control ctrl)
				{
					if (ctrl.Parent == null)
						return SkinInfo.BackColor;
					if (ctrl.Parent.BackColor != Color.Transparent)
						return ctrl.Parent.BackColor;
					return GetParentBackColor(ctrl.Parent);
				}
        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle r = new Rectangle(0, 0, Width, Height);
            if (this.ButtonState == CustomButtonState.Pressed)
            {
                r.Offset(1, 1);
            }

            int bitmapSize = 0;
            if(Image!=null)
            {
                bitmapSize = Math.Min(Width,Height);
		        double factor = Math.Min( ((double)bitmapSize)/Image.Width, ((double)bitmapSize)/Image.Height);
                using (Bitmap img = new Bitmap(this.Image, new System.Drawing.Size((int)(factor * this.Image.Width), (int)(factor * this.Image.Height))))
                    DrawImage(e.Graphics,img,r.Location);
            }

            r.X +=bitmapSize;
            r.Width-=bitmapSize;
            DrawText(e.Graphics,r);
            //DrawFocus(e.Graphics);
            base.OnPaint(e);
        }


        protected override void OnParentBackColorChanged(EventArgs e)
        {
            base.OnParentBackColorChanged(e);
            this.Invalidate();
        }


        protected override void OnParentBackgroundImageChanged(EventArgs e)
        {
            base.OnParentBackgroundImageChanged(e);
            this.Invalidate();
        }


        #endregion

        #region Internal Draw Methods

        private void DrawImage(Graphics g,Image _Image,Point pt)
        {

					ImageAttributes ia = new ImageAttributes();
					if (currentState == CustomButtonState.Hot)
					{
						ia.SetColorMatrix(SkinInfo.HoverColorMatrix);
					}
					if (this.Enabled)
					{
						g.DrawImage(Image, new Rectangle(pt,_Image.Size), 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, ia);
					}
					else
						ControlPaint.DrawImageDisabled(g, _Image, pt.X, pt.Y, SkinInfo.BackColor);

        }


        private void DrawText(Graphics g,Rectangle R)
        {
					SolidBrush TextBrush = new SolidBrush(SkinInfo.ForeColor);

            if (!this.Enabled)
                TextBrush.Color = SystemColors.GrayText;

            StringFormat sf = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.NoClip);

            if (ShowKeyboardCues)
                sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
            else
                sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Hide;

            switch (this.TextAlign)
            {
                case ContentAlignment.TopLeft:
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Near;
                    break;

                case ContentAlignment.TopCenter:
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Near;
                    break;

                case ContentAlignment.TopRight:
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Near;
                    break;

                case ContentAlignment.MiddleLeft:
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Center;
                    break;

                case ContentAlignment.MiddleCenter:
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    break;

                case ContentAlignment.MiddleRight:
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Center;
                    break;

                case ContentAlignment.BottomLeft:
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Far;
                    break;

                case ContentAlignment.BottomCenter:
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Far;
                    break;

                case ContentAlignment.BottomRight:
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Far;
                    break;
            }

            if (this.Enabled)
                g.DrawString(this.Text, this.Font, TextBrush, R, sf);
            else
							ControlPaint.DrawStringDisabled(g, this.Text, this.Font, SkinInfo.BackColor, R, sf);

        }


        private void DrawFocus(Graphics g)
        {
            Rectangle r = contentRect;
            r.Inflate(1, 1);
            if (this.Focused && this.ShowFocusCues && this.TabStop)
							ControlPaint.DrawFocusRectangle(g, r, SkinInfo.ForeColor, SkinInfo.BackColor);
        }


        #endregion


        private CustomButtonState currentState;
        private void OnStateChange(EventArgs e)
        {
            //Repaint the button only if the state has actually changed
            if (this.ButtonState == currentState)
                return;
            currentState = this.ButtonState;
            this.Invalidate();
        }


    }
}
