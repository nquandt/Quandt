using static Quandt.Expressions.Javascript.Services.TreeWalker;
using Expression = System.Linq.Expressions.Expression;

namespace Quandt.Expressions.Javascript.NodeHandlers
{
    [NodesEnum(Nodes.UpdateExpression)]
    internal class UpdateExpressionNodeHandler : INodeHandler<UpdateExpression>
    {
        public Expression Execute(Node node)
        {
            var update = node as UpdateExpression;
            if (update == null) return Expression.Empty();

            var vari = Walk(update.Argument);

            switch (update.Operator)
            {
                case UnaryOperator.Increment:
                    if (vari is System.Linq.Expressions.ParameterExpression param)
                    {
                        Services.VariableContextService.GetCurrent().AssignTypeTo(param.Name, typeof(int));
                    }
                    return Expression.PostIncrementAssign(vari);
                default:
                    throw new NotSupportedException($"This UnaryOperator hasn't been implemented: {update.Operator}");
            }
        }
    }
}
