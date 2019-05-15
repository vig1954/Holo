using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Camera.PhaseShiftDeviceCalibration;
using Common;
using PlotDrawer = Camera.PhaseShiftDeviceCalibration.PlotDrawer;

namespace Camera
{
    public partial class PhaseShiftDeviceCalibrationForm : Form
    {
        private const int _bufferCount = 30;

        private PlotDrawer _plotDrawer;
        private PhaseShiftDeviceController _phaseShiftDeviceController;
        private IFloatArrayDataProvider _dataProvider;
        private short _shiftParameterValue;
        private bool _captureSamples;
        private SampleEvaluationView _currentEvaluationView;
        private SampleEvaluation _currentEvaluation;
        private SampleEvaluation _evaluationToPreview;
        private SampleEvaluation _zeroEvaluation;

        public PhaseShiftDeviceCalibrationForm(PhaseShiftDeviceController phaseShiftDeviceController, IFloatArrayDataProvider dataProvider)
        {
            InitializeComponent();

            _phaseShiftDeviceController = phaseShiftDeviceController;
            _dataProvider = dataProvider;
            _dataProvider.OnDataUpdated += DataProviderOnDataUpdated;

            _plotDrawer = new PlotDrawer();
            _currentEvaluationView = new SampleEvaluationView
            {
                SampleEvaluation = null,
                IsRecording = true
            };

            CapturedSamplesList.Items.Add(_currentEvaluationView);
        }

        private void DataProviderOnDataUpdated(float[] sampleData)
        {
            if (!_captureSamples)
                return;

            if (_currentEvaluation == null || sampleData.Length != _currentEvaluation.Sample.BufferSize)
            {
                _currentEvaluation = new SampleEvaluation(new Sample(_shiftParameterValue, sampleData.Length, _bufferCount));
                _currentEvaluationView.SampleEvaluation = _currentEvaluation;
                CapturedSamplesList.Refresh();
            }

            if (_evaluationToPreview == null)
                _evaluationToPreview = _currentEvaluation;

            _currentEvaluation.Sample.AddSample(sampleData);
            _currentEvaluation.Update();

            Plot.Refresh();
        }

        private async void SetPhaseShiftButton_Click(object sender, EventArgs e)
        {
            _captureSamples = false;
            _shiftParameterValue = (short) ShiftValue.Value;
            await _phaseShiftDeviceController.SetShiftAsync(_shiftParameterValue);

            _captureSamples = true;
        }

        private void Plot_Paint(object sender, PaintEventArgs e)
        {
            if (_evaluationToPreview == null || _evaluationToPreview.Sample.SampleCount == 0)
                return;

            if (_zeroEvaluation != null)
            {
                var comparison = new SampleEvaluationComparison(_zeroEvaluation, _evaluationToPreview);
                comparison.Update();

                _plotDrawer.DrawSampleEvaluationDifference(comparison, GetImageLayout(), e.Graphics);
            }
            else
                _plotDrawer.DrawSampleEvaluation(_evaluationToPreview, GetImageLayout(), e.Graphics);
        }

        private ImageLayoutInfo GetImageLayout() => new ImageLayoutInfo(Plot.ClientSize.Width, Plot.ClientSize.Height);

        private void CapturedSamplesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _evaluationToPreview = (CapturedSamplesList.SelectedItem as SampleEvaluationView)?.SampleEvaluation;
        }

        private void SaveSample_Click(object sender, EventArgs e)
        {
            if (CapturedSamplesList.Items.Cast<SampleEvaluationView>().Any(v => !v.IsRecording && v.SampleEvaluation == _currentEvaluation))
                return;
            
            var evaluationView = new SampleEvaluationView
            {
                SampleEvaluation = _currentEvaluation
            };

            CapturedSamplesList.Items.Add(evaluationView);

            _currentEvaluationView.SampleEvaluation = null;
            _currentEvaluation = null;
            _evaluationToPreview = null;
        }

        private void SetAsZero_Click(object sender, EventArgs e)
        {
            var selectedEvaluationView = CapturedSamplesList.SelectedItem as SampleEvaluationView;

            if (selectedEvaluationView == null || selectedEvaluationView.IsRecording)
                return;

            var currentZeroSampleEvaluationView = CapturedSamplesList.Items.Cast<SampleEvaluationView>().SingleOrDefault(v => v.IsZero);
            if (currentZeroSampleEvaluationView != null)
                currentZeroSampleEvaluationView.IsZero = false;

            selectedEvaluationView.IsZero = true;
            _zeroEvaluation = selectedEvaluationView.SampleEvaluation;

            CapturedSamplesList.Refresh();
        }

        private class SampleEvaluationView
        {
            public SampleEvaluation SampleEvaluation { get; set; }
            public bool IsRecording { get; set; }
            public bool IsZero { get; set; }

            public override string ToString()
            {
                return $"{(IsRecording ? "<rec>" : "")}{SampleEvaluation}{(IsZero ? " [0]" : "")}";
            }
        }
    }
}