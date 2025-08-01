
using QuickFuzzr.UnderTheHood;
using QuickPulse.Explains.Deprecated;

namespace QuickFuzzr.Tests.Objects;

[Doc(Order = "1-3-9",
Caption = "Replacing Primitive Generators",
Content = @"Use the `.Replace()` extension method.")]
public class ReplacingPrimitiveGenerators
{
	[Fact]
	[Doc(Order = "1-3-9",
		Content =
@"Example
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
			from _ in Fuzz.Constant(42).Replace()
			from result in Fuzz.One<SomeThingToGenerate>()
			select result;

		var value = generator.Generate();

		Assert.Equal(42, value.AnInt);
	}

	[Fact]
	[Doc(Order = "1-3-9",
		Content =
@"Replacing a primitive generator automatically impacts its nullable counterpart.")]
	public void NullableUsesReplacement()
	{
		var generator =
			from _ in Fuzz.Int(42, 42).Replace()
			from result in Fuzz.One<SomeThingToGenerate>()
			select result;
		_Tools.CheckIf.GeneratedValuesShouldEventuallySatisfyAll(generator.Select(a => a.ANullableProperty),
			("is null", a => a == null), ("is not null", a => a != null));
	}

	[Fact]
	[Doc(Order = "1-3-9",
		Content =
@"Replacing a nullable primitive generator does not impacts it's non-nullable counterpart.")]
	public void NullableReplace()
	{
		var generator =
			from _ in Fuzz.Int(666, 666).Nullable().NeverReturnNull().Replace()
			from result in Fuzz.One<SomeThingToGenerate>()
			select result;
		var value = generator.Generate();
		Assert.True(value.AnInt < 100, value.AnInt.ToString());
		Assert.Equal(666, value.ANullableProperty);
	}

	[Fact]
	[Doc(Order = "1-3-9",
		Content =
@"Replacements can occur multiple times during one generation :
```
var generator =
	from _ in Fuzz.Constant(42).Replace()
	from result1 in Fuzz.One<SomeThingToGenerate>()
	from __ in Fuzz.Constant(666).Replace()
	from result2 in Fuzz.One<SomeThingToGenerate>()
	select new[] { result1, result2 };
```
When executing above generator result1 will have all integers set to 42 and result2 to 666.")]
	public void MultipleReplacements()
	{
		var generator =
			from _ in Fuzz.Constant(42).Replace()
			from result1 in Fuzz.One<SomeThingToGenerate>()
			from __ in Fuzz.Constant(666).Replace()
			from result2 in Fuzz.One<SomeThingToGenerate>()
			select new[] { result1, result2 };

		var array = generator.Generate();

		Assert.Equal(42, array[0].AnInt);
		Assert.Equal(666, array[1].AnInt);
	}

	[Fact]
	[Doc(Order = "1-3-9",
		Content = "*Note :* The Replace combinator does not actually generate anything, it only influences further generation.")]
	public void ReturnsUnit()
	{
		var generator = Fuzz.Int(42, 42).Replace();
		Assert.Equal(Unit.Instance, generator.Generate());
	}

	[Fact]
	public void JustChecking()
	{
		var generator =
			from i in Fuzz.ChooseFromThese(42, 43).Unique("key")
			from result in Fuzz.One<SomeThingToGenerate>().Apply(s => s.AnInt = i)
			select result;

		var values = generator.Many(2).Generate().ToArray();

		Assert.NotEqual(values[0].AnInt, values[1].AnInt);
	}

	public class SomeThingToGenerate
	{
		public int AnInt { get; set; }
		public int? ANullableProperty { get; set; }
	}
}