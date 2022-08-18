using Esprima.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quandt.Expressions.Javascript.Attributes
{
    internal class NodesEnumAttribute : Attribute
    {
        public Nodes NodeToHandle { get; }
        public NodesEnumAttribute(Nodes node)
        {
            NodeToHandle = node;
        }
    }
}
