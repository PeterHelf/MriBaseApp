using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MriBase.Models.Models
{
    public class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        private readonly TKey key;
        private readonly IEnumerable<TElement> values;

        public Grouping(TKey key, IEnumerable<TElement> values)
        {
            this.values = values ?? throw new ArgumentNullException("values");
            this.key = key;
        }

        public TKey Key
        {
            get { return key; }
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
