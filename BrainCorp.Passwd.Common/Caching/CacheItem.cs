using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainCorp.Passwd.Common.Caching
{
    public class CacheItem
    {
        public object Obj { get; set; }

        public DateTime TimeAddedUTC { get; set; }
    }
}
