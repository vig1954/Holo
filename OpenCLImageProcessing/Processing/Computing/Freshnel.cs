using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cloo;
using Infrastructure;
using OpenTK;
using Processing.DataAttributes;
using Processing.Utils;

namespace Processing.Computing
{
    public class Freshnel
    {
        private OpenClApplication App => Singleton.Get<OpenClApplication>();
        private Fourier _fourier;
        private IImageHandler _oldInput;
        private ComputeBuffer<Vector2> _freshnelMultipliersX;
        private ComputeBuffer<Vector2> _freshnelMultipliersY;
        private int _width;
        private int _height;
        private float _wavelength;
        private float _distance;
        private float _objectSize;

        public Freshnel(int width, int height)
        {
            _width = width;
            _height = height;

            _freshnelMultipliersX = new ComputeBuffer<Vector2>(App.ComputeContext, ComputeMemoryFlags.None, _width);
            _freshnelMultipliersY = new ComputeBuffer<Vector2>(App.ComputeContext, ComputeMemoryFlags.None, _height);
        }

        public void Compute(IImageHandler input, IImageHandler output, float wavelength, float distance, float objectSize, bool cyclicShift)
        {
            if (input.Width != _width || input.Height != _height)
                throw new InvalidOperationException("Неожиданные размеры изображения.");

            if (wavelength != _wavelength || distance != _distance || objectSize != _objectSize)
            {
                _wavelength = wavelength;
                _distance = distance;
                _objectSize = objectSize;

                App.ExecuteKernel("freshnelGenerateMultipliers", _width, 1, _freshnelMultipliersX, _wavelength / 1000f, _distance * 1000f, (float) _width, _objectSize * 1000f);
                App.ExecuteKernel("freshnelGenerateMultipliers", _height, 1, _freshnelMultipliersY, _wavelength / 1000f, _distance * 1000f, (float)_height, _objectSize * 1000f);
            }

            App.ExecuteKernel("freshnelMultiply", _width, _height, input, output, _freshnelMultipliersX, _freshnelMultipliersY);

            Fourier.Transform(output);

            App.ExecuteKernel("freshnelMultiply", _width, _height, output, output, _freshnelMultipliersX, _freshnelMultipliersY);

            if (cyclicShift)
                ImageUtils.CyclicShift(output);
        }

        #region public static

        private static Dictionary<string, Freshnel> _cachedProcessors = new Dictionary<string, Freshnel>();

        /// <summary>
        /// Преобразование Френеля
        /// </summary>
        /// <param name="input"></param>
        /// <param name="wavelength">Длина волны, нм</param>
        /// <param name="distance">Расстояние, мм</param>
        /// <param name="objectSize">Размер объекта, мм</param>
        /// <param name="cyclicShift"></param>
        [DataProcessor("Преобразование Френеля", ProcessorGroups.Transforms)]
        public static void Transform(IImageHandler input, float wavelength = 532, float distance = 135, float objectSize = 6.35f, bool cyclicShift = true, [ImageHandlerFilter(AllowedImageFormat.RealImaginative, AllowedImagePixelFormat.Float)] IImageHandler output = null)
        {
            var key = $"{input.Width}_{input.Height}";

            if (!_cachedProcessors.TryGetValue(key, out Freshnel processor))
                _cachedProcessors[key] = processor = new Freshnel(input.Width, input.Height);

            processor.Compute(input, output, wavelength, distance, objectSize, cyclicShift);
        }

        #endregion
    }
}
