using QuickFuzzr.Tests._Tools;

namespace QuickFuzzr.Tests.DocTests.Chapters.Primitives;

// [Enums(
// 	Content = "Use `Fuzz.Enum<T>()`, where T is the type of Enum you want to generate. \n\nNo overload exists.",
// 	Order = 0)]
public class Enums
{
	[Fact]
	// 		[Enums(
	// 			Content =
	// "The default generator just picks a random value from all enemeration values.",
	// 			Order = 1)]
	public void DefaultGenerator()
	{
		CheckIf.TheseValuesAreGenerated(Fuzz.Enum<MyEnumeration>(),
			MyEnumeration.MyOne, MyEnumeration.Mytwo);
	}

	[Fact]
	// [Enums(
	// 	Content = " - An Enumeration is automatically detected and generated for object properties.",
	// 	Order = 2)]
	public void Property()
	{
		CheckIf.TheseValuesAreGenerated(
			Fuzz.One<SomeThingToGenerate>().Select(a => a.AnEnumeration),
			MyEnumeration.MyOne, MyEnumeration.Mytwo);
	}

	[Fact]
	// [Enums(
	// 	Content = " - A nullable Enumeration is automatically detected and generated for object properties.",
	// 	Order = 3)]
	public void NullableProperty()
	{
		CheckIf.GeneratesNullAndNotNull(
			Fuzz.One<SomeThingToGenerate>().Select(a => a.ANullableProperty));
	}

	[Fact]
	// [Enums(
	// 	Content = " - Passing in a non Enum type for T throws an ArgumentException.",
	// 	Order = 3)]
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