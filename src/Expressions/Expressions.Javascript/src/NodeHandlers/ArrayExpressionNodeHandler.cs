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
            
            if (array.Elements.Count > 0)
            {
                
                var walkedElements = Walk(array.Elements);

                var listTypeExp = walkedElements.FirstOrDefault(x => typeof(System.Linq.Expressions.ParameterExpression).IsAssignableFrom(x.GetType())
                    || typeof(System.Linq.Expressions.ConstantExpression).IsAssignableFrom(x.GetType()));
                Type listType;
                if (listTypeExp != null)
                {
                    listType = listTypeExp.Type;                    
                }
                else
                {
                    listType = typeof(object);
                }
                var generic = typeof(List<>);
                var constructed = generic.MakeGenericType(listType);

                var newListXxp = Expression.New(constructed);

                System.Reflection.MethodInfo addMethod = constructed.GetMethod("Add");

                var elements = walkedElements.Select(x => Expression.ElementInit(addMethod, Expression.Convert(x, listType)));

                return Expression.ListInit(newListXxp, elements);
            }
            
            return Expression.New(typeof(List<object>));
        }
    }
}
