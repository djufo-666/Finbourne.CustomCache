using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finbourne.CustomCache.Exceptions
{
    public class CacheServiceCapacityException: Exception
    {
        public int Capacity { get; }

        public CacheServiceCapacityException(int capacity): base($"Capacity {capacity} not supported") { }
    }
}
