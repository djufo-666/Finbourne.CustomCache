using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finbourne.CustomCache
{
    public class CacheItem<T> : ICacheItem<T>
    {
        public DateTime CreatedAt { get; }

        public object Key { get; }

        public object Item { get; }

        internal CacheItem(object key, T item, DateTime createdAt)
        {
            this.Key = key;
            this.Item = item;
            this.CreatedAt = createdAt;
        }
        public CacheItem(object key, T item): this(key, item, DateTime.Now) { }

    }
}
