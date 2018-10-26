using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Common
{
    public static class ObservableCollectionExtensions
    {
        public static void AddRange<T>(this ObservableCollection<T> self, IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                self.Add(value);
            }
        }
    }
}
