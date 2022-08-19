using Quandt.Expressions.Javascript.Services;
using System.Linq.Expressions;
using Expression = System.Linq.Expressions.Expression;

namespace Quandt.Expressions.Javascript.NodeHandlers
{
    [NodesEnum(Nodes.ForOfStatement)]
    internal class ForOfStatementNodeHandler : INodeHandler<ForOfStatement>
    {
        public Expression Execute(Node node)
        {
            var forOf = node as ForOfStatement;
            if (forOf == null) return Expression.Empty();

            return VariableContextService.Enter(() =>
            {
                var left = (Walk(forOf.Left) as BlockExpression).Expressions.Single() as ParameterExpression;

                var right = Walk(forOf.Right);

                return ForEach(right, left, Walk(forOf.Body));
            });
        }

        private static Expression ForEach(Expression collection, System.Linq.Expressions.ParameterExpression loopVar, Expression loopContent)
        {
            var elementType = loopVar.Type;
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(elementType);
            var enumeratorType = typeof(IEnumerator<>).MakeGenericType(elementType);

            var enumeratorVar = Expression.Variable(enumeratorType, "enumerator");
            var getEnumeratorCall = Expression.Call(collection, enumerableType.GetMethod("GetEnumerator"));
            var enumeratorAssign = Expression.Assign(enumeratorVar, getEnumeratorCall);

            // The MoveNext method's actually on IEnumerator, not IEnumerator<T>
            var moveNextCall = Expression.Call(enumeratorVar, typeof(System.Collections.IEnumerator).GetMethod("MoveNext"));

            var breakLabel = Expression.Label("LoopBreak");

            var loop = Expression.Block(new[] { enumeratorVar },
                enumeratorAssign,
                Expression.Loop(
                    Expression.IfThenElse(
                        Expression.Equal(moveNextCall, Expression.Constant(true)),
                        Expression.Block(new[] { loopVar },
                            Expression.Assign(loopVar, Expression.Property(enumeratorVar, "Current")),
                            loopContent
                        ),
                        Expression.Break(breakLabel)
                    ),
                breakLabel)
            );

            return loop;
        }
    }
}
