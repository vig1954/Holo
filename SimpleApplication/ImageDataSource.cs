using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Processing;

namespace SimpleApplication
{
    public class ImageHandlerDataSource
    {
        public string Title { get; }
        public ImageHandler ImageHandler { get; }

        public ImageHandlerDataSource(string title, ImageHandler imageHandler)
        {
            Title = title;
            ImageHandler = imageHandler;
        }

        public override string ToString() => Title;
    }
}
