using Esprima.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using Expression = System.Linq.Expressions.Expression;
using System.Text;
using System.Threading.Tasks;

namespace Quandt.Expressions.Javascript.Abstractions
{
    internal interface INodeHandler<T> : INodeHandler
    {        
    }

    internal interface INodeHandler
    {
        Expression Execute(Node node);
    }
}
