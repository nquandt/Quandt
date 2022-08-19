using System.Linq.Expressions;
using static Quandt.Expressions.Javascript.Services.TreeWalker;
using Expression = System.Linq.Expressions.Expression;

namespace Quandt.Expressions.Javascript.NodeHandlers
{
    [NodesEnum(Nodes.AssignmentExpression)]
    internal class AssignmentExpressionNodeHandler : INodeHandler<AssignmentExpression>
    {
        public Expression Execute(Node node)
        {
            var assign = node as AssignmentExpression;
            if (assign == null) return Expression.Empty();

            var left = Walk(assign.Left);
            var right = Walk(assign.Right);

            if (left.Type == typeof(string) || right.Type == typeof(string))
            {
                return ResolveForStringTypes(assign.Operator, left, right); 
            }

            switch (assign.Operator)
            {
                case AssignmentOperator.PlusAssign:
                    return Expression.AddAssign(left, right);
                case AssignmentOperator.Assign:
                    if (left is ParameterExpression param) {
                        Services.VariableContextService.GetCurrent().AssignTypeTo(param.Name, right.Type);
                    }
                    return Expression.Assign(left, Expression.Convert(right, left.Type));
                case AssignmentOperator.MinusAssign:
                case AssignmentOperator.TimesAssign:
                case AssignmentOperator.DivideAssign:
                case AssignmentOperator.ModuloAssign:
                case AssignmentOperator.BitwiseAndAssign:
                case AssignmentOperator.BitwiseOrAssign:
                case AssignmentOperator.BitwiseXorAssign:
                case AssignmentOperator.LeftShiftAssign:
                case AssignmentOperator.RightShiftAssign:
                case AssignmentOperator.UnsignedRightShiftAssign:
                case AssignmentOperator.ExponentiationAssign:
                case AssignmentOperator.NullishAssign:
                case AssignmentOperator.AndAssign:
                case AssignmentOperator.OrAssign:
                default:
                    throw new NotSupportedException($"That AssignmentOperator hasn't been implemented yet: {assign.Operator}");
            }
        }

        private Expression ResolveForStringTypes(AssignmentOperator op, Expression left, Expression right)
        {
            switch (op)
            {
                case AssignmentOperator.PlusAssign:                    
                    return ResolveForStringTypes(AssignmentOperator.Assign, left, HandleBinaryAdditionForStrings(left, right));
                case AssignmentOperator.Assign:
                    if (left is ParameterExpression param)
                    {
                        Services.VariableContextService.GetCurrent().AssignTypeTo(param.Name, right.Type);
                    }
                    return Expression.Assign(left, Expression.Convert(right, left.Type));
                case AssignmentOperator.MinusAssign:
                case AssignmentOperator.TimesAssign:
                case AssignmentOperator.DivideAssign:
                case AssignmentOperator.ModuloAssign:
                case AssignmentOperator.BitwiseAndAssign:
                case AssignmentOperator.BitwiseOrAssign:
                case AssignmentOperator.BitwiseXorAssign:
                case AssignmentOperator.LeftShiftAssign:
                case AssignmentOperator.RightShiftAssign:
                case AssignmentOperator.UnsignedRightShiftAssign:
                case AssignmentOperator.ExponentiationAssign:
                case AssignmentOperator.NullishAssign:
                case AssignmentOperator.AndAssign:
                case AssignmentOperator.OrAssign:
                default:
                    throw new NotSupportedException($"That AssignmentOperator for strings hasn't been implemented yet: {op}");
            }
        }

        private Expression HandleBinaryAdditionForStrings(Expression left, Expression right)
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
    }
}
