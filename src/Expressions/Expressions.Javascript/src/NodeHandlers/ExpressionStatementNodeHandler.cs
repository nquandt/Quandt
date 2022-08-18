using Expression = System.Linq.Expressions.Expression;


namespace Quandt.Expressions.Javascript.NodeHandlers
{
    [NodesEnum(Nodes.ExpressionStatement)]
    internal class ExpressionStatementNodeHandler : INodeHandler<Esprima.Ast.ExpressionStatement>
    {
        public Expression Execute(Node node)
        {
            var expS = node as ExpressionStatement;
            if (expS == null) { return Expression.Empty(); }

            var walked = Walk(expS.Expression);

            return walked;
        }
    }
}
