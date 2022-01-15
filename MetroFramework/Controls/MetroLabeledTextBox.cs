/**
 * MetroFramework - Modern UI for WinForms
 * 
 * The MIT License (MIT)
 * Copyright (c) 2021 Edwin Zimmerman, https://github.com/9cb14c1ec0
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of 
 * this software and associated documentation files (the "Software"), to deal in the 
 * Software without restriction, including without limitation the rights to use, copy, 
 * modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
 * and to permit persons to whom the Software is furnished to do so, subject to the 
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
 * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
 using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Components;
using MetroFramework.Design;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;

namespace MetroFramework.Controls
{
    public class MetroLabeledTextBox : Control, IMetroControl
    {
        #region Interface

        private MetroColorStyle metroStyle = MetroColorStyle.Blue;
        [Category("Metro Appearance")]
        public MetroColorStyle Style
        {
            get
            {
                if (StyleManager != null)
                    return StyleManager.Style;

                return metroStyle;
            }
            set { metroStyle = value; }
        }

        private MetroThemeStyle metroTheme = MetroThemeStyle.Light;
        [Category("Metro Appearance")]
        public MetroThemeStyle Theme
        {
            get
            {
                if (StyleManager != null)
                    return StyleManager.Theme;

                return metroTheme;
            }
            set { metroTheme = value; }
        }

        private MetroStyleManager metroStyleManager = null;
        [Browsable(false)]
        public MetroStyleManager StyleManager
        {
            get { return metroStyleManager; }
            set { metroStyleManager = value; }
        }

        #endregion

        #region Fields

        private Bitmap current_icon;
        private TextBox baseTextBox;

        private MetroIcons icon = MetroIcons.None;
        public MetroIcons Icon
        {
            get {return icon; }
            set { icon = value;
                    UpdateBaseTextBox(); }
        }

        private bool useStyleColors = false;
        [Category("Metro Appearance")]
        public bool UseStyleColors
        {
            get { return useStyleColors; }
            set { useStyleColors = value; UpdateBaseTextBox(); }
        }

        private MetroTextBoxSize metroTextBoxSize = MetroTextBoxSize.Small;
        [Category("Metro Appearance")]
        public MetroTextBoxSize FontSize
        {
            get { return metroTextBoxSize; }
            set { metroTextBoxSize = value; UpdateBaseTextBox(); }
        }

        private MetroTextBoxWeight metroTextBoxWeight = MetroTextBoxWeight.Regular;
        [Category("Metro Appearance")]
        public MetroTextBoxWeight FontWeight
        {
            get { return metroTextBoxWeight; }
            set { metroTextBoxWeight = value; UpdateBaseTextBox(); }
        }

        #endregion

        #region Routing Fields

        public override string Text
        {
            get { return baseTextBox.Text; }
            set { baseTextBox.Text = value; }
        }

        string internal_label = "";
        public string Label
        {
            get { return internal_label; }
            set { internal_label = value; Invalidate(); }
        }

        public char PasswordChar
        {
            get { return baseTextBox.PasswordChar; }
            set { baseTextBox.PasswordChar = value; }
        }

        [Browsable(false)]
        public string SelectedText
        {
            get { return baseTextBox.SelectedText;  }
            set { baseTextBox.Text = value; }
        }

        #endregion

        #region Constructor

        public MetroLabeledTextBox()
        {
            SetStyle(ControlStyles.DoubleBuffer, true);
            CreateBaseTextBox();
            UpdateBaseTextBox();
            AddEventHandler();
            this.TabStop = false;
        }

        #endregion

        #region Routing Methods

        public event EventHandler IconMouseUp;
        
        public event EventHandler IconMouseDown;

        public event EventHandler AcceptsTabChanged;
        private void BaseTextBoxAcceptsTabChanged(object sender, EventArgs e)
        {
            if (AcceptsTabChanged != null)
                AcceptsTabChanged(this, e);
        }
        
        private void BaseTextBoxSizeChanged(object sender, EventArgs e)
        {
            base.OnSizeChanged(e);
        }

        private void BaseTextBoxCursorChanged(object sender, EventArgs e)
        {
            base.OnCursorChanged(e);
        }

        private void BaseTextBoxContextMenuStripChanged(object sender, EventArgs e)
        {
            base.OnContextMenuStripChanged(e);
        }

        private void BaseTextBoxClientSizeChanged(object sender, EventArgs e)
        {
            base.OnClientSizeChanged(e);
        }

        private void BaseTextBoxClick(object sender, EventArgs e)
        {
            base.OnClick(e);
        }

        private void BaseTextBoxChangeUiCues(object sender, UICuesEventArgs e)
        {
            base.OnChangeUICues(e);
        }

        private void BaseTextBoxCausesValidationChanged(object sender, EventArgs e)
        {
            base.OnCausesValidationChanged(e);
        }

        private void BaseTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            base.OnKeyUp(e);
        }

        private void BaseTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
        }

        private void BaseTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }

        private void BaseTextBoxTextChanged(object sender, EventArgs e)
        {
            base.OnTextChanged(e);
        }

        public void Select(int start, int length)
        {
            baseTextBox.Select(start, length);
        }

        public void SelectAll()
        {
            baseTextBox.SelectAll();
        }

        #endregion

        #region Paint Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            e.Graphics.Clear(MetroPaint.BackColor.Button.Normal(Theme));
            baseTextBox.BackColor = MetroPaint.BackColor.Button.Normal(Theme);
            baseTextBox.ForeColor = MetroPaint.ForeColor.Button.Normal(Theme);

            Color borderColor = MetroPaint.BorderColor.Button.Normal(Theme);
            if (useStyleColors)
                borderColor = MetroPaint.GetStyleColor(Style);

            using (Pen p = new Pen(borderColor))
            {
                e.Graphics.DrawRectangle(p, new Rectangle(0, 0, Width - 1, Height - 1));
            }

            if(icon != MetroIcons.None)
            {
                current_icon = MetroIconUtils.GetMetroIconBitmap(icon, MetroPaint.GetStyleColor(Style));
                e.Graphics.DrawImage(current_icon, new Point(Width - 30, (Height / 2) - (current_icon.Height / 2)  ));
            }

            var f = MetroFonts.Label(MetroLabelSize.Small, MetroLabelWeight.Light);
            e.Graphics.DrawString(internal_label, f, System.Drawing.Brushes.Gray,
                                    new Point(2,2));
        }

        #endregion

        #region Overridden Methods

        public virtual new void Focus()
        {
            baseTextBox.Focus();
        }

        public override void Refresh()
        {
            base.Refresh();
            UpdateBaseTextBox();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateBaseTextBox();
        }

        #endregion

        #region Private Methods

        private void CreateBaseTextBox()
        {
            if (baseTextBox != null) return;

            baseTextBox = new TextBox();

            baseTextBox.BorderStyle = BorderStyle.None;
            baseTextBox.Font = MetroFonts.TextBox(metroTextBoxSize, metroTextBoxWeight);
            baseTextBox.Location = new Point(3, 21);
            
            if(Icon != MetroIcons.None)
            {
                baseTextBox.Size = new Size(Width - 36, Height - 26);
                Size = new Size(baseTextBox.Width + 36, baseTextBox.Height + 26);
            }
            else
            {
                baseTextBox.Size = new Size(Width - 6, Height - 26);
                Size = new Size(baseTextBox.Width + 6, baseTextBox.Height + 26);
            }

            Controls.Add(baseTextBox);
        }

        private void AddEventHandler()
        {
            baseTextBox.AcceptsTabChanged += BaseTextBoxAcceptsTabChanged;

            baseTextBox.CausesValidationChanged += BaseTextBoxCausesValidationChanged;
            baseTextBox.ChangeUICues += BaseTextBoxChangeUiCues;
            baseTextBox.Click += BaseTextBoxClick;
            baseTextBox.ClientSizeChanged += BaseTextBoxClientSizeChanged;

            baseTextBox.ContextMenuStripChanged += BaseTextBoxContextMenuStripChanged;
            baseTextBox.CursorChanged += BaseTextBoxCursorChanged;

            baseTextBox.KeyDown += BaseTextBoxKeyDown;
            baseTextBox.KeyPress += BaseTextBoxKeyPress;
            baseTextBox.KeyUp += BaseTextBoxKeyUp;

            baseTextBox.SizeChanged += BaseTextBoxSizeChanged;

            baseTextBox.TextChanged += BaseTextBoxTextChanged;

            this.MouseDown += ControlMouseDown;
            this.MouseUp += ControlMouseUp;
            this.IconMouseUp += (object sender, EventArgs e) => {};
            this.IconMouseDown += (object sender, EventArgs e) => {};
        }


        private void ControlMouseDown(object sender, MouseEventArgs e)
        {
            if(Icon == MetroIcons.None) 
            {
                baseTextBox.Focus();
                return;
            }

            var icon_left = Width - 30;
            var icon_right = Width;


            if(e.X >= icon_left & e.X <= icon_right)
            {
                    IconMouseDown(this, new EventArgs());
            }
            else
            {
                baseTextBox.Focus();
            }
        }
        private void ControlMouseUp(object sender, MouseEventArgs e)
        {
            if(Icon == MetroIcons.None) return;

            var icon_left = Width - 30;
            var icon_right = Width;


            if(e.X >= icon_left & e.X <= icon_right)
            {
                    IconMouseUp(this, new EventArgs());
            }
        }

        private void UpdateBaseTextBox()
        {
            if (baseTextBox == null) return;

            baseTextBox.Font = MetroFonts.TextBox(metroTextBoxSize, metroTextBoxWeight);

            baseTextBox.Location = new Point(3, 21);

            if(Icon != MetroIcons.None)
            {
                baseTextBox.Size = new Size(Width - 36, Height - 26);
            }
            else
            {
                baseTextBox.Size = new Size(Width - 6, Height - 26);
            }
            
        }

        #endregion
    }

    public class MetroLabeledPasswordBox: MetroLabeledTextBox
    {
        public MetroLabeledPasswordBox() : base()
        {
            Icon = MetroIcons.Eye;
            PasswordChar = '•';
            IconMouseDown += (object sender, EventArgs e) => { PasswordChar = '\0'; };
            IconMouseUp += (object sender, EventArgs e) => { PasswordChar = '•'; };
        }

    }
}
