namespace Finbourne.CustomCache
{
    public interface ICacheService
    {
        /// <summary>
        /// Reaonly size of Objects in Cache
        /// </summary>
        int Capacity { get; }
        /// <summary>
        /// Current size of objects in Cache 
        /// </summary>
        int Size { get; }
        /// <summary>
        /// Event handler to subscribe to events when object is evicted from cache
        /// </summary>
        event EvictedHandler Evicted;
        /// <summary>
        /// Add new object to cache 
        /// </summary>
        /// <typeparam name="T">Type of object to cache</typeparam>
        /// <param name="key">Key in cache</param>
        /// <param name="itemToCache">object to cache</param>
        void Add<T>(object key, T itemToCache);
        /// <summary>
        /// Retrieves object from cache
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Key of cached item</param>
        /// <returns>Cached item</returns>
        T Get<T>(object key);
        /// <summary>
        /// Retrieves object from cache
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Key of cached item</param>
        /// <param name="item">Cached item</param>
        /// <returns>True if object exists in cache</returns>
        bool TryGet<T>(object key, out T item);
    }
}