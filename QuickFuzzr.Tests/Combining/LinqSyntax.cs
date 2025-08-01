using QuickPulse.Explains.Deprecated;

namespace QuickFuzzr.Tests.Combining;

[Doc(Order = "1-2-1",
	Caption = "Linq Syntax",
	Content = "Each Fuzz Generator can be used as a building block and combined using query expressions.")]
public class LinqSyntax
{
	[Fact]
	[Doc(Order = "1-2-1-1",
		Content =
@"F.i. the following :
```
var stringGenerator =
	from a in Fuzz.Int()
	from b in Fuzz.String()
	from c in Fuzz.Int()
	select a + b + c;
Console.WriteLine(stringGenerator.Generate());
```
Will output something like `28ziicuiq56`.")]
	public void SimpleCombination()
	{
		var generator =
			from a in Fuzz.Constant(42)
			from b in Fuzz.Constant("Hello")
			from c in Fuzz.Constant(666)
			select a + b + c;

		Assert.Equal("42Hello666", generator.Generate());
	}

	[Fact]
	[Doc(Order = "1-2-1-2",
		Content =
@"Generators are reusable building blocks. 

In the following :
```
var generator =
	from str in stringGenerator.Replace()
	from thing in Fuzz.One<SomeThingToGenerate>()
	select thing;
```
We reuse the 'stringGenerator' defined above and replace the default string generator with our custom one. 
All strings in the generated object will have the pattern defined by 'stringGenerator'.")]
	public void ReusingGenerators()
	{
		var generator =
			from a in Fuzz.Constant(42)
			from b in Fuzz.Constant("Hello")
			from c in Fuzz.Constant(666)
			select a + b + c;

		Assert.Equal("42Hello666", generator.Generate());
	}

	[Fact]
	[Doc(Order = "1-2-1-3",
		Content =
@"This approach removes the problem of combinatoral explosion. No need for a Transform<T, U>(...) combinator for example
as this can be easily achieved using Linq. 

```
var generator =
	from chars in Fuzz.Constant('-').Many(5)
	let composed = chars.Aggregate("", (a, b) => a + b.ToString())
	select composed;
```
Generates: ""-----"".")]
	public void AvoidingTransform()
	{
		var generator =
			from chars in Fuzz.Constant('-').Many(5)
			let composed = chars.Aggregate("", (a, b) => a + b.ToString())
			select composed;
		var result = generator.Generate();
		Assert.Equal("-----", result);
	}
}