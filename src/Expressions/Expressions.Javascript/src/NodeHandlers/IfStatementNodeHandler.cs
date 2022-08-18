using System.Linq.Expressions;

using Expression = System.Linq.Expressions.Expression;

namespace Quandt.Expressions.Javascript.NodeHandlers
{
    [NodesEnum(Nodes.IfStatement)]
    internal class IfStatementNodeHandler : INodeHandler<Esprima.Ast.IfStatement>
    {
        public Expression Execute(Node node)
        {
            var iff = node as IfStatement;
            if (iff == null) { return Expression.Empty(); }

            var test = Walk(iff.Test);
            Expression testExpr;
            if (test is ParameterExpression param)
            {
                testExpr = Expression.NotEqual(param, Expression.Constant(null, typeof(object)));
            }
            else
            {
                testExpr = test;
            }

            var block = Walk(iff.Consequent) as BlockExpression;


            return Expression.IfThen(testExpr, block);
        }
    }
}
