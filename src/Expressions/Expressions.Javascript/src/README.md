This project is a first shot attempt at converting JS to C# via Expressions... The end goal to to be able to convert .js to .cs files

This utilizes Esprima for js AST and ReadableExpressions for outputting LINQ Expressions as c# source code.

Possible use cases could be compiling common npm packages such as Rollup to run in an ASP.NET server to bundle JS at runtime..?

Anyone who wants to contribute I welcome. This is my first initial thoughts. I have organized the AST handlers into the NodeHandlers folder... 
and have only implemented a few Expression conversions so far. TreeWalker.cs exposes a static class and methods for the Esprima AST that utilizes the INodeHandlers.

I may consider just writing a big switch statement instead of the dictionary lookup but for now this was how I did it so I didnt have to add a switch case everytime I made a new handler.

The TreeWalker Walk method will log to console when an Expression node doesnt have a handler. you can uncomment line 41 of that file to just throw an exception.

Thanks for checking this out.

tag: Metaprogamming;