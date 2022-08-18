using Quandt.Expressions.Javascript;
;

//var transpiler = new JSToCSharpTranspiler(@"
//var topLevel = ""this is top level and hopefully works"";

//function hello(nod1){
//    var thisThing = 4 + nod1 + topLevel;
//    var thisThing2 = nod1 + thisThing;


//    return thisThing2;
//}

//function hello2(nod1){
//    var thisThing = 4 + nod1;
//    var thisThing2 = nod1 + thisThing;


//    return thisThing2;
//}
//");

var transpiler = new Transpiler(@"
function main(parm1, parm2) {
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
");


var cSharp = transpiler.Convert();

Console.WriteLine(cSharp);
File.WriteAllText(@"C:\Users\nquandt\source\repos\JSExpressions\JsExpressions\JsExpressions\Generated.cs", cSharp);
//try
//{


//    File.WriteAllText(@"C:\Users\nquandt\source\repos\JSExpressions\JsExpressions\JsExpressions\Generated.cs", cSharp);
//    //Console.WriteLine(cSharp);
//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.ToString());
//}
