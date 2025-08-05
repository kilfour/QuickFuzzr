using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters.Primitives;

[DocContent("Use `Fuzz.Bool()`. *No overload exists.*")]
public class Booleans
{
	[Fact]
	[DocContent("- The default generator generates True or False.")]
	public void DefaultGeneratorGeneratesTrueOrFalse()
	{
		_Tools.CheckIf.TheseValuesAreGenerated(Fuzz.Bool(), true, false);
	}

	[Fact]
	[DocContent("- Can be made to return `bool?` using the `.Nullable()` combinator.")]
	public void Nullable()
	{
		_Tools.CheckIf.GeneratesNullAndNotNull(Fuzz.Bool().Nullable());
	}

	[Fact]
	[DocContent("- `bool` is automatically detected and generated for object properties.")]
	public void Property()
	{
		_Tools.CheckIf.TheseValuesAreGenerated(
			Fuzz.One<SomeThingToGenerate>().Select(x => x.AProperty), true, false);
	}

	[Fact]
	[DocContent("- `bool?` is automatically detected and generated for object properties.")]
	public void NullableProperty()
	{
		_Tools.CheckIf.GeneratesNullAndNotNull(
			Fuzz.One<SomeThingToGenerate>().Select(x => x.ANullableProperty));
	}

	public class SomeThingToGenerate
	{
		public bool AProperty { get; set; }
		public bool? ANullableProperty { get; set; }
	}
}