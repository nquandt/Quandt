using Quandt.Expressions.Javascript.Services;
using Expression = System.Linq.Expressions.Expression;

namespace Quandt.Expressions.Javascript.NodeHandlers
{
    [NodesEnum(Nodes.Program)]
    internal class ProgramNodeHandler : INodeHandler<Esprima.Ast.Program>
    {
        public Expression Execute(Node node)
        {
            //TODO maybe do different things for FunctionDeclarations and top level statements..?

            
            var walked = Walk(node.ChildNodes);
            var variables = VariableContextService.GetCurrent().AllVariables;

            return Expression.Block(variables, walked);

        }
    }
}
