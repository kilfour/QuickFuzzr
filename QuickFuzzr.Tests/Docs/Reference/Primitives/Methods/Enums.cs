﻿using QuickFuzzr.Tests._Tools;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Docs.Reference.Primitives.Methods;

[DocFile]
[DocContent("Use `Fuzzr.Enum<T>()`, where T is the type of Enum you want to generate.")]
[DocContent("> Enums are included here for convenience. While not numeric primitives themselves, they are generated as atomic values from their defined members.")]
public class Enums
{
	[Fact]
	[DocContent("- The default generator just picks a random value from all enumeration values.")]
	public void DefaultGenerator()
	{
		CheckIf.TheseValuesAreGenerated(Fuzzr.Enum<MyEnumeration>(),
			MyEnumeration.MyOne, MyEnumeration.Mytwo);
	}

	[Fact]
	[DocContent("- An Enumeration is automatically detected and generated for object properties.")]
	public void Property()
	{
		CheckIf.TheseValuesAreGenerated(
			Fuzzr.One<SomeThingToGenerate>().Select(a => a.AnEnumeration),
			MyEnumeration.MyOne, MyEnumeration.Mytwo);
	}

	[Fact]
	[DocContent("- A nullable enumeration is automatically detected and generated for object properties.")]
	public void NullableProperty()
	{
		CheckIf.GeneratesNullAndNotNull(
			Fuzzr.One<SomeThingToGenerate>().Select(a => a.ANullableProperty));
	}

	[Fact]
	[DocContent("- Passing in a non Enum type for T throws an ArgumentException.")]
	public void Throws()
	{
		Assert.Throws<ArgumentException>(() => Fuzzr.Enum<int>().Generate());
	}

	public class SomeThingToGenerate
	{
		public MyEnumeration AnEnumeration { get; set; }
		public MyEnumeration? ANullableProperty { get; set; }
	}

	public enum MyEnumeration { MyOne, Mytwo }
}