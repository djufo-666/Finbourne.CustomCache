using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Finbourne.CustomCache
{
    public delegate void EvictedHandler(object item);

    public class CacheService : ICacheService
    {
        private object _lock;
        private Dictionary<object, LinkedListNode<ICacheItem>> _keyCacheRef;
        private LinkedList<ICacheItem> _recentCacheRef;
        public int Capacity { get; }
        public int Size { get; private set; }

        private EvictedHandler _evicted;
        public event EvictedHandler Evicted
        {
            add
            {
                lock (_lock)
                {
                    this._evicted += value;
                }
            }
            remove
            {
                lock (_lock)
                {
                    this._evicted -= value;
                }
            }
        }

        public CacheService(int capacity)
        {
            if (capacity < 1)
            {
                throw new Exceptions.CacheServiceCapacityException(capacity);
            }

            this._lock = new object();
            this.Capacity = capacity;
            this.Size = 0;
            this._keyCacheRef = new Dictionary<object, LinkedListNode<ICacheItem>>();
            this._recentCacheRef = new LinkedList<ICacheItem>();
        }

        internal void Add<T>(CacheItem<T> cacheItem)
        {
            ICacheItem evictedCacheItem = null;

            lock (_lock)
            {
                if (this._keyCacheRef.ContainsKey(cacheItem.Key))
                {
                    throw new Exceptions.CacheServiceKeyExistsException(cacheItem.Key);
                }

                if (this.Size == this.Capacity)
                {
                    evictedCacheItem = RemoveLast();
                }

                this.AddFirst(cacheItem);
            }

            if (evictedCacheItem != null)
            {
                this._evicted?.Invoke(cacheItem.Item);
            }
        }

        private ICacheItem RemoveLast()
        {
            var last = this._recentCacheRef.Last;
            this._recentCacheRef.RemoveLast();

            this._keyCacheRef.Remove(last.Value.Key);

            this.Size--;

            return last.Value;
        }
        private void AddFirst(ICacheItem cacheItem)
        {
            var first = this._recentCacheRef.AddFirst(cacheItem);

            this._keyCacheRef[cacheItem.Key] = first;

            this.Size++;
        }

        private void MoveFirst(LinkedListNode<ICacheItem> node)
        {
            this._recentCacheRef.Remove(node);
            this._recentCacheRef.AddFirst(node);
        }

        public void Add<T>(object key, T itemToCache)
        {
            Add(new CacheItem<T>(key, itemToCache));
        }

        public T Get<T>(object key)
        {
            ICacheItem item = null;

            lock (_lock)
            {
                LinkedListNode<ICacheItem> cacheNode;
                if (this._keyCacheRef.TryGetValue(key, out cacheNode))
                {
                    this.MoveFirst(cacheNode);
                    item = cacheNode.Value;
                }
                else
                {
                    throw new Exceptions.CacheServiceKeyNotFoundException(key);
                }
            }

            ICacheItem<T> typedCacheItem = (ICacheItem<T>)item;

            return typedCacheItem.TypedItem;
        }

        public bool TryGet<T>(object key, out T item)
        {
            lock (_lock)
            {
                LinkedListNode<ICacheItem> cacheNode;
                if (this._keyCacheRef.TryGetValue(key, out cacheNode))
                {
                    this.MoveFirst(cacheNode);
                    ICacheItem cacheItem = cacheNode.Value;
                    ICacheItem<T> typedCacheItem = (ICacheItem<T>)cacheItem;
                    item = typedCacheItem.TypedItem;
                    return true;
                }
                else
                {
                    item = default(T);
                    return false;
                }
            }

        }
    }
}
