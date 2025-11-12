using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters.Objects;

[DocContent("Use the `.Replace()` extension method.")]
public class ReplacingPrimitiveGenerators
{
	[Fact]
	[DocContent(@"Example
```
var fuzzr =
	from _ in Fuzzr.Constant(42).Replace()
	from result in Fuzzr.One<SomeThingToGenerate>()
	select result;
```
When executing above fuzzr it will return a SomeThingToGenerate object where all integers have the value 42.
")]
	public void UsesReplacement()
	{
		var fuzzr =
			from _ in Configr.Primitive(Fuzzr.Constant(42))
			from result in Fuzzr.One<SomeThingToGenerate>()
			select result;

		var value = fuzzr.Generate();

		Assert.Equal(42, value.AnInt);
	}

	[Fact]
	[DocContent("Replacing a primitive fuzzr automatically impacts its nullable counterpart.")]
	public void NullableUsesReplacement()
	{
		var fuzzr =
			from _ in Configr.Primitive(Fuzzr.Int(42, 42))
			from result in Fuzzr.One<SomeThingToGenerate>()
			select result;
		CheckIf.GeneratedValuesShouldEventuallySatisfyAll(
			fuzzr.Select(a => a.ANullableProperty),
				("is null", a => a == null),
				("is not null", a => a != null));
	}

	[Fact]
	[DocContent("Replacing a nullable primitive fuzzr does not impacts it's non-nullable counterpart.")]
	public void NullableReplace()
	{
		var fuzzr =
			from _ in Configr.Primitive(Fuzzr.Int(666, 666).Nullable().NeverReturnNull())
			from result in Fuzzr.One<SomeThingToGenerate>()
			select result;
		var value = fuzzr.Generate();
		Assert.True(value.AnInt < 100, value.AnInt.ToString());
		Assert.Equal(666, value.ANullableProperty);
	}

	public class SomeThingToGenerate
	{
		public int AnInt { get; set; }
		public int? ANullableProperty { get; set; }
	}
}