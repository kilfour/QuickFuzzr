using QuickFuzzr.UnderTheHood;
using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters.Objects;

[DocContent(@"Use the `Fuzz.For<T>().Ignore<TProperty>(Expression<Func<T, TProperty>> func)` method chain.

F.i. :
```
Fuzz.For<SomeThingToGenerate>().Ignore(s => s.Id)
```")]
public class IgnoringProperties
{
	[Fact]
	[DocContent("The property specified will be ignored during generation.")]
	public void StaysDefaultValue()
	{
		var generator =
			from _ in Fuzz.For<SomeThingToGenerate>().Ignore(s => s.AnInt)
			from result in Fuzz.One<SomeThingToGenerate>()
			select result;
		Assert.Equal(0, generator.Generate().AnInt);
	}

	[Fact]
	[DocContent("Derived classes generated also ignore the base property.")]
	public void WorksForDerived()
	{
		var generator =
			from _ in Fuzz.For<SomeThingToGenerate>().Ignore(s => s.AnInt)
			from result in Fuzz.One<SomeThingDerivedToGenerate>()
			select result;
		Assert.Equal(0, generator.Generate().AnInt);
	}

	[Fact]
	[DocContent(@"Sometimes it is useful to ignore all properties while generating an object.  
For this use `Fuzz.For<SomeThingToGenerate>().IgnoreAll()`")]
	public void IgnoreAll()
	{
		var generator =
			from _ in Fuzz.For<SomeThingToGenerate>().IgnoreAll()
			from result in Fuzz.One<SomeThingToGenerate>()
			select result;
		Assert.Equal(0, generator.Generate().AnInt);
	}

	[Fact]
	[DocContent(@"`IgnoreAll()` does not ignore properties on derived classes, even inherited properties.")]
	public void IgnoreAllDerived()
	{
		var generator =
			from r in Fuzz.Constant(13).Replace()
			from _ in Fuzz.For<SomeThingToGenerate>().IgnoreAll()
			from result in Fuzz.One<SomeThingDerivedToGenerate>()
			select result;
		var thing = generator.Generate();
		Assert.Equal(13, thing.AnInt);
		Assert.Equal(13, thing.AnotherInt);
	}

	[Fact]
	[DocContent("**Note :** `The Ignore(...)` combinator does not actually generate anything, it only influences further generation.")]
	public void ReturnsUnit()
	{
		var generator = Fuzz.For<SomeThingToGenerate>().Ignore(s => s.AnInt);
		Assert.Equal(Unit.Instance, generator.Generate());
	}

	public class SomeThingToGenerate
	{
		public int AnInt { get; set; }
	}

	public class SomeThingDerivedToGenerate : SomeThingToGenerate
	{
		public int AnotherInt { get; set; }
	}
}