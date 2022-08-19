using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Quandt.Expressions.Javascript.Extensions
{
    public static class ObjectExtensions
    {
        public static int Count(this object obj)
        {
            if (obj is string str)
            {
                return str.Length;
            }else if (obj is IList list)
            {
                return list.Count;
            }else if (obj is IDictionary dict)
            {
                return dict.Count;
            }else if (obj is IEnumerable enumerable)
            {
                return enumerable.Count();
            }
            return -1;
        }

        public static void Add(this object obj, object index, object value = null)
        {            
            if (obj is IList list)
            {
                list.Add(index);
            }
            else if (obj is IDictionary dict)
            {
                dict.Add(index, value);
            }         
        }
        public static int CharCodeAt(this object obj, int index)
        {
            if (obj is string str)
            {
                return str.CharCodeAt(index);
            }

            return -1;
        }

        public static object Index(this object obj, object index)
        {
            if (obj is string str)
            {
                return str[(int)index];
            }
            else if (obj is IList list)
            {
                return list[(int)index];
            }
            else if (obj is IDictionary dict)
            {
                return dict[(string)index];
            }
            
            throw new NotSupportedException();
        }
    }
}
