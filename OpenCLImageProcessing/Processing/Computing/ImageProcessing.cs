using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Processing.DataAttributes;
using Processing.DataProcessors;

namespace Processing.Computing
{
    public class ImageProcessing
    {
        public static void Shift(IImageHandler input, IImageHandler output, int dx, int dy, bool cyclic)
        {
            if (!input.SizeEquals(output))
                throw new InvalidOperationException("Изображения должны быть одинакового размера.");

            Singleton.Get<OpenClApplication>().ExecuteKernel("shift", input.Width, input.Height, input.ComputeBuffer, output.ComputeBuffer, dx, dy, cyclic ? 1 : 0);
        }

        /// <summary>
        /// Попиксельное деление комплексных изображения
        /// </summary>
        /// <param name="numerator">Делимое</param>
        /// <param name="denumerator">Делитель</param>
        [DataProcessor("Поэлементное комплексное деление", ProcessorGroups.Computing)]
        public static void Divide(IImageHandler numerator, IImageHandler denumerator, IImageHandler output)
        {
            if (!numerator.SizeEquals(denumerator) || !numerator.SizeEquals(output))
                throw new InvalidOperationException("Изображения должны быть одинакового размера.");

            Singleton.Get<OpenClApplication>().ExecuteKernel("divide", output.Width, output.Height, numerator.ComputeBuffer, denumerator.ComputeBuffer, output.ComputeBuffer);
        }

        [DataProcessor("Поэлементное комплексное умножение", ProcessorGroups.Computing)]
        public static void Multiply(IImageHandler multiplier1, IImageHandler multiplier2, IImageHandler output)
        {
            if (!multiplier1.SizeEquals(multiplier2) || !multiplier1.SizeEquals(output))
                throw new InvalidOperationException("Изображения должны быть одинакового размера.");

            Singleton.Get<OpenClApplication>().ExecuteKernel("multiply", output.Width, output.Height, multiplier1.ComputeBuffer, multiplier2.ComputeBuffer, output.ComputeBuffer);
        }

        [DataProcessor("Сложение", ProcessorGroups.Computing)]
        public static void Sum(IImageHandler image1, IImageHandler image2, IImageHandler output)
        {
            if (!image1.SizeEquals(image2) || !image1.SizeEquals(output))
                throw new InvalidOperationException("Изображения должны быть одинакового размера.");

            Singleton.Get<OpenClApplication>().ExecuteKernel("sum", output.Width, output.Height, image1.ComputeBuffer, image2.ComputeBuffer, output.ComputeBuffer);
        }

        [DataProcessor("Деление на число", ProcessorGroups.Computing)]
        public static void Divide(IImageHandler image, float num, IImageHandler output)
        {
            if (!image.SizeEquals(output))
                throw new InvalidOperationException("Изображения должны быть одинакового размера.");

            Singleton.Get<OpenClApplication>().ExecuteKernel("divideByNum", output.Width, output.Height, image.ComputeBuffer, output.ComputeBuffer, num);
        }

        [DataProcessor("Совмещение амплитуды и фазы", ProcessorGroups.Computing, tooltip: "Совмещение амплитудной и фазовой составляющих разных изображений")]
        public static void Combine(IImageHandler amplitudeImage, IImageHandler phaseImage, IImageHandler output)
        {
            if (!amplitudeImage.SizeEquals(phaseImage) || !amplitudeImage.SizeEquals(output))
                throw new InvalidOperationException("Изображения должны быть одинакового размера.");

            Singleton.Get<OpenClApplication>().ExecuteKernel("combineAmplitudeAndPhase", output.Width, output.Height, amplitudeImage.ComputeBuffer, phaseImage.ComputeBuffer, output.ComputeBuffer);
        }

        [DataProcessor("Интерференция", ProcessorGroups.Computing)]
        public static void Interference(IImageHandler image1, IImageHandler image2, [Range(0, 360), Precision(0), DefaultValue(0)]
            float delta, IImageHandler output)
        {
            if (!image1.SizeEquals(image1) || !output.SizeEquals(image1))
                throw new InvalidOperationException("Изображения должны быть одинакового размера.");

            Singleton.Get<OpenClApplication>().ExecuteKernel("interference", output.Width, output.Height, image1.ComputeBuffer, image2.ComputeBuffer, delta, output.ComputeBuffer);
        }

        [DataProcessor("Бесконечное накопление", ProcessorGroups.Computing)]
        public static void Accumulate(IImageHandler input, [Counter] float counter, IImageHandler output)
        {
            if (!input.SizeEquals(output))
                throw new InvalidOperationException("Изображения должны быть одинакового размера.");

            Singleton.Get<OpenClApplication>().ExecuteKernel("sumWithWeight", output.Width, output.Height, input.ComputeBuffer, output.ComputeBuffer, output.ComputeBuffer, 1f, counter);
        }
    }
}