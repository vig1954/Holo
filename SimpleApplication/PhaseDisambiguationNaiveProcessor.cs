using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Processing;

namespace SimpleApplication
{
    public class PhaseDisambiguationNaiveProcessor
    {
        private readonly ImageHandler _input;

        private float[] _initialData;
        private int[] _continiousZonesMap;
        private int _width;
        private int _height;

        public int Width => _width;
        public int Height => _height;
        public float[] InitialData => _initialData;

        public PhaseDisambiguationNaiveProcessor(ImageHandler input)
        {
            _input = input;
        }

        public void ExtractDataToProcess()
        {
            _initialData = _input.ReadPhaseFromComputingDevice();
            _continiousZonesMap = new int[_input.Width * _input.Height];
            _width = _input.Width;
            _height = _input.Height;
        }

        public void DefineContiniousZones()
        {
            if (_initialData == null)
                throw new InvalidOperationException();

            int zoneIndex = 1;
            int x, y, index;
            float diff, current, left, top;
            float thershold = (float) Math.PI;
            for (y = 0; y < _height; y++)
            {
                for (x = 0; x < _width; x++)
                {
                    index = y * _width + x;
                    current = _initialData[index];
                    
                    if (x > 0)
                    {
                        left = _initialData[index - 1];
                        diff = Math.Abs(current - left);
                    }

                    if (y > 0)
                    {
                        top = _initialData[index - _width];
                        diff = Math.Abs(current - top);
                    }
                }
            }
        }
    }
}
