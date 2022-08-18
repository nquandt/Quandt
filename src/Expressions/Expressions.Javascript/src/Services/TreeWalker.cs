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
            try
            {
                if (Handlers.ContainsKey(node.Type))
                {
                    return Handlers[node.Type].Execute(node);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            Console.WriteLine($"[Handler Not Implemented]: {node.Type}");
            return Expression.Empty();
        }

        public static IEnumerable<Expression> Walk(IEnumerable<Node> nodes)
        {
            return nodes.Select(x => Walk(x));
        }
    }
}
