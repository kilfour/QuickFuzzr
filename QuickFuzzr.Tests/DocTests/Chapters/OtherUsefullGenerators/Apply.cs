using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters.OtherUsefullGenerators;

[DocContent("Use the `.Apply<T>(Func<T, T> func)` extension method.")]
public class Apply
{
	[Fact]
	[DocContent(
@"Applies the specified Function to the generated value, returning the result.
F.i. `Fuzz.Constant(41).Apply(i =>  i + 1)` will return 42.")]
	public void FunctionIsApplied()
	{
		var generator = Fuzzr.Constant(41).Apply(i => i + 1);
		Assert.Equal(42, generator.Generate());
	}

	[Fact]
	[DocContent(
@"Par example, when you want all decimals to be rounded to a certain precision : 
```
var generator = 
	from _ in Fuzz.Decimal().Apply(d => Math.Round(d, 2)).Replace()
	from result in Fuzz.One<SomeThingToGenerate>()
	select result;
```")]
	public void RoundingExample()
	{
		var generator =
			from _ in Fuzzr.Decimal().Apply(d => Math.Round(d, 2)).Replace()
			from result in Fuzzr.One<SomeThingToGenerate>()
			select result;
		var value = generator.Generate().MyProperty;
		//var count = BitConverter.GetBytes(decimal.GetBits(generator.Generate().MyProperty)[3])[2];
		var count = value.ToString().Split('.', ',')[1].Count();
		Assert.Equal(2, count);
	}

	[Fact]
	[DocContent(
@"An overload exists with signature `Apply<T>(Action<T> action)`.
This is useful when dealing with objects and you just don't want to return said object.
E.g. `Fuzz.One<SomeThingToGenerate>().Apply(session.Save)`.")]
	public void ActionIsApplied()
	{
		var generator = Fuzzr.One<SomeThingToGenerate>().Apply(thing => thing.MyProperty = 42);

		Assert.Equal(42, generator.Generate().MyProperty);
	}

	public class SomeThingToGenerate
	{
		public decimal MyProperty { get; set; }
	}
}