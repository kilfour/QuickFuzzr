using QuickPulse.Explains;

namespace QuickFuzzr.Tests.Objects;

[Doc(Order = "1-3-1",
	Caption = "A simple object",
	Content = "Use `Fuzz.One<T>()`, where T is the type of object you want to generate.")]
public class OneObject
{
	[Fact]
	[Doc(Order = "1-3-1-1",
		Content =
@"- The primitive properties of the object will be automatically filled in using the default (or replaced) generators.")]
	public void FillsPrimitives()
	{
		Assert.NotEqual(0, Fuzz.One<SomeThingToGenerate>().Generate().AProperty);
	}

	[Fact]
	[Doc(Order = "1-3-1-2",
		Content =
@"- The enumeration properties of the object will be automatically filled in using the default (or replaced) Fuzz.Enum<T> generator.")]
	public void FillsEnumerations()
	{
		var generator = Fuzz.One<SomeThingToGenerate>();
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
	[Doc(Order = "1-3-1-3",
		Content =
@"- The object properties will also be automatically filled in using the default (or replaced) generators, similar to calling Fuzz.One<TProperty>() and setting the value using `Apply` (see below) explicitly.")]
	public void FillsObjectProperties()
	{
		var generator = Fuzz.One<RootObject>();
		var result = generator.Generate();
		Assert.NotNull(result);
		Assert.NotNull(result.Nest);
		Assert.NotEqual(0, result.Nest.Value);
	}
	public class RootObject { public NestedObject? Nest { get; set; } }

	public class NestedObject { public int Value { get; set; } }

	[Fact]
	[Doc(Order = "1-3-1-4",
		Content =
@"- Also works for properties with private setters.")]
	public void FillsPrivateSetterProperties()
	{
		Assert.NotEqual(0, Fuzz.One<SomeThingToGenerate>().Generate().APropertyWithPrivateSetters);
		var generator = Fuzz.One<SomeThingToGenerate>();
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
	[Doc(Order = "1-3-1-5",
		Content =
@"- Can generate any object that has a parameterless constructor, be it public, protected, or private.")]
	public void CanGenerateObjectsProtectedAndPrivate()
	{
		Fuzz.One<SomeThingProtectedToGenerate>().Generate();
		Fuzz.One<SomeThingPrivateToGenerate>().Generate();
	}

	[Fact]
	[Doc(Order = "1-3-1-6",
		Content =
@"- The overload `Fuzz.One<T>(Func<T> constructor)` allows for specific constructor selection.")]
	public void CustomConstructor()
	{
		var generator =
			from ignore in Fuzz.For<SomeThingWithAnAnswer>().Ignore(e => e.Answer)
			from result in Fuzz.One(() => new SomeThingWithAnAnswer(42))
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