using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AgileObjects.ReadableExpressions;
using Esprima.Utils;
using Quandt.Expressions.Javascript.Services;

namespace Quandt.Expressions.Javascript
{
    public class Transpiler
    {
        private readonly string _jsCode;
        private readonly string _className;
        //private readonly TreeWalker _walker;
        private IEnumerable<Type> _overrides;
        private static readonly Dictionary<Esprima.Ast.Nodes, INodeHandler> _handlers;
        


        public Transpiler(string code, string className = "ImAJsFile")
        {
            _className = className;
            _jsCode = code;
            //_walker = new TreeWalker();

            
            //_overrides = new List<Type>()
            //    {
            //        //typeof(LambdaTranslation2)
            //    };
        }

        private System.Linq.Expressions.Expression ConvertToLambdas()
        {
            var parser = new Esprima.JavaScriptParser(_jsCode);
            var program = parser.ParseScript();

            var tw = new StringWriter();
            program.WriteJson(tw, "    ");

            File.WriteAllText(@"C:\Users\nquandt\source\repos\JSExpressions\JsExpressions\JsExpressions\Generated.json", tw.ToString());

            return Walk(program);
        }

        public string Convert()
        {
            var lambs = ConvertToLambdas();
            var expr = lambs.ToReadableString(x =>
            {
                return x.ShowLambdaParameterTypes;//.OverrideTranslations(_overrides); //This might be needed I'm not sure yet. If so hopefully AgileObjects hops on my PR
            });
            var programSig = (string method) => $@"



namespace GenerateJS
{{
    public static class {_className}
    {{
        public static void Execute()
        {{
            {string.Join("\n            ", method.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))}
        }}
    }}
}}
";

            return programSig(expr);
        }
    }
}
