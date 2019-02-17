﻿using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace Sc
{
    public class ScTabHeadItem : ScLayer
    {
        ScTabHead scTabHead;


        //Color color = Color.FromArgb(234, 232, 233);
        //Color normalColor = Color.FromArgb(234, 232, 233);
        //Color enterColor = Color.FromArgb(248, 248, 245);
        //Color selectedColor = Color.FromArgb(153, 114, 49);
        //Color itemBoxColor = Color.FromArgb(255, 191, 152, 90);
        //Color normalFontColor = Color.FromArgb(255, 191, 152, 90);
        //Color selectFontColor = Color.FromArgb(255, 233, 233, 233);

        Color color = Color.FromArgb(202, 230, 255);
        Color normalColor = Color.FromArgb(202, 230, 255);
        Color enterColor = Color.FromArgb(248, 248, 245);
        Color selectedColor = Color.FromArgb(51, 153, 255);
        Color itemBoxColor = Color.FromArgb(255, 240, 240, 240);
        Color normalFontColor = Color.FromArgb(255, 0, 0, 0);
        Color selectFontColor = Color.FromArgb(255, 255, 255, 255);

        ScAnimation scAnim;
        ScLinearAnimation linearR;
        ScLinearAnimation linearG;
        ScLinearAnimation linearB;

        public int index;
        public int upOrBottom = 0;


        RectangleF msgHintsRect = new RectangleF(0, 0, 20, 20);
        int msgHintsCount = 0;
        bool useMsgHints = false;

        ScTabItemStyle style = ScTabItemStyle.Style1;
        float gradientSize = 0;


        public delegate void AnimalStopEventHandler(ScTabHeadItem selectedItem);
        public event AnimalStopEventHandler AnimalStopEvent = null;


        public ScTabHeadItem(ScMgr scMgr, ScTabHead scTabHead)
           : base(scMgr)
        {
            this.scTabHead = scTabHead;
            this.MouseDown += SimpleTabHeadItem_MouseDown;
            this.MouseEnter += ScTabHeadItem_MouseEnter;
            this.MouseLeave += ScTabHeadItem_MouseLeave;

            this.D2DPaint += ScTabHeadItem_D2DPaint;
            this.GDIPaint += ScTabHeadItem_GDIPaint;

            scAnim = new ScAnimation(this, 50, true);
            scAnim.AnimationEvent += ScAnim_AnimationEvent;

             IsUseOrgHitGeometry = false;
        }


        public bool UseMsgHints
        {
            get
            {
                return useMsgHints;
            }

            set
            {
                useMsgHints = value;
                Refresh();
            }
        }

        public int MsgHintsCount
        {
            get
            {
                return msgHintsCount;
            }

            set
            {
                msgHintsCount = value;
                Refresh();
            }
        }

        protected override Geometry CreateHitGeometryByD2D(D2DGraphics g)
        {
            RectangleF rect = new RectangleF(0, 0, Width, Height);

            PathGeometry pathGeometry = new PathGeometry(D2DGraphics.d2dFactory);

            GeometrySink pSink = null;
            pSink = pathGeometry.Open();
            pSink.SetFillMode(SharpDX.Direct2D1.FillMode.Winding);

            pSink.BeginFigure(new RawVector2(rect.Left, rect.Bottom), FigureBegin.Filled);

            float len = gradientSize;
            RawVector2[] points =
                {
                new RawVector2(rect.Left + len, rect.Top),
                new RawVector2(rect.Right - len,rect.Top),
                new RawVector2(rect.Right, rect.Bottom),
            };

            pSink.AddLines(points);
            pSink.EndFigure(FigureEnd.Closed);
            pSink.Close();
            pSink.Dispose();

            return pathGeometry;
        }

        protected override GraphicsPath CreateHitGeometryByGDIPLUS(GDIGraphics g)
        {
            GraphicsPath graphPath = null;

            switch (style)
            {
                case ScTabItemStyle.Style1:
                    graphPath = CreateHitGeometryStyle1ByGDIPLUS();
                    break;

                case ScTabItemStyle.Style2:
                    break;
            }

            return graphPath;
        }



        GraphicsPath CreateHitGeometryStyle1ByGDIPLUS()
        {
            RectangleF r = new RectangleF(0, 0, Width, Height);
            float len = gradientSize;

            PointF[] points1 =
           {
                new PointF(r.Left, r.Bottom),
                new PointF(r.Left + len, r.Top),
                new PointF(r.Right - len,r.Top),
                new PointF(r.Right, r.Bottom)
            };


            PointF[] points2 =
          {
                new PointF(r.Left, r.Top),
                new PointF(r.Left + len, r.Bottom - 1),
                new PointF(r.Right - len,r.Bottom - 1),
                new PointF(r.Right, r.Top)
            };

            PointF[] points;

            if (upOrBottom == 0)
                points = points1;
            else
                points = points2;

            GraphicsPath myPath = new GraphicsPath();//建立GraphicsPath()类对象
            myPath.AddLines(points);

            return myPath;
        }

        private void ScTabHeadItem_D2DPaint(D2DGraphics g)
        {
            FillItemGeometry(g);
            DrawItemGeometry(g);

            Graphics gdiGraph = g.CreateGdiGraphics();
            DrawString(gdiGraph);

            g.RelaseGdiGraphics(gdiGraph);

        }


        void FillItemGeometry(D2DGraphics g)
        {
            RectangleF rect = new RectangleF(0, 0, Width, Height);

            PathGeometry pathGeometry = new PathGeometry(D2DGraphics.d2dFactory);

            GeometrySink pSink = null;
            pSink = pathGeometry.Open();
            pSink.SetFillMode(SharpDX.Direct2D1.FillMode.Winding);

            pSink.BeginFigure(new RawVector2(rect.Left, rect.Bottom), FigureBegin.Filled);

            float len = gradientSize;
            RawVector2[] points =
                {
                new RawVector2(rect.Left + len, rect.Top),
                new RawVector2(rect.Right - len,rect.Top),
                new RawVector2(rect.Right, rect.Bottom),
            };

            pSink.AddLines(points);
            pSink.EndFigure(FigureEnd.Closed);
            pSink.Close();
            pSink.Dispose();

            RawColor4 rawColor = GDIDataD2DUtils.TransToRawColor4(color);
            SolidColorBrush brush = new SolidColorBrush(g.RenderTarget, rawColor);
            g.RenderTarget.FillGeometry(pathGeometry, brush);
        }


        void DrawItemGeometry(D2DGraphics g)
        {
            RectangleF rect = new RectangleF(0, 0, Width, Height);

            PathGeometry pathGeometry = new PathGeometry(D2DGraphics.d2dFactory);

            GeometrySink pSink = null;
            pSink = pathGeometry.Open();
            pSink.SetFillMode(SharpDX.Direct2D1.FillMode.Winding);

            pSink.BeginFigure(new RawVector2(rect.Left, rect.Bottom), FigureBegin.Filled);

            float len = gradientSize;
            RawVector2[] points =
                {
                new RawVector2(rect.Left + len, rect.Top),
                new RawVector2(rect.Right - len,rect.Top),
                new RawVector2(rect.Right, rect.Bottom),
                 new RawVector2(rect.Right -1, rect.Bottom),
                 new RawVector2(rect.Right - len - 0.5f,rect.Top + 1),
                  new RawVector2(rect.Left + len + 0.5f,rect.Top + 1),
                  new RawVector2(rect.Left + 1,rect.Bottom)
            };

            pSink.AddLines(points);
            pSink.EndFigure(FigureEnd.Closed);
            pSink.Close();
            pSink.Dispose();

            RawColor4 rawColor = GDIDataD2DUtils.TransToRawColor4(Color.FromArgb(255, 191, 152, 90));
            SolidColorBrush brush = new SolidColorBrush(g.RenderTarget, rawColor);
            g.RenderTarget.FillGeometry(pathGeometry, brush);
        }


        private void ScTabHeadItem_GDIPaint(GDIGraphics g)
        {
            Graphics gdiGraph = g.GdiGraph;
            DrawString(gdiGraph);
        }

        void DrawString(Graphics g)
        {
            Graphics gdiGraph = g;
            gdiGraph.SmoothingMode = SmoothingMode.HighQuality;// 指定高质量、低速度呈现。
            gdiGraph.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            float msgHintsWidth = 0;
            RectangleF rect = new RectangleF(0, 0, Width, Height);
            DrawItemRect(gdiGraph, rect);

            if (useMsgHints && msgHintsCount > 0)
            {
                msgHintsWidth = msgHintsRect.Width / 1.5f;
            }

            //
            rect = new RectangleF(gradientSize, 0, Width - gradientSize * 2 - msgHintsWidth, Height);
            Font font = new Font("微软雅黑", 10);

            System.Drawing.Brush brush2;
            if (this == scTabHead.GetSelectedItem())
                brush2 = new SolidBrush(selectFontColor);
            else
                brush2 = new SolidBrush(normalFontColor);

            DrawUtils.LimitBoxDraw(gdiGraph, Name, font, brush2, rect, true, 0);


            if (useMsgHints && msgHintsCount > 0)
            {
                msgHintsWidth = msgHintsRect.Width;
                float x = Width - msgHintsRect.Width - gradientSize - 5;
                rect = new RectangleF(x, 4, msgHintsRect.Width, msgHintsRect.Height);

                SolidBrush brush = new SolidBrush(Color.FromArgb(200, 255, 0, 0));
                gdiGraph.FillEllipse(brush, rect);

                DrawUtils.LimitBoxDraw(gdiGraph, msgHintsCount.ToString(), font, Brushes.White, rect, true, 0);
            }
        }

        void DrawItemRect(Graphics g, RectangleF rect)
        {
            ScTabHeadItem item = scTabHead.GetSelectedItem();

            float len = gradientSize;
            GraphicsPath myPath = new GraphicsPath();//建立GraphicsPath()类对象


            PointF[] points1 =
           {
                new PointF(rect.Left, rect.Bottom),
                new PointF(rect.Left + len, rect.Top),
                new PointF(rect.Right - len - 1,rect.Top),
                new PointF(rect.Right - 1, rect.Bottom)
            };


            PointF[] points2 =
          {
                new PointF(rect.Left, rect.Top),
                new PointF(rect.Left + len, rect.Bottom - 1),
                new PointF(rect.Right - len,rect.Bottom - 1),
                new PointF(rect.Right, rect.Top)
            };

            PointF[] points;

            if (upOrBottom == 0)
                points = points1;
            else
                points = points2;


            myPath.AddLines(points);

            Pen pen = new Pen(itemBoxColor, 1.5f);
            System.Drawing.Brush brush = new SolidBrush(color);
            g.FillPath(brush, myPath);
            g.DrawPath(pen, myPath);

        }


        public void SetSelectedItem()
        {
            color = selectedColor;
        }


        public void SetNormalItem()
        {
            color = normalColor;
        }


        private void SimpleTabHeadItem_MouseDown(object sender, ScMouseEventArgs e)
        {
            if (this == scTabHead.GetSelectedItem())
                return;

            StartAnim(selectedColor);
            scTabHead.MouseDownItem(this);
        }

        private void ScTabHeadItem_MouseEnter(object sender, ScMouseEventArgs e)
        {
            ScTabHeadItem item = scTabHead.GetSelectedItem();
            if (item == this)
                return;

            StartAnim(enterColor);
        }


        private void ScTabHeadItem_MouseLeave(object sender)
        {
            ScTabHeadItem item = scTabHead.GetSelectedItem();
            if (item == this)
                return;

            StartAnim(normalColor);
        }

        public void StartLeaveAnim()
        {
            StartAnim(normalColor);
        }
        public void StartAnim(Color stopColor)
        {
            scAnim.Stop();

            linearR = new ScLinearAnimation(color.R, stopColor.R, scAnim);
            linearG = new ScLinearAnimation(color.G, stopColor.G, scAnim);
            linearB = new ScLinearAnimation(color.B, stopColor.B, scAnim);
            scAnim.Start();
        }

        private void ScAnim_AnimationEvent(ScAnimation scAnimation)
        {
            int r, g, b;

            r = (int)linearR.GetCurtValue();
            g = (int)linearG.GetCurtValue();
            b = (int)linearB.GetCurtValue();

            color = Color.FromArgb(r, g, b);

            Refresh();
            Update();

            if (linearR.IsStop && linearG.IsStop && linearB.IsStop)
            {
                scAnimation.Stop();

                if (AnimalStopEvent != null)
                    AnimalStopEvent(this);
            }
        }

    }
}
