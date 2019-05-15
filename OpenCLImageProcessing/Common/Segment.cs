using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Common
{
    public struct Segment
    {
        public float X0 { get; set; }
        public float Y0 { get; set; }
        public float X1 { get; set; }
        public float Y1 { get; set; }

        public PointF P0
        {
            get => new PointF(X0, Y0);
            set
            {
                X0 = value.X;
                Y0 = value.Y;
            }
        }

        public PointF P1
        {
            get => new PointF(X1, Y1);
            set
            {
                X1 = value.X;
                Y1 = value.Y;
            }
        }

        public float Length
        {
            get
            {
                var dx = X1 - X0;
                var dy = Y1 - Y0;

                return (float) Math.Sqrt(dx * dx + dy * dy);
            }
        }

        public bool IsZeroLength => Length * Length < float.Epsilon;
    }
}