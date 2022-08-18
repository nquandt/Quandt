using static Quandt.Expressions.Javascript.Services.TreeWalker;
using Expression = System.Linq.Expressions.Expression;

namespace Quandt.Expressions.Javascript.NodeHandlers
{
    [NodesEnum(Nodes.ArrayExpression)]
    internal class ArrayExpressionNodeHandler : INodeHandler<ArrayExpression>
    {
        public Expression Execute(Node node)
        {
            var array = node as ArrayExpression;
            if (array == null) return Expression.Empty();

            var newListXxp = Expression.New(typeof(List<string>));

            if (array.Elements.Count > 0)
            {
                System.Reflection.MethodInfo addMethod = typeof(List<string>).GetMethod("Add");

                var elements = Walk(array.Elements).Select(x => Expression.ElementInit(addMethod, x));

                return Expression.ListInit(newListXxp, elements); //Expression.MakeBinary(System.Linq.Expressions.ExpressionType., left, right) };
            }

            //TODO use NewList not NewArray cuz we cant implement push pop on an array....
            return newListXxp;
        }
    }
}
