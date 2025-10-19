using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters.Combining;

[DocContent("Each Fuzz Generator can be used as a building block and combined using query expressions.")]
public class LinqSyntax
{
	[Fact]
	[DocContent(
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
			from a in Fuzzr.Constant(42)
			from b in Fuzzr.Constant("Hello")
			from c in Fuzzr.Constant(666)
			select a + b + c;

		Assert.Equal("42Hello666", generator.Generate());
	}

	[Fact]
	[DocContent(
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
			from a in Fuzzr.Constant(42)
			from b in Fuzzr.Constant("Hello")
			from c in Fuzzr.Constant(666)
			select a + b + c;

		Assert.Equal("42Hello666", generator.Generate());
	}

	[Fact]
	[DocContent(
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
			from chars in Fuzzr.Constant('-').Many(5)
			let composed = chars.Aggregate("", (a, b) => a + b.ToString())
			select composed;
		var result = generator.Generate();
		Assert.Equal("-----", result);
	}
}