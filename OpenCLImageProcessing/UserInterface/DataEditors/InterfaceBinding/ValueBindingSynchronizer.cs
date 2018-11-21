using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Common;
using Infrastructure;

namespace UserInterface.DataEditors.InterfaceBinding
{
    public interface IBindingSynchronizer : IDisposable
    {
        bool Enabled { get; set; }
        event Action<bool> StateChanged;
    }

    public interface ISynchronizableBinding : IValueBinding
    {
        string SynchronizationKey { get; }
        IBindingSynchronizer Synchronizer { get; }
    }

    public class ValueBindingSynchronizer : IBindingSynchronizer
    {
        private readonly ISynchronizableBinding _binding;
        private readonly string _sessionValuesHandlerKey;
        private string _valueKey => _binding.SynchronizationKey;
        private SessionValues _sessionValues => Singleton.Get<SessionValues>();

        private string _synchronizationStateKey => $"{_valueKey}_synchronized";

        public bool Enabled
        {
            get => _sessionValues.Has(_synchronizationStateKey);
            set
            {
                if (value)
                {
                    _sessionValues.Set(_synchronizationStateKey, true, this);

                    UpdateBindingValueIfSynchronizationIsEnabled();
                }
                else
                    _sessionValues.Delete(_synchronizationStateKey, this);
            }
        }

        public event Action<bool> StateChanged;

        public ValueBindingSynchronizer(ISynchronizableBinding binding)
        {
            _binding = binding;
            _binding.ValueUpdated += BindingOnValueUpdated;

            _sessionValuesHandlerKey = _valueKey + "_" + Guid.NewGuid();
            _sessionValues.AddConditionalValueEventHandler(_sessionValuesHandlerKey, e => e.Key == _valueKey || e.Key == _synchronizationStateKey, SessionValueUpdatedHandler);

            AddSynchronizer(_valueKey);
            UpdateBindingValueIfSynchronizationIsEnabled();
        }

        private void BindingOnValueUpdated(ValueUpdatedEventArgs e)
        {
            if (e.Sender == this)
                return;

            _sessionValues.Set(_valueKey, _binding.GetValue(), this);
        }

        public void UpdateBindingValueIfSynchronizationIsEnabled()
        {
            if (_sessionValues.Has(_valueKey) && Enabled)
                _binding.SetValue(_sessionValues.Get(_valueKey), this);
        }

        public void Dispose()
        {
            _sessionValues.RemoveConditionalValueEventHandler(_sessionValuesHandlerKey);
            RemoveSynchronizer(_valueKey);
        }

        private void SessionValueUpdatedHandler(SessionValues.SessionValueEventArgs e)
        {
            if (e.Key == _synchronizationStateKey)
                StateChanged?.Invoke(Enabled);
            else if (Enabled && e.Sender != this)
            {
                using (StartValueUpdateScope(_valueKey))
                {
                    _binding.SetValue(e.Value, this);
                }
            }
        }

        #region static members

        private static Dictionary<string, int> _valuesSynchronizersCount = new Dictionary<string, int>();
        private static Dictionary<string, int> _valuesValueUpdateSynchronizersLeft = new Dictionary<string, int>();

        public static event Action ValueUpdateStarted;
        public static event Action ValueUpdateFinished;

        private void AddSynchronizer(string valueKey)
        {
            if (_valuesSynchronizersCount.ContainsKey(valueKey))
                _valuesSynchronizersCount[valueKey]++;
            else
                _valuesSynchronizersCount.Add(valueKey, 1);
        }

        private void RemoveSynchronizer(string valueKey)
        {
            _valuesSynchronizersCount[valueKey]--;
        }

        private ValueUpdateScope StartValueUpdateScope(string valueKey)
        {
            return new ValueUpdateScope(valueKey, _valuesSynchronizersCount[_valueKey] - 1);
        }

        private class ValueUpdateScope : IDisposable
        {
            private string _valueKey;
            public bool AllValuesUpdated => !_valuesValueUpdateSynchronizersLeft.ContainsKey(_valueKey);

            private Timer _timer;

            public ValueUpdateScope(string valueKey, int synchronizersCount)
            {
                if (!_valuesValueUpdateSynchronizersLeft.ContainsKey(valueKey))
                {
                    _valuesValueUpdateSynchronizersLeft.Add(valueKey, synchronizersCount);
                    ValueUpdateStarted?.Invoke();
                }

                _valueKey = valueKey;

                var synchronizersLeft = 0;
                _valuesValueUpdateSynchronizersLeft.TryGetValue(valueKey, out synchronizersLeft);
                _timer = new Timer($">>>>> [{valueKey}] ValueUpdateScope, synchronizers left: {synchronizersLeft}");
            }

            private void UpdateValue()
            {
                var synchronizersLeft = _valuesValueUpdateSynchronizersLeft[_valueKey] - 1;

                if (synchronizersLeft == 0)
                    _valuesValueUpdateSynchronizersLeft.Remove(_valueKey);
                else
                    _valuesValueUpdateSynchronizersLeft[_valueKey] = synchronizersLeft;
            }

            public void Dispose()
            {
                UpdateValue();

                if (AllValuesUpdated)
                    ValueUpdateFinished?.Invoke();

                _timer.Dispose();
            }
        }

        #endregion
    }
}