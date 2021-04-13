using System;

namespace Finbourne.CustomCache
{
    public interface ICacheItem
    {
        DateTime CreatedAt { get; }
        object Key { get; }
        object Item { get; }
        Type ItemType { get; }
    }

    public interface ICacheItem<T>: ICacheItem
    {
        Type ICacheItem.ItemType => typeof(T);
        T TypedItem => (T)Item;
    }
}
