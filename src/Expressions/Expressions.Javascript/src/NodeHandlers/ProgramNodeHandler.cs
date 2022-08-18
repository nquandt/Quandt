using Quandt.Expressions.Javascript.Services;
using Expression = System.Linq.Expressions.Expression;

namespace Quandt.Expressions.Javascript.NodeHandlers
{
    [NodesEnum(Nodes.Program)]
    internal class ProgramNodeHandler : INodeHandler<Esprima.Ast.Program>
    {
        public Expression Execute(Node node)
        {
            //TODO maybe do different things for FunctionDeclarations and top level statements..?

            //OR do I just say fuck it and drop it all into a static void Main..?

            using (VariableContextService.Enter()) //TOP LEVEL VARS??..
            {
                var walked = Walk(node.ChildNodes);
                var childs = new List<Expression>();
                foreach(var child in walked)
                {
                    if (child is System.Linq.Expressions.LambdaExpression lambda)
                    {
                        var lambFunc = Expression.Variable(lambda.Type, lambda.Name);
                        VariableContextService.GetCurrent().Add(lambFunc);

                        var assign = Expression.Assign(lambFunc, lambda);
                        childs.Add(assign);
                        continue;
                    }

                    childs.Add(child);
                }

                return Expression.Block(VariableContextService.GetCurrent().AllVariables, childs);
            }
        }
    }
}
