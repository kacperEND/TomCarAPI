using System.Collections.Generic;

namespace Application.Dto
{
    public class CollectionResult<T>
    {
        public int TotalCount { get; set; }
        public IEnumerable<T> Items { get; set; }

        public CollectionResult()
        {
            this.TotalCount = 0;
            this.Items = new List<T>();
        }
    }
}