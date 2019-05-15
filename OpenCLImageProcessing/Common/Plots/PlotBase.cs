using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Plots
{
    public abstract class PlotBase
    {
        public PlotDrawer.PlotStyle Style { get; set; }

        public abstract PointF[] GetPoints(float xMin, float xMax, float step);
    }
}
