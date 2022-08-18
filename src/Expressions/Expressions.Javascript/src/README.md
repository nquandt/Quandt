This project is a first shot attempt at converting JS to C# via Expressions... The end goal to to be able to convert .js to .cs files

This utilizes Esprima for js AST and ReadableExpressions for outputting LINQ Expressions as c# source code.

Possible use cases could be compiling common npm packages such as Rollup to run in an ASP.NET server to bundle JS at runtime..?

Anyone who wants to contribute I welcome. This is my first initial thoughts. I have organized the AST handlers into the NodeHandlers folder... 
and have only implemented a few Expression conversions so far. TreeWalker.cs exposes a static class and methods for the Esprima AST that utilizes the INodeHandlers.

I may consider just writing a big switch statement instead of the dictionary lookup but for now this was how I did it so I didnt have to add a switch case everytime I made a new handler.

The TreeWalker Walk method will log to console when an Expression node doesnt have a handler. you can uncomment line 41 of that file to just throw an exception.

Thanks for checking this out.

tag: Metaprogamming
The following was converted using this library (if anyone knows how to two column this on github I'll put the code side-by-side
```javascript
function main(parm1, parm2) {
  var topLevel = ""this is top level and hopefully works"";

  function hello(nod1){
      var thisThing = 4 + nod1 + topLevel;
      var thisThing2 = nod1 + thisThing;


      return thisThing2;
  }

  function hello2(nod1){
      var thisThing = 4 + nod1;
      var thisThing2 = nod1 + thisThing;


      return thisThing2;
  }

  const files = [ 'foo.txt ', '.bar', '   ', 'baz.foo' ];
  let filePaths = [];

  for (let file of files) {
    const fileName = file.trim();
    if(fileName) {
      const filePath = `~/cool_app/${fileName}hello${fileName}yooo`;
      filePaths.push(filePath);
    }
  }

  return filePaths;
}
```


```csharp
namespace GenerateJS
{
    public static class ImAJsFile
    {
        public static void Execute()
        {
            Func<string, string, List<string>> main = (string parm1, string parm2) =>
            {
                var topLevel = "this is top level and hopefully works";
                Func<string, string> hello = (string nod1) =>
                {
                    var thisThing = 4 + nod1 + topLevel;
                    var thisThing2 = nod1 + thisThing;
                    return thisThing2;
                };
                Func<string, string> hello2 = (string nod1) =>
                {
                    thisThing = 4 + nod1;
                    thisThing2 = nod1 + thisThing;
                    return thisThing2;
                };
                files = new List<string>
                {
                    "foo.txt ",
                    ".bar",
                    "   ",
                    "baz.foo"
                };
                filePaths = new List<string>();
                IEnumerator<string> enumerator;
                enumerator = files.GetEnumerator();
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        string file;
                        file = enumerator.Current;
                        fileName = file.Trim();
                        if (fileName != null)
                        {
                            filePath = string.Format("`~/cool_app/{0}hello{1}yooo`", fileName, fileName);
                            filePaths.Add(filePath);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                return filePaths;
            };
        }
    }
}
```
