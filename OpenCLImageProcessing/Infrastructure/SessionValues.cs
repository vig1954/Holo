using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace Infrastructure
{
    public class SessionValues
    {
        private Dictionary<string, ConditionalEventHandler> _conditionalHandlers = new Dictionary<string, ConditionalEventHandler>();
        private Dictionary<string, object> _values = new Dictionary<string, object>();
        private Dictionary<string, List<object>> _valuesWithHistory = new Dictionary<string, List<object>>();

        public event Action<SessionValueEventArgs> ValueEvent;

        public void AddConditionalValueEventHandler(string handlerKey, Func<SessionValueEventArgs, bool> predicate, Action<SessionValueEventArgs> handler)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            if (handlerKey == null)
                throw new ArgumentNullException(nameof(handlerKey));

            _conditionalHandlers.Add(handlerKey, new ConditionalEventHandler
            {
                Handler = handler,
                Predicate = predicate
            });
        }

        public void RemoveConditionalValueEventHandler(string handlerKey)
        {
            _conditionalHandlers.Remove(handlerKey);
        }

        public bool Has(string key)
        {
            return _values.ContainsKey(key);
        }

        public object Get(string key)
        {
            return _values[key];
        }

        public T Get<T>(string key)
        {
            return (T) Get(key);
        }

        public bool CheckIfThereIsValueFromListInHistory(string key, IReadOnlyCollection<object> availableValues, out object foundValue)
        {
            foundValue = null;

            if (!_valuesWithHistory.ContainsKey(key))
                return false;

            foundValue = _valuesWithHistory[key].LastOrDefault(availableValues.Contains);

            return foundValue != null;
        }

        public bool CheckIfThereIsValueFromListInHistory<T>(string key, IReadOnlyCollection<T> availableValues, out T foundValue) where T:struct
        {
            var result = CheckIfThereIsValueFromListInHistory(key, availableValues.Select(v => (object) v).ToArray(), out var found);

            foundValue = (T) (found ?? typeof(T).GetDefaultValue());

            return result;
        }

        public void Set(string key, object value, object sender, bool saveHistory = false)
        {
            if (!value.GetType().IsPrimitive)
                throw new InvalidOperationException();

            if (_values.ContainsKey(key))
            {
                _values[key] = value;
                InvokeEventListeners(key, value, SessionValueEventArgs.EventType.Updated, sender);
            }
            else
            {
                _values.Add(key, value);
                InvokeEventListeners(key, value, SessionValueEventArgs.EventType.Added, sender);
            }

            if (saveHistory)
                SaveHistory(key, value);
        }

        public void Delete(string key, object sender)
        {
            if (_values.ContainsKey(key))
            {
                _values.Remove(key);
                InvokeEventListeners(key, null, SessionValueEventArgs.EventType.Deleted, sender);
            }
        }

        public string ToJson()
        {
            return new JsonStorage
            {
                Values = _values.ToDictionary(g => g.Key, g => new JsonStorage.ValueInfo(g.Value)),
                ValuesWithHistory = _valuesWithHistory.ToDictionary(g => g.Key, g => g.Value.Select(v => new JsonStorage.ValueInfo(v)).ToList())
            }.ToJson();
        }

        public void FromJson(string json)
        {
            var storage = json.FromJson<JsonStorage>();
            _values = storage.Values.ToDictionary(g => g.Key, g => g.Value.Value.UnboxAndCastTo(g.Value.ValueType));
            _valuesWithHistory = storage.ValuesWithHistory.ToDictionary(g => g.Key, g => g.Value.Select(v => v.Value.UnboxAndCastTo(v.ValueType)).ToList());
        }

        private void InvokeEventListeners(string key, object value, SessionValueEventArgs.EventType eventType, object sender)
        {
            var sessionValueEventArgs = new SessionValueEventArgs(eventType, key, value, sender);
            ValueEvent?.Invoke(sessionValueEventArgs);

            foreach (var conditionalEventHandler in _conditionalHandlers.Values.Where(h => h.Predicate(sessionValueEventArgs)))
            {
                conditionalEventHandler.Handler.Invoke(sessionValueEventArgs);
            }
        }

        private void SaveHistory(string key, object value)
        {
            if (!_valuesWithHistory.ContainsKey(key))
                _valuesWithHistory.Add(key, new List<object>());

            _valuesWithHistory[key].Add(value);
        }

        public class SessionValueEventArgs
        {
            public EventType Type { get; }
            public string Key { get; }
            public object Value { get; }
            public object Sender { get; }

            public SessionValueEventArgs(EventType type, string key, object value, object sender)
            {
                Type = type;
                Key = key;
                Value = value;
                Sender = sender;
            }

            public enum EventType
            {
                Added,
                Updated,
                Deleted
            }
        }

        private class ConditionalEventHandler
        {
            public Func<SessionValueEventArgs, bool> Predicate { get; set; }
            public Action<SessionValueEventArgs> Handler { get; set; }
        }

        private class JsonStorage
        {
            public class ValueInfo
            {
                public object Value { get; set; }
                public Type ValueType { get; set; }

                public ValueInfo(object value)
                {
                    Value = value;
                    ValueType = value.GetType();
                }
            }

            public Dictionary<string, ValueInfo> Values { get; set; }
            public Dictionary<string, List<ValueInfo>> ValuesWithHistory { get; set; }
        }
    }
}
