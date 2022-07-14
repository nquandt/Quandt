using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Quandt.Expressions.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool SequenceEqual(this IEnumerable first, IEnumerable second)
        {
            //if (comparer == null) comparer = EqualityComparer<TSource>.Default;
            if (first == null) throw new ArgumentNullException("The parameter " + nameof(first) + " cannot be null");
            if (second == null) throw new ArgumentNullException("The parameter " + nameof(second) + " cannot be null"); ;
            IEnumerator e1 = first.GetEnumerator();
            IEnumerator e2 = second.GetEnumerator();

            while (e1.MoveNext())
            {
                if (!(e2.MoveNext() && e1.Current.Equals(e2.Current))) return false;
            }
            if (e2.MoveNext()) return false;

            return true;
        }

        public static List<T> CastToList<T>(this IEnumerable source)
        {
            var list = new List<T>();
            if (source != null)
            {
                foreach (T item in source)
                {
                    list.Add(item);
                }
            }

            return list;
        }
    }
}
