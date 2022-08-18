using Quandt.Expressions.Javascript.Services;
using System.Linq.Expressions;
using System.Text;
using Expression = System.Linq.Expressions.Expression;

namespace Quandt.Expressions.Javascript.NodeHandlers
{
    [NodesEnum(Nodes.TemplateLiteral)]
    internal class TemplateLiteralNodeHandler : INodeHandler<Esprima.Ast.TemplateLiteral>
    {
        public Expression Execute(Node node)
        {
            var template = node as TemplateLiteral;
            if (template == null) { return Expression.Empty(); }

            var literals = Walk(template.Expressions).Cast<ParameterExpression>();
            var raw = template.ToString();

            foreach (var literal in literals)
            {
                raw = raw.Replace($"${{{literal.Name}}}", $"{{{literal.Name}}}");
            }

            var exp = CreateTemplate(raw);

            return exp;
        }

        public static string GetToken(string templateString, ref int i, out bool isToken)
        {
            int j = i;

            if (templateString[j] == '{')
            {
                isToken = true;

                j++;

                int k = templateString.IndexOf('}', j);

                if (k == -1)
                {
                    throw new Exception();
                }

                i = k + 1;

                return templateString.Substring(j, k - j);
            }
            else
            {
                isToken = false;
                i++;
                return templateString[j].ToString();
            }
        }

        public static Expression CreateTemplate(string templateString)
        {
            var formatObjs = new List<Expression>();
            var formatString = new StringBuilder();
            int parameterNumber = 0;

            for (int i = 0; i < templateString.Length;)
            {
                bool isToken;
                var token = GetToken(templateString, ref i, out isToken);

                if (isToken)
                {
                    var param = VariableContextService.GetCurrent()[token];
                    if (param == null) continue;

                    formatObjs.Add(param);

                    formatString.Append('{');
                    formatString.Append(parameterNumber);
                    formatString.Append('}');
                    parameterNumber++;
                }
                else
                {
                    formatString.Append(token);
                }
            }

            var formatMethod = typeof(string).GetMethod("Format", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public, null, new[] { typeof(string), typeof(object[]) }, null);
            var formatConstantExpression = Expression.Constant(formatString.ToString());
            var formatObjsExpression = Expression.NewArrayInit(typeof(object), formatObjs);
            return Expression.Call(formatMethod, formatConstantExpression, formatObjsExpression);
        }
    }
}
