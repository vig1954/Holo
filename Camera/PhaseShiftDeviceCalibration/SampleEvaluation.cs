using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace Camera.PhaseShiftDeviceCalibration
{
    public class SampleEvaluation
    {
        private float _min;
        private float _max;
        private float _middle;
        private float _average;
        private float[] _sampleAverage;
        private List<Area> _areas;
        private List<Loop> _loops;

        public float MinimumValue => _min;
        public float MaximumValue => _max;
        public float Average => _average;
        public float Middle => _middle;

        public Sample Sample { get; }
        public float[] SampleAverage => _sampleAverage;
        public IReadOnlyList<Area> Areas => _areas;
        public IReadOnlyList<Loop> Loops => _loops;

        public SampleEvaluation(Sample sample)
        {
            Sample = sample;
        }

        public void Update()
        {
            _sampleAverage = Sample.GetAverage();

            EvaluateMinMax();
            EvaluateAreas();
            EvaluateLoops();
        }

        public override string ToString()
        {
            return $"{Sample.PhaseShiftDeviceParameterValue}";
        }

        private void EvaluateMinMax()
        {
            _min = _sampleAverage[0];
            _max = _sampleAverage[0];

            float m = 0;
            float sampleValue;
            for (var i = 1; i < _sampleAverage.Length; i++)
            {
                sampleValue = _sampleAverage[i];

                if (sampleValue > _max)
                    _max = sampleValue;

                if (sampleValue < _min)
                    _min = sampleValue;

                m += sampleValue;
            }

            _average = m / _sampleAverage.Length;
            _middle = (_max + _min) / 2;
        }

        private void EvaluateAreas()
        {
            var start = 0;
            var end = 1;
            var firstValue = _sampleAverage[0];
            var secondValue = _sampleAverage[1];
            var min = Math.Min(firstValue, secondValue);
            var max = Math.Max(firstValue, secondValue);

            var minIndex = firstValue < secondValue ? 0 : 1;
            var maxIndex = firstValue > secondValue ? 0 : 1;

            var sum = firstValue + secondValue;
            var positive = secondValue > _average;
            bool currentPositive;
            float val;

            _areas = new List<Area>();
            for (var i = 2; i < _sampleAverage.Length; i++)
            {
                val = _sampleAverage[i];
                currentPositive = val > _average;

                if (currentPositive != positive)
                {
                    var area = new Area(start, end, positive ? max : min, positive ? maxIndex : minIndex, _average, sum);
                    _areas.Add(area);

                    start = i;
                    min = val;
                    max = val;
                    sum = val;
                    positive = currentPositive;

                    continue;
                }

                end = i;

                if (min > val)
                {
                    min = val;
                    minIndex = i;
                }

                if (max < val)
                {
                    max = val;
                    maxIndex = i;
                }

                sum += Math.Abs(val - _average);

                positive = currentPositive;
            }
        }

        private void EvaluateLoops()
        {
            _loops = new List<Loop>();

            Area first, nextOpposite, next;

            for (var i = 0; i < Areas.Count - 2; i++)
            {
                first = _areas[i];
                nextOpposite = _areas[i + 1];
                next = _areas[i + 2];

                _loops.Add(new Loop(first, next, nextOpposite));
            }
        }

        public class Area
        {
            public int Start { get; }
            public int End { get; }

            public bool IsPositive => Extremum > Origin;

            public int ExtremumIndex { get; }
            public float Extremum { get; }
            public float Origin { get; }

            public float Average { get; }
            public float Sum { get; }


            public int Count => End - Start;

            public Area(int start, int end, float extremum, int extremumIndex, float origin, float sum)
            {
                Start = start;
                End = end;
                Extremum = extremum;
                ExtremumIndex = extremumIndex;
                Origin = origin;
                Sum = sum;

                Average = sum / Count;
            }
        }

        public class Loop
        {
            public enum SpecificPointType
            {
                None,
                Start,
                Quarter,
                Half,
                ThreeQuarters,
                Loop
            }

            public int Start => StartArea.ExtremumIndex;
            public int End => EndArea.ExtremumIndex;
            public int OppositeExtremumIndex => OppositeArea.ExtremumIndex;
            public int Length => End - Start;
            public int FirstHalfLoopLength => OppositeExtremumIndex - Start;
            public int SecondHalfLoopLength => End - OppositeExtremumIndex;
            public bool IsPositive => StartArea.IsPositive;

            public Area StartArea { get; }
            public Area EndArea { get; }
            public Area OppositeArea { get; }

            public IReadOnlyDictionary<SpecificPointType, PointF> SpecificPoints { get; }

            public Interpolation Interpolation { get; } // интерполяция координаты в период

            public Loop(Area start, Area end, Area opposite)
            {
                if (start.IsPositive != end.IsPositive)
                    throw new InvalidOperationException();

                if (start.IsPositive == opposite.IsPositive)
                    throw new InvalidOperationException();

                StartArea = start;
                EndArea = end;
                OppositeArea = opposite;

                var flength = (float) Length;
                var x0 = 0;
                var x1 = (opposite.Start - start.ExtremumIndex) / flength;
                var x2 = (opposite.ExtremumIndex - start.ExtremumIndex) / flength;
                var x3 = (end.Start - start.ExtremumIndex) / flength;
                var x4 = (end.ExtremumIndex - start.ExtremumIndex) / flength;

                Interpolation = new Interpolation(x0, 0, x1, 0.25f, x2, 0.5f, x3, 0.75f, x4, 1);

                SpecificPoints = new Dictionary<SpecificPointType, PointF>()
                {
                    [SpecificPointType.Start] = new PointF(start.ExtremumIndex, start.Extremum),
                    [SpecificPointType.Quarter] = new PointF(opposite.Start, opposite.Origin),
                    [SpecificPointType.Half] = new PointF(opposite.ExtremumIndex, opposite.Extremum),
                    [SpecificPointType.ThreeQuarters] = new PointF(end.Start, end.Origin),
                    [SpecificPointType.Loop] = new PointF(end.ExtremumIndex, end.Extremum)
                };
            }
        }

        public class Interpolation
        {
            private readonly float x0, x1, x2, x3, x4;
            private readonly float y0, y1, y2, y3, y4;

            public Interpolation(float x0, float y0, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
            {
                this.x0 = x0;
                this.x1 = x1;
                this.x2 = x2;
                this.x3 = x3;
                this.x4 = x4;

                this.y0 = y0;
                this.y1 = y1;
                this.y2 = y2;
                this.y3 = y3;
                this.y4 = y4;
            }

            public float GetValue(float x) => y0 * l0(x) + y1 * l1(x) + y2 * l2(x) + y3 * l3(x) * y4 * l4(x);

            private float l0(float x) => (x - x1) / (x0 - x1) * (x - x2) / (x0 - x2) * (x - x3) / (x0 - x3) * (x - x4) / (x0 - x4);
            private float l1(float x) => (x - x0) / (x1 - x0) * (x - x2) / (x1 - x2) * (x - x3) / (x1 - x3) * (x - x4) / (x1 - x4);
            private float l2(float x) => (x - x0) / (x2 - x0) * (x - x1) / (x2 - x1) * (x - x3) / (x2 - x3) * (x - x4) / (x2 - x4);
            private float l3(float x) => (x - x0) / (x3 - x0) * (x - x1) / (x3 - x1) * (x - x2) / (x3 - x2) * (x - x4) / (x3 - x4);
            private float l4(float x) => (x - x0) / (x4 - x0) * (x - x1) / (x4 - x1) * (x - x2) / (x4 - x2) * (x - x3) / (x4 - x3);
        }
    }
}