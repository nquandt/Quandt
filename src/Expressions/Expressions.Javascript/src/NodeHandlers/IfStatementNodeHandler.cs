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
                testExpr = ResolveFalsyTest(param);
            }
            else
            {
                testExpr = test;
            }

            var walked = Walk(iff.Consequent);

            var block =  walked as BlockExpression;
            if (block == null)
            {
                block = Expression.Block(walked);
            }

            return Expression.IfThen(testExpr, block);
        }

        private Expression ResolveFalsyTest(ParameterExpression param)
        {
            if (param.Type == typeof(string))
            {
                var nullOrWhiteSpace = typeof(string).GetMethod("IsNullOrWhiteSpace");
                var callit = Expression.Call(param, nullOrWhiteSpace);
                return Expression.Negate(callit);
            }else if (param.Type == typeof(object))
            {
                return Expression.NotEqual(param, Expression.Constant(null, typeof(object)));
            }


            throw new NotSupportedException($"Haven't implemented that falsy test: {param.Type}");
        }
    }
}
