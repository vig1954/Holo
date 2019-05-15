using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;

namespace UserInterface.Events
{
    public class ImageSeriesCreatedEvent : EventBase
    {
        public ImageSeries.ImageSeries ImageSeries { get; }

        public ImageSeriesCreatedEvent(ImageSeries.ImageSeries series, object sender) : base(sender)
        {
            ImageSeries = series;
        }
    }
}