using Quandt.Expressions.Javascript.Extensions;
using Expression = System.Linq.Expressions.Expression;

namespace Quandt.Expressions.Javascript.NodeHandlers
{
    [NodesEnum(Nodes.CallExpression)]
    internal class CallExpressionNodeHandler : INodeHandler<CallExpression>
    {
        public Expression Execute(Node node)
        {
            var cal = node as CallExpression;
            if (cal == null) return Expression.Empty();

            //var walked = Walk(cal.Callee);
            
            if (cal.Callee is Esprima.Ast.MemberExpression member)
            {
                var identt = Walk(member.Object) as System.Linq.Expressions.ParameterExpression;

                var method = member.Property as Identifier;
                var methodName = method.Name;

                var args = cal.Arguments.Any() ? Walk(cal.Arguments) : Enumerable.Empty<Expression>();

               return GetMethodCall(identt, methodName, args);
            }
            else if (cal.Callee is Esprima.Ast.Identifier ident)
            {
                Services.VariableContextService.GetCurrent().GetVariable(ident.Name);
            }



            throw new Exception("That Callee is not implemented yes");
            //TODO use NewList not NewArray cuz we cant implement push pop on an array....
            //return new Expression[] { Expression.Empty() };
        }


        private Expression GetMethodCall(System.Linq.Expressions.ParameterExpression param, string methodName, IEnumerable<Expression> args)
        {
            var cSharpMethod = GetMethodNameForType(param, methodName, args);

            if (cSharpMethod == null)
            {
                throw new Exception("That method hasn't been implemented");
            }

            return cSharpMethod;
        }

        private Expression GetMethodNameForType(System.Linq.Expressions.ParameterExpression param, string methodName, IEnumerable<Expression> args)
        {
            if (param.Type == typeof(string))
            {
                return GetMethodForString(param, methodName, args);
            }
            else if (param.Type.IsGenericType && param.Type.GetGenericTypeDefinition() == typeof(List<>))
            {
                return GetMethodNameForList(param, methodName, args, param.Type);
            }else if (param.Type == typeof(object))
            {
                return GetMethodFromObjectShims(param, methodName, args);                
            }

            return null;
        }

        private Expression GetMethodFromObjectShims(Expression param, string methodName, IEnumerable<Expression> args)
        {
            Expression expr;
            switch (methodName)
            {
                case "push":
                    var args2 = new List<Expression>() { param };
                    args2.AddRange(args.Select(x => Expression.Convert(x, typeof(object))));
                    if (args2.Count < 4)
                    {
                        args2.Add(Expression.Constant(null, typeof(object)));
                    }
                    expr = Expression.Call(null, typeof(ObjectExtensions).GetMethod(nameof(ObjectExtensions.Add)), args2);
                    break;
                case "charCodeAt":
                    var args3 = new List<Expression>() { param };
                    args3.AddRange(args);
                    expr = Expression.Call(null, typeof(ObjectExtensions).GetMethod(nameof(ObjectExtensions.CharCodeAt)), args3);
                    break;
                default:
                    throw new NotSupportedException();
            }
            return expr;
        }


        private Expression GetMethodForString(System.Linq.Expressions.Expression param, string methodName, IEnumerable<Expression> args)
        {         
            Expression expr;
            switch (methodName)
            {
                case "trim":
                    expr = GetFirstMatchingMethod<string>(param, "Trim", args);
                    break;
                case "charCodeAt":
                    var args2 = new List<Expression>() { param };
                    args2.AddRange(args);
                    expr = Expression.Call(null, typeof(StringExtensions).GetMethod("CharCodeAt"), args2);
                    break;
                default:
                    throw new NotSupportedException();
            }

            if (param is System.Linq.Expressions.ParameterExpression param1)
            {
                Services.VariableContextService.GetCurrent().AssignTypeTo(param1.Name, typeof(string));
            }
            return expr;
        }

        private Expression GetMethodNameForList(System.Linq.Expressions.Expression param, string methodName, IEnumerable<Expression> args, Type listType)
        {            
            Expression expr;
            switch (methodName)
            {
                case "push":
                    expr = GetFirstMatchingMethod(param, "Add", args, listType);
                    break;
                default:
                    throw new NotSupportedException();
            }

            if (param is System.Linq.Expressions.ParameterExpression param1)
            {
                Services.VariableContextService.GetCurrent().AssignTypeTo(param1.Name, typeof(List<string>));
            }
            return expr;
        }

        private Expression GetFirstMatchingMethod<T>(System.Linq.Expressions.Expression param, string methodName, IEnumerable<Expression> args)
        {
            return GetFirstMatchingMethod(param,methodName, args, typeof(T));
        }

        private Expression GetFirstMatchingMethod(System.Linq.Expressions.Expression param, string methodName, IEnumerable<Expression> args, Type type)
        {
            if (args.Any())
            {
                return Expression.Call(param, type.GetMethods().First(m => m.Name == methodName && m.IsPublic), args);
            }

            return Expression.Call(param, type.GetMethods().First(m => m.Name == methodName && m.IsPublic));
        }
    }
}
