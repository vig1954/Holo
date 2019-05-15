using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Common
{
    public static class BitmapExtensions
    {
        public static Rectangle GetRectangle(this Bitmap self)
        {
            return new Rectangle(0, 0, self.Width, self.Height);
        }
    }
}
