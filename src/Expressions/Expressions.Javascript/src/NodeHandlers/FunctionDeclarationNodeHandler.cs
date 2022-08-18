using Quandt.Expressions.Javascript.Services;
using System.Linq.Expressions;
using Expression = System.Linq.Expressions.Expression;

namespace Quandt.Expressions.Javascript.NodeHandlers
{
    [NodesEnum(Nodes.FunctionDeclaration)]
    internal class FunctionDeclarationNodeHandler : INodeHandler<Esprima.Ast.FunctionDeclaration>
    {
        public Expression Execute(Node node)
        {
            var func = node as FunctionDeclaration;
            if (func == null) { return Expression.Empty(); }

            using (VariableContextService.Enter())
            {
                var name = func.Id!.Name;

                var paramExprs = func.Params.Cast<Identifier>().Select(x => Expression.Parameter(typeof(string), x.Name));
                foreach (var param in paramExprs)
                {
                    VariableContextService.GetCurrent().Add(param);
                }

                BlockExpression block = Walk(func.Body) as BlockExpression;


                var lambda = Expression.Lambda(block, name, paramExprs.Select(x => VariableContextService.GetCurrent()[x.Name]).ToArray());

                var lambFunc = Expression.Variable(lambda.Type, lambda.Name);
                VariableContextService.GetCurrent().Add(lambFunc);

                var assign = Expression.Assign(lambFunc, lambda);               

                return assign;
            }
        }
    }
}
