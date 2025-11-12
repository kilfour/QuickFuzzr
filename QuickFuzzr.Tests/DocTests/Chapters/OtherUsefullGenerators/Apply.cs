using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters.OtherUsefullGenerators;

[DocContent("Use the `.Apply<T>(Func<T, T> func)` extension method.")]
public class Apply
{
	[Fact]
	[DocContent(
@"Applies the specified Function to the generated value, returning the result.
F.i. `Fuzzr.Constant(41).Apply(i =>  i + 1)` will return 42.")]
	public void FunctionIsApplied()
	{
		var fuzzr = Fuzzr.Constant(41).Apply(i => i + 1);
		Assert.Equal(42, fuzzr.Generate());
	}

	[Fact]
	[DocContent(
@"Par example, when you want all decimals to be rounded to a certain precision : 
```
var fuzzr = 
	from _ in Fuzzr.Decimal().Apply(d => Math.Round(d, 2)).Replace()
	from result in Fuzzr.One<SomeThingToGenerate>()
	select result;
```")]
	public void RoundingExample()
	{
		var fuzzr =
			from _ in Configr.Primitive(Fuzzr.Decimal(1, 100).Apply(d => Math.Round(d, 4)))
			from result in Fuzzr.One<SomeThingToGenerate>()
			select result;
		var value = fuzzr.Generate().MyProperty;
		//var count = BitConverter.GetBytes(decimal.GetBits(fuzzr.Generate().MyProperty)[3])[2];
		var count = value.ToString().Split('.', ',')[1].Count();
		Assert.Equal(4, count);
	}

	[Fact]
	[DocContent(
@"An overload exists with signature `Apply<T>(Action<T> action)`.
This is useful when dealing with objects and you just don't want to return said object.
E.g. `Fuzzr.One<SomeThingToGenerate>().Apply(session.Save)`.")]
	public void ActionIsApplied()
	{
		var fuzzr = Fuzzr.One<SomeThingToGenerate>().Apply(thing => thing.MyProperty = 42);

		Assert.Equal(42, fuzzr.Generate().MyProperty);
	}

	public class SomeThingToGenerate
	{
		public decimal MyProperty { get; set; }
	}
}