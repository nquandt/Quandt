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
                var ident = Walk(member.Object) as System.Linq.Expressions.ParameterExpression;

                var method = member.Property as Identifier;
                var methodName = method.Name;

                var methInfo = GetMethodInfo(ident.Type, methodName);

                if (cal.Arguments.Any())
                {
                    var args = Walk(cal.Arguments);

                    return Expression.Call(ident, methInfo, args);
                }


                return Expression.Call(ident, methInfo);
            }




            throw new Exception("That walked is not implemented");
            //TODO use NewList not NewArray cuz we cant implement push pop on an array....
            //return new Expression[] { Expression.Empty() };
        }


        private System.Reflection.MethodInfo GetMethodInfo(Type baseType, string methodName)
        {
            var cSharpMethodName = GetMethodNameForType(baseType, methodName);

            if (cSharpMethodName == null)
            {
                throw new Exception("That method hasn't been implemented");
            }

            return baseType.GetMethods().First(m => m.Name == cSharpMethodName && m.IsPublic);
        }

        private string GetMethodNameForType(Type baseType, string methodName)
        {
            if (baseType == typeof(string))
            {
                return GetMethodNameForString(methodName);
            }
            else if (baseType == typeof(List<string>))
            {
                return GetMethodNameForList(methodName);
            }

            return null;
        }

        private string GetMethodNameForString(string methodName)
        {
            switch (methodName)
            {
                case "trim":
                    return "Trim";
            }

            return null;
        }

        private string GetMethodNameForList(string methodName)
        {
            switch (methodName)
            {
                case "push":
                    return "Add";
            }

            return null;
        }
    }
}
