using Quandt.Expressions.Javascript.Services;
using Expression = System.Linq.Expressions.Expression;

namespace Quandt.Expressions.Javascript.NodeHandlers
{
    [NodesEnum(Nodes.BlockStatement)]
    internal class BlockStatementNodeHandler : INodeHandler<Esprima.Ast.BlockStatement>
    {
        public Expression Execute(Node node)
        {
            var block = node as BlockStatement;
            if (block == null) { return Expression.Empty(); }


            return VariableContextService.Enter(() =>
            {
                var exprs = Walk(block.Body);

                var param = VariableContextService.GetCurrent().CurrentVariables;

                return Expression.Block(param, exprs);
            });
        }
    }
}
