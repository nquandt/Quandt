using Expression = System.Linq.Expressions.Expression;

namespace Quandt.Expressions.Javascript.NodeHandlers
{
    [NodesEnum(Nodes.BinaryExpression)]
    internal class BinaryExpressionNodeHandler : INodeHandler<BinaryExpression>
    {
        public Expression Execute(Node node)
        {
            var bin = node as BinaryExpression;

            var expType = GetExpressionTypeFromOperator(bin.Operator);

            var left = Walk(bin.Left);
            var right = Walk(bin.Right);

            var concatMethod = typeof(string)
                .GetMethod(nameof(string.Concat), new[] { typeof(string), typeof(string) });

            if (left is System.Linq.Expressions.ConstantExpression param)
            {
                if (param.Type != typeof(string))
                {                    
                    left = Expression.Call(left, "ToString", null);
                }
            }

            if (right is System.Linq.Expressions.ConstantExpression param2)
            {
                if (param2.Type != typeof(string))
                {
                    right = Expression.Call(right, "ToString", null);
                }
            }

            var concat = Expression.Call(
                concatMethod,
                left,
                right);

            return concat; //Expression.MakeBinary(System.Linq.Expressions.ExpressionType., left, right) };
        }

        private static System.Linq.Expressions.ExpressionType GetExpressionTypeFromOperator(BinaryOperator @operator)
        {
            switch (@operator)
            {
                case BinaryOperator.Plus:
                    return System.Linq.Expressions.ExpressionType.Add;
                case BinaryOperator.Minus:
                    return System.Linq.Expressions.ExpressionType.Subtract;
                default:
                    throw new NotImplementedException("That BinaryOperator hasn't been implemented " + @operator.ToString());
            }
        }
    }
}
