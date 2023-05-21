using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MriBase.App.Base.ExtensionMethods
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this IList<T> list, Random rnd)
        {
            var len = list.Count;
            for (var i = len - 1; i >= 1; --i)
            {
                var j = rnd.Next(i);
                list.Swap(i, j);
            }
        }

        public static void Swap<T>(this IList<T> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        public static IEnumerable<List<T>> SplitList<T>(this IEnumerable<T> locations, int nSize = 30)
        {
            var locationsList = locations.ToList();

            for (var i = 0; i < locationsList.Count; i += nSize)
                yield return locationsList.GetRange(i, Math.Min(nSize, locationsList.Count - i));
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> iEnumerable)
        {
            return new ObservableCollection<T>(iEnumerable);
        }
    }
}