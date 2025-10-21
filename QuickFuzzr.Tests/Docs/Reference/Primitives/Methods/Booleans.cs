using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.Reference.Primitives;

[DocFile]
[DocContent("Use `Fuzzr.Bool()`.")]
public class Booleans
{
	[Fact]
	[DocContent("- The default generator generates True or False.")]
	public void DefaultGeneratorGeneratesTrueOrFalse()
	{
		CheckIf.TheseValuesAreGenerated(Fuzzr.Bool(), true, false);
	}

	[Fact]
	[DocContent("- Can be made to return `bool?` using the `.Nullable()` combinator.")]
	public void Nullable()
	{
		CheckIf.GeneratesNullAndNotNull(Fuzzr.Bool().Nullable());
	}

	[Fact]
	[DocContent("- `bool` is automatically detected and generated for object properties.")]
	public void Property()
	{
		CheckIf.TheseValuesAreGenerated(
			Fuzzr.One<SomeThingToGenerate>().Select(x => x.AProperty), true, false);
	}

	[Fact]
	[DocContent("- `bool?` is automatically detected and generated for object properties.")]
	public void NullableProperty()
	{
		CheckIf.GeneratesNullAndNotNull(
			Fuzzr.One<SomeThingToGenerate>().Select(x => x.ANullableProperty));
	}

	public class SomeThingToGenerate
	{
		public bool AProperty { get; set; }
		public bool? ANullableProperty { get; set; }
	}
}