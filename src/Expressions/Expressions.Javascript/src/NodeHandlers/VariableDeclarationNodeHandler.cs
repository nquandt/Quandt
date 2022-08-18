using Quandt.Expressions.Javascript.Services;
using Expression = System.Linq.Expressions.Expression;

namespace Quandt.Expressions.Javascript.NodeHandlers
{
    [NodesEnum(Nodes.VariableDeclaration)]
    internal class VariableDeclarationNodeHandler : INodeHandler<VariableDeclaration>
    {

        //TODO fix variable declaration because usually it would return a list...?
        public Expression Execute(Node node)
        {
            var dec = node as VariableDeclaration;
            if (dec == null) { return Expression.Empty(); }

            var variables = new List<Expression>();
            foreach (var declaration in dec.Declarations)
            {
                if (declaration.Init == null)
                {
                    var vari = Expression.Variable(typeof(string), ((Identifier)declaration.Id).Name);
                    VariableContextService.GetCurrent().Add(vari);

                    variables.Add(vari);
                }
                else
                {
                    var walked = Walk(declaration.Init);

                    var vari = Expression.Variable(walked.Type, ((Identifier)declaration.Id).Name);
                    VariableContextService.GetCurrent().Add(vari);

                    var assign = Expression.Assign(vari, walked);
                    variables.Add(assign);
                }
            }
            return Expression.Block(variables);
        }
    }
}
