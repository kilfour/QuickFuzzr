using QuickPulse.Explains.Deprecated;

namespace QuickFuzzr.Tests.DocTests.Chapters.Primitives;

[Doc(Order = "1-6-4", Caption = "Booleans",
	Content = "Use `Fuzz.Bool()`. \n\nNo overload exists.")]
public class Booleans
{
	[Fact]
	[Doc(Order = "1-6-4-1",
		Content = "The default generator generates True or False.")]
	public void DefaultGeneratorGeneratesTrueOrFalse()
	{
		_Tools.CheckIf.TheseValuesAreGenerated(Fuzz.Bool(), true, false);
	}

	[Fact]
	[Doc(Order = "1-6-4-2",
		Content = "Can be made to return `bool?` using the `.Nullable()` combinator.")]
	public void Nullable()
	{
		_Tools.CheckIf.GeneratesNullAndNotNull(Fuzz.Bool().Nullable());
	}

	[Fact]
	[Doc(Order = "1-6-4-3",
		Content = " - `bool` is automatically detected and generated for object properties.")]
	public void Property()
	{
		_Tools.CheckIf.TheseValuesAreGenerated(
			Fuzz.One<SomeThingToGenerate>().Select(x => x.AProperty), true, false);
	}

	[Fact]
	[Doc(Order = "1-6-4-4",
		Content = " - `bool?` is automatically detected and generated for object properties.")]
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