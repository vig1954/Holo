using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Common;
using Infrastructure;
using OpenTK;
using Processing.DataAttributes;

namespace Processing.Computing
{
    public static class Psi
    {
        [DataProcessor("PSI", ProcessorGroups.Computing)]
        public static void Psi4(
            [Range(0, 360), Precision(0), DefaultValue(0)] float phaseShift1, [ImageHandlerFilter(AllowedImageFormat.Greyscale)] IImageHandler image1,
            [Range(0, 360), Precision(0), DefaultValue(90)] float phaseShift2, [ImageHandlerFilter(AllowedImageFormat.Greyscale)] IImageHandler image2,
            [Range(0, 360), Precision(0), DefaultValue(180)] float phaseShift3, [ImageHandlerFilter(AllowedImageFormat.Greyscale)] IImageHandler image3,
            [Range(0, 360), Precision(0), DefaultValue(270)] float phaseShift4, [ImageHandlerFilter(AllowedImageFormat.Greyscale)] IImageHandler image4,
            [Range(0, 100), Precision(1)] float amplitude, 
            [ImageHandlerFilter(AllowedImageFormat.RealImaginative, AllowedImagePixelFormat.Float)]IImageHandler output)
        {
            var phaseShift = new[] { phaseShift1, phaseShift2, phaseShift3, phaseShift4 };
            var phaseShiftInRadians = phaseShift.Select(ps => (float) (Math.PI * ps / 180)).ToArray();
            var kSin = phaseShiftInRadians.Select(ps => (float) Math.Sin(ps)).ToArray();
            var sinOrto = kSin.OrtogonalVector();
            var kCos = phaseShiftInRadians.Select(ps => (float) Math.Cos(ps)).ToArray();
            var cosOrto = kCos.OrtogonalVector();
            var denomenator = sinOrto.VectorMul(kCos);

            var app = Singleton.Get<OpenClApplication>();
            app.ExecuteKernel("psi4Kernel", image1.Width, image1.Height, image1, image2, image3, image4,
                new Vector4(sinOrto[0], sinOrto[1], sinOrto[2], sinOrto[3]),
                new Vector4(cosOrto[0], cosOrto[1], cosOrto[2], cosOrto[3]),
                Math.Abs(denomenator), amplitude, output);
        }

        [DataProcessor("Pseudo PSI", ProcessorGroups.Computing)]
        public static void PseudoPsi4(
            [ImageHandlerFilter(AllowedImageFormat.Greyscale)] IImageHandler image1,
            [Range(0, 100), Precision(1)] float amplitude, 
            [ImageHandlerFilter(AllowedImageFormat.RealImaginative, AllowedImagePixelFormat.Float)]IImageHandler output)
        {
            var phaseShift = new[] { 0, 90, 180, 270 };
            var phaseShiftInRadians = phaseShift.Select(ps => (float) (Math.PI * ps / 180)).ToArray();
            var kSin = phaseShiftInRadians.Select(ps => (float) Math.Sin(ps)).ToArray();
            var sinOrto = kSin.OrtogonalVector();
            var kCos = phaseShiftInRadians.Select(ps => (float) Math.Cos(ps)).ToArray();
            var cosOrto = kCos.OrtogonalVector();
            var denomenator = sinOrto.VectorMul(kCos);

            var app = Singleton.Get<OpenClApplication>();
            app.ExecuteKernel("pseudoPsi4Kernel", image1.Width, image1.Height, image1,
                new Vector4(sinOrto[0], sinOrto[1], sinOrto[2], sinOrto[3]),
                new Vector4(cosOrto[0], cosOrto[1], cosOrto[2], cosOrto[3]),
                Math.Abs(denomenator), amplitude, output);
        }
    }
}