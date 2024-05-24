using System.Collections.Generic;

namespace TeamWork.Core.Sheard
{
    public class PaginationRecord<T>
    {
        public IEnumerable<T> DataRecord { get; set; }
        public int CountRecord { get; set; }
    }
}
