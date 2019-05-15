using System.Drawing;

namespace Common
{
    public class ImageLayoutInfo
    {
        public float CanvasWidth { get; }
        public float CanvasHeight { get; }
        public float ResizedImageWidth { get; }
        public float ResizedImageHeight { get; }
        public float ImageLeft { get; }
        public float ImageTop { get; }
        public float ZoomFactor { get; }

        public ImageLayoutInfo(float canvasWidth, float canvasHeight, float resizedImageWidth, float resizedImageHeight, float imageTop, float imageLeft, float zoomFactor)
        {
            CanvasWidth = canvasWidth;
            CanvasHeight = canvasHeight;
            ResizedImageWidth = resizedImageWidth;
            ResizedImageHeight = resizedImageHeight;
            ImageLeft = imageLeft;
            ImageTop = imageTop;
            ZoomFactor = zoomFactor;
        }

        public ImageLayoutInfo(float canvasWidth, float canvasHeight) : this(canvasWidth, canvasHeight, canvasWidth, canvasHeight, 0, 0, 1)
        {
        }

        public PointF ResizedImagePointToCanvas(PointF imagePoint) => new PointF(imagePoint.X + (int) ImageLeft, imagePoint.Y + (int) ImageTop);
        public PointF CanvasPointToResizedImage(PointF canvasPoint) => new PointF(canvasPoint.X - (int) ImageLeft, canvasPoint.Y - (int) ImageTop);

        public RectangleF ResizedImageRectangleToCanvas(RectangleF imageRectangle) => new RectangleF(ResizedImagePointToCanvas(imageRectangle.Location), imageRectangle.Size);
        public RectangleF CanvasRectangleToResizedImage(RectangleF canvasRectangle) => new RectangleF(CanvasPointToResizedImage(canvasRectangle.Location), canvasRectangle.Size);

        public PointF OriginalImagePointToResizedImagePoint(PointF originalImagePoint) => new PointF((int) (originalImagePoint.X * ZoomFactor), (int) (originalImagePoint.Y * ZoomFactor));
        public PointF ResizedImagePointToOriginalImagePoint(PointF resizedImagePoint) => new PointF((int) (resizedImagePoint.X / ZoomFactor), (int) (resizedImagePoint.Y / ZoomFactor));

        public PointF OriginalImagePointToCanvas(PointF originalImagePoint) => ResizedImagePointToCanvas(OriginalImagePointToResizedImagePoint(originalImagePoint));
        public PointF CanvasPointToOriginalImage(PointF canvasPoint) => CanvasPointToResizedImage(ResizedImagePointToOriginalImagePoint(canvasPoint));

        public RectangleF OriginalImageRectangleToResizedImage(RectangleF r) => new RectangleF((int) (r.X * ZoomFactor), (int) (r.Y * ZoomFactor), (int) (r.Width * ZoomFactor), (int) (r.Height * ZoomFactor));
        public RectangleF ResizedImageRectangleToOriginalImage(RectangleF r) => new RectangleF((int) (r.X / ZoomFactor), (int) (r.Y / ZoomFactor), (int) (r.Width / ZoomFactor), (int) (r.Height / ZoomFactor));

        public RectangleF OriginalImageRectangleToCanvas(RectangleF r) => ResizedImageRectangleToCanvas(OriginalImageRectangleToResizedImage(r));
        public RectangleF CanvasRectangleToOriginalImage(RectangleF r) => CanvasRectangleToResizedImage(ResizedImageRectangleToOriginalImage(r));
    }
}