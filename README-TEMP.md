# Quick Fuzzr
> A type-walking cheetah with a hand full of random.
## Introduction
An evolution from the QuickMGenerate library.
## Installation
QuickFuzzrQuickFuzzr is available on NuGet:
```bash
Install-Package QuickFuzzr
```
Or via the .NET CLI:
```bash
dotnet add package QuickFuzzr
```
## Generating Primitives
The Fuzz class has many methods which can be used to obtain a corresponding primitive.
F.i. `Fuzz.Int()`. 

Full details below in the chapter 'The Primitive Generators'.
## Combining Generators
### Linq Syntax
Each Fuzz Generator can be used as a building block and combined using query expressions.
F.i. the following :
```
var stringGenerator =
	from a in Fuzz.Int()
	from b in Fuzz.String()
	from c in Fuzz.Int()
	select a + b + c;
Console.WriteLine(stringGenerator.Generate());
```
Will output something like `28ziicuiq56`.
Generators are reusable building blocks. 

In the following :
```
var generator =
	from str in stringGenerator.Replace()
	from thing in Fuzz.One<SomeThingToGenerate>()
	select thing;
```
We reuse the 'stringGenerator' defined above and replace the default string generator with our custom one. 
All strings in the generated object will have the pattern defined by 'stringGenerator'.
This approach removes the problem of combinatoral explosion. No need for a Transform<T, U>(...) combinator for example
as this can be easily achieved using Linq. 

```
var generator =
	from chars in Fuzz.Constant('-').Many(5)
	let composed = chars.Aggregate(", (a, b) => a + b.ToString())
	select composed;
```
Generates: "-----".
### Using Extensions
When applying the various extension methods onto a generator, they get *combined* into a new generator.
Jumping slightly ahead of ourselves as below example will use methods that are explained more thoroughly further below.

E.g. :
```
Fuzz.ChooseFrom(someValues).Unique("key").Many(2)
```

