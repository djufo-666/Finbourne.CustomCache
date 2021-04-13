using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finbourne.CustomCache.Exceptions
{
    public class CacheServiceKeyNotFoundException: Exception
    {
        public object Key { get; }
        public CacheServiceKeyNotFoundException(object key)
            : base ($"Key '{key}' not found in cache")
        {
            this.Key = key;
        }
    }
}
