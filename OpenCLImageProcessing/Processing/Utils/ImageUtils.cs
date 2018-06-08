using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Cloo;
using Common;
using Infrastructure;
using Processing.Computing;

namespace Processing.Utils
{
    public static class ImageUtils
    {
        public static void PopulateMinMax(IImageHandler imageHandler)
        {
            const int channelsCount = 2;
            // Обрабатывается сразу два канала
            using (new Timer("Populate MinMax"))
            {
                var app = Singleton.Get<OpenClApplication>();
                var kernel = app.Program.CreateKernel("minMax" + channelsCount);

                const int workSize = 16;
                var bufferWidth = imageHandler.Width / workSize;
                var bufferHeight = imageHandler.Height / workSize;
                var bufferMin = new float[bufferHeight * bufferWidth * 2];
                var bufferMax = new float[bufferHeight * bufferWidth * 2];

                var computeBufferMin = new ComputeBuffer<float>(app.ComputeContext, ComputeMemoryFlags.WriteOnly, bufferMin);
                var computeBufferMax = new ComputeBuffer<float>(app.ComputeContext, ComputeMemoryFlags.WriteOnly, bufferMax);

                imageHandler.UploadToComputingDevice();
                app.Acquire(imageHandler);
                kernel.SetMemoryArgument(0, imageHandler.ComputeBuffer);
                kernel.SetMemoryArgument(1, computeBufferMin);
                kernel.SetMemoryArgument(2, computeBufferMax);
                kernel.SetValueArgument(3, workSize);
                kernel.SetValueArgument(4, imageHandler.Width);
                kernel.SetValueArgument(5, imageHandler.Height);
                kernel.SetValueArgument(6, bufferWidth);

                app.ExecuteInQueue(kernel, bufferWidth, bufferHeight);
                app.Queue.Finish();

                app.Queue.ReadFromBuffer(computeBufferMin, ref bufferMin, true, null);
                app.Queue.ReadFromBuffer(computeBufferMax, ref bufferMax, true, null);

                app.Release(imageHandler);

                var min = new float[channelsCount];
                var max = new float[channelsCount];
                float val;
                int j;

                for (var i = 0; i < bufferHeight * bufferWidth * 2; i+=2)
                {
                    for (j = 0; j < channelsCount; j++)
                    {
                        val = bufferMin[i + j];
                        if (min[j] > val)
                            min[j] = val;

                        val = bufferMax[i + j];
                        if (max[j] < val)
                            max[j] = val;
                    }
                }

                DebugLogger.Log($"MinMax: [{min[0]} - {max[0]}] ; [{min[1]} - {max[1]}]");

                imageHandler.Tags.SetOrAdd(ImageHandlerTagKeys.MinimumValueF, min[0]);
                imageHandler.Tags.SetOrAdd(ImageHandlerTagKeys.MaximumValueF, max[0]);
                if (imageHandler.GetChannelsCount() == 2)
                {
                    imageHandler.Tags.SetOrAdd(ImageHandlerTagKeys.MinimumValue2F, min[1]);
                    imageHandler.Tags.SetOrAdd(ImageHandlerTagKeys.MaximumValue2F, max[1]);
                }
            }
        }

        public static ImageHandler Transpose(IImageHandler input)
        {
            var output = ImageHandler.Create(input.GetTitle() + " transponed", input.Height, input.Width, input.Format, input.PixelFormat);
            output.UploadToComputingDevice();

            Transpose(input, output);

            return output;
        }

        public static void Transpose(IImageHandler input, IImageHandler output)
        {
            if (output.Height != input.Width || output.Width != input.Height)
                throw new InvalidOperationException("Не подходящие размеры изображения вывода.");

            Singleton.Get<OpenClApplication>().ExecuteKernel("transpose_img", input.Width, input.Height, input, output);
        }

        public static void CyclicShift(IImageHandler image)
        {
            using (new Timer("Cyclic Shift"))
            {
                var app = Singleton.Get<OpenClApplication>();
                app.Acquire(image);
                var w = image.Width / 2;
                var h = image.Height / 2;
                app.ExecuteKernel("cyclicShift_img", w, h, image, image);
                app.Release(image);
            }
        }
    }
}
