using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters.Objects;


[DocContent("Use `Fuzz.One<T>()`, where T is the type of object you want to generate.")]
public class ASimpleObject
{
	[Fact]
	[DocContent("- The primitive properties of the object will be automatically filled in using the default (or replaced) generators.")]
	public void FillsPrimitives()
	{
		Assert.NotEqual(0, Fuzzr.One<SomeThingToGenerate>().Generate().AProperty);
	}

	[Fact]
	[DocContent("- The enumeration properties of the object will be automatically filled in using the default (or replaced) Fuzz.Enum<T> generator.")]
	public void FillsEnumerations()
	{
		var generator = Fuzzr.One<SomeThingToGenerate>();
		var one = false;
		var two = false;
		for (int i = 0; i < 20; i++)
		{
			var value = generator.Generate().AnEnumeration;
			one = one || value == MyEnumeration.MyOne;
			two = two || value == MyEnumeration.Mytwo;
		}
		Assert.True(one);
		Assert.True(two);
	}

	[Fact]
	[DocContent("- The object properties will also be automatically filled in using the default (or replaced) generators, similar to calling Fuzz.One<TProperty>() and setting the value using `Apply` (see below) explicitly.")]
	public void FillsObjectProperties()
	{
		var generator = Fuzzr.One<RootObject>();
		var result = generator.Generate();
		Assert.NotNull(result);
		Assert.NotNull(result.Nest);
		Assert.NotEqual(0, result.Nest.Value);
	}
	public class RootObject { public NestedObject? Nest { get; set; } }

	public class NestedObject { public int Value { get; set; } }

	[Fact]
	[DocContent("- Also works for properties with private setters.")]
	public void FillsPrivateSetterProperties()
	{
		Assert.NotEqual(0, Fuzzr.One<SomeThingToGenerate>().Generate().APropertyWithPrivateSetters);
		var generator = Fuzzr.One<SomeThingToGenerate>();
		var one = false;
		var two = false;
		for (int i = 0; i < 20; i++)
		{
			var value = generator.Generate().AnEnumerationWithPrivateSetter;
			one = one || value == MyEnumeration.MyOne;
			two = two || value == MyEnumeration.Mytwo;
		}
		Assert.True(one);
		Assert.True(two);
	}

	[Fact]
	[DocContent("- Can generate any object that has a parameterless constructor, be it public, protected, or private.")]
	public void CanGenerateObjectsProtectedAndPrivate()
	{
		Fuzzr.One<SomeThingProtectedToGenerate>().Generate();
		Fuzzr.One<SomeThingPrivateToGenerate>().Generate();
	}

	[Fact]
	[DocContent("- `record` generation is also possible.")]
	public void CanGenerateRecords()
	{
		var generator =
			from _ in Fuzzr.Constant(42).Replace()
			from record in Fuzzr.One<MyRecord>()
			select record;
		var result = generator.Generate();
		Assert.Equal(42, result.Value);
	}

	public record MyRecord
	{
		public MyRecord() { }

		public MyRecord(int Value)
		{
			this.Value = Value;
		}

		public int Value { get; init; }
	}

	[Fact]
	[DocContent("- The overload `Fuzz.One<T>(Func<T> constructor)` allows for specific constructor selection.")]
	public void CustomConstructor()
	{
		var generator =
			from ignore in Fuzzr.For<SomeThingWithAnAnswer>().Ignore(e => e.Answer)
			from result in Fuzzr.One(() => new SomeThingWithAnAnswer(42))
			select result;
		Assert.Equal(42, generator.Generate().Answer);
	}

	public class SomeThingToGenerate
	{
		public int AProperty { get; set; }
		public int APropertyWithPrivateSetters { get; private set; }
		public MyEnumeration AnEnumeration { get; set; }
		public MyEnumeration AnEnumerationWithPrivateSetter { get; private set; }

		public int APublicField;
	}

	public enum MyEnumeration
	{
		MyOne,
		Mytwo
	}

	public class SomeThingProtectedToGenerate
	{
		protected SomeThingProtectedToGenerate() { }
	}

	public class SomeThingPrivateToGenerate
	{
		protected SomeThingPrivateToGenerate() { }
	}

	public class SomeThingWithAnAnswer
	{
		public int Answer { get; set; }

		public SomeThingWithAnAnswer(int answer)
		{
			Answer = answer;
		}
	}
}