using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;

namespace Camera
{
    public class SegmentSelectionTool : PictureBoxToolBase
    {
        public enum Mode
        {
            None,
            DragPoint0,
            DragPoint1
        }

        private const float _toggleModeRadius = 5f;
        private Mode _mode;
        private Segment _segment;

        public Segment Segment => _segment;

        public event Action<Segment> OnSegmentUpdated;

        public void DrawUi(Graphics graphics)
        {
            if (_segment.IsZeroLength)
                return;

            var backgroundBen = new Pen(Color.White, 2f);
            var pen = new Pen(Color.Black, 2f);
            //pen.DashStyle = DashStyle.Dash;
            pen.EndCap = LineCap.ArrowAnchor;
            pen.StartCap = LineCap.RoundAnchor;
            pen.DashPattern = new[] {3f, 3f};

            var p0 = ImageLayoutInfo.OriginalImagePointToCanvas(_segment.P0);
            var p1 = ImageLayoutInfo.OriginalImagePointToCanvas(_segment.P1);

            graphics.DrawLine(backgroundBen, p0, p1);
            graphics.DrawLine(pen, p0, p1);


//            var hR = _toggleModeRadius / 2;
//            graphics.FillEllipse(Brushes.Black, p0.X - hR, p0.Y - hR, _toggleModeRadius, _toggleModeRadius);
//            graphics.DrawEllipse(Pens.White, p0.X - hR, p0.Y - hR, _toggleModeRadius, _toggleModeRadius);
        }

        protected override void PictureBoxOnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            var cursor = ImageLayoutInfo.CanvasPointToOriginalImage(e.Location);

            if (_segment.IsZeroLength)
            {
                _segment.P0 = cursor;
                _mode = Mode.DragPoint1;
            }
            else
                _mode = ResolveMode(cursor, _segment);
        }

        protected override void PictureBoxOnMouseMove(object sender, MouseEventArgs e)
        {
            var cursor = ImageLayoutInfo.CanvasPointToOriginalImage(e.Location);
            if (_mode == Mode.DragPoint0)
                _segment.P0 = cursor;
            else if (_mode == Mode.DragPoint1)
                _segment.P1 = cursor;

            if (_mode != Mode.None)
                OnSegmentUpdated?.Invoke(_segment);

            if (_segment.IsZeroLength)
                return;

            var possibleMode = _mode == Mode.None ? ResolveMode(cursor, _segment) : _mode;

            Cursor.Current = possibleMode != Mode.None ? Cursors.SizeAll : Cursors.Default;
        }

        protected override void PictureBoxOnMouseUp(object sender, MouseEventArgs e)
        {
            _mode = Mode.None;
        }
        
        private Mode ResolveMode(PointF cursor, Segment segment)
        {
            if (GeometryUtil.InRadius(_segment.P0, cursor, _toggleModeRadius))
                return Mode.DragPoint0;

            if (GeometryUtil.InRadius(_segment.P1, cursor, _toggleModeRadius))
                return Mode.DragPoint1;

            return Mode.None;
        }
    }
}