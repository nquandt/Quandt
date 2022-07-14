// Credit to Matt Weber (https://badecho.com/2012/02/expression-equality-comparer-part-i/) for the Expression Equality Tooling

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Quandt.Expressions
{
    /// <summary>
    /// Provides an <see cref="IEnumerable{Expression}"/> created by walking through an expression
    /// tree.
    /// </summary>
    public class ExpressionCollection : ExpressionVisitor, IEnumerable<Expression>
    {
        private readonly List<Expression> _expressions = new List<Expression>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionCollection"/> class.
        /// </summary>
        /// <param name="expression">
        /// The expression tree to walk when populating this collection.
        /// </param>
        public ExpressionCollection(Expression expression)
        {
            Visit(expression);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        public IEnumerator<Expression> GetEnumerator()
        {
            return _expressions.GetEnumerator();
        }

        /// <summary>
        /// Processes the provided <see cref="Expression"/> object by adding it to this collection
        /// and then walking further down the tree.
        /// </summary>
        /// <param name="node">
        /// The expression to process to add to this collection and walk through.
        /// </param>
        /// <returns>
        /// The modified expression, assuming the expression was modified; otherwise, returns the
        /// original expression.
        /// </returns>
        public override Expression Visit(Expression node)
        {
            if (null != node)
                _expressions.Add(node);

            return base.Visit(node);
        }
    }
}
