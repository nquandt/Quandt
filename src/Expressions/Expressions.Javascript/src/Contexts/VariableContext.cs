using System.Linq.Expressions;

namespace Quandt.Expressions.Javascript.Contexts
{
    public class VariableContext
    {
        private readonly Dictionary<string, ParameterExpression> _variables;
        public VariableContext? ParentContext = null;

        public VariableContext(VariableContext? parentContext = null)
        {
            _variables = new Dictionary<string, ParameterExpression>();
            ParentContext = parentContext;
        }

        public IEnumerable<ParameterExpression> CurrentVariables
        {
            get
            {
                return _variables.Values;
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
                allVars = allVars.Concat(_variables.Values);

                return allVars;
            }
        }

        public void Add(ParameterExpression parameterExpression)
        {
            if (ParentContext != null && ParentContext[parameterExpression.Name] != null)
            {
                throw new InvalidOperationException("Cannot have a second variable of same name in this scope");
            }

            _variables.Add(parameterExpression.Name!, parameterExpression);
        }

        public ParameterExpression? this[string name]
        {
            get
            {
                if (!_variables.ContainsKey(name))
                {
                    if (ParentContext != null)
                    {
                        var expr = ParentContext[name];
                        if (expr != null)
                        {
                            return ParentContext[name];
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
}