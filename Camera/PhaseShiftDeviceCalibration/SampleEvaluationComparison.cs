using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Camera.PhaseShiftDeviceCalibration
{
    public class SampleEvaluationComparison
    {
        public SampleEvaluation ZeroSampleEvaluation { get; }
        public SampleEvaluation OtherSampleEvaluation { get; }

        public float NormalizedLinearDifference { get; private set; }
        public float EvaluatedDifferenceValue { get; private set; }
        public SampleEvaluation.Loop.SpecificPointType ClosestSpecificPoint { get; private set; }
        public float ClosestSpecificPointInterval { get; private set; }
        public SampleEvaluation.Loop ZeroLoop { get; private set; }
        public SampleEvaluation.Loop OtherLoop { get; private set; }


        public SampleEvaluationComparison(SampleEvaluation zeroSampleEvaluation, SampleEvaluation otherSampleEvaluation)
        {
            ZeroSampleEvaluation = zeroSampleEvaluation;
            OtherSampleEvaluation = otherSampleEvaluation;
        }

        public void Update()
        {
            if (ZeroSampleEvaluation.Loops.Count < 2)
                return;

            var zeroLoop = ZeroSampleEvaluation.Loops[1];
            var otherLoop = OtherSampleEvaluation.Loops.Skip(1).First(l => l.IsPositive == zeroLoop.IsPositive);
            ZeroLoop = zeroLoop;
            OtherLoop = otherLoop;

            var linearDifference = (float) (otherLoop.Start - zeroLoop.Start);
            var normalizedLinearDifference = linearDifference / zeroLoop.Length;
            NormalizedLinearDifference = normalizedLinearDifference;

            var evaluatedDifference = zeroLoop.Interpolation.GetValue(normalizedLinearDifference);
            EvaluatedDifferenceValue = evaluatedDifference;

            ClosestSpecificPoint = ResolveSpecificPoint(evaluatedDifference, out var interval);
            ClosestSpecificPointInterval = interval;
        }

        public static string GetSpecificPointLabel(SampleEvaluation.Loop.SpecificPointType difference)
        {
            switch (difference)
            {
                case SampleEvaluation.Loop.SpecificPointType.Start:
                    return "0";

                case SampleEvaluation.Loop.SpecificPointType.Quarter:
                    return "¼";

                case SampleEvaluation.Loop.SpecificPointType.Half:
                    return "½";

                case SampleEvaluation.Loop.SpecificPointType.ThreeQuarters:
                    return "¾";

                case SampleEvaluation.Loop.SpecificPointType.Loop:
                    return "1";

                default:
                    return "";
            }
        }

        private SampleEvaluation.Loop.SpecificPointType ResolveSpecificPoint(float differenceValue, out float interval)
        {
            const float eighth = 1 / 8f;

            if (IsInRange(0, eighth, differenceValue, out interval))
                return SampleEvaluation.Loop.SpecificPointType.Start;

            if (IsInRange(1 / 4f, eighth, differenceValue, out interval))
                return SampleEvaluation.Loop.SpecificPointType.Quarter;

            if (IsInRange(2 / 4f, eighth, differenceValue, out interval))
                return SampleEvaluation.Loop.SpecificPointType.Half;

            if (IsInRange(3 / 4f, eighth, differenceValue, out interval))
                return SampleEvaluation.Loop.SpecificPointType.ThreeQuarters;

            if (IsInRange(1, eighth, differenceValue, out interval))
                return SampleEvaluation.Loop.SpecificPointType.Loop;

            interval = 0;
            return SampleEvaluation.Loop.SpecificPointType.None;
        }

        private static bool IsInRange(float point, float range, float value, out float interval)
        {
            var diff = point - value;

            interval = Math.Abs(diff) / range * 100;

            return diff * diff < range * range;
        }
    }
}