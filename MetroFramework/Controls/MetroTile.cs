﻿/**
 * MetroFramework - Modern UI for WinForms
 * 
 * The MIT License (MIT)
 * Copyright (c) 2011 Sven Walter, http://github.com/viperneo
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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Windows.Forms;

using MetroFramework.Drawing;
using MetroFramework.Design;
using MetroFramework.Components;
using MetroFramework.Interfaces;
using MetroFramework.Forms;

namespace MetroFramework.Controls
{
    [Designer(typeof(MetroTileDesigner))]
    [ToolboxBitmap(typeof(Button))]
    public class MetroTile : Button, IContainerControl, IMetroControl
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

        private Control activeControl = null;
        [Browsable(false)]
        public Control ActiveControl
        {
            get { return activeControl; }
            set { activeControl = value; }
        }

        public bool ActivateControl(Control ctrl)
        {
            if (Controls.Contains(ctrl))
            {
                ctrl.Select();
                activeControl = ctrl;
                return true;
            }

            return false;
        }

        #endregion

        #region Fields

        [Browsable(false)]
        public override Color BackColor
        {
            get
            {
                return MetroPaint.BackColor.Form(Theme);
            }
        }

        private bool isHovered = false;
        private bool isPressed = false;
        private bool isFocused = false;

        #endregion

        #region Constructor

        public MetroTile()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            TextAlign = ContentAlignment.BottomLeft;
            Font = MetroFonts.Tile;
        }

        #endregion

        #region Paint Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            Color backColor, foreColor;

            backColor = MetroPaint.GetStyleColor(Style);

            if (isHovered && !isPressed && Enabled)
            {
                foreColor = MetroPaint.ForeColor.Tile.Hover(Theme);
            }
            else if (isHovered && isPressed && Enabled)
            {
                foreColor = MetroPaint.ForeColor.Tile.Press(Theme);
            }
            else if (!Enabled)
            {
                foreColor = MetroPaint.ForeColor.Tile.Disabled(Theme);
            }
            else
            {
                foreColor = MetroPaint.ForeColor.Tile.Normal(Theme);
            }

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            // get the parent's BackColor or Theme color
            Color parent_color;
            if(Theme == MetroThemeStyle.Dark)
            {
                parent_color = Color.FromArgb(17,17,17);
            }
            else
            {
                parent_color = Color.White;
            }
            
            if (!isPressed)
            {
                e.Graphics.Clear(parent_color);
                using (SolidBrush b = new SolidBrush(MetroPaint.GetStyleColor(Style)))
                {
                    var path = new System.Drawing.Drawing2D.GraphicsPath();
                    path.AddArc(new Rectangle(0, 0, 12, 12),180, 90);
                    path.AddLine(new Point(6,0), new Point(Width-6, 0));
                    path.AddArc(new Rectangle(Width-13, 0, 12, 12), 270, 90);
                    path.AddLine(new Point(Width-1, 6), new Point(Width-1,Height-12));
                    path.AddArc(new Rectangle(Width-13, Height - 13, 12, 12), 0, 90);
                    path.AddLine(new Point(Width-7,Height-1), new Point(6,Height-1));
                    path.AddArc(new Rectangle(0, Height - 13, 12, 12), 90, 90);
                    path.AddLine(new Point(0,Height-6), new Point(0,6));
                    e.Graphics.FillPath(b, path);
                    e.Graphics.DrawPath(new Pen(Color.Black), path);
                }
            }
            else
            {
                e.Graphics.Clear(parent_color);
                using (SolidBrush b = MetroPaint.GetStyleBrush(Style))
                {
                    var path = new System.Drawing.Drawing2D.GraphicsPath();
                    path.AddArc(new Rectangle(0, 0, 12, 12),180, 90);
                    path.AddLine(new Point(6,0), new Point(Width-4, 2));
                    path.AddArc(new Rectangle(Width-7, 2, 6, 6), 270, 90);
                    path.AddLine(new Point(Width-1, 5), new Point(Width-1,Height-8));
                    path.AddArc(new Rectangle(Width-7, Height - 8, 6, 6), 0, 90);
                    path.AddLine(new Point(Width-4,Height-2), new Point(0,Height));
                    path.AddArc(new Rectangle(0, Height - 12, 12, 12), 90, 90);
                    path.AddLine(new Point(0,Height-6), new Point(0,6));
                    e.Graphics.FillPath(b, path);
                    e.Graphics.DrawPath(new Pen(Color.Black), path);
                }
            }

            Size textSize = TextRenderer.MeasureText(Text, MetroFonts.Tile);
            var top = 0;
            var left = 0;
            switch(TextAlign)
            {
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    top = Height - textSize.Height;
                    break;
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                    top = (Height / 2) - (textSize.Height / 2);
                    break;
                default:
                    break;
            }

            switch(TextAlign)
            {
                case ContentAlignment.BottomCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.TopCenter:
                    left = (Width / 2) - (textSize.Width / 2);
                    break;
                case ContentAlignment.BottomRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.TopRight:
                    left = Width - textSize.Width;
                    break;
                default:
                    break;
            }

            TextRenderer.DrawText(e.Graphics, Text, Font, new Point(left, top), foreColor);


            if (false && isFocused)
                ControlPaint.DrawFocusRectangle(e.Graphics, ClientRectangle);
        }

        #endregion

        #region Focus Methods

        protected override void OnGotFocus(EventArgs e)
        {
            isFocused = true;
            Invalidate();

            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            isFocused = false;
            isHovered = false;
            isPressed = false;
            Invalidate();

            base.OnLostFocus(e);
        }

        protected override void OnEnter(EventArgs e)
        {
            isFocused = true;
            Invalidate();

            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            isFocused = false;
            isHovered = false;
            isPressed = false;
            Invalidate();

            base.OnLeave(e);
        }

        #endregion

        #region Keyboard Methods

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                isHovered = true;
                isPressed = true;
                Invalidate();
            }

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            isHovered = false;
            isPressed = false;
            Invalidate();

            base.OnKeyUp(e);
        }

        #endregion

        #region Mouse Methods

        protected override void OnMouseEnter(EventArgs e)
        {
            isHovered = true;
            Invalidate();

            base.OnMouseEnter(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isPressed = true;
                Invalidate();
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            isPressed = false;
            Invalidate();

            base.OnMouseUp(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            isHovered = false;
            Invalidate();

            base.OnMouseLeave(e);
        }

        #endregion

        #region Overridden Methods

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        #endregion
    }
}
