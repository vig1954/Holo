using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    public class EventManager
    {
        private Dictionary<Type, List<EventHandlerInfo>> _eventHandlers = new Dictionary<Type, List<EventHandlerInfo>>();

        public void Emit(EventBase @event)
        {
            var eventHandlerInfos = GetOrAddEventHandlerList(@event.GetType());

            foreach (var eventHandlerInfo in eventHandlerInfos)
            {
                eventHandlerInfo.Invoke(@event);
            }
        }

        public void Subscribe<TEvent>(IEventHandler<TEvent> eventHandler) where TEvent : EventBase
        {
            var eventHandlerInfos = GetOrAddEventHandlerList(typeof(TEvent));

            eventHandlerInfos.Add(new EventHandlerInfo(eventHandler.GetType(), @event => eventHandler.Handle((TEvent) @event)));
        }

        public void Unsubscribe<TEvent>(IEventHandler<TEvent> eventHandler) where TEvent : EventBase
        {
            var eventHandlerInfos = GetOrAddEventHandlerList(typeof(TEvent));
            var eventHandlerInfo = eventHandlerInfos.SingleOrDefault(h => h.HandlerType == eventHandler.GetType());
            eventHandlerInfos.Remove(eventHandlerInfo);
        }

        private List<EventHandlerInfo> GetOrAddEventHandlerList(Type eventType)
        {
            if (_eventHandlers.ContainsKey(eventType))
                return _eventHandlers[eventType];

            var eventHandlerInfos = new List<EventHandlerInfo>();
            _eventHandlers.Add(eventType, eventHandlerInfos);

            return eventHandlerInfos;
        }

        private class EventHandlerInfo
        {
            private readonly Action<EventBase> _handlerAction;

            public Type HandlerType { get; }

            public EventHandlerInfo(Type handlerType, Action<EventBase> handlerAction)
            {
                _handlerAction = handlerAction;
                HandlerType = handlerType;
            }

            public void Invoke(EventBase @event)
            {
                _handlerAction(@event);
            }
        }
    }

    public abstract class EventBase
    {
        public object Sender { get; }

        protected EventBase(object sender)
        {
            Sender = sender;
        }
    }

    public interface IEventHandler<in TEvent> where TEvent : EventBase
    {
        void Handle(TEvent @event);
    }
}
