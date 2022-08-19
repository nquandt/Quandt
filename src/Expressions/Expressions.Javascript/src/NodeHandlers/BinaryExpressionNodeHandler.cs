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

            if (left.Type == typeof(string) || right.Type == typeof(string))
            {
                return HandleBinaryForStrings(left, right);
            }

            if (left.Type == typeof(object) && right.Type != typeof(object))
            {
                left = Expression.Convert(left, right.Type);
            }

            if (left.Type != typeof(object) && right.Type == typeof(object))
            {
                right = Expression.Convert(right, left.Type);
            }

            return Expression.MakeBinary(expType, left, right);
        }

        private Expression HandleBinaryForStrings(Expression left, Expression right)
        {
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

            return concat;
        }


        private static System.Linq.Expressions.ExpressionType GetExpressionTypeFromOperator(BinaryOperator @operator)
        {
            switch (@operator)
            {
                case BinaryOperator.Plus:
                    return System.Linq.Expressions.ExpressionType.Add;
                case BinaryOperator.Minus:
                    return System.Linq.Expressions.ExpressionType.Subtract;
                case BinaryOperator.Less:
                    return System.Linq.Expressions.ExpressionType.LessThan;
                case BinaryOperator.Greater:
                    return System.Linq.Expressions.ExpressionType.GreaterThan;
                case BinaryOperator.LessOrEqual:
                    return System.Linq.Expressions.ExpressionType.LessThanOrEqual;
                case BinaryOperator.GreaterOrEqual:
                    return System.Linq.Expressions.ExpressionType.GreaterThanOrEqual;
                case BinaryOperator.Equal:
                    return System.Linq.Expressions.ExpressionType.Equal;
                case BinaryOperator.NotEqual:
                    return System.Linq.Expressions.ExpressionType.NotEqual;
                case BinaryOperator.StrictlyEqual:
                    return System.Linq.Expressions.ExpressionType.Equal;
                case BinaryOperator.Times:                    
                case BinaryOperator.Divide:                    
                case BinaryOperator.Modulo:                    
                case BinaryOperator.StrictlyNotEqual:                    
                case BinaryOperator.BitwiseAnd:                    
                case BinaryOperator.BitwiseOr:                    
                case BinaryOperator.BitwiseXor:
                case BinaryOperator.LeftShift:
                case BinaryOperator.RightShift:
                case BinaryOperator.UnsignedRightShift:
                case BinaryOperator.InstanceOf:
                case BinaryOperator.In:
                case BinaryOperator.LogicalAnd:
                case BinaryOperator.LogicalOr:
                case BinaryOperator.Exponentiation:
                case BinaryOperator.NullishCoalescing:
                default:
                    throw new NotImplementedException("That BinaryOperator hasn't been implemented " + @operator.ToString());
            }
        }
    }
}
