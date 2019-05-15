using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Common
{
    public static class BitmapUtil
    {
        public static Bitmap Resize(Bitmap image, Size canvasSize, out ImageLayoutInfo layoutInfo)
        {
            var canvasWidth = (float) canvasSize.Width;
            var canvasHeight = (float) canvasSize.Height;

            var zoomFactor = Math.Min(canvasWidth / image.Width, canvasHeight / image.Height);

            var newWidth = image.Width * zoomFactor;
            var newHeight = image.Height * zoomFactor;
            var top = (canvasHeight - newHeight) / 2;
            var left = (canvasWidth - newWidth) / 2;

            var result = new Bitmap(canvasSize.Width, canvasSize.Height);
            using (var graphics = Graphics.FromImage(result))
            {
                //graphics.FillRectangle(Brushes.Blue, 0, 0, canvasWidth, canvasHeight);
                graphics.DrawImage(image, new Rectangle((int) left, (int) top, (int) newWidth, (int) newHeight));
                graphics.Save();
            }

            layoutInfo = new ImageLayoutInfo(canvasWidth, canvasHeight, newWidth, newHeight, top, left, zoomFactor);

            return result;
        }

        public static unsafe float[] ExtractSegment(Bitmap bitmap, Segment segment)
        {
            var length = segment.Length;
            var integerLength = (int) length;

            var dx = (segment.X1 - segment.X0) / integerLength;
            var dy = (segment.Y1 - segment.Y0) / integerLength;

            var data = new float[integerLength];

            const float RedCoefficient = 0.2126f;
            const float GreenCoefficient = 0.7152f;
            const float BlueCoefficient = 0.0722f;
            var rk = RedCoefficient / 255;
            var bk = BlueCoefficient / 255;
            var gk = GreenCoefficient / 255;
            
            var bitmapData = bitmap.LockBits(bitmap.GetRectangle(), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            var p = (RGBPoint*) bitmapData.Scan0;
            var x = segment.X0;
            var y = segment.Y0;
            var d = 0;
            RGBPoint pt;
            for (var i = 0; i < integerLength; i++)
            {
                d = (int) y * bitmap.Width + (int) x;
                
                pt = p[d];
                data[i] = ConvertRGBPointToFloat(pt, rk, gk, bk);

                x += dx;
                y += dy;
            }

            bitmap.UnlockBits(bitmapData);

            return data;
        }

        
        private static float ConvertRGBPointToFloat(RGBPoint pt, float rk, float gk, float bk)
        {
            return pt.R * rk + pt.G * gk + pt.B * bk;
        }

        public struct RGBPoint
        {
            public byte R { get; set; }
            public byte G { get; set; }
            public byte B { get; set; }

            public RGBPoint(byte r, byte g, byte b)
            {
                R = r;
                G = g;
                B = b;
            }
        }

        public struct ARGBPoint
        {
            public byte B { get; set; }
            public byte G { get; set; }
            public byte R { get; set; }
            public byte A { get; set; }

            public ARGBPoint(byte r, byte g, byte b, byte a)
            {
                R = r;
                G = g;
                B = b;
                A = a;
            }
        }
    }
}