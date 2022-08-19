using static Quandt.Expressions.Javascript.Services.TreeWalker;
using Expression = System.Linq.Expressions.Expression;

namespace Quandt.Expressions.Javascript.NodeHandlers
{
    [NodesEnum(Nodes.Property)]
    internal class PropertyNodeHandler : INodeHandler<Property>
    {
        public Expression Execute(Node node) //TODO this better
        {
            var prop = node as Property;
            if (prop == null) return Expression.Empty();

            var key = Expression.Constant((prop.Key as Identifier).Name);
            var value = Walk(prop.Value);

            return Expression.Assign(key, value);
        }
    }
}
