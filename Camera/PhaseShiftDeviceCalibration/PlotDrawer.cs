using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using Common;

namespace Camera.PhaseShiftDeviceCalibration
{
    public class PlotDrawer : Common.Plots.PlotDrawer
    {
        private Color[] PositiveColors = new Color[]
        {
            Color.LightBlue,
            Color.CornflowerBlue,
            Color.DeepSkyBlue,
            Color.DodgerBlue
        };

        private Color[] NegativeColors = new Color[]
        {
            Color.Pink,
            Color.Violet,
            Color.LightPink,
            Color.Magenta
        };

        public void DrawSampleEvaluation(SampleEvaluation evaluation, ImageLayoutInfo imageLayout, Graphics graphics)
        {
            ScaleY = imageLayout.CanvasHeight / evaluation.MaximumValue;
            var scale = (int) Math.Max(imageLayout.CanvasWidth / evaluation.SampleAverage.Length, 1);

            DrawAreaBackgrounds(evaluation, imageLayout, graphics, scale);

            var plotStyle = PlotStyle.Default;
            DrawArray(evaluation.SampleAverage, graphics, imageLayout, plotStyle, scale);
            DrawLoops(evaluation, imageLayout, graphics, scale);
        }

        public void DrawSampleEvaluationDifference(SampleEvaluationComparison comparison, ImageLayoutInfo imageLayout, Graphics graphics)
        {
            var zero = comparison.ZeroSampleEvaluation;
            var other = comparison.OtherSampleEvaluation;
            var zeroLoop = comparison.ZeroLoop;
            var otherLoop = comparison.OtherLoop;

            ScaleY = imageLayout.CanvasHeight / (Math.Max(zero.MaximumValue, other.MaximumValue) * 1.1f);

            var scale = (int) Math.Max(imageLayout.CanvasWidth / zero.SampleAverage.Length, 1);
            var zeroStyle = PlotStyle.Default;
            zeroStyle.DrawPoints = false;
            zeroStyle.PlotPen = new Pen(Brushes.Gray, 2f);

            var otherStyle = PlotStyle.Default;
            otherStyle.DrawPoints = false;
            otherStyle.PlotPen = new Pen(Brushes.Blue, 2f);

            DrawArray(zero.SampleAverage, graphics, imageLayout, zeroStyle, scale);
            DrawArray(other.SampleAverage, graphics, imageLayout, otherStyle, scale);

            var zeroLoopSpecificPointPen = new Pen(Color.Gray)
            {
                DashStyle = DashStyle.Dot
            };

            DrawZeroLoopSpecificPoint(SampleEvaluation.Loop.SpecificPointType.Start);
            DrawZeroLoopSpecificPoint(SampleEvaluation.Loop.SpecificPointType.Quarter);
            DrawZeroLoopSpecificPoint(SampleEvaluation.Loop.SpecificPointType.Half);
            DrawZeroLoopSpecificPoint(SampleEvaluation.Loop.SpecificPointType.ThreeQuarters);
            DrawZeroLoopSpecificPoint(SampleEvaluation.Loop.SpecificPointType.Loop);

            var otherLoopSpecificColor = Color.DeepSkyBlue;
            var otherLoopSpecificPointPen = new Pen(otherLoopSpecificColor)
            {
                DashStyle = DashStyle.Dot
            };

            var otherStartPoint = otherLoop.SpecificPoints[SampleEvaluation.Loop.SpecificPointType.Start];
            var pt0 = PointToCanvas(otherStartPoint.X * scale, otherStartPoint.Y, imageLayout);
            var pt1 = PointToCanvas(otherStartPoint.X * scale, otherLoop.StartArea.Origin, imageLayout);

            graphics.DrawLine(otherLoopSpecificPointPen, pt0, pt1);
            graphics.FillEllipse(new SolidBrush(otherLoopSpecificColor), GetPointRectangle(pt0, 2f));

            var label = comparison.NormalizedLinearDifference.ToString("#.###");
            if (comparison.ClosestSpecificPoint != SampleEvaluation.Loop.SpecificPointType.None)
            {
                var differenceLabel = SampleEvaluationComparison.GetSpecificPointLabel(comparison.ClosestSpecificPoint);
                label += $"|{differenceLabel} [±{comparison.ClosestSpecificPointInterval:0.##}%]";
            }

            DrawLineWithLabel(GetExtremumPoint(zeroLoop.StartArea, scale), GetExtremumPoint(otherLoop.StartArea, scale), label, graphics, imageLayout);

            void DrawZeroLoopSpecificPoint(SampleEvaluation.Loop.SpecificPointType pointType)
            {
                var pt = zeroLoop.SpecificPoints[pointType];

                graphics.DrawLine(zeroLoopSpecificPointPen, pt.X * scale, 0, pt.X * scale, imageLayout.CanvasHeight);
            }
        }

