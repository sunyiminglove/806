using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Modustest
{
    class MyCv
    {
        /// <summary>
        /// 在图片上画框
        /// </summary>
        /// <param name="bmp">原始图</param>
        /// <param name="p0">起始点</param>
        /// <param name="p1">终止点</param>
        /// <param name="RectColor">矩形框颜色</param>
        /// <param name="LineWidth">矩形框边界</param>
        /// <returns></returns>
        public static Bitmap DrawRectangle(Bitmap bmp, Point p0, Point p1, Color RectColor, int LineWidth, DashStyle ds)
        {
            if (bmp == null) return null;
            Graphics g = Graphics.FromImage(bmp);
            Brush brush = new SolidBrush(RectColor);
            Pen pen = new Pen(brush, LineWidth);
            pen.DashStyle = ds;
            g.DrawRectangle(pen, new Rectangle(p0.X, p0.Y, Math.Abs(p0.X - p1.X), Math.Abs(p0.Y - p1.Y)));
            g.Dispose();
            return bmp;
        }
        // 在图片上实心矩形
        public static Bitmap DrawRectangle(Bitmap bmp, Color color, Rectangle rect)
        {
            SolidBrush brush1 = new SolidBrush(color);
            Graphics formGraphics = Graphics.FromImage(bmp);
            formGraphics.FillRectangle(brush1, rect);
            brush1.Dispose();
            formGraphics.Dispose();
            return bmp;
        }

        // 在图片上实心圆形
        public static Bitmap DrawCircle(Bitmap bmp, Color color, Rectangle rect)
        {
            SolidBrush brush1 = new SolidBrush(color);
            Graphics formGraphics = Graphics.FromImage(bmp);
            formGraphics.FillEllipse(brush1,rect);
            brush1.Dispose();
            formGraphics.Dispose();
            return bmp;
        }
        /// <summary>
        /// 在图片上画框
        /// </summary>
        /// <param name="bmp">原始图</param>
        /// <param name="p0">起始点</param>
        /// <param name="p1">终止点</param>
        /// <param name="RectColor">矩形框颜色</param>
        /// <param name="LineWidth">矩形框边界</param>
        /// <returns></returns>
        public static Bitmap DrawRectangle(Bitmap bmp, Point location, Size size, Color RectColor, int LineWidth)
        {
            if (bmp == null) return null;

            Graphics g = Graphics.FromImage(bmp);

            Brush brush = new SolidBrush(RectColor);
            Pen pen = new Pen(brush, LineWidth);
            pen.DashStyle = DashStyle.Custom;

            g.DrawRectangle(pen, new Rectangle(location, size));

            g.Dispose();

            return bmp;
        }

        //============================在图片上画椭圆==============================
        /// <summary>
        /// 在图片上画椭圆
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="p0"></param>
        /// <param name="RectColor"></param>
        /// <param name="LineWidth"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static Bitmap DrawRoundInPicture(Bitmap bmp, Point p0, Point p1, Color RectColor, int LineWidth, DashStyle ds)
        {
            if (bmp == null) return null;

            Graphics g = Graphics.FromImage(bmp);

            Brush brush = new SolidBrush(RectColor);
            Pen pen = new Pen(brush, LineWidth);
            pen.DashStyle = ds;

            g.DrawEllipse(pen, new Rectangle(p0.X, p0.Y, Math.Abs(p0.X - p1.X), Math.Abs(p0.Y - p1.Y)));

            g.Dispose();

            return bmp;
        }



        //============================在图片上画线==============================
        /// <summary>
        /// 在图片上画线
        /// </summary>
        /// <param name="bmp">原始图</param>
        /// <param name="p0">起始点</param>
        /// <param name="p1">终止点</param>
        /// <param name="RectColor">线的颜色</param>
        /// <param name="LineWidth">线宽</param>
        /// <param name="ds">线条样式</param>
        /// <returns>输出图</returns>
        public static Bitmap DrawLine(Bitmap bmp, Point p0, Point p1, Color LineColor, int LineWidth, DashStyle ds)
        {
            if (bmp == null) return null;

            if (p0.X == p1.X || p0.Y == p1.Y) return bmp;

            Graphics g = Graphics.FromImage(bmp);

            Brush brush = new SolidBrush(LineColor);

            Pen pen = new Pen(brush, LineWidth);
            //pen.Alignment = PenAlignment.Inset;

            pen.DashStyle = ds;

            g.DrawLine(pen, p0, p1);

            g.Dispose();

            return bmp;
        }
        public static Bitmap DrawLine(Bitmap bmp, Point location, int lonth, Color LineColor, int LineWidth)
        {
            if (bmp == null) return null;

            if (lonth <= 0) return bmp;

            Graphics g = Graphics.FromImage(bmp);

            Brush brush = new SolidBrush(LineColor);

            Pen pen = new Pen(brush, LineWidth);
            //pen.Alignment = PenAlignment.Inset;

            pen.DashStyle = DashStyle.Solid;

            g.DrawLine(pen, location.X - lonth / 2, location.Y, location.X + lonth / 2, location.Y);

            g.Dispose();

            return bmp;
        }


        //============================在图片上画中心位置十字线==============================
        /// <summary>
        /// 在图片上画中心位置十字线
        /// </summary>
        /// <param name="bmp">输入图像</param>
        /// <param name="Centerlocation">十字线中心点坐标</param>
        /// <param name="lonth">十字线长度</param>
        /// <param name="LineColor">颜色</param>
        /// <param name="LineWidth">线宽</param>
        /// <returns>返回处理结果图片</returns>
        public static Bitmap DrawCross(Bitmap bmp, Point Centerlocation, int lonth, Color LineColor, int LineWidth)
        {
            if (bmp == null) return null;

            if (lonth <= 0) return bmp;

            Graphics g = Graphics.FromImage(bmp);

            Brush brush = new SolidBrush(LineColor);

            Pen pen = new Pen(brush, LineWidth);
            //pen.Alignment = PenAlignment.Inset;

            pen.DashStyle = DashStyle.Solid;

            g.DrawLine(pen, Centerlocation.X - lonth / 2, Centerlocation.Y, Centerlocation.X + lonth / 2, Centerlocation.Y);
            g.DrawLine(pen, Centerlocation.X, Centerlocation.Y - lonth / 2, Centerlocation.X, Centerlocation.Y + lonth / 2);

            g.Dispose();

            return bmp;
        }

        //============================在图片上画四个角点的定位框==============================
        /// <summary>
        /// 在图片上定位框
        /// </summary>
        /// <param name="bmp">输入图像</param>
        /// <param name="LineColor">颜色</param>
        /// <param name="LineWidth">线宽</param>
        /// <returns>返回处理结果图片</returns>
        public static Bitmap DrawFourAngle(Bitmap bmp, Color LineColor, int LineWidth)
        {
            if (bmp == null) return null;

            int lonth = (int)(bmp.Width * 0.05);

            Graphics g = Graphics.FromImage(bmp);

            Brush brush = new SolidBrush(LineColor);

            Pen pen = new Pen(brush, LineWidth * 2);
            // pen.Alignment = PenAlignment.Inset;

            pen.DashStyle = DashStyle.Solid;

            int temp = 0;
            //LU
            g.DrawLine(pen, 0, 0, 0, lonth);
            g.DrawLine(pen, 0, 0, lonth, 0);
            //LD
            g.DrawLine(pen, 0, bmp.Height, 0, bmp.Height - lonth);
            g.DrawLine(pen, 0, bmp.Height - temp, lonth, bmp.Height - temp);
            //RU
            g.DrawLine(pen, bmp.Width, 0, bmp.Width - lonth, 0);
            g.DrawLine(pen, bmp.Width - temp, 0, bmp.Width - temp, lonth);
            //RD
            g.DrawLine(pen, bmp.Width - temp, bmp.Height - temp, bmp.Width - lonth - temp, bmp.Height - temp);
            g.DrawLine(pen, bmp.Width - temp, bmp.Height - temp, bmp.Width - temp, bmp.Height - lonth - temp);
            g.Dispose();

            return bmp;
        }


        //============================添加文字==============================
        /// <summary>
        /// 设置文字
        /// </summary>
        /// <param name="b">原始图片</param>
        /// <param name="txt">显示的文字</param>
        /// <param name="x">在图片上的x坐标</param>
        /// <param name="y">在图片上的y坐标</param>
        /// <returns>输出后的图片</returns>
        public static Bitmap AddText(Bitmap b,Color color, string txt, int x, int y)
        {
            if (b == null)
            {
                return null;
            }

            Graphics g = Graphics.FromImage(b);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            // 作为演示，我们用Arial字体，大小为32，红色。
            FontFamily fm = new FontFamily("Arial");
            Font font = new Font(fm, 32, FontStyle.Regular, GraphicsUnit.Pixel);
            SolidBrush sb = new SolidBrush(color);

            g.DrawString(txt, font, sb, new PointF(x, y));
            g.Dispose();

            return b;
        }

        public static Bitmap AddText(Bitmap b, string txt, string TextFont, int TextSize, Color c, int x, int y)
        {
            if (b == null)
            {
                return null;
            }

            Graphics g = Graphics.FromImage(b);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            FontFamily fm = new FontFamily(TextFont);
            Font font = new Font(fm, TextSize, FontStyle.Regular, GraphicsUnit.Pixel);
            SolidBrush sb = new SolidBrush(c);

            g.DrawString(txt, font, sb, new PointF(x, y));
            g.Dispose();

            return b;
        }

        public static Bitmap AddText(Bitmap b, string txt, Font font, Color c, int x, int y)
        {
            if (b == null)
            {
                return null;
            }

            Graphics g = Graphics.FromImage(b);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            SolidBrush sb = new SolidBrush(c);

            g.DrawString(txt, font, sb, new PointF(x, y));
            g.Dispose();

            return b;
        }


        //============================任意角度旋转==============================
        /// <summary>
        /// 任意角度旋转
        /// </summary>
        /// <param name="bmp">原始图Bitmap</param>
        /// <param name="angle">旋转角度</param>
        /// <param name="bkColor">背景色</param>
        /// <returns>输出Bitmap</returns>
        public static Bitmap Rotate(Bitmap bmp, float angle, Color bkColor)
        {
            int w = bmp.Width + 2;
            int h = bmp.Height + 2;

            PixelFormat pf;

            if (bkColor == Color.Transparent)
            {
                pf = PixelFormat.Format32bppArgb;
            }
            else
            {
                pf = bmp.PixelFormat;
            }

            Bitmap tmp = new Bitmap(w, h, pf);
            Graphics g = Graphics.FromImage(tmp);
            g.Clear(bkColor);
            g.DrawImageUnscaled(bmp, 1, 1);
            g.Dispose();

            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0f, 0f, w, h));
            Matrix mtrx = new Matrix();
            mtrx.Rotate(angle);
            RectangleF rct = path.GetBounds(mtrx);

            Bitmap dst = new Bitmap((int)rct.Width, (int)rct.Height, pf);
            g = Graphics.FromImage(dst);
            g.Clear(bkColor);
            g.TranslateTransform(-rct.X, -rct.Y);
            g.RotateTransform(angle);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.DrawImageUnscaled(tmp, 0, 0);
            g.Dispose();

            tmp.Dispose();

            return dst;
        }
    }
}
