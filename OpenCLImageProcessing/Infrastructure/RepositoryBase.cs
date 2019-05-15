using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    public interface IRepository<T> : ICollection<T>, INotifyCollectionChanged
    {
    }

    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        protected List<T> _items = new List<T>();

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event Action<T> ItemAdded;
        public event Action<T> ItemRemoved;

        public int Count => _items.Count;
        public bool IsReadOnly => false;

        public void Add(T item)
        {
            _items.Add(item);

            var changedItems = new List<T> { item };
            CollectionChanged?.Invoke(null, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, changedItems));
            ItemAdded?.Invoke(item);
        }

        public void Clear()
        {
            _items.Clear();

            CollectionChanged?.Invoke(null, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        bool ICollection<T>.Remove(T item)
        {
            var result = _items.Remove(item);

            if (result)
            {
                var changedItems = new List<T> { item };
                CollectionChanged?.Invoke(null, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, changedItems));
            }

            return result;
        }

        public IReadOnlyCollection<T> Get(Func<T, bool> predicate)
        {
            return _items.Where(predicate).ToArray();
        }

        public IReadOnlyCollection<T> GetAll()
        {
            return _items.ToArray();
        }

        public void Remove(T item)
        {
            _items.Remove(item);

            ItemRemoved?.Invoke(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}