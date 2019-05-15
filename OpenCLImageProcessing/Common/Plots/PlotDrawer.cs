using System;
using System.Drawing;

namespace Common.Plots
{
    public class PlotDrawer
    {
        public float ScaleX { get; set; } = 1f;
        public float ScaleY { get; set; } = 1f;

        public float StartX { get; set; }

        public void DrawArray(float[] values, Graphics graphics, ImageLayoutInfo imageLayout, PlotStyle style, float valuesXScale = 1f)
        {
            var points = new PointF[values.Length];

            for (var i = 0; i < values.Length; i++)
            {
                points[i] = new PointF(i * valuesXScale, values[i]);
            }

            DrawArray(points, graphics, imageLayout, style);
        }

        public void DrawArray(PointF[] points, Graphics graphics, ImageLayoutInfo imageLayout, PlotStyle style)
        {
            PointF pt, prevPt = Point.Empty;

            var isFirstPoint = true;

            foreach (var point in points)
            {
                pt = PointToCanvas(point, imageLayout);

                if (pt.X < 0 || pt.Y > imageLayout.CanvasWidth)
                    continue;

                if (!isFirstPoint)
                    graphics.DrawLine(style.PlotPen, prevPt, pt);
                else
                    isFirstPoint = false;

                if (style.DrawPoints)
                    graphics.FillEllipse(style.PointBrush, style.GetPointRectangle(pt.X, pt.Y));

                prevPt = pt;
            }
        }

        public void DrawFunc(Func<float, float> func, Graphics graphics, ImageLayoutInfo imageLayout, PlotStyle style, float xMin, float xMax, int decimation)
        {
            var x0 = Math.Max(xMin, StartX);
            var x1 = Math.Min(imageLayout.CanvasWidth / ScaleX, xMax);

            float x, y;
            
            var step = ScaleX * decimation;
            var length = (int) ((x1 - x0) / step);
            var points = new PointF[length];

            for (var i = 0; i < length; i++)
            {
                x = x0 + i * step;
                y = func(x);

                points[i] = new PointF(x, y);
            }

            DrawArray(points, graphics, imageLayout, style);
        }

        public void DrawPlot(PlotBase plot, ImageLayoutInfo imageLayout)
        {
            var points = plot.GetPoints(StartX, imageLayout.CanvasWidth / ScaleX, ScaleX);
        }

        public PointF PointToCanvas(PointF pt, ImageLayoutInfo imageLayout)
        {
            return PointToCanvas(pt.X, pt.Y, imageLayout);
        }

        public PointF PointToCanvas(float x, float y, ImageLayoutInfo imageLayout)
        {
            x = (x - StartX) * ScaleX;
            y = imageLayout.CanvasHeight - y * ScaleY;

            return new PointF(x, y);
        }

        protected RectangleF GetPointRectangle(float x, float y, float radius) => new RectangleF(x - radius, y - radius, radius * 2, radius * 2);
        protected RectangleF GetPointRectangle(PointF pt, float radius) => GetPointRectangle(pt.X, pt.Y, radius);

        public class PlotStyle
        {
            public bool DrawPoints { get; set; }
            public Brush PointBrush { get; set; }
            public float PointRadius { get; set; }
            public Pen PlotPen { get; set; }

            public RectangleF GetPointRectangle(float x, float y) => new RectangleF(x - PointRadius, y - PointRadius, PointRadius * 2, PointRadius * 2);

            public static PlotStyle Default => new PlotStyle
            {
                DrawPoints = true,
                PlotPen = Pens.Black,
                PointRadius = 2f,
                PointBrush = Brushes.Black
            };
        }

        public class PlotLayout
        {

        }
    }
}