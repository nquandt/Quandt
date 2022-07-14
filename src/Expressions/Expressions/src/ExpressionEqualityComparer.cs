//// Credit to Matt Weber (https://badecho.com/2012/02/expression-equality-comparer-part-i/) for the Expression Equality Tooling

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Quandt.Expressions
{
    /// <summary>
    /// Provides methods to compare <see cref="Expression"/> objects for equality.
    /// </summary>
    public sealed class ExpressionEqualityComparer : IEqualityComparer<Expression>
    {
        private static readonly ExpressionEqualityComparer _Instance = new ExpressionEqualityComparer();

        /// <summary>
        /// Gets the default <see cref="ExpressionEqualityComparer"/> instance.
        /// </summary>
        public static ExpressionEqualityComparer Instance
        {
            get { return _Instance; }
        }

        /// <inheritdoc/>
        public bool Equals(Expression x, Expression y)
        {
            return new ExpressionComparison(x, y).ExpressionsAreEqual;
        }

        /// <inheritdoc/>
        public int GetHashCode(Expression obj)
        {
            return new ExpressionHashCodeCalculator(obj).Output;
        }
    }
}
