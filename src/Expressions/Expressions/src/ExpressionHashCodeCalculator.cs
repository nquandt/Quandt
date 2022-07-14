//// Credit to Matt Weber (https://badecho.com/2012/02/expression-equality-comparer-part-i/) for the Expression Equality Tooling

using Quandt.Expressions.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Quandt.Expressions
{
    /// <summary>
    /// Provides a visitor that calculates a hash code for an entire expression tree.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class ExpressionHashCodeCalculator : ExpressionVisitor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionHashCodeCalculator"/> class.
        /// </summary>
        /// <param name="expression">The expression tree to walk when calculating the has code.</param>
        public ExpressionHashCodeCalculator(Expression expression)
        {
            Visit(expression);
        }

        /// <summary>
        /// Gets the calculated hash code for the expression tree.
        /// </summary>
        public int Output
        { get; private set; }

        /// <summary>
        /// Calculates the hash code for the common <see cref="Expression"/> properties offered by the provided
        /// node before dispatching it to more specialized visit methods for further calculations.
        /// </summary>
        /// <inheritdoc/>
        public override Expression Visit(Expression node)
        {
            if (null == node)
                return null;

            Output += node.GetHashCode(node.NodeType, node.Type);

            return base.Visit(node);
        }

        /// <inheritdoc/>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            Output += node.GetHashCode(node.Method, node.IsLifted, node.IsLiftedToNull);

            return base.VisitBinary(node);
        }

        /// <inheritdoc/>
        protected override CatchBlock VisitCatchBlock(CatchBlock node)
        {
            Output += node.GetHashCode(node.Test);

            return base.VisitCatchBlock(node);
        }

        /// <inheritdoc/>
        protected override Expression VisitConstant(ConstantExpression node)
        {
            var collection = node.Value as IEnumerable;
            if (collection != null)
            {
                foreach (var item in collection)
                    Output += item.GetHashCode();

                return base.VisitConstant(node);
            }

            Output += node.GetHashCode(node.Value);

            return base.VisitConstant(node);
        }

        /// <inheritdoc/>
        protected override Expression VisitDebugInfo(DebugInfoExpression node)
        {
            Output += node.GetHashCode(node.Document,
                                            node.EndColumn,
                                            node.EndLine,
                                            node.IsClear,
                                            node.StartColumn,
                                            node.StartLine);

            return base.VisitDebugInfo(node);
        }

        /// <inheritdoc/>
        protected override Expression VisitDynamic(DynamicExpression node)
        {
            Output += node.GetHashCode(node.Binder, node.DelegateType);

            return base.VisitDynamic(node);
        }

        /// <inheritdoc/>
        protected override ElementInit VisitElementInit(ElementInit node)
        {
            Output += node.GetHashCode(node.AddMethod);

            return base.VisitElementInit(node);
        }

        /// <inheritdoc/>
        protected override Expression VisitGoto(GotoExpression node)
        {
            Output += node.GetHashCode(node.Kind);

            return base.VisitGoto(node);
        }

        /// <inheritdoc/>
        protected override Expression VisitIndex(IndexExpression node)
        {
            Output += node.GetHashCode(node.Indexer);

            return base.VisitIndex(node);
        }

        /// <inheritdoc/>
        protected override LabelTarget VisitLabelTarget(LabelTarget node)
        {
            Output += node.GetHashCode(node.Name, node.Type);

            return base.VisitLabelTarget(node);
        }

        /// <inheritdoc/>
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            Output += node.GetHashCode(node.Name, node.ReturnType, node.TailCall);

            return base.VisitLambda(node);
        }

        /// <inheritdoc/>
        protected override Expression VisitMember(MemberExpression node)
        {
            Output += node.GetHashCode(node.Member);

            return base.VisitMember(node);
        }

        /// <inheritdoc/>
        protected override MemberBinding VisitMemberBinding(MemberBinding node)
        {
            Output += node.GetHashCode(node.BindingType, node.Member);

            return base.VisitMemberBinding(node);
        }

        /// <inheritdoc/>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            Output += node.GetHashCode(node.Method);

            return base.VisitMethodCall(node);
        }

        /// <inheritdoc/>
        protected override Expression VisitNew(NewExpression node)
        {
            Output += node.GetHashCode(node.Constructor);

            return base.VisitNew(node);
        }

        /// <inheritdoc/>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            Output += node.GetHashCode(node.IsByRef);

            return base.VisitParameter(node);
        }

        /// <inheritdoc/>
        protected override Expression VisitSwitch(SwitchExpression node)
        {
            Output += node.GetHashCode(node.Comparison);

            return base.VisitSwitch(node);
        }

        /// <inheritdoc/>
        protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            Output += node.GetHashCode(node.TypeOperand);

            return base.VisitTypeBinary(node);
        }

        /// <inheritdoc/>
        protected override Expression VisitUnary(UnaryExpression node)
        {
            Output += node.GetHashCode(node.IsLifted, node.IsLiftedToNull, node.Method);

            return base.VisitUnary(node);
        }
    }
}
