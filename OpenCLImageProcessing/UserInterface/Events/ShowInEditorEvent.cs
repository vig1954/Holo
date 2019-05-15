using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;
using Processing;

namespace UserInterface.Events
{
    public class ShowInEditorEvent : EventBase
    {
        public IImageHandler ImageHandler { get; }

        public ShowInEditorEvent(IImageHandler imageHandler, object sender) : base(sender)
        {
            ImageHandler = imageHandler;
        }
    }
}
