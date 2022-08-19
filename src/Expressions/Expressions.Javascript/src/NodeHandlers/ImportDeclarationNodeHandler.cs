using Quandt.Expressions.Javascript.Services;
using System.Linq.Expressions;
using Expression = System.Linq.Expressions.Expression;

namespace Quandt.Expressions.Javascript.NodeHandlers
{
    [NodesEnum(Nodes.ImportDeclaration)]
    internal class ImportDeclarationNodeHandler : INodeHandler<Esprima.Ast.ImportDeclaration>
    {
        public Expression Execute(Node node)
        {
            var import = node as ImportDeclaration;
            if (import == null) { return Expression.Empty(); }

            //TODO get variable from shims instead....
            var imports = new List<Expression>();
            foreach (var imported in import.Specifiers)
            {
                if (imported.Type == Nodes.ImportDefaultSpecifier)
                {
                    var importDefault = imported as ImportDefaultSpecifier;
                    var vari = VariableContextService.GetCurrent().AddVariable(typeof(string), importDefault.Local.Name, node);
                    imports.Add(Expression.Assign(vari, Expression.Constant(""))); //Assign to SHIM
                }
                else if (imported.Type == Nodes.ImportSpecifier)
                {
                    var importDefault = imported as ImportSpecifier;
                    var vari = VariableContextService.GetCurrent().AddVariable(typeof(string), importDefault.Local.Name, node);
                    imports.Add(Expression.Assign(vari, Expression.Constant(""))); //Assign to SHIM
                }
            }


            return Expression.Block(imports);
        }
    }
}
