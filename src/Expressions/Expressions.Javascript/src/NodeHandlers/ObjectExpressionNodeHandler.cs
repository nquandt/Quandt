using static Quandt.Expressions.Javascript.Services.TreeWalker;
using Expression = System.Linq.Expressions.Expression;

namespace Quandt.Expressions.Javascript.NodeHandlers
{
    [NodesEnum(Nodes.ObjectExpression)]
    internal class ObjectExpressionNodeHandler : INodeHandler<ObjectExpression>
    {
        public Expression Execute(Node node)
        {
            var obj = node as ObjectExpression;
            if (obj == null) return Expression.Empty();

            

            if (obj.Properties.Count > 0)
            {
                var props = Walk(obj.Properties).Cast<System.Linq.Expressions.BinaryExpression>();               

                var leftTypeExp = props.FirstOrDefault(x => typeof(System.Linq.Expressions.ParameterExpression).IsAssignableFrom(x.Left.GetType())
                    || typeof(System.Linq.Expressions.ConstantExpression).IsAssignableFrom(x.Left.GetType()));
                Type leftType;
                if (leftTypeExp != null)
                {
                    leftType = leftTypeExp.Type;
                }
                else
                {
                    leftType = typeof(object);
                }

                var generic = typeof(Dictionary<,>);
                var constructed = generic.MakeGenericType(leftType, typeof(object));


                var newDictExp = Expression.New(constructed);
                System.Reflection.MethodInfo addMethod = constructed.GetMethod("Add");
                
                var elements = props.Select(x => Expression.ElementInit(addMethod, Expression.Convert(x.Left, leftType), Expression.Convert(x.Right, typeof(object))));

                return Expression.ListInit(newDictExp, elements); //Expression.MakeBinary(System.Linq.Expressions.ExpressionType., left, right) };
            }

            //TODO use NewList not NewArray cuz we cant implement push pop on an array....
            return Expression.New(typeof(Dictionary<object, object>));
        }
    }
}
