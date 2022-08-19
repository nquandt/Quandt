using Quandt.Expressions.Javascript.Services;
using Expression = System.Linq.Expressions.Expression;

namespace Quandt.Expressions.Javascript.NodeHandlers
{
    [NodesEnum(Nodes.ReturnStatement)]
    internal class ReturnStatementNodeHandler : INodeHandler<Esprima.Ast.ReturnStatement>
    {
        public Expression Execute(Node node)
        {
            var ret = node as ReturnStatement;
            if (ret == null) { return Expression.Empty(); }

            if (ret.Argument!.Type == Nodes.Identifier)
            {
                var vari = VariableContextService.GetCurrent().GetVariable(((Identifier)ret.Argument).Name);

                return  vari;
            }



            return Expression.Empty();
        }
    }
}
