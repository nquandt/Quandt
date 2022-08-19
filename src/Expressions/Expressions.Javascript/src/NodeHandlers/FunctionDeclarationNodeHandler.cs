using Quandt.Expressions.Javascript.Services;
using System.Linq.Expressions;
using Expression = System.Linq.Expressions.Expression;

namespace Quandt.Expressions.Javascript.NodeHandlers
{
    [NodesEnum(Nodes.FunctionDeclaration)]
    internal class FunctionDeclarationNodeHandler : INodeHandler<Esprima.Ast.FunctionDeclaration>
    {
        // ReadableExpressions doesn't seem to know if a
        // lambda is just an if statement and outputs without braces...
        // For now I pulled ReadableExpressions locally to change the LambdaTranslation.cs but hope to find a better fix.

        public Expression Execute(Node node)
        {
            var func = node as FunctionDeclaration;
            if (func == null) { return Expression.Empty(); }

            var lambda = (LambdaExpression)VariableContextService.Enter(() =>
            {
                var name = func.Id!.Name;

                var paramExprs = func.Params.Cast<Identifier>().Select(x => VariableContextService.GetCurrent().AddParameter(typeof(object), x.Name, node)).ToArray();

                BlockExpression block = Walk(func.Body) as BlockExpression;

                var lambda1 = Expression.Lambda(block, name, paramExprs);

                return lambda1;
                //return Expression.Lambda(block, name, paramExprs);
            });

            VariableContextService.GetCurrent().AssignTypeTo(lambda.Name, lambda.Type);

            var lambFunc = VariableContextService.GetCurrent().GetVariable(lambda.Name);            

            var assign = Expression.Assign(lambFunc, lambda);

            return Expression.Block(new ParameterExpression[] { lambFunc }, assign);


        }
    }
}
