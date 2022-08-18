using Quandt.Expressions.Javascript.Services;
using Expression = System.Linq.Expressions.Expression;

namespace Quandt.Expressions.Javascript.NodeHandlers
{
    [NodesEnum(Nodes.Identifier)]
    internal class IdentifierNodeHandler : INodeHandler<Esprima.Ast.Identifier>
    {
        public Expression Execute(Node node)
        {
            var ident = node as Identifier;

            var vari = VariableContextService.GetCurrent()[ident.Name];

            return vari;
        }
    }
}
