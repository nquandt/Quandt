using Quandt.Expressions.Javascript.Services;
using System.Linq.Expressions;
using Expression = System.Linq.Expressions.Expression;

namespace Quandt.Expressions.Javascript.NodeHandlers
{
    [NodesEnum(Nodes.ForStatement)]
    internal class ForStatementNodeHandler : INodeHandler<ForStatement>
    {
        public Expression Execute(Node node)
        {
            var forOf = node as ForStatement;
            if (forOf == null) return Expression.Empty();

            return VariableContextService.Enter(() =>
            {
                var walkedInit = Walk(forOf.Init);

                var vars = (walkedInit as BlockExpression).Variables;

                var inits = (walkedInit as BlockExpression).Expressions.ToList(); //WTF do I do if this has more than one variable....



                var test = Walk(forOf.Test);

                var update = Walk(forOf.Update);

                var bodyExprs = (Walk(forOf.Body) as BlockExpression);


                var breakLabel = Expression.Label("break");

                inits.Add(Expression.Loop(
                    Expression.Block(
                        vars,
                        Expression.IfThenElse(test, bodyExprs, Expression.Break(breakLabel)),
                        update
                    ),
                    breakLabel
                ));

                return Expression.Block(
                    vars,
                    inits                
                );

            });
        }


    }
}
