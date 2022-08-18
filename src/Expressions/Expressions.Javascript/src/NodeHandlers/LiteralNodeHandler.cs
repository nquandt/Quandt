using Expression = System.Linq.Expressions.Expression;

namespace Quandt.Expressions.Javascript.NodeHandlers
{
    [NodesEnum(Nodes.Literal)]
    internal class LiteralNodeHandler : INodeHandler<Esprima.Ast.Literal>
    {
        public Expression Execute(Node node)
        {
            var func = node as Literal;            

            switch(func.TokenType)
            {
                case (Esprima.TokenType.StringLiteral):
                    return Expression.Constant(func.StringValue, typeof(string));
                case (Esprima.TokenType.BooleanLiteral):
                    return Expression.Constant(func.BooleanValue, typeof(bool));
                case (Esprima.TokenType.NumericLiteral):
                    if (func.Raw.Contains("."))
                    {
                        return Expression.Constant(func.NumericValue, typeof(double));
                    }
                    return Expression.Constant(Convert.ToInt32(func.NumericValue), typeof(int));
            }           

            return Expression.Constant(func.Value, typeof(string));
        }
    }
}
