using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;

namespace Processing
{
    public class ImageSelectionManager
    {
        private Dictionary<IImageHandler, ImageSelectionWrapper> _imageSelections = new Dictionary<IImageHandler, ImageSelectionWrapper>();

        public void SetSelection(IImageHandler imageHandler, ImageSelection selection)
        {
            if (_imageSelections.ContainsKey(imageHandler))
                _imageSelections[imageHandler].Selection = selection;
            else
                _imageSelections.Add(imageHandler, new ImageSelectionWrapper(selection));
        }

        public bool TryGetSelection(IImageHandler imageHandler, out ImageSelection selection)
        {
            if (_imageSelections.TryGetValue(imageHandler, out ImageSelectionWrapper selectionWrapper))
            {
                selection = selectionWrapper.Selection;
                return true;
            }

            selection = ImageSelection.Empty;
            return false;
        }

        public bool HasSelection(IImageHandler imageHandler)
        {
            return TryGetSelection(imageHandler, out _);
        }

        public ImageSelection GetSelection(IImageHandler imageHandler)
        {
            if (!_imageSelections.TryGetValue(imageHandler, out ImageSelectionWrapper selectionWrapper))
                throw new InvalidOperationException("Отсутствует область выделения для заданного изображения.");

            return selectionWrapper.Selection;
        }

        public ref ImageSelection GetSelectionByRef(IImageHandler imageHandler)
        {
            if (!_imageSelections.TryGetValue(imageHandler, out ImageSelectionWrapper selectionWrapper))
                throw new InvalidOperationException("Отсутствует область выделения для заданного изображения.");

            return ref selectionWrapper.Selection;
        }

        private class ImageSelectionWrapper
        {
            public ImageSelection Selection;

            public ImageSelectionWrapper(ImageSelection selection)
            {
                Selection = selection;
            }
        }
    }
}