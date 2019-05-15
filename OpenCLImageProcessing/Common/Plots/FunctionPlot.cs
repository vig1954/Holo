using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Common.Plots
{
    public class FunctionPlot : PlotBase
    {
        private Func<float, float> _func;

        public float? XMin { get; set; }

        public float? XMax { get; set; }

        public int Decimation { get; set; } = 1;

        public Func<float, float> Function
        {
            get => _func;
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                _func = value;
            }
        }

        public FunctionPlot(Func<float, float> func)
        {
            _func = func;
        }

        public override PointF[] GetPoints(float xMin, float xMax, float step)
        {
            var x0 = XMin.HasValue ? Math.Max(xMin, XMin.Value) : xMin;
            var x1 = XMax.HasValue ? Math.Min(xMax, XMax.Value) : xMax;

            float x, y;

            step *= Decimation;
            var length = (int) ((x1 - x0) / step);
            var points = new PointF[length];

            for (var i = 0; i < length; i++)
            {
                x = x0 + i * step;
                y = _func(x);

                points[i] = new PointF(x, y);
            }

            return points;
        }
    }
}