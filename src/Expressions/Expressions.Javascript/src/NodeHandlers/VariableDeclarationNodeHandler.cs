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

            var inits = new List<System.Linq.Expressions.ParameterExpression>(); 
            var variables = new List<Expression>();
            foreach (var declaration in dec.Declarations)
            {
                if (declaration.Init == null)
                {
                    Type type = typeof(object);
                    Type intendedType = (Type)node.GetAdditionalData("IntendedType");
                    if (intendedType != null)
                    {
                        type = intendedType;
                    }
                     
                    var vari = VariableContextService.GetCurrent().AddVariable(type, ((Identifier)declaration.Id).Name, node);

                    inits.Add(vari);
                    var assign = Expression.Assign(vari, Expression.New(vari.Type));
                    variables.Add(assign);
                }
                else
                {
                    var walked = Walk(declaration.Init);

                    var vari = VariableContextService.GetCurrent().AddVariable(walked.Type, ((Identifier)declaration.Id).Name, node);
                    inits.Add(vari);
                    var assign = Expression.Assign(vari, walked);
                    variables.Add(assign);
                }
            }            

            return Expression.Block(inits, variables);
        }
    }
}
