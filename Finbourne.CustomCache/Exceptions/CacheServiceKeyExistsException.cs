using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finbourne.CustomCache.Exceptions
{
    public class CacheServiceKeyExistsException: Exception
    {
        public object Key { get; }
        public CacheServiceKeyExistsException(object key)
            : base ($"Key '{key}' exists in cache")
        {
            this.Key = key;
        }
    }
}
