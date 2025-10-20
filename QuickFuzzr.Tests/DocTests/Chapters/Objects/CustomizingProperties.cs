using QuickFuzzr.UnderTheHood;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters.Objects;

[DocContent(@"Use the `Fuzz.For<T>().Customize<TProperty>(Expression<Func<T, TProperty>> func, Generator<TProperty>)` method chain.

F.i. :
```
Fuzz.For<SomeThingToGenerate>().Customize(s => s.MyProperty, Fuzz.Constant(42))
```")]
public class CustomizingProperties
{
	[Fact]
	[DocContent("The property specified will be generated using the passed in generator.")]
	public void StaysDefaultValue()
	{
		var generator =
			from c in Configr<SomeThingToGenerate>.Property(s => s.AnInt, Fuzzr.Constant(42))
			from r in Fuzzr.One<SomeThingToGenerate>()
			select r;
		Assert.Equal(42, generator.Generate().AnInt);
	}

	[Fact]
	[DocContent("An overload exists which allows for passing a value instead of a generator.")]
	public void UsingValue()
	{
		var generator =
			from c in Configr<SomeThingToGenerate>.Property(s => s.AnInt, 42)
			from r in Fuzzr.One<SomeThingToGenerate>()
			select r;
		Assert.Equal(42, generator.Generate().AnInt);
	}

	[Fact]
	[DocContent("Derived classes generated also use the custom property.")]
	public void WorksForDerived()
	{
		var generator =
			from _ in Configr<SomeThingToGenerate>.Property(s => s.AnInt, 42)
			from result in Fuzzr.One<SomeThingDerivedToGenerate>()
			select result;
		Assert.Equal(42, generator.Generate().AnInt);
	}

	[Fact]
	[DocContent("*Note :* The Customize combinator does not actually generate anything, it only influences further generation.")]
	public void ReturnsUnit()
	{
		var generator = Configr<SomeThingToGenerate>.Property(s => s.AnInt, 42);
		Assert.Equal(Unit.Instance, generator.Generate());
	}

	public class SomeThingToGenerate
	{
		public int AnInt { get; set; }
		public int AnIntField;
	}

	public class SomeThingDerivedToGenerate : SomeThingToGenerate { }
}