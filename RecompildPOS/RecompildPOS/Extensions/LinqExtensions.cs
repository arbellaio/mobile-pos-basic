using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace RecompildPOS.Extensions
{
    public static class LinqExtensions
    {
        /// <summary>
        /// Transforms IEnumerable to Observable collection
        /// </summary>
        /// <typeparam name="T">ObservableCollection</typeparam>
        /// <param name="source">source</param>
        /// <returns>New observable collection</returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            return new ObservableCollection<T>(source);
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                return true;
            }

            return !source.Any();
        }
    }
}
