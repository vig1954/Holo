using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Common;
using Processing;
using UserInterface.DataProcessorViews;
using UserInterface.WorkspacePanel.ImageSeries;

namespace UserInterface.ImageSeries
{
    public class ImageSeries
    {
        private readonly List<IDataProcessorView> _dataProcessors = new List<IDataProcessorView>();

        public int Length { get; } = 4;

        public Size Size { get; }

        public string Title { get; set; }

        public ImageHandler[] Inputs { get; }

        public event Action<IDataProcessorView> OnDataProcessorAdded;

        public IReadOnlyCollection<IDataProcessorView> DataProcessors => _dataProcessors;


        public ImageSeries(Size size, string title)
        {
            Title = title;
            Inputs = new ImageHandler[Length];
            Size = size;

            for (var i = 0; i < Length; i++)
            {
                Inputs[i] = ImageHandler.Create($"{Title}_{i}", Size.Width, Size.Height, ImageFormat.RGB, ImagePixelFormat.Byte);
                Inputs[i].ImageUpdated += OnImageUpdated;
            }
        }

        private void OnImageUpdated(ImageUpdatedEventData e)
        {
            if (Inputs.All(i => i.IsReady()))
                Update();
        }

        public void AddDataProcessor(IDataProcessorView dataProcessor)
        {
            _dataProcessors.Add(dataProcessor);

            OnDataProcessorAdded?.Invoke(dataProcessor);
        }
        
        public void Update()
        {
            if (_dataProcessors.IsNullOrEmpty())
                return;

            var inputImages = (IImageHandler[]) Inputs;
            foreach (var dataProcessor in _dataProcessors)
            {
                var imageInputParameters = dataProcessor.Parameters.ParametersOfType<IImageHandler>().ToArray();

                if (imageInputParameters.Length != inputImages.Length)
                    throw new InvalidOperationException($"Количество изображений на входе '{dataProcessor.Info.Name}' не соответствует количеству выходных изображений на предыдущем этапе.");

                for (var i = 0; i < imageInputParameters.Length; i++)
                {
                    imageInputParameters[i].SetValue(inputImages[i], this);
                }

                dataProcessor.Compute();

                inputImages = dataProcessor.GetOutputValues().OfType<IImageHandler>().ToArray();
            }
        }

        public override string ToString()
        {
            return $"{Title} [{Size.Width} x {Size.Height}]";
        }
    }
}