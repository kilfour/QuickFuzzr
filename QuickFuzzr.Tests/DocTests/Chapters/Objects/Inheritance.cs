using QuickPulse.Explains;

namespace QuickFuzzr.Tests.DocTests.Chapters.Objects;

[DocContent(@"Use The `Fuzzr.For<T>().GenerateAsOneOf(params Type[] types)` method chain.

F.i. :
```
Fuzzr.For<SomeThingAbstract>().GenerateAsOneOf(
	typeof(SomethingDerived), typeof(SomethingElseDerived))
```")]
public class Inheritance
{
	[Fact]
	[DocContent("When generating an object of type T, an object of a random chosen type from the provided list will be generated instead.")]
	public void UsingDerived()
	{
		var generator =
			from _ in Configr<SomeThingAbstract>.AsOneOf(typeof(SomeThingDerivedToGenerate))
			from thing in Fuzzr.One<SomeThingAbstract>()
			select thing;
		var result = generator.Generate();
		Assert.IsType<SomeThingDerivedToGenerate>(result);
	}

	[Fact]
	[DocContent("**Note :** The `GenerateAsOneOf(...)` combinator does not actually generate anything, it only influences further generation.")]
	public void ReturnsUnit()
	{
		var generator = Configr<SomeThingAbstract>.AsOneOf(typeof(SomeThingDerivedToGenerate));
		Assert.Equal(Intent.Fixed, generator.Generate());
	}

	public abstract class SomeThingAbstract
	{
		public int AnInt { get; set; }
	}

	public class SomeThingDerivedToGenerate : SomeThingAbstract { }
}