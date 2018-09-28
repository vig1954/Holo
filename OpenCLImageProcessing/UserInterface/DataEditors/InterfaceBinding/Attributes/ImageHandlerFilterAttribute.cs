using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Processing;

namespace UserInterface.DataEditors.InterfaceBinding.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ImageHandlerFilterAttribute : Attribute
    {
        public List<ImageFormat> AllowedImageFormats { get; set; } = new List<ImageFormat>();
        public List<ImagePixelFormat> AllowedPixelFormats { get; set; } = new List<ImagePixelFormat>();
        public int? RequiredChannelCount { get; set; }
        public bool OnlyImages { get; set; }
    }
}