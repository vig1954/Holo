using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camera.PhaseShiftDeviceCalibration
{
    public class Sample
    {
        private int _currentBuffer;
        private float[][] _buffers;

        public int SampleCount { get; private set; }

        public short PhaseShiftDeviceParameterValue { get; }
        public int BufferSize { get; }
        public int BufferCount { get; }

        public Sample(short phaseShiftDeviceParameterValue, int bufferSize, int bufferCount)
        {
            PhaseShiftDeviceParameterValue = phaseShiftDeviceParameterValue;
            BufferSize = bufferSize;
            BufferCount = bufferCount;

            _buffers = new float[bufferCount][];
        }

        public void AddSample(float[] sample)
        {
            if (sample.Length != BufferSize)
                throw new InvalidOperationException();

            _buffers[_currentBuffer++] = sample;

            if (SampleCount < BufferCount)
                SampleCount++;

            if (_currentBuffer >= BufferCount)
                _currentBuffer = 0;
        }

        public float[] GetAverage()
        {
            var result = new float[BufferSize];

            int i, sampleIndex;
            float v;

            for (i = 0; i < BufferSize; i++)
            {
                v = 0;

                for (sampleIndex = 0; sampleIndex < SampleCount; sampleIndex++)
                {
                    v += _buffers[sampleIndex][i];
                }

                v /= SampleCount;

                result[i] = v;
            }

            return result;
        }
    }
}