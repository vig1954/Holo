using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cloo;
using Common;
using Infrastructure;
using OpenTK;
using Processing.DataAttributes;
using Processing.Utils;

namespace Processing.Computing
{
    // TODO: понять, почистить
    public class Fourier
    {
        private IImageHandler _transposed;
        private ComputeKernel _spinFactKernel;
        private ComputeKernel _butterflyKernel;
        private ComputeKernel _splitFftKernel;
        private ComputeKernel _mergeFftKernel;
        private ComputeBuffer<Vector2> _tmpBuffer;
        private ComputeBuffer<Vector2> _tmpBuffer2;
        private ComputeBuffer<Vector2> _spinFactBuffer;
        private int _width;
        private int _height;
        private int _m;
        private int _l;
        private int _t;
        private int _n;
        private OpenClApplication _app;

        public Fourier(int width, int height)
        {
            _width = width;
            _height = height;

            _n = width;
            // n должно быть четным
            if (_n % 2 != 0)
                _n--;

            _m = OddDenominator(_n); // нечетный делитель ширины изображения
            _l = _n / _m; // четный делитель ширины изображения
            _t = PowerOfTwo(_l);

            _transposed = ImageHandler.Create("transponed", height, width, ImageFormat.RealImaginative, ImagePixelFormat.Float);

            _app = Singleton.Get<OpenClApplication>();
            _splitFftKernel = _app.Program.CreateKernel("splitFft");
            _mergeFftKernel = _app.Program.CreateKernel("mergeFft");
            _spinFactKernel = _app.Program.CreateKernel("spinFact");
            _butterflyKernel = _app.Program.CreateKernel("butterfly");

            _tmpBuffer = new ComputeBuffer<Vector2>(_app.ComputeContext, ComputeMemoryFlags.None, width * height);
            _tmpBuffer2 = new ComputeBuffer<Vector2>(_app.ComputeContext, ComputeMemoryFlags.None, width * height);
            _spinFactBuffer = new ComputeBuffer<Vector2>(_app.ComputeContext, ComputeMemoryFlags.None, _n / 2);
            GenerateSpinFactor();
        }
        
        public void FastTransform(IImageHandler input, IImageHandler output = null)
        {
            using (new Timer("Fast transform"))
            {
                if (output == null)
                    _app.Acquire(input, _transposed);
                else
                    _app.Acquire(input, output, _transposed);

                Pass(input, output);
                ImageUtils.Transpose(output ?? input, _transposed);
                Pass(_transposed);
                ImageUtils.Transpose(_transposed, output ?? input);
            
                if (output == null)
                    _app.Release(input, _transposed);
                else
                    _app.Release(input, output, _transposed);
            }
        }
        
        /// <summary>
        /// Проход по строкам изображения
        /// Если input == null, данные будут взяты из output и туда же записаны
        /// </summary>
        /// <param name="output"></param>
        /// <param name="input"></param>
        private void Pass(IImageHandler input, IImageHandler output = null, bool inverse = false)
        {
            SplitFft(input, _tmpBuffer);    // input => Buffer
            Fft(_tmpBuffer, inverse);   // Buffer
            MergeFft(_tmpBuffer, _tmpBuffer2, output ?? input); // Buffer => output ?? input
        }

        /// <summary>
        /// Переписываем изображение в буфер, разбивая на подмассивы для БПФ (если m > 1) и изменяя индексы на bitreversed
        /// См Глава 4 "4.1.4 Быстрое преобразование Фурье для четного количества точек"
        ///  </summary>
        /// <param name="input"></param>
        private void SplitFft(IImageHandler input, ComputeBuffer<Vector2> output)
        {
            using (new Timer("Split"))
            {
                _splitFftKernel.SetMemoryArgument(0, input.ComputeBuffer);
                _splitFftKernel.SetMemoryArgument(1, output);
                _splitFftKernel.SetValueArgument(2, _n);
                _splitFftKernel.SetValueArgument(3, _m);
                _splitFftKernel.SetValueArgument(4, _l);
                _splitFftKernel.SetValueArgument(5, _t);

                _app.ExecuteInQueue(_splitFftKernel, _width, _height);
            }
        }
        
        private void Fft(ComputeBuffer<Vector2> input, bool inverse)
        {
            using (new Timer("FFT"))
            {
                _butterflyKernel.SetMemoryArgument(0, input);
                _butterflyKernel.SetMemoryArgument(1, _spinFactBuffer);
                _butterflyKernel.SetValueArgument(2, _t);
                _butterflyKernel.SetValueArgument(3, _l);
                _butterflyKernel.SetValueArgument(5, inverse ? 0x80000000 : 0);
                for (var i = 1; i <= _t; i++)
                {
                    _butterflyKernel.SetValueArgument(4, i);

                    _app.ExecuteInQueue(_butterflyKernel, _l / 2,_height * _m);
                }
            }
        }

        private void MergeFft(ComputeBuffer<Vector2> input, ComputeBuffer<Vector2> tmp, IImageHandler output)
        {
            using (new Timer("Merge"))
            {
                _mergeFftKernel.SetMemoryArgument(0, input);
                _mergeFftKernel.SetMemoryArgument(1, output.ComputeBuffer);
                _mergeFftKernel.SetValueArgument(2, _n);
                _mergeFftKernel.SetValueArgument(3, _m);
                _mergeFftKernel.SetValueArgument(4, _l);

                _app.ExecuteInQueue(_mergeFftKernel, _width, _height);
            }
        }

        private void GenerateSpinFactor()
        {
            _spinFactKernel.SetMemoryArgument(0, _spinFactBuffer);
            _spinFactKernel.SetValueArgument(1, _l);

            _app.ExecuteInQueue(_spinFactKernel, _l / 2, 1);
        }

        #region private static

        /// <summary>
        /// Нахождение нечентного делителя
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int OddDenominator(int num)
        {
            // делим число на 2 пока оно не перестанет быть четным
            while(true)
            {
                if (num == 1)
                    break;
                if (num % 2 == 0)
                    num = num / 2;
                else
                    break;
            }
            return num;
        }

        /// <summary>
        /// Получаем, в какую степень нужно возвести двойку, чтобы получить l (log по основанию два, но быстрее)
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public static int PowerOfTwo(int l)
        {
            return (int) Math.Log(l, 2);
        }
        #endregion

        #region public static

        private static Dictionary<string, Fourier> _cachedProcessors = new Dictionary<string, Fourier>();

        [DataProcessor("Преобразование Фурье", ProcessorGroups.Transforms)]
        public static void Transform(IImageHandler input, bool cyclicShift = true, [ImageHandlerFilter(AllowedImageFormat.RealImaginative, AllowedImagePixelFormat.Float)] IImageHandler output = null)
        {
            var key = $"{input.Width}_{input.Height}";
            
            if (!_cachedProcessors.TryGetValue(key, out Fourier processor))
                _cachedProcessors[key] = processor = new Fourier(input.Width, input.Height);

            processor.FastTransform(input, output);

            if (cyclicShift)
                ImageUtils.CyclicShift(output ?? input);
        }

        #endregion
    }
}