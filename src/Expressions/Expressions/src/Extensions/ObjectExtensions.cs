using System;
using System.Collections.Generic;
using System.Text;

namespace Quandt.Expressions.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Calculates the hash code for an object using two of the object's properties.
        /// </summary>
        /// <param name="value">The object we're creating the hash code for.</param>
        /// <typeparam name="TFirstProperty">The type of the first property the hash is based on.</typeparam>
        /// <typeparam name="TSecondProperty">The type of the second property the hash is based on.</typeparam>
        /// <param name="firstProperty">The first property of the object to use in calculating the hash.</param>
        /// <param name="secondProperty">The second property of the object to use in calculating the hash.</param>
        /// <returns>
        /// A hash code for <c>value</c> based on the values of the provided properties.
        /// </returns>
        /// ReSharper disable UnusedParameter.Global
        public static int GetHashCode<TFirstProperty, TSecondProperty>(this object value,
                                                                      TFirstProperty firstProperty,
                                                                      TSecondProperty secondProperty)
        {
            unchecked
            {
                int hash = 17;

                if (!firstProperty.IsNull())
                    hash = hash * 23 + firstProperty.GetHashCode();
                if (!secondProperty.IsNull())
                    hash = hash * 23 + secondProperty.GetHashCode();

                return hash;
            }
        }

        public static int GetHashCode<TFirstProperty>(this object value,
                                                                      TFirstProperty firstProperty)
        {
            unchecked
            {
                int hash = 17;

                if (!firstProperty.IsNull())
                    hash = hash * 23 + firstProperty.GetHashCode();

                return hash;
            }
        }

        public static int GetHashCode<TFirstProperty, TSecondProperty, TThirdProperty>(this object value,
                                                                      TFirstProperty firstProperty,
                                                                      TSecondProperty secondProperty,
                                                                      TThirdProperty thirdProperty)
        {
            unchecked
            {
                int hash = 17;

                if (!firstProperty.IsNull())
                    hash = hash * 23 + firstProperty.GetHashCode();
                if (!secondProperty.IsNull())
                    hash = hash * 23 + secondProperty.GetHashCode();
                if (!thirdProperty.IsNull())
                    hash = hash * 23 + thirdProperty.GetHashCode();

                return hash;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6>(this object value,
                                                                      T1 firstProperty,
                                                                      T2 secondProperty,
                                                                      T3 thirdProperty,
                                                                      T4 P4,
                                                                      T5 P5,
                                                                      T6 P6)
        {
            unchecked
            {
                int hash = 17;

                if (!firstProperty.IsNull())
                    hash = hash * 23 + firstProperty.GetHashCode();
                if (!secondProperty.IsNull())
                    hash = hash * 23 + secondProperty.GetHashCode();
                if (!thirdProperty.IsNull())
                    hash = hash * 23 + thirdProperty.GetHashCode();
                if (!P4.IsNull())
                    hash = hash * 23 + P4.GetHashCode();
                if (!P5.IsNull())
                    hash = hash * 23 + P5.GetHashCode();
                if (!P6.IsNull())
                    hash = hash * 23 + P6.GetHashCode();
                return hash;
            }
        }

        public static bool IsNull(this object @this)
        {
            return @this == null;
        }
    }
}
