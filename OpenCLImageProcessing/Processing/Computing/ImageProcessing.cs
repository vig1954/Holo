using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
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
        /// <param name="image1">Делимое</param>
        /// <param name="image2">Делитель</param>
        [DataProcessor("Поэлементное комплексное деление", ProcessorGroups.Computing)]
        public static void Divide(IImageHandler image1, IImageHandler image2, IImageHandler output)
        {
            if (!image1.SizeEquals(image2) || !image1.SizeEquals(output))
                throw new InvalidOperationException("Изображения должны быть одинакового размера.");

            Singleton.Get<OpenClApplication>().ExecuteKernel("divide", output.Width, output.Height, image1.ComputeBuffer, image2.ComputeBuffer, output.ComputeBuffer);
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

        [DataProcessor("Вычитание", ProcessorGroups.Computing)]
        public static void Extract(IImageHandler image1, IImageHandler image2, IImageHandler output)
        {
            if (!image1.SizeEquals(image2) || !image1.SizeEquals(output))
                throw new InvalidOperationException("Изображения должны быть одинакового размера.");

            Singleton.Get<OpenClApplication>().ExecuteKernel("extract", output.Width, output.Height, image1.ComputeBuffer, image2.ComputeBuffer, output.ComputeBuffer);
        }

        [DataProcessor("Вычитание с весами", ProcessorGroups.Computing)]
        public static void ExtractWithAutoWeight(IImageHandler image1, IImageHandler image2, IImageHandler output)
        {
            if (!image1.SizeEquals(image2) || !image1.SizeEquals(output))
                throw new InvalidOperationException("Изображения должны быть одинакового размера.");

            var weight1 = 1 / GetRangeForImage(image1);
            var weight2 = 1 / GetRangeForImage(image2);

            Singleton.Get<OpenClApplication>().ExecuteKernel("extractWithWeight", output.Width, output.Height, image1.ComputeBuffer, image2.ComputeBuffer, weight1, weight2, output.ComputeBuffer);

            float GetRangeForImage(IImageHandler image)
            {
                var range = image.GetValueRangeForChannel(0);

                if (image.GetChannelsCount() > 1)
                {
                    var range1 = range;
                    var range2 = image.GetValueRangeForChannel(1);

                    range = new FloatRange((range1.Min + range2.Min) / 2, (range1.Max + range2.Max) / 2);
                }

                return range.Max - range.Min;
            }
        }

        [DataProcessor("Деление на число", ProcessorGroups.Computing)]
        public static void DivideByNumber(IImageHandler image, float num, IImageHandler output)
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

//            var summ = 1 + counter;
//            var inputWeight = 1 / summ;
//            var outputWeight = 1 / counter;
//            Singleton.Get<OpenClApplication>().ExecuteKernel("sumWithWeight", output.Width, output.Height, input.ComputeBuffer, output.ComputeBuffer, output.ComputeBuffer, inputWeight, outputWeight);
            Singleton.Get<OpenClApplication>().ExecuteKernel("sumWithWeight", output.Width, output.Height, input.ComputeBuffer, output.ComputeBuffer, output.ComputeBuffer, 1f, 1f);
        }
    }
}