        private void DrawAreaBackgrounds(SampleEvaluation evaluation, ImageLayoutInfo imageLayout, Graphics graphics, int scale)
        {
            float x, y;
            for (var i = 0; i < evaluation.Areas.Count; i++)
            {
                var area = evaluation.Areas[i];
                Brush areaBrush = new SolidBrush(GetColor(i / 2, area.IsPositive));
                var points = new List<PointF> {PointToCanvas(area.Start * scale, area.Origin, imageLayout)};

                for (var j = area.Start; j < area.End; j++)
                {
                    x = j * scale;
                    y = evaluation.SampleAverage[j];

                    points.Add(PointToCanvas(x, y, imageLayout));
                }

                points.Add(PointToCanvas(area.End * scale, area.Origin, imageLayout));

                graphics.FillPolygon(areaBrush, points.ToArray());
            }
        }

        private void DrawLoops(SampleEvaluation evaluation, ImageLayoutInfo imageLayout, Graphics graphics, int scale)
        {
            foreach (var loop in evaluation.Loops)
            {
                DrawLineWithLabel(GetExtremumPoint(loop.StartArea, scale), GetExtremumPoint(loop.EndArea, scale), loop.Length.ToString(), graphics, imageLayout);
            }
        }

        private void DrawLineWithLabel(PointF start, PointF end, string label, Graphics graphics, ImageLayoutInfo imageLayout)
        {
            const float LoopEdgePointRaius = 2f;

            var loopPen = new Pen(Color.Gray, 2f)
            {
                DashStyle = DashStyle.Dash,
                StartCap = LineCap.ArrowAnchor,
                EndCap = LineCap.ArrowAnchor
            };

            var loopEdgesPen = new Pen(Color.Blue, 2f);

            var labelFont = new Font(FontFamily.GenericMonospace, 12f);
            var labelOutlinePen = Pens.Black;
            var labelBackground = Brushes.White;
            var labelBrush = Brushes.Black;

            start = PointToCanvas(start, imageLayout);
            end = PointToCanvas(end, imageLayout);

            graphics.DrawLine(loopPen, start, end);
            graphics.DrawEllipse(loopEdgesPen, GetPointRectangle(start, LoopEdgePointRaius));
            graphics.DrawEllipse(loopEdgesPen, GetPointRectangle(end, LoopEdgePointRaius));

            var labelSize = graphics.MeasureString(label, labelFont);
            var labelCenter = new PointF((end.X + start.X) / 2, (end.Y + start.Y) / 2);
            var labelRectangle = new Rectangle((int) (labelCenter.X - labelSize.Width / 2), (int) (labelCenter.Y - labelSize.Height / 2), (int) labelSize.Width, (int) labelSize.Height);
            labelRectangle.Inflate(2, 2);

            graphics.FillRectangle(labelBackground, labelRectangle);
            graphics.DrawRectangle(labelOutlinePen, labelRectangle);

            labelRectangle.Inflate(-2, -2);
            graphics.DrawString(label, labelFont, labelBrush, labelRectangle.X, labelRectangle.Y);
        }

        private PointF GetExtremumPoint(SampleEvaluation.Area area, int xScale)
        {
            return new PointF(area.ExtremumIndex * xScale, area.Extremum);
        }

        private Color GetColor(int i, bool isPositive)
        {
            return isPositive ? PositiveColors[i % PositiveColors.Length] : NegativeColors[i % NegativeColors.Length];
        }
    }
}