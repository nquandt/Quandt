using Quandt.Expressions.Javascript.Extensions;
using Expression = System.Linq.Expressions.Expression;

namespace Quandt.Expressions.Javascript.NodeHandlers
{
    [NodesEnum(Nodes.MemberExpression)]
    internal class MemberExpressionNodeHandler : INodeHandler<MemberExpression>
    {
        public Expression Execute(Node node)
        {
            var member = node as MemberExpression;
            if (member == null) return Expression.Empty();

            var propInstance = Walk(member.Object) as System.Linq.Expressions.ParameterExpression;

            var prop = Walk(member.Property);

            if (member.Computed) //Indicates an indexer.... wtf do I do
            {
                var propInfo = GetIndexerProperty(propInstance, prop); //I hope it has an indexer prop..

                if (propInfo == null && propInstance.Type == typeof(object))
                {
                    var args2 = new List<Expression>() { propInstance, Expression.Convert(prop, typeof(object)) };                    
                    return Expression.Call(null, typeof(ObjectExtensions).GetMethod(nameof(ObjectExtensions.Index) ), args2);
                }
                //throw new InvalidOperationException("idk why but this is null good luck");

                return propInfo;
            }

            if (prop is System.Linq.Expressions.ConstantExpression constant)
            {
                return GetPropertyForType(propInstance, constant.Value as string);
            }

            throw new InvalidOperationException("Something went wrong, I guess I haven't implemented that");
        }

        //public static bool IsIndexerPropertyMethod( System.Reflection.MethodInfo method)
        //{
        //    var declaringType = method.DeclaringType;
        //    if (declaringType is null) return false;
        //    var indexerProperty = GetIndexerProperty(method.DeclaringType);
        //    if (indexerProperty is null) return false;
        //    return method == indexerProperty.GetMethod || method == indexerProperty.SetMethod;
        //}

        private static Expression GetIndexerProperty(System.Linq.Expressions.ParameterExpression param, Expression prop)
        {
            var type = param.Type;
            var defaultPropertyAttribute = System.Reflection.CustomAttributeExtensions.GetCustomAttributes<System.Reflection.DefaultMemberAttribute>(type)
                .FirstOrDefault();
            if (defaultPropertyAttribute is null) return null;
            var propInfo = type.GetProperty(defaultPropertyAttribute.MemberName,
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);

            return Expression.Property(param, propInfo, Expression.Convert(prop, propInfo.PropertyType));
        }

        private Expression GetPropertyForType(Expression param, string methodName)
        {
            if (param.Type == typeof(string))
            {
                return GetPropertyForString(param, methodName);
            }
            else if (param.Type == typeof(List<string>))
            {
                return GetPropertyForList(param, methodName);
            }
            else if (param.Type == typeof(object))
            {
                //SHIM FOR OBJECT?

                return GetPropertyFromObjectShims(param, methodName);
            }

            return null;
        }

        private Expression GetPropertyFromObjectShims(Expression param, string methodName)
        {
            Expression expr;
            switch (methodName)
            {
                case "length":
                    expr = Expression.Call(null, typeof(ObjectExtensions).GetMethod(nameof(ObjectExtensions.Count)), param);
                    break;
                default:
                    throw new NotSupportedException();
            }
            return expr;
        }

        private Expression GetPropertyForString(Expression param, string methodName)
        {
            Expression expr;
            switch (methodName)
            {
                case "length":
                    expr = Expression.Property(param, "Length");
                    break;
                default:
                    throw new NotSupportedException();
            }


            //Services.VariableContextService.GetCurrent().AssignTypeTo(param.Name, typeof(string));
            return expr;
        }

        private Expression GetPropertyForList(Expression param, string methodName)
        {
            Expression expr;
            switch (methodName)
            {
                case "length":
                    expr = Expression.Property(param, "Count");
                    break;
                default:
                    throw new NotSupportedException();
            }

            //Services.VariableContextService.GetCurrent().AssignTypeTo(param.Name, typeof(List<string>));
            return expr;
        }
    }
}
