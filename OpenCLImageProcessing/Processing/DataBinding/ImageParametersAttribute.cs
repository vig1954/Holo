using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Processing.DataBinding
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class ImageParametersAttribute : Attribute
    {
        public IReadOnlyCollection<ImageFormat> AllowedImageFormats;
        public ImagePixelFormat? AllowedPixelFormat;
        public int? RequiredChannelCount;
        public bool OnlyImages;
        public string OnImageUpdated;
    }
}
