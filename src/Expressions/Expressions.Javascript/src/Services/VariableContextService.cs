using Quandt.Expressions.Javascript.Contexts;
using Quandt.Expressions.Javascript.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quandt.Expressions.Javascript.Services
{
    public class VariableContextService
    {
        private static readonly List<VariableContext> _stack = new List<VariableContext>();

        public static VariableContext GetCurrent() => Peek();

        public static System.Linq.Expressions.Expression Enter(Func<System.Linq.Expressions.Expression> func)
        {
            var doAgain = false;
            System.Linq.Expressions.Expression exp;
            using (Push(GetStack()))
            {
                exp = func();

                var currVars = VariableContextService.GetCurrent().CurrentVariables.ToArray();

                if (currVars.Any(x => x.Type == typeof(object) || x.Type == typeof(List<object>)))
                {
                    doAgain = true;
                }
            }
            if (doAgain)
            {
                using (Push(GetStack()))
                {
                    exp = func();
                 
                }
            }
            return exp;
        }

        protected static List<VariableContext> GetStack()
        {
            return _stack;
        }

        protected static VariableContext Peek()
        {
            List<VariableContext> stack = GetStack();
            return stack.Count != 0 ? stack[stack.Count - 1] : throw new InvalidOperationException($"Attempt to retrieve context object of type '{typeof(VariableContext).FullName}' from empty stack.");
        }

        protected static void Pop(List<VariableContext> stack)
        {

            if (stack.Count == 0)
                throw new InvalidOperationException($"Attempt to pop context object of type '{typeof(VariableContext).FullName}' from empty stack.");
            stack.RemoveAt(stack.Count - 1);
        }

        protected static IDisposable Push(List<VariableContext> stack)
        {
            VariableContext context;
            if (stack.Count > 0)
            {
                context = new VariableContext(stack.Last());
            }
            else
            {
                context = new VariableContext();
            }

            stack.Add(context);
            return (IDisposable)new GenericDisposable((Action)(() => Pop(stack)));
        }
    }
}
