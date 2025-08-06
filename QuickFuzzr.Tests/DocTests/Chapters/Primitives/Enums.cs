using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters.Primitives;

[DocContent("Use `Fuzz.Enum<T>()`, where T is the type of Enum you want to generate. *No overload exists.*")]
public class Enums
{
	[Fact]
	[DocContent("- The default generator just picks a random value from all enemeration values.")]
	public void DefaultGenerator()
	{
		CheckIf.TheseValuesAreGenerated(Fuzz.Enum<MyEnumeration>(),
			MyEnumeration.MyOne, MyEnumeration.Mytwo);
	}

	[Fact]
	[DocContent("- An Enumeration is automatically detected and generated for object properties.")]
	public void Property()
	{
		CheckIf.TheseValuesAreGenerated(
			Fuzz.One<SomeThingToGenerate>().Select(a => a.AnEnumeration),
			MyEnumeration.MyOne, MyEnumeration.Mytwo);
	}

	[Fact]
	[DocContent("- A nullable Enumeration is automatically detected and generated for object properties.")]
	public void NullableProperty()
	{
		CheckIf.GeneratesNullAndNotNull(
			Fuzz.One<SomeThingToGenerate>().Select(a => a.ANullableProperty));
	}

	[Fact]
	[DocContent("- Passing in a non Enum type for T throws an ArgumentException.")]
	public void Throws()
	{
		Assert.Throws<ArgumentException>(() => Fuzz.Enum<int>().Generate());
	}

	public class SomeThingToGenerate
	{
		public MyEnumeration AnEnumeration { get; set; }
		public MyEnumeration? ANullableProperty { get; set; }
	}

	public enum MyEnumeration { MyOne, Mytwo }
}