using Expression = System.Linq.Expressions.Expression;

namespace Quandt.Expressions.Javascript.Services
{
    internal static class TreeWalker
    {
        public static Dictionary<Nodes, INodeHandler> Handlers { get; set; }

        static TreeWalker()
        {
            Handlers = typeof(TreeWalker).Assembly
                .GetTypes()
                .Where(x => !x.IsInterface && !x.IsAbstract && typeof(INodeHandler).IsAssignableFrom(x))
                //.Where(x => !x.IsInterface && !x.IsAbstract && x.GetInterfaces().Where(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(INodeHandler<>)).Any())
                .ToDictionary(x => x.GetCustomAttributes(typeof(NodesEnumAttribute), true).Cast<NodesEnumAttribute>().First().NodeToHandle,
                    y =>
                    {
                        var nodeHandler = (INodeHandler)Activator.CreateInstance(y)!;

                        return nodeHandler;
                    });
        }

        public static Expression Walk(Node node)
        {
            Console.WriteLine(node.Type);

            if (Handlers.ContainsKey(node.Type))
            {
                var funcs = node.ChildNodes.Where(x => x.Type == Nodes.FunctionDeclaration).Cast<FunctionDeclaration>(); //This will ned up running twice... because we dont yet know what type this should be
                foreach (var func in funcs)
                {
                    Type type = typeof(object);
                    Type intendedType = (Type)func.GetAdditionalData("IntendedType");
                    if (intendedType != null)
                    {
                        type = intendedType;
                    }

                    VariableContextService.GetCurrent().AddVariable(type, func.Id.Name, func);
                }

                var expr = Handlers[node.Type].Execute(node);
                return expr;
            }

            Console.WriteLine($"[Handler Not Implemented]: {node.Type}");
            throw new Exception($"[Handler Not Implemented]: {node.Type}");            
        }

        public static IEnumerable<Expression> Walk(IEnumerable<Node> nodes)
        {
            return nodes.Select(x => Walk(x)).ToArray(); //sadly need to invoke enumeration for the VariableContext....
        }
    }
}
