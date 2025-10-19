using QuickFuzzr.Tests._Tools;
using QuickFuzzr.UnderTheHood;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters.Objects;

[DocContent("Use the `.Replace()` extension method.")]
public class ReplacingPrimitiveGenerators
{
	[Fact]
	[DocContent(@"Example
```
var generator =
	from _ in Fuzz.Constant(42).Replace()
	from result in Fuzz.One<SomeThingToGenerate>()
	select result;
```
When executing above generator it will return a SomeThingToGenerate object where all integers have the value 42.
")]
	public void UsesReplacement()
	{
		var generator =
			from _ in Fuzzr.Constant(42).Replace()
			from result in Fuzzr.One<SomeThingToGenerate>()
			select result;

		var value = generator.Generate();

		Assert.Equal(42, value.AnInt);
	}

	[Fact]
	[DocContent("Replacing a primitive generator automatically impacts its nullable counterpart.")]
	public void NullableUsesReplacement()
	{
		var generator =
			from _ in Fuzzr.Int(42, 42).Replace()
			from result in Fuzzr.One<SomeThingToGenerate>()
			select result;
		CheckIf.GeneratedValuesShouldEventuallySatisfyAll(
			generator.Select(a => a.ANullableProperty),
				("is null", a => a == null),
				("is not null", a => a != null));
	}

	[Fact]
	[DocContent("Replacing a nullable primitive generator does not impacts it's non-nullable counterpart.")]
	public void NullableReplace()
	{
		var generator =
			from _ in Fuzzr.Int(666, 666).Nullable().NeverReturnNull().Replace()
			from result in Fuzzr.One<SomeThingToGenerate>()
			select result;
		var value = generator.Generate();
		Assert.True(value.AnInt < 100, value.AnInt.ToString());
		Assert.Equal(666, value.ANullableProperty);
	}

	[Fact]
	[DocContent(@"Replacements can occur multiple times during one generation :
```
var generator =
	from _ in Fuzz.Constant(42).Replace()
	from result1 in Fuzz.One<SomeThingToGenerate>()
	from __ in Fuzz.Constant(666).Replace()
	from result2 in Fuzz.One<SomeThingToGenerate>()
	select new[] { result1, result2
};
```
When executing above generator result1 will have all integers set to 42 and result2 to 666.")]
	public void MultipleReplacements()
	{
		var generator =
			from _ in Fuzzr.Constant(42).Replace()
			from result1 in Fuzzr.One<SomeThingToGenerate>()
			from __ in Fuzzr.Constant(666).Replace()
			from result2 in Fuzzr.One<SomeThingToGenerate>()
			select new[] { result1, result2 };

		var array = generator.Generate();

		Assert.Equal(42, array[0].AnInt);
		Assert.Equal(666, array[1].AnInt);
	}

	[Fact]
	[DocContent("*Note :* The Replace combinator does not actually generate anything, it only influences further generation.")]
	public void ReturnsUnit()
	{
		var generator = Fuzzr.Int(42, 42).Replace();
		Assert.Equal(Unit.Instance, generator.Generate());
	}

	public class SomeThingToGenerate
	{
		public int AnInt { get; set; }
		public int? ANullableProperty { get; set; }
	}
}