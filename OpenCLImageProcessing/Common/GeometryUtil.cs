using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Common
{
    public static class GeometryUtil
    {
        public static bool InRadius(PointF center, PointF toCheck, float radius)
        {
            var dx = center.X - toCheck.X;
            var dy = center.Y - toCheck.Y;

            return dx * dx + dy * dy < radius * radius;
        }
    }
}
