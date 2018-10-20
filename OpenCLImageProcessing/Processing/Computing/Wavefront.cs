using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Processing.DataAttributes;

namespace Processing.Computing
{
    public static class Wavefront
    {
        [DataProcessor("Сферический волновой фронт", ProcessorGroups.Generation)]
        public static void Spheric(
            [DefaultValue(2816), OutputImageWidth] int imageWidth,
            [DefaultValue(2816), OutputImageHeight] int imageHeight,
            [DefaultValue(532), Precision(2)] float lambda,
            [DefaultValue(200), Precision(2)] float distance,
            [DefaultValue(5), Precision(2)] float sizeX,
            [DefaultValue(5), Precision(2)] float sizeY,
            [DefaultValue(1), Precision(2)] float amplitude,
            [ImageHandlerFilter(AllowedImageFormat.RealImaginative, AllowedImagePixelFormat.Float)]IImageHandler output)
        {
            Singleton.Get<OpenClApplication>().ExecuteKernel("sphericWavefront", imageWidth, imageHeight, output, lambda / 1000f, distance * 1000f, sizeX * 1000f, sizeY * 1000f, amplitude);
        }

        [DataProcessor("Плоский волновой фронт", ProcessorGroups.Generation)]
        public static void Flat(
            [DefaultValue(2816), OutputImageWidth] int imageWidth,
            [DefaultValue(2816), OutputImageHeight] int imageHeight,
            [DefaultValue(90), Precision(2)] float alpha,
            [DefaultValue(1), Precision(2)] float amplitude,
            [ImageHandlerFilter(AllowedImageFormat.RealImaginative, AllowedImagePixelFormat.Float)]IImageHandler output)
        {
            Singleton.Get<OpenClApplication>().ExecuteKernel("flatWavefront", imageWidth, imageHeight, alpha / 180f * (float)Math.PI, amplitude, output);
        }
    }
}
