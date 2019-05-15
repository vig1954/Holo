using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UserInterface.DataEditors.Tools;

namespace Camera
{
    public class RectangleSelectionTool : PictureBoxToolBase
    {
        private const float EditRadius = 5f;
        private Rectangle _rectangle;
        private Size? _fixedSize;

        private SelectionUtil.SelectionEditAction _currentAction = SelectionUtil.SelectionEditAction.None;
        private PointF _previousCursorPosition;

        public Size RectangleSize => _rectangle.Size;

        public Rectangle Rectangle => _rectangle;

        public event Action<Rectangle> OnRectangleUpdated;

        public Size? FixedSize
        {
            get => _fixedSize;
            set
            {
                _fixedSize = value;

                if (_fixedSize.HasValue)
                {
                    _rectangle.Width = _fixedSize.Value.Width;
                    _rectangle.Height = _fixedSize.Value.Height;
                }

                PictureBox?.Refresh();
            }
        }
        
        protected override void PictureBoxOnMouseMove(object sender, MouseEventArgs e)
        {
            var originalImageCursorLocation = ImageLayoutInfo.CanvasPointToOriginalImage(e.Location);

            if (_currentAction != SelectionUtil.SelectionEditAction.None)
            {
                SelectionUtil.ApplySelectionTransformation(_currentAction, originalImageCursorLocation, _previousCursorPosition, ref _rectangle);
                OnRectangleUpdated?.Invoke(Rectangle);

                PictureBox.Refresh();
            }
            else
            {
                Cursor.Current = SelectionUtil.ResolveCursor(SelectionUtil.ResolveSelectionEditAction(originalImageCursorLocation, _rectangle, EditRadius));
            }

            _previousCursorPosition = originalImageCursorLocation;
        }

        protected override void PictureBoxOnMouseUp(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.Default;
            _currentAction = SelectionUtil.SelectionEditAction.None;
        }

        protected override void PictureBoxOnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            var originalImageCursorLocation = ImageLayoutInfo.CanvasPointToOriginalImage(e.Location);

            if (!FixedSize.HasValue)
            {
                if (Rectangle.Width == 0 || Rectangle.Height == 0)
                {
                    _rectangle = new Rectangle((int) originalImageCursorLocation.X, (int) originalImageCursorLocation.Y, (int) EditRadius * 2, (int) EditRadius * 2);
                    _currentAction = SelectionUtil.SelectionEditAction.ResizeSE;
                }
                else
                {
                    _currentAction = SelectionUtil.ResolveSelectionEditAction(originalImageCursorLocation, Rectangle, EditRadius);
                    Cursor.Current = SelectionUtil.ResolveCursor(_currentAction);
                }
            }
            else
            {
                if (Rectangle.Width == 0 || Rectangle.Height == 0)
                    _rectangle = new Rectangle((int) (originalImageCursorLocation.X - FixedSize.Value.Width / 2), (int) (originalImageCursorLocation.Y - FixedSize.Value.Height / 2), FixedSize.Value.Width, FixedSize.Value.Height);

                _currentAction = SelectionUtil.SelectionEditAction.Move;
            }

            _previousCursorPosition = originalImageCursorLocation;
        }

        public void DrawUi(Graphics graphics)
        {
            if (_rectangle.Width == 0 || Rectangle.Height == 0)
                return;

            var pen = new Pen(new HatchBrush(HatchStyle.DarkDownwardDiagonal, Color.Black, Color.White), 2f);
            var canvasSelection = ImageLayoutInfo.OriginalImageRectangleToCanvas(_rectangle);

            var points = new[]
            {
                new PointF(canvasSelection.Right, canvasSelection.Top),
                new PointF(canvasSelection.Left, canvasSelection.Top),
                new PointF(canvasSelection.Left, canvasSelection.Bottom),
                new PointF(canvasSelection.Right, canvasSelection.Bottom),
                new PointF(canvasSelection.Right, 0),
                new PointF((int) ImageLayoutInfo.CanvasWidth, 0),
                new PointF((int) ImageLayoutInfo.CanvasWidth, (int) ImageLayoutInfo.CanvasHeight),
                new PointF(0, (int) ImageLayoutInfo.CanvasHeight),
                new PointF(0, 0),
                new PointF(canvasSelection.Right, 0),
            };

            graphics.FillPolygon(new SolidBrush(Color.FromArgb(100, Color.Gray)), points);

            graphics.DrawRectangle(pen, canvasSelection.X, canvasSelection.Y, canvasSelection.Width, canvasSelection.Height);
        }
    }
}