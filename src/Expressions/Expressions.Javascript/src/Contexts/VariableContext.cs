using System.Linq.Expressions;

namespace Quandt.Expressions.Javascript.Contexts
{
    public class ExpressionWrap
    {
        public ParameterExpression Expression { get; set; }
        public Node Node { get; set; }
    }

    public class VariableContext
    {
        private readonly Dictionary<string, ExpressionWrap> _variables;
        public VariableContext? ParentContext = null;

        public VariableContext(VariableContext? parentContext = null)
        {
            _variables = new Dictionary<string, ExpressionWrap>();
            ParentContext = parentContext;
        }

        public IEnumerable<ParameterExpression> CurrentVariables
        {
            get
            {
                return _variables.Values.Select(x => x.Expression);
            }
        }

        public void AssignTypeTo(string name, Type type)
        {
            var wrap = GetVariableAsWrap(name);

            if (wrap != null)
            {
                //var currentType = (Type)wrap.Node.GetAdditionalData("IntendedType");

                wrap.Node.SetAdditionalData("IntendedType", type);
            }
        }

        public IEnumerable<ParameterExpression> AllVariables
        {
            get
            {
                IEnumerable<ParameterExpression> allVars = Enumerable.Empty<ParameterExpression>();

                if (ParentContext != null)
                {
                    allVars = allVars.Concat(ParentContext.AllVariables);
                }
                allVars = allVars.Concat(CurrentVariables);

                return allVars;
            }
        }

        private string CleanName(string name)
        {
            return name.Replace("$", "_dollar_");
        }

        public ParameterExpression AddParameter(Type type, string name, Node node)
        {
            if (ParentContext != null && ParentContext.GetVariableAsWrap(name) != null)
            {
                throw new InvalidOperationException("Cannot have a second variable of same name in this scope");
            }

            var vari = System.Linq.Expressions.Expression.Parameter(type, CleanName(name));

            _variables.Add(name, new ExpressionWrap
            {
                Expression = vari,
                Node = node
            });

            return vari;
        }

        public ParameterExpression AddVariable(Type type, string name, Node node)
        {
            if (ParentContext != null && ParentContext.GetVariableAsWrap(name) != null)
            {
                throw new InvalidOperationException("Cannot have a second variable of same name in this scope");
            }

            var vari = System.Linq.Expressions.Expression.Variable(type, CleanName(name));

            _variables.Add(name, new ExpressionWrap
            {
                Expression = vari,
                Node = node
            });

            return vari;
        }

        public ParameterExpression? GetVariable(string name)
        {            
            return GetVariableAsWrap(name)?.Expression;
        }

        public ExpressionWrap? GetVariableAsWrap(string name)
        {            
            if (!_variables.ContainsKey(name))
            {
                if (ParentContext != null)
                {
                    var expr = ParentContext.GetVariableAsWrap(name);
                    if (expr != null)
                    {
                        return expr;
                    }
                }
                else
                {
                    //throw new InvalidOperationException("That variable was never declared");
                }

                return null;
            }

            return _variables[name];

        }
    }
}